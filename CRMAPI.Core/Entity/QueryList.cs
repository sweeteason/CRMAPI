using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMAPI.Core.Entity
{
    public class QueryList
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 頁數
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }
    }
}
