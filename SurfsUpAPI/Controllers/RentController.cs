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
            var boards = await _appDbContext.Board.ToListAsync();

            foreach (var board in boards)
            {
                if (board.BoardId == rentDto.BoardId)
                {
                    Rent rent = new Rent()
                    {
                        StartRent = DateTime.Now,
                        EndRent = rentDto.EndRent,
                        RentState = RentState.RentedOut,
                        Board = board,
                        Total = board.Price,
                        UserId = rentDto.UserId
                    };
                    board.State = BoardState.Rented;

                    _appDbContext.Add(rent);
                    await _appDbContext.SaveChangesAsync();

                    return Ok(rent);
                }
            }

            return BadRequest();
        }
    }
}
