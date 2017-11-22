using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CRMAPI.Core.Entity;
using ADOTool;
using ADOTool.CustomException;

namespace CRMAPI.Core.Repository
{
    public class MobileRepository
    {
        string sqlConnectionString;
        public MobileRepository(string connectionString)
        {
            this.sqlConnectionString = connectionString;
        }

        /// <summary>
        /// 取得維修單列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<tek_repair> GetRepairList(QueryList query)
        {
            string SQL = @"
                select top (@PageSize) * 
                from (
                    select *,row_number() over (order by tek_recipient_date desc) as rownumber from tek_repair
                ) a
                where rownumber > @PageSize * (@Page - 1)
            ";

            var parameters = new SqlParameter[]
            {
                 new SqlParameter("PageSize", query.PageSize),
                 new SqlParameter("Page", query.Page),
            };
            try
            {
                return AdoSupport.GetEntityList<tek_repair>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "取得維修單列表時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 依帳號取得維修單列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<tek_repair> GetRepairListByAccount(QueryList query)
        {
            string SQL = @"
                select top (@PageSize) * 
                from (
                    select *,row_number() over (order by tek_recipient_date desc) as rownumber from tek_repair where tek_account = @account
                ) a
                where rownumber > @PageSize * (@Page - 1)
            ";

            var parameters = new SqlParameter[]
            {
                 new SqlParameter("account", query.Keyword),
                 new SqlParameter("PageSize", query.PageSize),
                 new SqlParameter("Page", query.Page),
            };
            try
            {
                return AdoSupport.GetEntityList<tek_repair>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依帳號取得維修單列表時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 更新時間回報
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool UpdateMobileTime(string account, string status)
        {
            string SQL = @"
                if exists (select * from tek_mobiletime where tek_repair_tek_mobiletime = @id)
                   begin
                       update tek_mobiletime set tek_m_status = @status,tek_flag = getdate() where tek_repair_tek_mobiletime = @id;
	                   select '1'
                   end
                else
                   begin
	                   select '0'
                   end 
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("status", status),
                 new SqlParameter("id", account),
            };
            try
            {
                DataTable dt = AdoSupport.GetDataTable(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString() == "1";
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依帳號取得維修單列表時發生錯誤", ex);
            }
        }
    }
}
