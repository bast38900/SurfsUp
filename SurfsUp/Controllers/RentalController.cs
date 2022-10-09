using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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

        // GET: Boards
        public async Task<IActionResult> Store(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;

            using HttpClient client = new() { BaseAddress = new Uri("https://localhost:7276") };
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
        public async Task<IActionResult> Rent([FromRoute] Guid? id, [FromBody] Rent rent)
        {
            var boards = from b in _context.Board select b;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IList<Rent> newrent = new List<Rent>();

            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri($"https://localhost:7276/api/RentBoard/{id}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage postData = await client.PostAsJsonAsync<Rent>("rent", (Rent)newrent);

                if (postData.IsSuccessStatusCode)
                {
                    string results = postData.Content.ReadAsStringAsync().Result;
                    newrent = JsonConvert.DeserializeObject<List<Rent>>(results);
                }else
                {
                    return RedirectToAction("Store", "Rental");
                }
            }

            //string newRent = JsonConvert.SerializeObject(rent);

            #region old

            //if (ModelState.IsValid)
            //{
            //    foreach (var board in boards)
            //    {
            //        if (board.BoardId == id)
            //        {
            //            rent.Board = board;
            //            rent.StartRent = DateTime.Now;
            //            rent.Total = board.Price;
            //            rent.RentState = RentState.RentedOut;
            //            rent.UserId = Guid.Parse(userId);
            //            board.State = BoardState.Rented;
            //            break;
            //        }
            //    }

            //    _context.Add(rent);
            //    await _context.SaveChangesAsync();

            //    TempData["SuccessMessage"] = $"You have now rented the \"{rent.Board?.BoardName}\" board";
            //    return RedirectToAction("Store", "Rental", new { ac = "success" });
            //}

            //return RedirectToAction("Store", "Rental");

            #endregion
        }
    }
}
