using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;

namespace SurfsUp.Controllers
{
    // Controller class for CRUD opreations on boards
    // Authorization used to deny access for unapproved users
    [Authorize(Roles = "SuperAdmin")]
    [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Create([Bind("BoardName,Length,Width,Thickness,Volume,Type,Price,Equipment")] Board board)
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
        public async Task<IActionResult> Edit(Guid id, [Bind("BoardId,BoardName,Picture,Length,Width,Thickness,Volume,Type,Price,Equipment")] Board board, byte[] rowVersion)
        {
            if (id != board.BoardId)
            {
                return NotFound();
            }

            _context.Entry(board).Property("RowVersion").OriginalValue = rowVersion;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Board)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The board was deleted by another user.");
                        return View(board);
                    }
                    else
                    {
                        var databaseValues = (Board)databaseEntry.ToObject();

                        if (databaseValues.BoardName != clientValues.BoardName)
                        {
                            ModelState.AddModelError("Name", $"Current value: {databaseValues.BoardName}");
                        }
                        if (databaseValues.Picture != clientValues.Picture)
                        {
                            ModelState.AddModelError("Picture", $"Current value: {databaseValues.Picture}");
                        }
                        if (databaseValues.Length != clientValues.Length)
                        {
                            ModelState.AddModelError("Length", $"Current value: {databaseValues.Length}");
                        }
                        if (databaseValues.Width != clientValues.Width)
                        {
                            ModelState.AddModelError("Width", $"Current value: {databaseValues.Width}");
                        }
                        if (databaseValues.Thickness != clientValues.Thickness)
                        {
                            ModelState.AddModelError("Thickness", $"Current value: {databaseValues.Thickness}");
                        }
                        if (databaseValues.Volume != clientValues.Volume)
                        {
                            ModelState.AddModelError("Volume", $"Current value: {databaseValues.Volume}");
                        }
                        if (databaseValues.Type != clientValues.Type)
                        {
                            ModelState.AddModelError("Type", $"Current value: {databaseValues.Type}");
                        }
                        if (databaseValues.Price != clientValues.Price)
                        {
                            ModelState.AddModelError("Price", $"Current value: {databaseValues.Price}");
                        }
                        if (databaseValues.Equipment != clientValues.Equipment)
                        {
                            ModelState.AddModelError("Equipment", $"Current value: {databaseValues.Equipment}");
                        }

                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you got the original value. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to edit this record, click "
                                + "the Save button again. Otherwise click the Back to List hyperlink.");

                        board.RowVersion = (byte[])databaseValues.RowVersion;

                        ModelState.Remove("RowVersion");
                        //-----------------------
                        return View(board);
                    }
                    
                }
            }
            return RedirectToAction(nameof(Index));
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
