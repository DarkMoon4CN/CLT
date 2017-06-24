#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>PaginationHelper ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/26 10:43:46</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ChuanglitouP2P.Common.Util
{

    [Serializable]
    public sealed class Pagination<T> : IPagination<T>
    {
        private readonly List<T> _collection;
          
        /// <param name="collection">The collection.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="requestedPageSize">The requested count.</param>
        /// <param name="recordCount">The total count.</param>
        /// <param name="pageCount"> </param>
        /// <param name="currentPage"> </param>
        /// <param name="other"> </param>
        public Pagination(IEnumerable<T> collection, int startIndex, int requestedPageSize, int recordCount, int pageCount, int currentPage, object other)
        {
            _collection = new List<T>(collection);
            Count = _collection.Count();
            StartIndex = startIndex;
            PageSize = requestedPageSize;
            RecordCount = recordCount;
            PageCount = pageCount;
            CurrentPage = currentPage;
            Other = other;
        }

        /// <summary>
        /// Gets the total item count.
        /// </summary>
        /// <value>The total item count.</value>
        public int RecordCount { get; protected internal set; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize { get; protected internal set; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>The start index.</value>
        public int StartIndex { get; protected internal set; }

        /// <summary>
        /// Gets the current page number which start from 1.
        /// </summary>
        /// <value>The page number.</value>
        public int CurrentPage { get; protected internal set; }

        /// <summary>
        /// Gets the total page count.
        /// </summary>
        /// <value>The total page count.</value>
        public int PageCount { get; protected internal set; }

        /// <summary>
        /// Determine if next page is available.
        /// </summary>
        public bool HasNextPage
        {
            get { return CurrentPage < PageCount; }
        }


        public int Count { get; private set; }
        public object Other { get; protected internal set; }

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public bool HasItems
        {
            get { return _collection.Any(); }
        }

        public bool HasNotItems
        {
            get { return !HasItems; }
        }

        /// <summary>
        /// Determine if previous page is abailable.
        /// </summary>
        public bool HasPrevPage
        {
            get { return CurrentPage > 1; }
        }

        #endregion
        public static Pagination<T> Empty
        {
            get { return new Pagination<T>(new List<T>(0), 1, 10, 0, 0, 1, null); }
        }

        #region Implementation of IPagination<T>

        public T this[int index]
        {
            get
            {
                if (index >= Count) throw new ArgumentOutOfRangeException("index");
                return _collection.ElementAt(index);
            }
        }

        #endregion

        public List<T> ToList()
        {
            return _collection;
        }


    }
}
