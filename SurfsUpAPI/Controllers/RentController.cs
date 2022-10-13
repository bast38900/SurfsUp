using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Data;
using SurfsUpAPI.Models;
using System.Security.Claims;

namespace SurfsUpAPI.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class RentController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public RentController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("AvailableBoards")]
        public async Task<IActionResult> GetAllAvailableBoardsV1()
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

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("AvailableBoards")]
        public async Task<IActionResult> GetAllAvailableBoardsV2()
        {
            var boards = await _appDbContext.Board.ToListAsync();
            var rentals = await _appDbContext.Rent.ToListAsync();

            List<Board> availableBoards = new();

            foreach (var board in boards)
            {
                if (board.Type == "Shortboard")
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
            }

            return Ok(availableBoards);
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [Route("RentBoard")]
        public async Task<ActionResult> RentBoard([FromBody] RentDto rentDto)
        {
            try
            {
                var boards = await _appDbContext.Board.ToListAsync();
                Board board1 = new Board();
                var existingboard = _appDbContext.Board.Find(1);

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
