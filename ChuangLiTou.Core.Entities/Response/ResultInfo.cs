using Newtonsoft.Json;
namespace ChuangLiTou.Core.Entities.Response
{

    /// <summary>
    /// 响应实体类
    /// </summary>
    /// <typeparam name="T">响应体对象</typeparam>
    public class ResultInfo<T>
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 响应体(泛型)
        /// </summary>
        public T body { get; set; }
        public ResultInfo()
        { 
        }
        public ResultInfo(string code)
        {
            this.code = code;
        } 
    }
}
