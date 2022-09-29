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

            List<Board> availabelBoards = new();

            foreach (var board in boards)
            {
                if (board.State == BoardState.Available)
                {
                    availabelBoards.Add(board);
                }
            }
            return Ok(availabelBoards);
        }

        [HttpPut]
        [Route("RentBoard/{boardId:guid}")]

        public async Task<ActionResult> RentBoard([FromRoute] Guid boardId, [Bind("EndRent")] Rent rent)
        {
            var boards = await _appDbContext.Board.ToListAsync();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
            return Ok(boards);
        }
    }
}
