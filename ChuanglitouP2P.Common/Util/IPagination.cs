#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>IPagination ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/26 10:45:34</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System.Collections.Generic;

namespace ChuanglitouP2P.Common.Util
{
    public interface IPagination<T> : IEnumerable<T>, IPagination
    {
        T this[int index] { get; }
    }

    public interface IPagination
    {
        /// <summary>
        /// Gets the current page number.
        /// </summary>
        /// <value>The page number.</value>
        int CurrentPage { get; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        int PageSize { get; }

        /// <summary> 
        /// Gets the total page count.
        /// </summary>
        /// <value>The total page count.</value>
        int PageCount { get; }

        /// <summary>
        /// Gets the start index of the item.
        /// </summary>
        /// <value>The start index of the item.</value>
        int StartIndex { get; }

        /// <summary>
        /// Gets the total item count.
        /// </summary>
        /// <value>The total item count.</value>
        int RecordCount { get; }

        /// <summary>
        /// Determine if there are items.
        /// </summary>
        bool HasItems { get; }

        /// <summary>
        /// Determine if there are not items.
        /// </summary>
        bool HasNotItems { get; }

        /// <summary>
        /// Determine if previous page is abailable.
        /// </summary>
        bool HasPrevPage { get; }

        /// <summary>
        /// Determine if next page is available.
        /// </summary>
        bool HasNextPage { get; }

        int Count { get; }

        object Other { get; }

    }
}
