using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParksApi.Models;
using System.Reflection;

namespace ParksApi.Controllers
{
  [Produces("application/json")]
  [Route("api/[controller]")]
  [ApiController]
  public class ParksController : ControllerBase
  {
    private readonly ParksApiContext _db;
    public ParksController(ParksApiContext db)
    {
      _db = db;
    }
    /// <summary>
    /// Shows all parks. You can query by name or location.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Park>>> Get(string name, string location)
    {
      var query = _db.Parks.AsQueryable();
      if (name != null)
      {
        query = query.Where(entry => entry.Name == name);
      }
      if (location != null)
      {
        query = query.Where(entry => entry.Location == location);
      }
      return await query.ToListAsync();
    }
    /// <summary>
    /// Shows a specific park.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<Park>> GetPark(int id)
    {
      var park = await _db.Parks.FindAsync(id);
      if (park == null)
      {
        return NotFound();
      }
      return park;
    }
    /// <summary>
    /// Gets a random park for you to check out.
    /// </summary>
    [HttpGet("random")]
    public async Task<ActionResult<Park>> GetRandomPark()
    {
      Random rand = new Random();
      int id = rand.Next(1, _db.Parks.Count());
      var park = await _db.Parks.FindAsync(id);
      if (park == null)
      {
        return NotFound();
      }
      return park;
    }
    /// <summary>
    /// Creates a Park.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Park
    ///     {
    ///        "name": "Zion National Park",
    ///        "sqmiles": 229,
    ///        "location": "Utah"
    ///     }
    ///
    /// </remarks>
    /// <param name="park"></param>
    /// <returns>A newly created Park</returns>
    /// <response code="201">Returns the newly created park</response>
    /// <response code="400">If the park is null</response>   
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Park>> Post(Park park)
    {
      _db.Parks.Add(park);
      await _db.SaveChangesAsync();
      return CreatedAtAction(nameof(GetPark), new { id = park.ParkId }, park);
    }
    /// <summary>
    /// Edit a park.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Park
    ///     {
    ///        "id": 5,
    ///        "name": "Frontenac State Park",
    ///        "sqmiles": 4,
    ///        "location": "MN"
    ///     }
    ///
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Park park)
    {
      if (id != park.ParkId)
      {
        return BadRequest();
      }
      _db.Entry(park).State = EntityState.Modified;
      try
      {
        await _db.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ParkExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return NoContent();
    }
    /// <summary>
    /// Deletes a specific Park from API.
    /// </summary>
    /// <param name="id"></param>     
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePark(int id)
    {
      var park = await _db.Parks.FindAsync(id);
      if (park == null)
      {
        return NotFound();
      }
      _db.Parks.Remove(park);
      await _db.SaveChangesAsync();
      return NoContent();
    }
    private bool ParkExists(int id)
    {
      return _db.Parks.Any(e => e.ParkId == id);
    }
  }
}