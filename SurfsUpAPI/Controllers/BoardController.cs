using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfsUpAPI.Data;
using SurfsUpLibrary.Models;

namespace SurfsUpAPI.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class BoardController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public BoardController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [MapToApiVersion("1.0")]
        [MapToApiVersion("2.0")]
        [Route("CreateBoard")]
        public async Task<ActionResult<BoardDto>> CreateBoard([FromBody] BoardDto board)
        {
            Board createdBoard = new Board()
            {
                BoardName = board.BoardName,
                Price = board.Price,
                Type = board.Type,
                Length = board.Length,
                Width = board.Width,
                Thickness = board.Thickness,
                Volume = board.Volume,
                Picture = board.Picture,
                Equipment = board.Equipment
            };

            if (ModelState.IsValid)
            {
                _appDbContext.Add(createdBoard);
                await _appDbContext.SaveChangesAsync();
                return Ok(createdBoard);
            }
            return BadRequest();
        }
    }
}
