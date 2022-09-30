using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System.Security.Claims;

namespace SurfsUpAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class RentController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public RentController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        [HttpGet]
        [Route("AvailableBoards")]
        public async Task<ActionResult> GetAllAvailableBoards()
        {
            var boards = await _appDbContext.Board.ToListAsync();
            var rentals = await _appDbContext.Rent.ToListAsync();

            List<Board> availableBoards = new();

            foreach (var board in boards)
            {
                if (board.State == BoardState.Rented)
                {
                    foreach (var rental in rentals)
                    {
                        if (rental.EndRent < DateTime.Now && rental.RentState == RentState.RentedOut && board.State == BoardState.Rented)
                        {
                            rental.RentState = RentState.RentFinished;
                            board.State = BoardState.Available;
                            availableBoards.Add(board);
                            await _appDbContext.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    availableBoards.Add(board);
                    await _appDbContext.SaveChangesAsync();
                }
            }

            return Ok(availableBoards);
        }

        [HttpPost]
        [Route("RentBoard/{boardId:guid}")]
        public async Task<ActionResult> RentBoard([FromRoute] Guid boardId, [FromBody] Rent rent)
        {
            var boards = await _appDbContext.Board.ToListAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                foreach (var board in boards)
                {
                    if (boardId == board.BoardId)
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

                _appDbContext.Add(rent);
                await _appDbContext.SaveChangesAsync();
                
                return StatusCode(201);
            }

            return BadRequest();
        }
    }
}
