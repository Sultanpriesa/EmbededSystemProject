using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMSAPI.Data;
using EMSAPI.Models;
using EMSAPI.Dtos;
using System.IO.Ports;
using EMSAPI.Services;

namespace EMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SerialService _serialService;

        public MovementController(ApplicationDbContext context, SerialService serialService)
        {
            _context = context;
            _serialService = serialService;
        }

        // GET: api/Movement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movement>>> GetMovements()
        {
            return await _context.Movements.ToListAsync();
        }

        // GET: api/Movement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movement>> GetMovement(int id)
        {
            var movement = await _context.Movements.FindAsync(id);

            if (movement == null)
            {
                return NotFound();
            }

            return movement;
        }

        //get latest movement
        [HttpGet, Route("latest")]
        public async Task<ActionResult<Movement>> GetLatestMovement()
        {
            var movement = await _context.Movements.OrderByDescending(m => m.MoveID).FirstOrDefaultAsync();

            if (movement == null)
            {
                return NotFound();
            }

            return movement;
        }

        // PUT: api/Movement/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovement(int id, Movement movement)
        {
            if (id != movement.MoveID)
            {
                return BadRequest();
            }

            _context.Entry(movement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }            return NoContent();
        }
          // POST: api/Movement
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movement>> PostMovement(MovementDto createDto)
        {
            var movement = createDto.ToMovementModel();

            _context.Movements.Add(movement);
            await _context.SaveChangesAsync();

            if (movement.DataFrom == "Frontend")
            {
                await _serialService.SendMovementAsync(movement);
            }
            else if (movement.DataFrom == "Serial")
            {
                //show a message that the data is coming from serial
                Console.WriteLine(movement.Message);

                
                // No need to call serial service here, as it is already handled in the SerialService

            }

            // Call serial service after new data is added

            return CreatedAtAction("GetMovement", new { id = movement.MoveID }, movement);
        }



        // DELETE: api/Movement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovement(int id)
        {
            var movement = await _context.Movements.FindAsync(id);
            if (movement == null)
            {
                return NotFound();
            }

            _context.Movements.Remove(movement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovementExists(int id)
        {
            return _context.Movements.Any(e => e.MoveID == id);
        }
    }
}
