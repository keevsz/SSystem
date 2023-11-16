using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Context;
using SalesSystem.Models;
using System.Security.Claims;
using static SalesSystem.Models.Roles;

namespace SalesSystem.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext DB;
        public UserController(ApplicationDBContext context)
        {
            DB = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await DB.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await DB.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = RoleStrings.ADMIN)]
        public async Task<ActionResult<UserModel>> CreateUser(UserModel user)
        {
            string defaultHashedPassword = BCrypt.Net.BCrypt.HashPassword("password");
            user.Password = defaultHashedPassword;
            DB.Users.Add(user);
            await DB.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserById), new { id = user.ID }, user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, UserModel user)
        {

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var uuserLoggedId = identity.Claims.FirstOrDefault(x => x.Type == "id")!.Value;
            var userLoggedRole = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value;



            if (id != user.ID)
            {
                return BadRequest("Bad ids request");
            }

            if (userLoggedRole != RoleStrings.ADMIN)
            {
                if (user.ID != int.Parse(uuserLoggedId))
                {
                    return Unauthorized("You do not have permissions");
                }
            }

            DB.Entry(user).State = EntityState.Modified;

            try
            {
                await DB.SaveChangesAsync();
                var updatedUser = await DB.Users.FindAsync(id);

                if (updatedUser == null)
                {
                    return NotFound();
                }

                return updatedUser;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DB.Users.Any(s => s.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles=RoleStrings.ADMIN)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            UserModel user =  await DB.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("User not found");
            }

            DB.Users.Remove(user);
            await DB.SaveChangesAsync();
            return Ok("User deleted");
        }
    }
}
