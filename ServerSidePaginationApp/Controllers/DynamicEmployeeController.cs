using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSidePaginationApp.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ServerSidePaginationApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DynamicEmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public DynamicEmployeeController(AppDbContext context)
        {
            _context = context;
        }

        #region Employee

        // This action renders the Razor view for employees
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // This action handles the server-side pagination for employee DataTables
        [HttpPost("paginate")]
        public async Task<IActionResult> Paginate([FromBody] TableRequest request)
        {
            // Query the Employee table
            var query = _context.Employees.AsQueryable();

            // Apply filtering (optional)
            if (!string.IsNullOrEmpty(request.FilterColumn) && !string.IsNullOrEmpty(request.FilterValue))
            {
                query = ApplyEmployeeFilter(query, request.FilterColumn, request.FilterValue);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = Sort(query, request.SortColumn, request.SortDirection);
            }

            // Apply pagination
            var totalRecords = await query.CountAsync();
            var data = await query.Skip(request.Start).Take(request.Length).ToListAsync();

            // Return DataTables-friendly response
            return Ok(new
            {
                draw = request.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data
            });
        }

        // Method to apply filtering dynamically for employees
        private IQueryable<Employee> ApplyEmployeeFilter(IQueryable<Employee> query, string filterColumn, string filterValue)
        {
            return filterColumn switch
            {
                "name" => query.Where(e => e.Name.Contains(filterValue)),
                "position" => query.Where(e => e.Position.Contains(filterValue)),
                "office" => query.Where(e => e.Office.Contains(filterValue)),
                "salary" => query.Where(e => e.Salary.ToString().Contains(filterValue)),
                _ => query
            };
        }

        #endregion

        #region Department

        // This action renders the Razor view for departments
        [HttpGet("departments")]
        public IActionResult DepartmentIndex()
        {
            return View();
        }

        // This action handles the server-side pagination for department DataTables
        [HttpPost("departments/paginate")]
        public async Task<IActionResult> PaginateDepartments([FromBody] TableRequest request)
        {
            // Query the Department table
            var query = _context.Departments.AsQueryable();

            // Apply filtering (optional)
            if (!string.IsNullOrEmpty(request.FilterColumn) && !string.IsNullOrEmpty(request.FilterValue))
            {
                query = ApplyDepartmentFilter(query, request.FilterColumn, request.FilterValue);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = Sort(query, request.SortColumn, request.SortDirection);
            }

            // Apply pagination
            var totalRecords = await query.CountAsync();
            var data = await query.Skip(request.Start).Take(request.Length).ToListAsync();

            // Return DataTables-friendly response
            return Ok(new
            {
                draw = request.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data
            });
        }

        // Method to apply filtering dynamically for departments
        private IQueryable<Department> ApplyDepartmentFilter(IQueryable<Department> query, string filterColumn, string filterValue)
        {
            if (string.IsNullOrWhiteSpace(filterValue))
            {
                return query; // No filtering if the filter value is null, empty, or whitespace
            }

            return filterColumn.ToLower() switch
            {
                "name" => query.Where(d => d.Name.Contains(filterValue, StringComparison.OrdinalIgnoreCase)),
                "location" => query.Where(d => d.Location.Contains(filterValue, StringComparison.OrdinalIgnoreCase)),
                "employeecount" when int.TryParse(filterValue, out var employeeCount) =>
                    query.Where(d => d.EmployeeCount == employeeCount), // Adjust to exact match or range as needed
                _ => query // Add more filters as necessary
            };
        }

        #endregion

        #region Common

        // Method to apply sorting dynamically
        private IQueryable<T> Sort<T>(IQueryable<T> query, string sortColumn, string sortDirection) where T : class
        {
            if (sortDirection.ToLower() == "asc")
            {
                return query.OrderBy(EvaluateSortExpression<T>(sortColumn));
            }
            else
            {
                return query.OrderByDescending(EvaluateSortExpression<T>(sortColumn));
            }
        }

        // Method to dynamically create a sorting expression
        private Expression<Func<T, object>> EvaluateSortExpression<T>(string sortColumn) where T : class
        {
            return typeof(T).Name switch
            {
                nameof(Employee) => sortColumn switch
                {
                    "name" => e => ((Employee)(object)e).Name,
                    "position" => e => ((Employee)(object)e).Position,
                    "office" => e => ((Employee)(object)e).Office,
                    "salary" => e => ((Employee)(object)e).Salary,
                    _ => e => ((Employee)(object)e).Id // Default sorting by Id
                },
                nameof(Department) => sortColumn switch
                {
                    "name" => d => ((Department)(object)d).Name,
                    _ => d => ((Department)(object)d).Id // Default sorting by Id
                },
                _ => e => e // Default fallback
            };
        }

        #endregion
    }

    // Request model for DataTables pagination
    public class TableRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public string FilterColumn { get; set; }
        public string FilterValue { get; set; }
    }
}
