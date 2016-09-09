using System;
using Microsoft.AspNetCore.Mvc;
using GameStop.SupplyChain.DataContracts.ThinkGeek.SKUUpsert;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WMISKUWeb.Models;

namespace GameStop.SupplyChain.Services.WMISKUWeb.Controllers
{
    [Route("api/[controller]")]
    public class SKUController : Controller
    {
        private IConfiguration _config;
        private ILogger<SKUController> _logger;

        public SKUController(IConfiguration config, ILogger<SKUController> logger)
        {
            _config = config;
            _logger = logger;
        }

        //// GET api/values
        [HttpGet]
        public string Get()
        {
            _logger.LogInformation("HTTP GET Invoked @ {requestTime}", DateTime.Now);
            return "AVAILABLE";
        }

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]SKUUpsertMessageContract value)
        {
            string json = JSONHelper.ObjectToJSON(value);

            DataLayer dl = new DataLayer();

            dl.InsertSKU(json);

            return Json("Message Received: " + value.GUID);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
