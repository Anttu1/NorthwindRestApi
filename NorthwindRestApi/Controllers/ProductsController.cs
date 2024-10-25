using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using System.Linq.Expressions;

namespace NorthwindRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //Tietokantayhteyden alustaminen

        //Perinteinen tapa
        //(private) NorthwindOriginalContext db = new NorthwindOriginalContext();

        //Dependency Injektion tapa
        private NorthwindOriginalContext db;

        public ProductsController(NorthwindOriginalContext dbparametri)
        {
            db = dbparametri;
        }



        //Hakee kaikki tuotteet
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            try
            {
                var tuotteet = db.Products.ToList();
                return Ok(tuotteet);
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e);
            }
        }

        //Hakee yhden tuotteen pääavaimella
        [HttpGet("{id:int}")]
        public ActionResult GetOneProductById(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);
                if (tuote != null)
                {
                    return Ok(tuote);
                }
                else
                {
                    return BadRequest("Tuotetta ei löytynyt tällä id:llä:" + id);
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
        public ActionResult AddNew([FromBody] Product prod)
        {
            try
            {
                db.Products.Add(prod);
                db.SaveChanges();
                return Ok($"Lisättiin uusi tuote: {prod.ProductName}");
            }
            catch (Exception e)
            {
                return BadRequest("Tapahtui virhe, lue lisää:" + e.InnerException);
            }
        }
        //Tuotteen poistaminen
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var tuote = db.Products.Find(id);

                if (tuote != null)
                {
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok("Tuote" + tuote.ProductName + "poistettiin.");
                }
                return NotFound("Tuotetta id:llä" + id + "ei löytynyt.");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Muokkaa
        [HttpPut("{id:int}")]
        public ActionResult<Product> EditProduct(int id, [FromBody] Product product)
        {
            try
            {
                var tuote = db.Products.Find(id);
                if (product != null)
                {
                    tuote.ProductName = product.ProductName;
                    tuote.QuantityPerUnit = product.QuantityPerUnit;
                    tuote.UnitPrice = product.UnitPrice;
                    tuote.UnitsInStock = product.UnitsInStock;
                    tuote.UnitsOnOrder = product.UnitsOnOrder;
                    tuote.ReorderLevel = product.ReorderLevel;
                    tuote.Discontinued = product.Discontinued;
                    tuote.ImageLink = product.ImageLink;


                    db.SaveChanges();
                    return Ok("Muokattu asiakasta: " + tuote.ProductName);
                }
                return NotFound("Tuotetta ei löytynyt id:llä: " + id);
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }
        //Hakee nimen osalla: /api/productname/hakusana
        [HttpGet("productname/{pname}")]
        public ActionResult GetByName(string pname)
        {
            try
            {
                var prod = db.Products.Where(p => p.ProductName.Contains(pname));
                return Ok(prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

