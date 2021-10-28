using ProposeAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProposeAppAPI.Controllers
{
    [Authorize]
    [RoutePrefix("Products")]
    public class ProductsController : ApiController
    {
        // GET api/Products/Take/Brands
        /// <summary>
        /// Gives a product's brands list.
        /// </summary>
        [HttpGet]
        [Route("Take/Brands")]
        public List<string> TakeBrands()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<string> brands = db.Products
                    .GroupBy(x => x.brand)
                    .Select(x => x.Key)
                    .OrderBy(x => x)
                    .ToList();

                return brands;
            }
        }

        // GET api/Products/Take/Models
        /// <summary>
        /// Gives a product's models list.
        /// </summary>
        /// <param name="brand">Model's brand.</param>
        [HttpGet]
        [Route("Take/Models")]
        public List<string> TakeModels([FromUri] string brand)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<string> models = db.Products
                    .Where(x => x.brand == brand)
                    .Select(x => x.model)
                    .OrderBy(x => x)
                    .ToList();

                return models;
            }
        }

        // GET api/Products/Take
        /// <summary>
        /// Gives a single product.
        /// </summary>
        /// <param name="model">Product's model.</param>
        [HttpGet]
        [Route("Take")]
        public object Take([FromUri] string model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var product = db.Products
                    .Where(x => x.model == model)
                    .Select(x => new
                    {
                        x.id,
                        x.brand,
                        x.model,
                        x.stockCode,
                        x.description,
                        x.price,
                        x.pricePer,
                        x.unit,
                        currency = new
                        {
                            x.currency.id,
                            x.currency.symbol,
                            x.currency.code
                        },
                        x.isActive
                    })
                    .FirstOrDefault();

                return product;
            }
        }
    }
}
