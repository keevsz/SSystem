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
            var users = await DB.Users.Include(u => u.Role).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserById(int id)
        {
            var user = await DB.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Authorize(Roles = RoleStrings.ADMIN)]
        public async Task<ActionResult<UserModel>> CreateUser([FromBody] UserCreateDTO userDto)
        {
            string defaultHashedPassword = BCrypt.Net.BCrypt.HashPassword("password");

            var newUser = new UserModel
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Age = userDto.Age,
                Gender = userDto.Gender,
                Username = userDto.Username,
                Password = defaultHashedPassword, 
                RoleID = userDto.RoleID
            };

            var role = await DB.Roles.FirstOrDefaultAsync(r => r.Id == userDto.RoleID);
            DB.Users.Add(newUser);
            await DB.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.ID }, newUser);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, UserUpdateDTO userDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userLoggedId = identity.Claims.FirstOrDefault(x => x.Type == "id")!.Value;
            var userLoggedRole = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)!.Value;

            if (id != userDto.ID)
            {
                return BadRequest("Bad id request");
            }

            var existingUser = await DB.Users.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (userLoggedRole != RoleStrings.ADMIN)
            {
                if (userDto.ID != int.Parse(userLoggedId))
                {
                    return Unauthorized("You do not have permissions");
                }

                // Si el usuario no es un administrador, no permitir la actualización del rol
                userDto.RoleID = existingUser.RoleID;
            }
            else
            {
                // Solo actualizar el rol si el usuario es un administrador
                existingUser.RoleID = userDto.RoleID;
            }

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Email = userDto.Email;
            existingUser.Age = userDto.Age;
            existingUser.Gender = userDto.Gender;
            existingUser.Username = userDto.Username;

            Console.WriteLine("Nueva password: " + userDto.Password);

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            DB.Entry(existingUser).State = EntityState.Modified;

            try
            {
                await DB.SaveChangesAsync();
                return existingUser;
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
