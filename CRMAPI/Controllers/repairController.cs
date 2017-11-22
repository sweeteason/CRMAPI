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
    public class repairController : ApiController
    {
        MobileRepository mobileRepository = new MobileRepository(DatabaseName.ConnectionString);

        // GET api/<controller>
        public IEnumerable<tek_repair> GetRepair(int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            return mobileRepository.GetRepairList(query);
        }

        // GET api/<controller>/5
        public IEnumerable<tek_repair> GetRepair(string id, int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            query.Keyword = id;
            return mobileRepository.GetRepairListByAccount(query);
        }

        public IEnumerable<tek_repair> GetRepair(string id, string status, int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            query.Keyword = id;
            return mobileRepository.GetRepairListByAccount(query).Where(p=> p.tek_repairstatus == status);
        }

        [HttpGet]
        public bool UpdateStatus(string id, string status)
        {
            return mobileRepository.UpdateMobileTime(id, status);
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }


        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}