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

        /// <summary>
        /// 取得全部維修資料
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<tek_repair> GetRepair(int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            return mobileRepository.GetRepairList(query);
        }

        /// <summary>
        /// 依照員工帳號取得維修資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        public IEnumerable<tek_repair> GetRepair(string id, int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            query.Keyword = id;
            return mobileRepository.GetRepairListByAccount(query);
        }

        /// <summary>
        /// 依照員工帳號與維修狀態，取得維修資料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<tek_repair> GetRepair(string id, string status, int page = 1, int pageSize = 10)
        {
            QueryList query = new QueryList();
            query.Page = page;
            query.PageSize = pageSize;
            query.Keyword = id;
            return mobileRepository.GetRepairListByAccount(query).Where(p=> p.tek_repairstatus == status);
        }

        /// <summary>
        /// 更新維修單狀態
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public bool UpdateStatus(string id, string status)
        {
            return mobileRepository.UpdateMobileTime(id, status);
        }

        /// <summary>
        /// 檢查登入
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        public bool CheckLogin(string id, string status)
        {
            return mobileRepository.CheckLogin(id, status);
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