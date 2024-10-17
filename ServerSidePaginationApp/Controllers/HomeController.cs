using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSidePaginationApp.Models;

namespace ServerSidePaginationApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> LoadData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            // Fetching data from the database
            var employeeData = _context.Employees.AsNoTracking();

            // Filtering logic
            if (!string.IsNullOrEmpty(searchValue))
            {
                employeeData = employeeData.Where(m => m.Name.Contains(searchValue)
                                                    || m.Position.Contains(searchValue)
                                                    || m.Office.Contains(searchValue));
            }

            // Sorting logic based on specific columns
            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                switch (sortColumn)
                {
                    case "Name":
                        employeeData = sortColumnDirection == "asc" ? employeeData.OrderBy(e => e.Name) : employeeData.OrderByDescending(e => e.Name);
                        break;
                    case "Position":
                        employeeData = sortColumnDirection == "asc" ? employeeData.OrderBy(e => e.Position) : employeeData.OrderByDescending(e => e.Position);
                        break;
                    case "Office":
                        employeeData = sortColumnDirection == "asc" ? employeeData.OrderBy(e => e.Office) : employeeData.OrderByDescending(e => e.Office);
                        break;
                    default:
                        employeeData = employeeData.OrderBy(e => e.Id); // Default sort by Id
                        break;
                }
            }

            recordsTotal = employeeData.Count();

            // Pagination
            var data = await employeeData.Skip(skip).Take(pageSize).ToListAsync();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
    }
}
