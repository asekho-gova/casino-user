using Casino.User.Api.Services;
using Casino.User.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Casino.User.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet("{userId}", Name = "GetUser")]
    public async Task<ActionResult<CasinoUser>> GetUserAsync(int userId)
    {
      try
      {
        var user = await _userService.GetUserAsync(userId);
        if (user == null)
        {
          return NotFound();
        }

        return Ok(user);
      }
      catch (Exception)
      {
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    [HttpPost]
    public async Task<ActionResult<CasinoUser>> CreateUserAsync([FromBody] CreateUserRequest request)
    {
      try
      {
        var user = await _userService.CreateUserAsync(request);
        if (user == null)
        {
          return StatusCode(500, "An error occurred while creating the user.");
        }

        return CreatedAtAction("GetUser", new { user.UserId }, user);
      }
      catch (ValidationException ex)
      {
        return BadRequest(ex.Message);
      }
      catch (Exception)
      {
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    [HttpPost("{userId}/updateBalance")]
    public async Task<ActionResult<UpdateBalanceResponse>> UpdateBalance(int userId, decimal amount)
    {
      try
      {
        var response = await _userService.UpdateBalanceAsync(userId, amount);
        
        return StatusCode((int)response.Status, response);
      }
      catch (Exception)
      {
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserAsync(int userId)
    {
      try
      {
        var deleted = await _userService.DeleteUserAsync(userId);
        if (!deleted)
        {
          return NotFound($"User with ID {userId} does not exist.");
        }
        return NoContent();
      }
      catch (Exception)
      {
        return StatusCode(500, "An error occurred while processing your request.");
      }
    }
  }
}
