using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;

namespace SurfsUp.Controllers
{
    [AllowAnonymous]
    public class RentalController : Controller
    {
        private readonly SurfsUpContext _context;
        private readonly UserManager<AppUser> _userManager;

        public RentalController(SurfsUpContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                        if (rental.EndRent < DateTime.Now && rental.RentState == RentState.RentedOut)
                        {
                            board.State = BoardState.Available;
                            rental.RentState = RentState.RentFinished;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                foreach (var board in boards)
                {
                    if (board.BoardId == id)
                    {
                        rent.Board = board;
                        rent.StartRent = DateTime.Now;
                        rent.Total = board.Price;
                        rent.RentState = RentState.RentedOut;
                        rent.UserId = Guid.Parse(userId);
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
