using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;

namespace NorthwindRestApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Tietokantayhteyden alustaminen

        //Perinteinen tapa
        //(private) NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency Injektion tapa
        private NorthwindOriginalContext db;

        public CustomersController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }



        //Hakee kaikki asiakkaat
        [HttpGet]
        public ActionResult GetAllCustomers()
        {
            try
            {
                var asiakkaat = db.Customers.ToList();
                return Ok(asiakkaat);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e);
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
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e);
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
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e.InnerException);
            }
        }
        //Asiakkaan poistaminen
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);

                if (asiakas != null)
                {
                    db.Customers.Remove(asiakas);
                    db.SaveChanges();
                    return Ok("Asiakas" + asiakas.CompanyName + "poistettiin.");
                }
                return NotFound("Asiakasta id:llä" + id + "ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Muokkaa
        [HttpPut("{id}")]
        public ActionResult EditCustomer(string id, [FromBody] Customer customer)
        {
            try
            {
                var asiakas = db.Customers.Find(id);
                if (customer != null)
                {
                    asiakas.CompanyName = customer.CompanyName;
                    asiakas.ContactName = customer.ContactName;
                    asiakas.Address = customer.Address;
                    asiakas.City = customer.City;
                    asiakas.Region = customer.Region;
                    asiakas.PostalCode = customer.PostalCode;
                    asiakas.Country = customer.Country;
                    asiakas.Phone = customer.Phone;
                    asiakas.Fax = customer.Fax;

                    db.SaveChanges();
                    return Ok("Muokattu asiakasta: " + asiakas.CompanyName);
                }
                return NotFound("Asiakasta ei löytynyt id:llä: " + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Hakee nimen osalla: /api/companyname/hakusana
        [HttpGet("companyname/{cname}")]
        public ActionResult GetByName(string cname)
        {
            try
            {
                var cust = db.Customers.Where(c => c.CompanyName.Contains(cname));
                //var cust = from c in db.Customers where c.CompanyName.Contains(cname) select c; <--- sama eri muodossa
                //var cust = db.Customers.Where(c => c.CompanyName == cname); <--- perfect match
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
