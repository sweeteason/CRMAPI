using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CRMAPI.Core.Entity;
using CRMAPI.Core.Repository;

namespace CRMAPI.Controllers
{
    public class mobiletimeController : ApiController
    {
        MobileTimeRepository mobileTimeRepository = new MobileTimeRepository(DatabaseName.ConnectionString);

        // GET api/<controller>
        public IEnumerable<tek_mobiletime> Get()
        {
            return mobileTimeRepository.GetMobileTimeList();
        }

        // GET api/<controller>/5
        public tek_mobiletime Get(string id)
        {
            return mobileTimeRepository.GetMobiletimeByNo(id);
        }

        // GET api/<controller>/<action>/<id>
        public IEnumerable<tek_mobiletime> GetByStatus(string id)
        {
            return mobileTimeRepository.GetMobiletimeByStatus(id);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}