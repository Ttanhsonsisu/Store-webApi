using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Extensions;
using api.interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        public CommentController(ICommentRepository commentRepo, 
        IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var comment = await _commentRepo.GetAllAsync();
            var commentDto = comment.Select(e => e.ToCommentDto());
          
            return Ok(commentDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByID([FromRoute] int id) {

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);
            
            if(comment == null) 
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }
        
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentDto commentDto)
        {
            
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if(!await _stockRepo.StockExitsts(stockId))
            {
                return BadRequest("stock doesn't exist"); 
            }

            var userName = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(userName);

            var commentModel = commentDto.ToCommentCreateDto(stockId);
            commentModel.AppUserId = appUser.Id;
            
            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetByID) , new {id = commentModel.ID}, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{idComment:int}")]
        public async Task<IActionResult> Update([FromRoute] int idComment, [FromBody] UpdateCommentRequestDto updateCommentDto) {
            
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(idComment , updateCommentDto.ToCommentUpdateDto(idComment));

            if (comment == null) 
            {
                return NotFound("Comment Not found");
            }
            
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var commentDel = await _commentRepo.DeleteAsync(id);

            if (commentDel == null) 
            {
                return NotFound("comment doesn't exist");
            }
            //return NoContent(); or
            return Ok(commentDel);
        }
    }
}