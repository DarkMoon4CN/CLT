#region 描述信息
/*-------------------------------------------------------------------------
 * <copyright>CollectionExtensions ©2012 XieZhihui</copyright>
 * <author>XieZhihui<author>
 *<createdOn>2012/12/27 16:04:47</createdOn>
 * <ver>v1.0</ver>
 *  -------------------------------------------------------------------------*/
#endregion

using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace ChuangLiTou.Core.Helpers.Util
{ 
  
    public static class CollectionExtensions
    {
        public static MvcHtmlString ToHtml(this string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
                return MvcHtmlString.Empty;
            else
                return MvcHtmlString.Create(inputText);
        }
        public static Pagination<T> AsPagination<T>(this IEnumerable<T> collection, PageParam pageParam)
        {
            return new Pagination<T>(
                collection,
                pageParam.StartIndex,
                pageParam.PageSize,
                pageParam.RecordCount,
                pageParam.PageCount,
                pageParam.PageCurrent,
                pageParam.Other
            );
        }


       
    }

    public static class XmlDocumentExtensions
    {
        public static XDocument ToXDocument(this XmlDocument document)
        {
            return document.ToXDocument(LoadOptions.None);
        }

        public static XDocument ToXDocument(this XmlDocument document, LoadOptions options)
        {
            using (var reader = new XmlNodeReader(document))
            {
                return XDocument.Load(reader, options);
            }
        }
    }
}
