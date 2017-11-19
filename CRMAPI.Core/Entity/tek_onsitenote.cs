using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMAPI.Core.Entity
{
    public class tek_onsitenote
    {
        /// <summary>
        /// 流水號
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string tek_serviceaccount { get; set; }

        /// <summary>
        /// 收件單號
        /// </summary>
        public string tek_workorder_no { get; set; }

        /// <summary>
        /// 維修單號
        /// </summary>
        public string tek_repair_no { get; set; }

        /// <summary>
        /// 留言類別
        /// </summary>
        public string tek_notetype { get; set; }

        /// <summary>
        /// 客戶留言
        /// </summary>
        public string tek_note { get; set; }
    }
}
