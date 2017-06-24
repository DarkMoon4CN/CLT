using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChuangLiTou.Core.Entities.wdzg
{
    public class ResponseWdzgBase
    {
        [JsonProperty("return")]
        public int code { get; set; }
        public object data { get; set; }
    }
}
