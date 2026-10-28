using EmployeeManagement.Api.Models;

namespace EmployeeManagement.Api.Services;

/// <summary>In-memory demo store. Data resets whenever the API process restarts.</summary>
public sealed class EmployeeService
{
    private readonly object _gate = new();
    private readonly List<Employee> _employees =
    [
        new(1, "Asha Rao", "asha@example.com", "Engineering", 85000m),
        new(2, "Ravi Shah", "ravi@example.com", "Finance", 72000m),
        new(3, "Meera Nair", "meera@example.com", "Engineering", 91000m)
    ];
    private int _nextId = 4;

    public IReadOnlyList<Employee> GetAll(string? department, string? search)
    {
        lock (_gate)
        {
            // LINQ filters and sorts a snapshot, so callers cannot mutate the store.
            IEnumerable<Employee> query = _employees;
            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(employee => employee.Department.Equals(
                    department.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(employee => employee.FullName.Contains(
                    search.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            return query.OrderBy(employee => employee.FullName).ToList();
        }
    }

    public Employee? GetById(int id)
    {
        lock (_gate)
        {
            return _employees.FirstOrDefault(employee => employee.Id == id);
        }
    }

    public Employee Add(EmployeeInput input)
    {
        lock (_gate)
        {
            var employee = new Employee(_nextId++, input.FullName, input.Email, input.Department, input.Salary);
            _employees.Add(employee);
            return employee;
        }
    }

    public Employee? Update(int id, EmployeeInput input)
    {
        lock (_gate)
        {
            var index = _employees.FindIndex(employee => employee.Id == id);
            if (index < 0) return null;

            var employee = new Employee(id, input.FullName, input.Email, input.Department, input.Salary);
            _employees[index] = employee;
            return employee;
        }
    }

    public bool Delete(int id)
    {
        lock (_gate)
        {
            var employee = _employees.FirstOrDefault(item => item.Id == id);
            return employee is not null && _employees.Remove(employee);
        }
    }
}
