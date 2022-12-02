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
        public async Task<ActionResult<Board>> CreateBoard([Bind("BoardName,Picture,Length,Width,Thickness,Volume,Type,Price,Equipment")] Board board)
        {
            if (ModelState.IsValid)
            {
                _appDbContext.Add(board);
                await _appDbContext.SaveChangesAsync();
                return Ok(board);
            }
            return BadRequest();
        }
    }
}
