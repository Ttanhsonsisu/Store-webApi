using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controller
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }    

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) 
        {
            if (!ModelState.IsValid)
                return BadRequest();
                
            var stockModel = await _stockRepo.GetAllAsync(query);
            var stock = stockModel.Select( e => e.ToStockDto()).ToList();

            return Ok(stock); 
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByID([FromRoute] int id) 
        {
            if (!ModelState.IsValid) 
               return BadRequest(ModelState);

            var result =  await _stockRepo.GetByIdAsync(id);
            
            if(result == null)
            {
                return NotFound();
            }

            return Ok(result.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetByID) , new {id = stockModel.ID}, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
            
            if(stockModel == null) 
            {
                return NotFound();
            }
            
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) 
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var stockModel = await _stockRepo.DeleteAsync(id);
            
            if(stockModel == null) {
                return NotFound();
            }

            return NoContent();
        }
    }
}