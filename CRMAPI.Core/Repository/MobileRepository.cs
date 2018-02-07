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
            //狀態5的不傳
            string SQL = @"
                with tmp as (
                    select a.*,b.tek_m_status,
                    (select count(*) from Mobiletime_Staging c where c.tek_repair_tek_mobiletime = a.tek_name and tek_m_status = '5') as has5,
                    row_number() over (partition by tek_name,tek_m_status order by tek_recipient_date desc) as con from Repair_Staging a left outer join Mobiletime_Staging b on a.tek_name = tek_repair_tek_mobiletime where isnull(tek_m_status,'') <> '5'
                )
                select top (@PageSize) * 
                from (
                    select *,row_number() over (order by tek_recipient_date desc) as rownumber from tmp where con = 1 and has5 = 0 
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
        /// 取得留言列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<tek_onsitenote> GetOnSiteNoteList(QueryList query)
        {
            string SQL = @"
                select top (@PageSize) * 
                from (
                    select *,row_number() over (order by id desc) as rownumber from Onsitenote_Staging
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
                return AdoSupport.GetEntityList<tek_onsitenote>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "取得留言列表時發生錯誤", ex);
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
                with tmp as (
                    select a.*,b.tek_m_status,
                    (select count(*) from Mobiletime_Staging c where c.tek_repair_tek_mobiletime = a.tek_name and tek_m_status = '5') as has5,
                    row_number() over (partition by tek_name,tek_m_status order by tek_recipient_date desc) as con from Repair_Staging a left outer join Mobiletime_Staging b on a.tek_name = tek_repair_tek_mobiletime where isnull(tek_m_status,'') <> '5'
                )

                select top (@PageSize) * 
                from (
                    select *,row_number() over (order by tek_recipient_date desc) as rownumber from tmp where tek_m_user = @account and con = 1 and has5 = 0
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
        /// 寫入預約記錄
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool AddGPS(string no, string status, string user,string gps)
        {

            string SQL = @"
                insert into Mobiletime_Staging (tek_repair_tek_mobiletime,tek_m_status,tek_m_user,tek_GPS) values (@tek_repair_tek_mobiletime,@tek_m_status,@tek_m_user,@tek_GPS)
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("tek_repair_tek_mobiletime", no),
                 new SqlParameter("tek_m_status", status),
                 new SqlParameter("tek_m_user", user),
                 new SqlParameter("tek_GPS", gps),
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

        /// <summary>
        /// CRM留言
        /// </summary>
        /// <param name="tek_repair_no"></param>
        /// <returns></returns>
        public tek_onsitenote GetOnSiteNote(string tek_repair_no)
        {
            string SQL = @"
                select top 1 * from Onsitenote_Staging where tek_repair_no = @id
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("id", tek_repair_no),
            };
            try
            {
                return AdoSupport.GetEntity<tek_onsitenote>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依維修單號取得留言時發生錯誤", ex);
            }
        }

        /// <summary>
        /// CRM留言
        /// </summary>
        /// <param name="tek_repair_no"></param>
        /// <returns></returns>
        public List<tek_onsitenote> GetOnSiteNoteList(string tek_repair_no)
        {
            string SQL = @"
                select * from Onsitenote_Staging where tek_repair_no = @id order by id desc
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("id", tek_repair_no),
            };
            try
            {
                return AdoSupport.GetEntityList<tek_onsitenote>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依維修單號取得留言時發生錯誤", ex);
            }
        }


        /// <summary>
        /// CRM留言
        /// </summary>
        /// <param name="tek_repair_no"></param>
        /// <returns></returns>
        public List<tek_onsitenote> GetOnSiteNoteByUser(string user)
        {
            string SQL = @"
                select * from Onsitenote_Staging where tek_m_user = @id order by id desc
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("id", user),
            };
            try
            {
                return AdoSupport.GetEntityList<tek_onsitenote>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依帳號取得留言時發生錯誤", ex);
            }
        }


        /// <summary>
        /// 依照編號取得維修資料
        /// </summary>
        /// <param name="tek_repair_no"></param>
        /// <returns></returns>
        public tek_repair GetRepairById(string tek_name)
        {
            string SQL = @"
                with tmp as (
                    select a.*,b.tek_m_status,
                    (select count(*) from Mobiletime_Staging c where c.tek_repair_tek_mobiletime = a.tek_name and tek_m_status = '5') as has5,
                    row_number() over (partition by tek_name,tek_m_status order by tek_recipient_date desc) as con from Repair_Staging a left outer join Mobiletime_Staging b on a.tek_name = tek_repair_tek_mobiletime where isnull(tek_m_status,'') <> '5'
                )
                select top 1 * from tmp where tek_name = @tek_name and con = 1 and has5 = 0
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("tek_name", tek_name),
            };
            try
            {
                return AdoSupport.GetEntity<tek_repair>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);

            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "依照編號取得維修資料時發生錯誤", ex);
            }
        }


        /// <summary>
        /// 維修單變更狀態
        /// </summary>
        /// <param name="tek_name">維修單號</param>
        /// <param name="status">狀態</param>
        /// <param name="Log">Log</param>
        public void UpdateRepairStatus(string tek_name, string status, string Log)
        {
            string SQL = @"
                update Repair_Staging set status = @status, [log] = @log 
                where id in (select top 1 id from Repair_Staging where tek_name = @id and status = 'Waiting' order by id)
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("id", tek_name),
                 new SqlParameter("status", status),
                 new SqlParameter("Log", Log),
            };
            try
            {
                 AdoSupport.ExecuteNonQuery(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "維修單變更狀態時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 留言變更狀態
        /// </summary>
        /// <param name="tek_name">維修單號</param>
        /// <param name="status">狀態</param>
        /// <param name="Log">Log</param>
        public void UpdateOnsitenoteStatus(string tek_repair_no, string status, string Log)
        {
            string SQL = @"
                update Onsitenote_Staging set status = @status, [log] = @log 
                where id in (select top 1 id from Onsitenote_Staging where tek_repair_no = @id and status = 'Waiting' order by id)
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("id", tek_repair_no),
                 new SqlParameter("status", status),
                 new SqlParameter("Log", Log),
            };
            try
            {
                AdoSupport.ExecuteNonQuery(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "留言變更狀態時發生錯誤", ex);
            }
        }

        /// <summary>
        /// 更新 token
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool SetUserToken(string user, string token)
        {
            bool flag = true;
            string SQL = @"
                if not exists(select * from MToken_Staging where tek_m_user = @user)
                   begin
                      insert into MToken_Staging (tek_m_user,tek_m_user_token) values (@user, @token)
                   end
                else 
                   begin
                      update MToken_Staging set tek_m_user = @user, tek_m_user_token = @token where tek_m_user = @user
                   end 
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("token", token),
                 new SqlParameter("user", user),
            };
            try
            {
                flag = AdoSupport.ExecuteNonQuery(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters) > 0 ? true : false;
                return flag;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 取得 user token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GetToken(string user)
        {
            string SQL = @"
                select * from MToken_Staging where tek_m_user = @user
            ";
            var parameters = new SqlParameter[]
            {
                 new SqlParameter("user", user),
            };
            try
            {
                MToken_Staging obj = AdoSupport.GetEntity<MToken_Staging>(System.Data.CommandType.Text, SQL, sqlConnectionString, parameters);
                if(obj == null)
                {
                    return "";
                }
                else
                {
                    return obj.tek_m_user_token;
                }
            }
            catch (Exception ex)
            {
                throw new DaoException(SQL, "取得 user token時發生錯誤", ex);
            }
        }
        
    }

}
