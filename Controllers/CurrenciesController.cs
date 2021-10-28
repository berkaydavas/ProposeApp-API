using ProposeAppAPI.Helpers;
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
    [RoutePrefix("Currencies")]
    public class CurrenciesController : ApiController
    {
        // POST api/Currencies/Take
        /// <summary>
        /// Gives a currencies list.
        /// </summary>
        [HttpGet]
        [Route("Take")]
        public object Take([FromUri] bool onlyPrimaries = true)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var currencies = (onlyPrimaries ?
                    db.Currencies.Where(x => x.isPrimary) :
                    db.Currencies)
                    .Select(x => new
                    {
                        x.id,
                        x.code,
                        x.symbol
                    })
                    .ToList();

                return currencies;
            }
        }

        // POST api/Currencies/Values
        /// <summary>
        /// Gives a currencies list with TRY value.
        /// </summary>
        [HttpGet]
        [Route("Values")]
        public object Values([FromUri] DateTime date, [FromUri] bool onlyPrimaries = true)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var currencies = (onlyPrimaries ?
                    db.Currencies.Where(x => x.isPrimary) :
                    db.Currencies)
                    .Select(x => new
                    {
                        x.id,
                        x.code,
                        x.symbol
                    })
                    .ToList();

                Dictionary<string, int> currIds = currencies.ToDictionary(x => x.code, x => x.id);
                Dictionary<string, string> currSymbols = currencies.ToDictionary(x => x.code, x => x.symbol);
                List<string> currCodes = currencies.Select(x => x.code).ToList();

                var TCMBCurrencies = TCMBCurrency
                    .GetCurrencies(date)
                    .Where(x => currCodes.Contains(x.CurrencyCode))
                    .Select(x => new
                    {
                        id = currIds[x.CurrencyCode],
                        symbol = currSymbols[x.CurrencyCode],
                        code = x.CurrencyCode,
                        value = decimal.Parse((x.BanknoteSelling ?? "0").Replace(".", ","))
                    })
                    .ToList();

                return TCMBCurrencies;
            }
        }
    }
}
