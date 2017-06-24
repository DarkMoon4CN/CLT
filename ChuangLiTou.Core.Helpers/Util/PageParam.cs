#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>PageParam ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/26 10:48:07</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

namespace ChuangLiTou.Core.Helpers.Util
{
    /// <summary>
    /// 分页参数的设定
    /// </summary>
    public class PageParam
    {


        public PageParam()
            : this(1, 10)
        {
            Sort = string.Empty;
        }

        public PageParam(int pageCurrent)
            : this(pageCurrent, 10)
        {

        }

        public PageParam(int pageCurrent, int pageSize)
        {
            PageCurrent = pageCurrent;
            PageSize = pageSize;
        }


        /// <summary>
        /// 获得或设置页面的序数,从1开始。
        /// </summary>
        public int PageCurrent { get; set; }

        /// <summary>
        /// 获得或设置返回的记录的起始记录数，从1开始
        /// </summary>
        public int StartIndex
        {
            get
            {
                return (PageCurrent - 1) * PageSize + 1;
            }
        }

        /// <summary>
        /// 获得或设置返回的记录的最大记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 获得或设置符合条件的总记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 获得或设置符合条件的总页数
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 其他参数 xiezhihui add 2012-10-10
        /// </summary>
        public object Other { get; set; }
    }
}
