using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //Tietokantayhteyden alustaminen

        //Perinteinen tapa
        //(private) NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency Injektion tapa
        private NorthwindOriginalContext db;

        public EmployeesController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }



        //Hakee kaikki työntekijät
        [HttpGet]
        public ActionResult GetAllEmployees()
        {
            try
            {
                var henkilot = db.Employees.ToList();
                return Ok(henkilot);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e);
            }
        }

        //Hakee yhden työntekijän pääavaimella
        [HttpGet("{id:int}")]
        public ActionResult GetOneEmployeeById(int id)
        {
            try
            {
                var henkilo = db.Employees.Find(id);
                if (henkilo != null)
                {
                    return Ok(henkilo);
                }
                else
                {
                    return BadRequest("Työntekijää ei löytynyt tällä id:llä:" + id);
                    //return BadRequest($"Tuotetta ei löytnyt tällä id:llä: {id}");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e);
            }
        }
        //Uuden lisääminen
        [HttpPost]
        public ActionResult AddNew([FromBody] Employee emp)
        {
            try
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                return Ok($"Lisättiin uusi työntekijä: {emp.FirstName} {emp.LastName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e.InnerException);
            }
        }
        //Työntekijän poistaminen
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var henkilo = db.Employees.Find(id);

                if (henkilo != null)
                {
                    db.Employees.Remove(henkilo);
                    db.SaveChanges();
                    return Ok("Työntekijä" + henkilo.FirstName + henkilo.LastName + "poistettiin.");
                }
                return NotFound("Työntekijää id:llä" + id + "ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Muokkaa
        [HttpPut("{id:int}")]
        public ActionResult<Product> EditEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                var henkilo = db.Employees.Find(id);
                if (employee != null)
                {
                    henkilo.FirstName = employee.FirstName;
                    henkilo.LastName = employee.LastName;
                    henkilo.Title = employee.Title;
                    henkilo.TitleOfCourtesy = employee.TitleOfCourtesy;
                    henkilo.BirthDate = employee.BirthDate;
                    henkilo.HireDate = employee.HireDate;
                    henkilo.Address = employee.Address;
                    henkilo.City = employee.City;
                    henkilo.Region = employee.Region;
                    henkilo.PostalCode = employee.PostalCode;
                    henkilo.Country = employee.Country;
                    henkilo.HomePhone = employee.HomePhone;
                    henkilo.Extension = employee.Extension;
                    henkilo.Photo = employee.Photo;
                    henkilo.Notes = employee.Notes;
                    henkilo.ReportsTo = employee.ReportsTo;
                    henkilo.PhotoPath = employee.PhotoPath;
                    db.SaveChanges();
                    return Ok("Muokattu työntekijää: " + henkilo.FirstName + henkilo.LastName);
                }
                return NotFound("Työntekijää ei löytynyt id:llä: " + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Hakee nimen osalla: /api/firstname/hakusana
        [HttpGet("employeename/{ename}")]
        public ActionResult GetByName(string ename)
        {
            try
            {
                var emp = db.Employees.Where(e => e.FirstName.Contains(ename) || e.LastName.Contains(ename));
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

