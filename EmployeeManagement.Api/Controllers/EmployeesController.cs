using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class EmployeesController(EmployeeService employees, ILogger<EmployeesController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<Employee>> GetAll(
        [FromQuery] string? department,
        [FromQuery] string? search)
    {
        logger.LogInformation("Listing employees; department filter: {Department}", department);
        return Ok(employees.GetAll(department, search));
    }

    [HttpGet("{id:int}")]
    public ActionResult<Employee> GetById(int id)
    {
        var employee = employees.GetById(id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public ActionResult<Employee> Create(EmployeeInput input)
    {
        var employee = employees.Add(input);
        logger.LogInformation("Created employee {EmployeeId}", employee.Id);
        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Employee> Update(int id, EmployeeInput input)
    {
        var employee = employees.Update(id, input);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return employees.Delete(id) ? NoContent() : NotFound();
    }
}
