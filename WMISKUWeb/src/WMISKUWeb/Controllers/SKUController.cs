using Microsoft.AspNetCore.Mvc;
using GameStop.SupplyChain.ThinkGeekDataContracts;
using WMISKUWeb.Models;

namespace WMISKUWeb.Controllers
{
    [Route("api/[controller]")]
    public class SKUController : Controller
    {

        public SKUController()
        {
            
        }

        //// GET api/values
        [HttpGet]
        public string Get()
        {
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
            SKUUpsertMessageContract message = new SKUUpsertMessageContract();

            //message = (SKUUpsertMessageContract)value.ToObject(message.GetType());

            string json = JSONHelper.ObjectToJSON(value);

            DataLayer dl = new DataLayer();

            dl.SubmitSKU(json);

            return Json("Message Received: " + message.ID);
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
