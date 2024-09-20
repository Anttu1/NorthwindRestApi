using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {   
        //Tietokantayhteyden alustaminen
        NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers() 
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + ex);
            }
        }

        //Hakee yhden asiakkaan pääavaimella
        [HttpGet("{id}")]
        public ActionResult GetOneCustomerById(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);
                if (asiakas != null)
                {
                    return Ok(asiakas);
                }
                else
                {
                    return BadRequest("Asiakasta ei löytynyt tällä id:llä:" + id);
                    //return BadRequest($"Asiakasta ei löytnyt tällä id:llä: {id}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest( "Tapahtui virhe, lue lisää:" + ex);
            }
        }
        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Customer cust)
        {
            try
            {
                db.Customers.Add(cust);
                db.SaveChanges();
                return Ok($"Lisättiin uusi asiakas: {cust.CompanyName} from {cust.City}");
            }
            catch (Exception ex)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + ex.InnerException);
            }
        }
    }
}
