using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSidePaginationApp.Models;

namespace ServerSidePaginationApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("paginate")]
        public async Task<IActionResult> GetPaginatedEmployees([FromBody] DataTableRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request.");
            }

            // Get the total count of employees
            var totalEmployees = await _context.Employees.CountAsync();

            // Apply sorting
            var employeesQuery = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                if (request.SortDirection == "asc")
                {
                    employeesQuery = employeesQuery.OrderBy(e => EF.Property<object>(e, request.SortColumn));
                }
                else
                {
                    employeesQuery = employeesQuery.OrderByDescending(e => EF.Property<object>(e, request.SortColumn));
                }
            }

            // Get the paginated data
            var employees = await employeesQuery
                .Skip(request.Start)
                .Take(request.Length)
                .Select(e => new
                {
                    e.Name,
                    e.Position,
                    e.Office,
                    e.Salary
                })
                .ToListAsync();

            var response = new
            {
                draw = request.Draw,
                recordsTotal = totalEmployees,
                recordsFiltered = totalEmployees,
                data = employees
            };

            return Ok(response);
        }
    }

    public class DataTableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}
