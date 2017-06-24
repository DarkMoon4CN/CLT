namespace ChuangLiTou.Core.Entities.Response
{
    public class BasePage<T>
    {
        public int pageCount { get; set; }
        public int recordCount { get; set; }
        public T rows { get; set; }
    }
}
