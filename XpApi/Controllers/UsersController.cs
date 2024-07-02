using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Case.Models;

namespace Case.Controllers
{
    [Route("case/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Lists all users registered on the XML
        /// </summary>
        /// <returns>
        ///     - An empty list, in case the file doesn't exists; or
        ///     - A UserList with all the users from the xml file.
        /// </returns>
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            string filename = "userlist.xml";
            string currentDir = Directory.GetCurrentDirectory();
            string userFilePath = Path.Combine(currentDir, filename);

            if (!System.IO.File.Exists(userFilePath))
            {
                return Enumerable.Empty<User>();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(UserList), new XmlRootAttribute("userlist"));
            using (StreamReader reader = new StreamReader(userFilePath))
            {
                return ((UserList)serializer.Deserialize(reader)).Users;
            }
        }

        /// <summary>
        ///     It searches for the user by its id and returns its data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     - Status Code 404 (NotFound), in case "user" is null; or
        ///     - user (User var type).
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        ///     Changes an existing user's data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns>
        ///     - Status Code 400 (BadRequest), in case the id's are different; or
        ///     - Status Code 404 (NotFound), in case "user" is null; or
        ///     - Status Code 204 (NoContent), in case no data is returned.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        ///     Adds an user to the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        ///     - New user created.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostUser), new { id = user.Id }, user);
        }

        /// <summary>
        ///     Removes an user from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     - Status Code 404 (NotFound), in case "user" is null; or
        ///     - Status Code 204 (NoContent), in case no data is returned.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        ///     Verifies if a given id has a valid user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        ///     - false, in case no user is found; or
        ///     - true, in case a user is found.
        /// </returns>
        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
