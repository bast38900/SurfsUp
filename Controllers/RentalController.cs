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

        public IActionResult Index()
        {
            return View();
        }

        // GET: Boards
        public async Task<IActionResult> Store(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;

            var boards = from b in _context.Board select b;
            var rentals = from r in _context.Rent select r;

            // Check if rental has ended
            foreach (var board in boards)
            {
                if (board.State == BoardState.Rented)
                {
                    foreach (var rental in rentals)
                    {
                        if (rental.EndRent < DateTime.Now)
                        {
                            board.State = BoardState.Available;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            await _context.Board.ToListAsync();

            boards = boards.Where(board => board.State == BoardState.Available);

            // Search
            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.BoardName!.Contains(searchString));
            }

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            int pageSize = 4;
            return View(await PaginatedList<Board>.CreateAsync(boards.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Rent(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent([FromRoute] Guid? id, [Bind("EndRent")] Rent rent)
        {
            var boards = from b in _context.Board select b;

            if (ModelState.IsValid)
            {
                foreach (var board in boards)
                {
                    if (board.BoardId == id)
                    {
                        rent.Board = board;
                        rent.StartRent = DateTime.Now;
                        rent.Total = board.Price;
                        board.State = BoardState.Rented;
                        break;
                    }
                }

                _context.Add(rent);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"You have now rented the \"{rent.Board?.BoardName}\" board";
                return RedirectToAction("Store", "Rental", new { ac = "success" });
            }

            return RedirectToAction("Store", "Rental");
        }
    }
}
