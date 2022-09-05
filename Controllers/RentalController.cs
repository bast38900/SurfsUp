using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    public class RentalController : Controller
    {
        private readonly SurfsUpContext _context;

        public RentalController(SurfsUpContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var boards = from b in _context.Board select b;

            int pageSize = 4;
            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
    }
}
