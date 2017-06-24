using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChuangLiTou.Core.Entities.Response
{
    public class ResultInfoExtensions<T> : ResultInfo<T>
    {
        public ResultInfoExtensions() { }
        public ResultInfoExtensions(string code) : base(code) { }

        public DateTime ServerTime { get; set; }
    }
}
