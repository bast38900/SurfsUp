using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System.Data;
using System.Linq;
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
        public async Task<IActionResult> GetAllAvailableBoards()
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
        [Route("RentBoard")]
        public async Task<ActionResult> RentBoard([FromBody] RentDto rentDto)
        {
            try
            {

                var boards = await _appDbContext.Board.ToListAsync();
                Board board1 = new Board();
                var existingboard = _appDbContext.Board.Find(1);

                //var existingboard = _appDbContext.Board.FirstOrDefault(t => t.RowVersion()


                if (Convert.ToBase64String(existingboard.RowVersion) != Convert.ToBase64String(board1.RowVersion))
                {
                    return StatusCode(409); // conflict
                }
                Rent rent = new Rent();
                foreach (var board in boards)
                {
                    if (board.BoardId == rentDto.BoardId)
                    {
                        rent.StartRent = DateTime.Now;
                        rent.EndRent = rentDto.EndRent;
                        rent.RentState = RentState.RentedOut;
                        rent.Board = board;
                        rent.Total = board.Price;
                        rent.UserId = rentDto.UserId;
                        board.State = BoardState.Rented;
                        break;
                    }
                }

                _appDbContext.Add(rent);
                await _appDbContext.SaveChangesAsync();

                return Ok(rent);
            }
            catch (BadHttpRequestException)
            {
                return BadRequest();
            }
        }
    }
}
