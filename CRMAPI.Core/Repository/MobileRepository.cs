﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CRMAPI.Core.Entity;
using ADOTool;
using ADOTool.CustomException;
using System.DirectoryServices;

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
                    select *,row_number() over (order by tek_recipient_date desc) as rownumber from Repair_Staging
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
                    select *,row_number() over (order by tek_recipient_date desc) as rownumber from Repair_Staging where tek_account = @account
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
        /// 寫入預約記錄
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool AddReserve(string no, string status,string user, string reserve)
        {
            string SQL = @"
                if exists (select * from Mobiletime_Staging where tek_repair_tek_mobiletime = @id)
                   begin
                       update Mobiletime_Staging set tek_m_status = @status,tek_flag = getdate() where tek_repair_tek_mobiletime = @id;
	                   select '1'
                   end
                else
                   begin
	                   select '0'
                   end 
            ";
            SQL = @"
                insert into Mobiletime_Staging (tek_repair_tek_mobiletime,tek_m_status,tek_flag,tek_m_user) values (@tek_repair_tek_mobiletime,@tek_m_status,@tek_flag,@tek_m_user)
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("tek_repair_tek_mobiletime", no),
                 new SqlParameter("tek_m_status", status),
                 new SqlParameter("tek_flag", reserve),
                 new SqlParameter("tek_m_user", user),
            };
            try
            {
                //DataTable dt = AdoSupport.GetDataTable(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
                //if (dt.Rows.Count > 0)
                //{
                //    return dt.Rows[0][0].ToString() == "1";
                //}
                //else
                //{
                //    return false;
                //}
                return AdoSupport.ExecuteNonQuery(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters) > 0;
            }
            catch (Exception ex)
            {
                return false;
                //throw new DaoException(SQL, "寫入預約記錄時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 寫入預約記錄
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool AddReserve(string no, string status,string user)
        {

            string SQL = @"
                insert into Mobiletime_Staging (tek_repair_tek_mobiletime,tek_m_status,tek_m_user) values (@tek_repair_tek_mobiletime,@tek_m_status,@tek_m_user)
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("tek_repair_tek_mobiletime", no),
                 new SqlParameter("tek_m_status", status),
                 new SqlParameter("tek_m_user", user),
            };
            try
            {
                return AdoSupport.ExecuteNonQuery(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 檢查會員登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public string CheckLogin(string account, string pwd)
        {
            string flag = "no user";
            try
            {
                //string ldap_Path = "LDAP://suncolor.com.tw/DC=suncolor, DC=com, DC=tw";
                string ldap_Path = "LDAP://apo.epson.net/OU=tek,OU=ett,DC=apo,DC=Epson,DC=net";
                DirectoryEntry entry = new DirectoryEntry(ldap_Path, account, pwd, AuthenticationTypes.Secure);
                DirectorySearcher searcher = new DirectorySearcher(entry);
                searcher.Filter = "(SAMAccountName=" + account + ")";
                searcher.SearchRoot = entry;
                searcher.PropertiesToLoad.Add("displayName");
                SearchResult result = searcher.FindOne();
                if (result != null)
                {
                    DirectoryEntry user = result.GetDirectoryEntry();
                    if (user.Properties.Contains("displayName"))
                    {
                        flag = user.Properties["displayName"][0].ToString();
                    }
                }
                return flag;
            }
            catch
            {
                //AD登入失敗
                return "no user";
            }
        }
    }

}
