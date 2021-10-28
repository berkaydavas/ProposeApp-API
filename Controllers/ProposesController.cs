using ProposeAppAPI.DataModels;
using ProposeAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using ProposeAppAPI.Helpers;
using ProposeAppAPI.ResponseModels;
using System.Web;
using Microsoft.AspNet.Identity;

namespace ProposeAppAPI.Controllers
{
    [Authorize]
    [RoutePrefix("Proposes")]
    public class ProposesController : ApiController
    {
        // GET api/Proposes/Take
        /// <summary>
        /// Gives a proposes list.
        /// </summary>
        [HttpGet]
        [Route("Take")]
        public List<TakeProposesResponseModel> Take()
        {
            string ActiveUserId = HttpContext.Current.User.Identity.GetUserId();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var proposes = db.Proposes
                    .Include(x => x.currency)
                    .Include(x => x.createdBy)
                    .Include(x => x.versions)
                    .Where(x => x.createdById == ActiveUserId)
                    .Select(x => new
                    {
                        id = x.id,
                        name = x.keyName,
                        description = x.description,
                        customer = x.customer,
                        company = x.company,
                        inCharge = x.inCharge,
                        project = x.project,
                        startDate = x.startDate,
                        currency = new
                        {
                            x.currency.id,
                            x.currency.symbol,
                            x.currency.code
                        },
                        exrateDate = x.exrateDate,
                        createdDate = x.createdDate,
                        createdBy = new
                        {
                            x.createdBy.Id,
                            x.createdBy.Email,
                            x.createdBy.UserName
                        },
                        versions = x.versions
                    })
                    .ToList();

                double calculateProposeTotalPrice(List<ProposeVersionJsonLine> lines)
                {
                    double totalPrice = 0;

                    if (lines != null)
                    {
                        lines.ForEach(x =>
                        {
                            if (x.type == 0)
                            {
                                totalPrice += Math.Round(x.qty * Math.Round(x.unitPrice, 2), 2);
                            }
                            else
                            {
                                totalPrice += calculateProposeTotalPrice(x.children);
                            }
                        });
                    }

                    return Math.Round(totalPrice, 2);
                }

                List<TakeProposesResponseModel> response = (from prp in proposes
                                                            let updatedVersion = prp.versions.OrderByDescending(y => y.createdDate).Select(y => y.json ?? "{}").FirstOrDefault() ?? "{}"
                                                            let versionLines = JsonConvert.DeserializeObject<ProposeVersionJson>(updatedVersion).lines
                                                            select new TakeProposesResponseModel()
                                                            {
                                                                id = prp.id,
                                                                name = prp.name,
                                                                description = prp.description,
                                                                customer = prp.customer,
                                                                company = prp.company,
                                                                inCharge = prp.inCharge,
                                                                project = prp.project,
                                                                startDate = prp.startDate,
                                                                totalPrice = versionLines == null ? 0 : calculateProposeTotalPrice(versionLines),
                                                                currency = new TakeSingleProposeCurrency()
                                                                {
                                                                    id = prp.currency.id,
                                                                    symbol = prp.currency.symbol,
                                                                    code = prp.currency.code
                                                                },
                                                                exrateDate = prp.exrateDate,
                                                                createdDate = prp.createdDate,
                                                                createdBy = new TakeSingleProposeCreated()
                                                                {
                                                                    id = prp.createdBy.Id,
                                                                    email = prp.createdBy.Email,
                                                                    userName = prp.createdBy.UserName
                                                                }
                                                            })
                                                            .ToList();

                return response;
            }
        }

        // GET api/Proposes/Take/{id}
        /// <summary>
        /// Gives a single propose.
        /// </summary>
        /// <param name="id">The ID of the propose.</param>
        [HttpGet]
        [Route("Take/{id}")]
        public TakeSingleProposeResponseModel Take(int id)
        {
            string ActiveUserId = HttpContext.Current.User.Identity.GetUserId();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var propose = db.Proposes
                    .Include(x => x.currency)
                    .Include(x => x.createdBy)
                    .Include(x => x.versions)
                    .Where(x => x.id == id && x.createdById == ActiveUserId)
                    .Select(x => new
                    {
                        id = x.id,
                        name = x.keyName,
                        description = x.description,
                        customer = x.customer,
                        company = x.company,
                        inCharge = x.inCharge,
                        project = x.project,
                        startDate = x.startDate,
                        currency = new TakeSingleProposeCurrency()
                        {
                            id = x.currency.id,
                            symbol = x.currency.symbol,
                            code = x.currency.code
                        },
                        exrateDate = x.exrateDate,
                        createdDate = x.createdDate,
                        createdBy = new TakeSingleProposeCreated()
                        {
                            id = x.createdBy.Id,
                            email = x.createdBy.Email,
                            userName = x.createdBy.UserName
                        },
                        linesJson = x.versions.OrderByDescending(y => y.createdDate).Select(y => y.json ?? "{}").FirstOrDefault() ?? "{}"
                    })
                    .FirstOrDefault();

                if (propose == null)
                    return null;

                var primaryCurrs = db.Currencies.Where(x => x.isPrimary).Select(x => new { x.id, x.symbol, x.code }).ToList();
                Dictionary<string, int> currIds = primaryCurrs.ToDictionary(x => x.code, x => x.id);
                Dictionary<string, string> currSymbols = primaryCurrs.ToDictionary(x => x.code, x => x.symbol);
                List<string> primaryCurrCodes = primaryCurrs.Select(x => x.code).ToList();

                var TCMBCurrencies = TCMBCurrency
                    .GetCurrencies(propose.exrateDate)
                    .Where(x => primaryCurrCodes.Contains(x.CurrencyCode))
                    .Select(x => new TakeSingleProposeCurrency()
                    {
                        id = currIds[x.CurrencyCode],
                        symbol = currSymbols[x.CurrencyCode],
                        code = x.CurrencyCode,
                        value = decimal.Parse((x.BanknoteSelling ?? "0").Replace(".", ","))
                    })
                    .ToList();

                TCMBCurrencies.Add(new TakeSingleProposeCurrency()
                {
                    id = currIds["TRY"],
                    symbol = currSymbols["TRY"],
                    code = "TRY",
                    value = (decimal)1
                });

                List<string> brands = db.Products
                    .GroupBy(x => x.brand)
                    .Select(x => x.Key)
                    .OrderBy(x => x)
                    .ToList();

                return new TakeSingleProposeResponseModel()
                {
                    propose = new TakeSingleProposeModel()
                    {
                        id = propose.id,
                        name = propose.name,
                        description = propose.description,
                        customer = propose.customer,
                        company = propose.company,
                        inCharge = propose.inCharge,
                        project = propose.project,
                        startDate = propose.startDate,
                        currency = new TakeSingleProposeCurrency()
                        {
                            id = propose.currency.id,
                            symbol = propose.currency.symbol,
                            code = propose.currency.code,
                            value = TCMBCurrencies.Where(x => x.code == propose.currency.code).Select(x => x.value).FirstOrDefault()
                        },
                        exrateDate = propose.exrateDate,
                        createdDate = propose.createdDate,
                        createdBy = new TakeSingleProposeCreated()
                        {
                            id = propose.createdBy.id,
                            email = propose.createdBy.email,
                            userName = propose.createdBy.userName
                        },
                        lines = JsonConvert.DeserializeObject<ProposeVersionJson>(propose.linesJson).lines
                    },
                    currencies = TCMBCurrencies,
                    brands = brands
                };
            }
        }

        // POST api/Proposes/Add
        /// <summary>
        /// For adding a propose.
        /// </summary>
        [HttpPost]
        [Route("Add")]
        public async Task<IHttpActionResult> Add(AddProposeBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Propose propose = new Propose()
                {
                    keyName = model.name,
                    description = model.description,
                    customer = model.customer,
                    company = model.company,
                    inCharge = model.inCharge,
                    project = model.project,
                    startDate = model.startDate,
                    currencyId = model.currency
                };

                db.Proposes.Add(propose);

                await db.SaveChangesAsync();

                return Ok(propose);
            }
        }

        // POST api/Proposes/Edit
        /// <summary>
        /// For editing a propose.
        /// </summary>
        [HttpPost]
        [Route("Edit")]
        public IHttpActionResult Edit(EditProposeBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Propose propose = db.Proposes
                    .Include(x => x.versions)
                    .Where(x => x.id == model.propose.id)
                    .FirstOrDefault();

                propose.keyName = model.propose.name;
                propose.description = model.propose.description;
                propose.customer = model.propose.customer;
                propose.company = model.propose.company;
                propose.inCharge = model.propose.inCharge;
                propose.project = model.propose.project;
                propose.startDate = model.propose.startDate;
                propose.currencyId = model.propose.currency.id;
                propose.exrateDate = model.propose.exrateDate;

                if (propose.versions.Count() == 5)
                {
                    ProposeVersion oldestVersion = propose.versions.OrderBy(x => x.createdDate).First();

                    oldestVersion.json = JsonConvert.SerializeObject(new { lines = model.lines });
                    oldestVersion.reviseNumber = propose.reviseNumber;
                    oldestVersion.createdDate = DateTime.Now;
                    oldestVersion.createdById = User.Identity.GetUserId();
                }
                else
                {
                    ProposeVersion version = new ProposeVersion()
                    {
                        proposeId = propose.id,
                        json = JsonConvert.SerializeObject(new { lines = model.lines }),
                        reviseNumber = propose.reviseNumber
                    };
                    propose.versions.Add(version);
                }

                db.SaveChanges();

                return Ok();
            }
        }
    }
}