using Microsoft.AspNetCore.Mvc;
using ServiceStatServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSStatServer.Interfaces;
using WSStatServer.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WSStatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IConnexion _connexion;

        public StatisticController(IConnexion connexion)
        {
            _connexion = connexion;
        }

        /*
        // GET: api/<StatisticController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StatisticController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        */

        // POST api/<StatisticController>
        [HttpPost]
        //public void Post([FromBody] string value)
        public async Task<ActionResult<Statistic>> PostStatistic(Statistic statistic)
        {
            StatRequest statRequest = new StatRequest { Object = statistic.Id, Stat = statistic.Stat };

            try
            {
                StatReply statReply = await _connexion.client.GetStatAsync(statRequest);

                statistic.Value = statReply.Value;
            }
            catch (Exception)
            {
                statistic.Value = "-1";
            }

            return statistic;
        }

        /*
        // PUT api/<StatisticController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StatisticController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}
