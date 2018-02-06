using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMAPI.Core.Entity
{
    public class tek_repair
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 維修狀態
        /// </summary>
        public string tek_repairstatus { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string tek_account { get; set; }

        /// <summary>
        /// 收件單號
        /// </summary>
        public string tek_workorder_tek_repair { get; set; }

        /// <summary>
        /// 維修單號
        /// </summary>
        public string tek_name { get; set; }

        /// <summary>
        /// 收件日期
        /// </summary>
        public string tek_recipient_date
        {
            get { return Convert.ToDateTime(_tek_recipient_date).ToString("yyyy/MM/dd HH:mm:ss"); }
            set { _tek_recipient_date = value; }
        }
        private string _tek_recipient_date;

        /// <summary>
        /// 服務方式
        /// </summary>
        public string tek_service { get; set; }

        /// <summary>
        /// 服務項目
        /// </summary>
        public string tek_service_item { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        public string tek_product { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        public string tek_serial_no { get; set; }

        /// <summary>
        /// 保固狀態
        /// </summary>
        public string tek_warrenty { get; set; }

        /// <summary>
        /// 連絡人
        /// </summary>
        public string tek_contact { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string tek_m_status { get; set; }

        /// <summary>
        /// tek_flag
        /// </summary>
        public string tek_flag { get; set; }

        /// <summary>
        /// 客戶電話
        /// </summary>
        public string tek_telephone { get; set; }

        /// <summary>
        /// 手機
        /// </summary>
        public string tek_mobile { get; set; }

        /// <summary>
        /// 客戶地址
        /// </summary>
        public string tek_clientadd { get; set; }

        /// <summary>
        /// tek_m_user
        /// </summary>
        public string tek_m_user { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// tek_remark
        /// </summary>
        public string tek_remark { get; set; }

        /// <summary>
        /// tek_note
        /// </summary>
        public string tek_note { get; set; }

        /// <summary>
        /// tek_errordescip
        /// </summary>
        public string tek_errordescip { get; set; }

    }
}
