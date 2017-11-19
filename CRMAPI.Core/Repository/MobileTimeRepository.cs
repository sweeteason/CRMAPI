using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADOTool;
using ADOTool.CustomException;
using ADOTool.Utility;
using System.Data.SqlClient;
using System.Data;
using CRMAPI.Core.Entity;

namespace CRMAPI.Core.Repository
{
    public class MobileTimeRepository
    {
        string sqlConnectionString;

        public MobileTimeRepository(string connectionString)
        {
            this.sqlConnectionString = connectionString;
        }

        /// <summary>
        /// 取得MobileTimeList
        /// </summary>
        /// <returns></returns>
        public List<tek_mobiletime> GetMobileTimeList()
        {
            string SQL = @"
                select * from tek_mobiletime
            ";
            try
            {
                return AdoSupport.GetEntityList<tek_mobiletime>(System.Data.CommandType.Text, SQL, sqlConnectionString);
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "取得MobileTimeList時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 依維修單號取得 mobiletime
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public tek_mobiletime GetMobiletimeByNo(string no)
        {
            string SQL = @"
                select * from tek_mobiletime where tek_repair_tek_mobiletime = @no
            ";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("no", no),
            };
            try
            {
                return AdoSupport.GetEntity<tek_mobiletime>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依維修單號取得 mobiletime時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 依維修狀態取得 mobiletime
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public List<tek_mobiletime> GetMobiletimeByStatus(string status)
        {
            string SQL = @"
                select * from tek_mobiletime where tek_m_status = @status
            ";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("status", status),
            };
            try
            {
                return AdoSupport.GetEntityList<tek_mobiletime>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依維修狀態取得mobiletime時發生錯誤", ex);
            }
        }
    }
}
