using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ChuangLiTou.Core.Helpers.Util;
using ChuanglitouP2P.Common;
namespace ChuangLiTou.Core.Helpers
{
    /// <summary>
    /// 把枚举值按照指定的文本显示
    /// <remarks>
    /// </remarks>
    /// </summary>
    /// <example>
    /// [EnumHelper("枚举列表")]
    /// enum MyEnum
    /// {
    ///		[EnumHelper("枚举1")]
    /// 	One = 1, 
    /// 
    ///		[EnumHelper("枚举2")]
    ///		Two, 
    /// 
    ///		[EnumHelper("枚举3")]
    ///		Three
    /// }
    /// EnumHelper.GetEnumText(typeof(MyEnum));
    /// EnumHelper.GetFieldText(MyEnum.Two);
    /// EnumHelper.GetFieldTexts(typeof(MyEnum)); 
    /// </example>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumHelper : Attribute
    {
        private readonly string _enumDisplayText;
        private readonly int _enumRank;
        private FieldInfo _fieldIno;

        /// <summary>
        /// 描述枚举值
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        /// <param name="enumRank">排列顺序</param>
        public EnumHelper(string enumDisplayText, int enumRank)
        {
            _enumDisplayText = enumDisplayText;
            _enumRank = enumRank;
        }

        /// <summary>
        /// 描述枚举值，默认排序为5
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        public EnumHelper(string enumDisplayText)
            : this(enumDisplayText, 5)
        {
        }

        public string EnumDisplayText
        {
            get { return _enumDisplayText; }
        }

        public int EnumRank
        {
            get { return _enumRank; }
        }

        public int EnumValue
        {
            get { return (int)_fieldIno.GetValue(null); }
        }

        public string FieldName
        {
            get { return _fieldIno.Name; }
        }

        #region 对枚举描述属性的解释相关函数 add by xiezhihui 2012/9/25



        private static readonly Hashtable CachedEnum = new Hashtable();


        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string GetEnumText(Type enumType)
        {
            var eds = (EnumHelper[])enumType.GetCustomAttributes(typeof(EnumHelper), false);
            if (eds.Length != 1) return string.Empty;
            return eds[0].EnumDisplayText;
        }
        public static int GetEnumValue(Type enumType)
        {
            var eds = (EnumHelper[])enumType.GetCustomAttributes(typeof(EnumHelper), false);
            if (eds.Length != 1) return -1;
            return eds[0].EnumValue;
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本。
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(object enumValue)
        {
            var descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (var ed in descriptions)
            {
                if (ed._fieldIno.Name == enumValue.ToString()) return ed.EnumDisplayText;
            }
            return string.Empty;
        }
        public static string GetFieldText(object enumType, string text)
        {
            var descriptions = GetFieldTexts(enumType.GetType(), SortType.Default);
            foreach (var ed in descriptions)
            {
                if (ed.EnumValue == ConvertHelper.ParseValue(text, 0)) return ed.EnumDisplayText;
            }
            return string.Empty;
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本，按定义的顺序返回
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumHelper[] GetFieldTexts(Type enumType)
        {
            return GetFieldTexts(enumType, SortType.Default);
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumHelper[] GetFieldTexts(Type enumType, SortType sortType)
        {
            EnumHelper[] descriptions = null;
            //缓存中没有找到，通过反射获得字段的描述信息
            if (enumType.FullName != null && CachedEnum.Contains(enumType.FullName) == false)
            {
                var fields = enumType.GetFields();
                var edAL = new ArrayList();
                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumHelper), false);
                    if (eds.Length != 1) continue;
                    ((EnumHelper)eds[0])._fieldIno = fi;
                    edAL.Add(eds[0]);
                }

                if (enumType.FullName != null)
                    CachedEnum.Add(enumType.FullName, edAL.ToArray(typeof(EnumHelper)));
            }
            if (enumType.FullName != null && CachedEnum.ContainsKey(enumType.FullName))
                descriptions = (EnumHelper[])CachedEnum[enumType.FullName];
            if (descriptions != null && descriptions.Length <= 0)
                throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumHelper");

            //按指定的属性冒泡排序
            for (int m = 0; m < descriptions.Length; m++)
            {
                //默认就不排序了
                if (sortType == SortType.Default) break;

                for (int n = m; n < descriptions.Length; n++)
                {
                    bool swap = false;

                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (
                                String.CompareOrdinal(descriptions[m].EnumDisplayText, descriptions[n].EnumDisplayText) >
                                0) swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].EnumRank > descriptions[n].EnumRank) swap = true;
                            break;
                    }

                    if (swap)
                    {
                        var temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }

            return descriptions;
        }

        #endregion


        /// <summary>
        /// 存储枚举的键值对对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="S"></typeparam>
        public class KVP<T, S>
        {
            public T Key { get; set; }
            public S Value { get; set; }

            public KVP(T k, S v)
            {
                Key = k;
                Value = v; 
            }
        }

        //将枚举转换为泛型类;T为枚举名称
        public static List<KVP<int, string>> EnumToList<T>()
        {
            var list = new List<KVP<int, string>>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                var tmp = new KVP<int, string>(i, GetFieldText(Enum.Parse(typeof(T), i.ToString(CultureInfo.InvariantCulture))));
                tmp.Value = HttpHelper.RemoveHtml(tmp.Value);
                list.Add(tmp);
            }
            return list;
        }
    }
}