using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    public class BoardsController : Controller
    {
        private readonly SurfsUpContext _context;

        public BoardsController(SurfsUpContext context)
        {
            _context = context;
        }

        // GET: Boards
        public async Task<IActionResult> Index(string searchString, string sortOrder, string boardType)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.TypeSortParm = sortOrder == "Type" ? "type_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            IQueryable<string> typeQuery = from b in _context.Board
                                            orderby b.Type
                                            select b.Type;

            var boards = from b in _context.Board
                         select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.BoardName!.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(boardType))
            {
                boards = boards.Where(x => x.Type == boardType);
            }

            boards = sortOrder switch
            {
                "name_desc" => boards.OrderByDescending(b => b.BoardName),
                "Type" => boards.OrderBy(b => b.Type),
                "type_desc" => boards.OrderByDescending(b => b.Type),
                "Price" => boards.OrderBy(b => b.Price),
                "price_desc" => boards.OrderByDescending(b => b.Price),
                _ => boards.OrderBy(b => b.BoardName),
            };

            var boardTypeVM = new BoardTypeViewModel
            {
                Types = new SelectList(await typeQuery.Distinct().ToListAsync()),
                Boards = await boards.ToListAsync()
            };

            return View(boardTypeVM);

            //return View(await boards.ToListAsync());

            //    return _context.Board != null ? 
            //                 View(await _context.Board.ToListAsync());
            //                 Problem("Entity set 'SurfsUpContext.Board'  is null.");
        }

        // GET: Boards/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // GET: Boards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoardId,BoardName,Length,Width,Thickness,Volume,Type,Price,Equipment")] Board board)
        {
            //ModelState.Remove("Equipment");

            if (ModelState.IsValid)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("BoardId,BoardName,Picture,Length,Width,Thickness,Volume,Type,Price,Equipment")] Board board)
        {
            if (id != board.BoardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.BoardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(board);
        }

        // GET: Boards/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            var board = await _context.Board
                .FirstOrDefaultAsync(m => m.BoardId == id);
            if (board == null)
            {
                return NotFound();
            }

            return View(board);
        }

        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'SurfsUpContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoardExists(Guid id)
        {
          return (_context.Board?.Any(e => e.BoardId == id)).GetValueOrDefault();
        }
    }
}
