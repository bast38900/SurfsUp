using Microsoft.AspNetCore.Mvc;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;
using SurfsUpLibrary.Models;
using System.Net;

namespace SurfsUp.Controllers
{
    [AllowAnonymous]
    public class RentalController : Controller
    {
        private readonly SurfsUpContext _context;

        private readonly UserManager<AppUser> _userManager;

        public RentalController(UserManager<AppUser> userManager, SurfsUpContext surfsUpContext)
        {
            _userManager = userManager;
            _context = surfsUpContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyRents()
        {
            return View();
        }

        // GET: Boards
        public async Task<IActionResult> Store(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            string message = TempData["Message"] as string;
            if (!string.IsNullOrEmpty(message))
                ViewBag.Message = message;

            ViewData["CurrentSort"] = sortOrder;

            using HttpClient client = new() { BaseAddress = new Uri("https://localhost:7009") };
            string Uri = "/api/v2/AvailableBoards";
            if (User.Identity.IsAuthenticated)
            {
                Uri = "/api/v1/AvailableBoards";
            }

            using HttpResponseMessage response = await client.GetAsync(Uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            IEnumerable<Board>? boards = JsonConvert.DeserializeObject<List<Board>>(jsonResponse);

            // Search
            if (!string.IsNullOrEmpty(searchString))
            {
                boards = boards.Where(s => s.BoardName!.ToUpper().Contains(searchString.ToUpper()));
            }

            
            // Paging
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            int pageSize = 8;

            return View(await PaginatedList<Board>.CreateAsync(boards, pageNumber ?? 1, pageSize));
        }

        public IActionResult CantRentMore()
        {
            return View();
        }

        public IActionResult Rent(Guid? id)
        {
            if (id == null || _context.Board == null)
            {
                return NotFound();
            }

            return View();
        }

        //
        // Done
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Rent([FromRoute] Guid id, [FromForm] DateTime EndRent)
        {
            Guid userId = new Guid();            

            using HttpClient client = new() { BaseAddress = new Uri("https://localhost:7009") };
            
            string Uri = "/api/v2/RentBoard";
            if (User.Identity.IsAuthenticated)
            {
                Uri = "/api/v1/RentBoard";
                userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            RentDto rentDto = new RentDto()
            {
                BoardId = id,
                UserId = userId,
                EndRent = EndRent
            };

            var post = await client.PostAsJsonAsync(Uri, rentDto);
            post.EnsureSuccessStatusCode();

            return RedirectToAction("Store");
        }
    }
}
