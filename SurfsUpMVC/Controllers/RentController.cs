using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace SurfsUp.Controllers
{
    [AllowAnonymous]
    public class RentController : Controller
    {
        private readonly SurfsUpContext _context;

        private readonly UserManager<AppUser> _userManager;

        public RentController(UserManager<AppUser> userManager, SurfsUpContext surfsUpContext)
        {
            _userManager = userManager;
            _context = surfsUpContext;
        }

        #region Forside
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Butiksside
        // GET: Boards
        public async Task<IActionResult> Store(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;

            using HttpClient client = new() { BaseAddress = new Uri("https://localhost:7009") };
            string Uri = "/api/AvailableBoards";

            using HttpResponseMessage response = await client.GetAsync(Uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            IEnumerable<Board>? boards = JsonConvert.DeserializeObject<List<Board>>(jsonResponse);

            // Search
            if (!string.IsNullOrEmpty(searchString))
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

            return View(await PaginatedList<Board>.CreateAsync(boards, pageNumber ?? 1, pageSize));
        }
        #endregion

        #region Leje af board side
        public IActionResult Rent(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            return View();
        }
        #endregion

        //
        // Need to be done (Krav-5)
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent()
        {
            return null;
        }
    }
}
