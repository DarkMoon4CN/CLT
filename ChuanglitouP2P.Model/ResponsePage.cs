using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.Model
{
    public class ResponsePage
    {
        /// <summary>
        /// Gets or sets the dt.
        /// </summary>
        /// <value>The dt.</value>
        public DataTable dataBody { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int pageSize { get; set; }

        /// <summary>
        /// Gets or sets the record count.
        /// </summary>
        /// <value>The record count.</value>
        public int recordCount { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>The page count.</value>
        public int pageCount { get; set; }
        public int currentCount { get; set; }



    }
}
