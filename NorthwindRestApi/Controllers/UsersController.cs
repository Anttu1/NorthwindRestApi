using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindRestApi.Models;

namespace NWRestApi2022k.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // private readonly NorthwindOriginalContext db = new NorthwindOriginalContext();

        // Dependency Injection tyyli
        private readonly NorthwindOriginalContext db;

        public UsersController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var users = db.Users;


            foreach (var user in users)
            {
                user.PassWord = null;
            }
            return Ok(users);

        }
        [HttpGet("{userId}")]
        public ActionResult<User> GetUserById(int userId)
        {
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Uuden lisääminen
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {

                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.UserName);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut. Tässä lisätietoa: " + e);
            }
        }
        //poistaminen
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);

                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    return Ok("Käyttäjä" + user.UserName + "poistettiin.");
                }
                return NotFound("Käyttäjää id:llä" + id + "ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Muokkaa
        [HttpPut("{id}")]
        public ActionResult<User> EditUser(int id, [FromBody] User user)
        {
            try
            {
                var kayttaja = db.Users.Find(id);
                if (user != null)
                {
                    kayttaja.FirstName = user.FirstName;
                    kayttaja.LastName = user.LastName;
                    kayttaja.Email = user.Email;
                    kayttaja.UserName = user.UserName;
                    kayttaja.PassWord = user.PassWord;
                    kayttaja.AccessId = user.AccessId;
                    db.SaveChanges();
                    return Ok("Muokattu käyttäjää: " + user.UserName);
                }
                return NotFound("Käyttäjää ei löytynyt id:llä: " + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
    }
}
