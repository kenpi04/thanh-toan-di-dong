﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;
using Telerik.Web.Mvc.UI.Fluent;

namespace Nop.Web.Framework
{
    public static class Extensions
    {
        public static IEnumerable<T> ForCommand<T>(this IEnumerable<T> current, GridCommand command)
        {
            var queryable = current.AsQueryable() as IQueryable;
            if (command.FilterDescriptors.Any())
            {
                queryable = queryable.Where(command.FilterDescriptors.AsEnumerable()).AsQueryable() as IQueryable;
            }

            IList<SortDescriptor> temporarySortDescriptors = new List<SortDescriptor>();

            if (!command.SortDescriptors.Any() && queryable.Provider.IsEntityFrameworkProvider())
            {
                // The Entity Framework provider demands OrderBy before calling Skip.
                SortDescriptor sortDescriptor = new SortDescriptor
                {
                    Member = queryable.ElementType.FirstSortableProperty()
                };
                command.SortDescriptors.Add(sortDescriptor);
                temporarySortDescriptors.Add(sortDescriptor);
            }

            if (command.GroupDescriptors.Any())
            {
                command.GroupDescriptors.Reverse().Each(groupDescriptor =>
                {
                    SortDescriptor sortDescriptor = new SortDescriptor
                    {
                        Member = groupDescriptor.Member,
                        SortDirection = groupDescriptor.SortDirection
                    };

                    command.SortDescriptors.Insert(0, sortDescriptor);
                    temporarySortDescriptors.Add(sortDescriptor);
                });
            }

            if (command.SortDescriptors.Any())
            {
                queryable = queryable.Sort(command.SortDescriptors);
            }

            return queryable as IQueryable<T>;
        }

        public static IEnumerable<T> PagedForCommand<T>(this IEnumerable<T> current, GridCommand command)
        {
            return current.Skip((command.Page - 1) * command.PageSize).Take(command.PageSize);
        }

        public static bool IsEntityFrameworkProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName == "System.Data.Objects.ELinq.ObjectQueryProvider";
        }

        public static bool IsLinqToObjectsProvider(this IQueryProvider provider)
        {
            return provider.GetType().FullName.Contains("EnumerableQuery");
        }

        public static string FirstSortableProperty(this Type type)
        {
            PropertyInfo firstSortableProperty = type.GetProperties().FirstOrDefault(property => property.PropertyType.IsPredefinedType());

            if (firstSortableProperty == null)
            {
                throw new NotSupportedException("Cannot find property to sort by.");
            }

            return firstSortableProperty.Name;
        }

        internal static bool IsPredefinedType(this Type type)
        {
            return PredefinedTypes.Any(t => t == type);
        }

        public static readonly Type[] PredefinedTypes = {
            typeof(Object),
            typeof(Boolean),
            typeof(Char),
            typeof(String),
            typeof(SByte),
            typeof(Byte),
            typeof(Int16),
            typeof(UInt16),
            typeof(Int32),
            typeof(UInt32),
            typeof(Int64),
            typeof(UInt64),
            typeof(Single),
            typeof(Double),
            typeof(Decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Convert)
        };

        public static GridBoundColumnBuilder<T> Centered<T>(this GridBoundColumnBuilder<T> columnBuilder) where T : class
        {
            return columnBuilder.HtmlAttributes(new { align = "center" })
                            .HeaderHtmlAttributes(new { style = "text-align:center;" });
        }

        public static GridTemplateColumnBuilder<T> Centered<T>(this GridTemplateColumnBuilder<T> columnBuilder) where T : class
        {
            return columnBuilder.HtmlAttributes(new { align = "center" })
                            .HeaderHtmlAttributes(new { style = "text-align:center;" });
        }

        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, bool markCurrentAsSelected = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("An Enumeration type is required.", "enumObj");

            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         select new { ID = Convert.ToInt32(enumValue), Name = enumValue.GetLocalizedEnum(localizationService, workContext) };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }

        public static string GetValueFromAppliedFilter(this IFilterDescriptor filter, string valueName, FilterOperator? filterOperator = null)
        {
            if (filter is CompositeFilterDescriptor)
            {
                foreach (IFilterDescriptor childFilter in ((CompositeFilterDescriptor)filter).FilterDescriptors)
                {
                    var val1 = GetValueFromAppliedFilter(childFilter, valueName, filterOperator);
                    if (!String.IsNullOrEmpty(val1))
                        return val1;
                }
            }
            else
            {
                var filterDescriptor = (FilterDescriptor)filter;
                if (filterDescriptor != null &&
                    filterDescriptor.Member.Equals(valueName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!filterOperator.HasValue || filterDescriptor.Operator == filterOperator.Value)
                        return Convert.ToString(filterDescriptor.Value);
                }
            }

            return "";
        }

        public static string GetValueFromAppliedFilters(this IList<IFilterDescriptor> filters, string valueName, FilterOperator? filterOperator = null)
        {
            foreach (var filter in filters)
            {
                var val1 = GetValueFromAppliedFilter(filter, valueName, filterOperator);
                if (!String.IsNullOrEmpty(val1))
                    return val1;
            }
            return "";
        }

        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(this DateTime source)
        {
            return RelativeFormat(source, string.Empty);
        }

        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(this DateTime source, string defaultFormat)
        {
            return RelativeFormat(source, false, defaultFormat);
        }

        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <param name="convertToUserTime">A value indicating whether we should convet DateTime instance to user local time (in case relative formatting is not applied)</param>
        /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(this DateTime source,
            bool convertToUserTime, string defaultFormat)
        {
            string result = "";

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - source.Ticks);
            double delta = ts.TotalSeconds;

            if (delta > 0)
            {
                if (delta < 60) // 60 (seconds)
                {
                    result = ts.Seconds == 1 ? "một giây trước" : ts.Seconds + " giây trước";
                }
                else if (delta < 120) //2 (minutes) * 60 (seconds)
                {
                    result = "một phút trước";
                }
                else if (delta < 2700) // 45 (minutes) * 60 (seconds)
                {
                    result = ts.Minutes + " phút trước";
                }
                else if (delta < 5400) // 90 (minutes) * 60 (seconds)
                {
                    result = "một giờ trước";
                }
                else if (delta < 86400) // 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int hours = ts.Hours;
                    if (hours == 1)
                        hours = 2;
                    result = hours + " giờ trước";
                }
                else if (delta < 172800) // 48 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = "hôm qua";
                }
                else if (delta < 2592000) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    result = ts.Days + " ngày trước";
                }
                else if (delta < 31104000) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    result = months <= 1 ? "một tháng trước" : months + " tháng trước";
                }
                else
                {
                    int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    result = years <= 1 ? "một năm trước" : years + " năm trước";
                }
            }
            else
            {
                DateTime tmp1 = source;
                if (convertToUserTime)
                {
                    tmp1 = EngineContext.Current.Resolve<IDateTimeHelper>().ConvertToUserTime(tmp1, DateTimeKind.Utc);
                }
                //default formatting
                if (!String.IsNullOrEmpty(defaultFormat))
                {
                    result = tmp1.ToString(defaultFormat);
                }
                else
                {
                    result = tmp1.ToString();
                }
            }
            return result;
        }
        /// <summary>
        /// Trim String by length
        /// </summary>
        /// <param name="s">String</param>
        /// <param name="length">length trim include space char</param>
        /// <returns>String</returns>
        public static string TrimString(this string s, int length)
        {
            try
            {
                if (String.IsNullOrEmpty(s))
                    return null;
                if (s.Length <= length)
                    return s;
                var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words[0].Length > length)
                    throw new ArgumentException("Từ đầu tiên dài hơn chuỗi cần cắt");
                var sb = new StringBuilder();
                foreach (var word in words)
                {
                    if ((sb + word).Length > length)
                        return string.Format("{0}...", sb.ToString().TrimEnd(' '));
                    sb.Append(word + " ");
                }
                return string.Format("{0}...", sb.ToString().TrimEnd(' '));
            }
            catch
            {
                return "";
            }
        }
        private static readonly string[] VietnameseSigns = new string[]{
        "aAeEoOuUiIdDyY",
        "áàạảãâấầậẩẫăắằặẳẵ",
        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
        "éèẹẻẽêếềệểễ",
        "ÉÈẸẺẼÊẾỀỆỂỄ",
        "óòọỏõôốồộổỗơớờợởỡ",
        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
        "úùụủũưứừựửữ",
        "ÚÙỤỦŨƯỨỪỰỬỮ",
        "íìịỉĩ",
        "ÍÌỊỈĨ",
        "đ",
        "Đ",
        "ýỳỵỷỹ",
        "ÝỲỴỶỸ"
        };
        public static string RemoveSign4VietnameseString(this string str)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            //  str = str.Replace(".", "");
            return str;
        }

        /// <summary>
        /// Get danh sach gia ban hoac cho thue
        /// </summary>
        /// <param name="isForRent">la cho thue</param>
        /// <returns>Prices collections</returns>
        public static List<SelectListItem> GetPrice(bool isForRent = false)
        {
            List<SelectListItem> listItems = null;
            if (!isForRent)//ban nha
            {
                listItems = new List<SelectListItem>(){
                    new SelectListItem{Value = "0-0", Text ="Chọn mức giá"},
                    new SelectListItem{Value = "0-1000", Text ="Dưới 1 tỷ"},
                    new SelectListItem{Value = "1000-1500", Text ="1 tỷ ~ 1.5 tỷ"},
                    new SelectListItem{Value = "1500-2000", Text ="1.5 tỷ ~ 2 tỷ"},
                    new SelectListItem{Value = "2000-2500", Text ="2 tỷ ~ 2.5 tỷ"},
                    new SelectListItem{Value = "2500-3000", Text ="2.5 tỷ ~ 3 tỷ"},
                    new SelectListItem{Value = "3000-0", Text ="Trên 3 tỷ"},
                };
            }
            else //cho thue
            {
                listItems = new List<SelectListItem>(){
                    new SelectListItem{Value = "0-0", Text ="Chọn mức giá"},
                    new SelectListItem{Value = "0-1", Text ="Dưới 1 triệu"},
                    new SelectListItem{Value = "1-2", Text ="1 triệu ~ 2 triệu"},
                    new SelectListItem{Value = "2-3", Text ="2 triệu ~ 3 triệu"},
                    new SelectListItem{Value = "3-5", Text ="3 triệu ~ 5 triệu"},
                    new SelectListItem{Value = "5-10", Text ="5 triệu ~ 10 triệu"},
                    new SelectListItem{Value = "10-20", Text ="10 triệu ~ 20 triệu"},
                    new SelectListItem{Value = "20-30", Text ="20 triệu ~ 30 triệu"},
                    new SelectListItem{Value = "30-0", Text ="Trên 30 triệu"},
                };
            }
            return listItems;
        }
        public static List<SelectListItem> GetArea()
        {
            return new List<SelectListItem>(){
                    new SelectListItem{Value = "0-0", Text ="Chọn diện tích"},
                    new SelectListItem{Value = "0-40", Text ="Dưới 40 m2"},
                    new SelectListItem{Value = "40-70", Text ="40 m2 ~ 70 m2"},
                    new SelectListItem{Value = "70-100", Text ="70 m2 ~ 100 m2"},
                    new SelectListItem{Value = "100-150", Text ="100 m2 ~ 150 m2"},
                    new SelectListItem{Value = "150-250", Text ="150 m2 ~ 250 m2"},
                    new SelectListItem{Value = "250-500", Text ="250 m2 ~ 500 m2"},
                    new SelectListItem{Value = "500-1000", Text ="500 m2 ~ 1000 m2"},
                    new SelectListItem{Value = "1000-0", Text ="Trên 1000 m2"},
                };
        }
        public static string ReturnPriceString(decimal price, string symbol)
        {
            price = price / 1000000;
            if (Math.Floor(price / 1000) > 0)
            {
                if (price % 1000 != 0)
                {
                    return ((int)Math.Floor(price / 1000)).ToString() + " tỉ " + ((int)(price % 1000)).ToString() + " triệu " + symbol;
                }
                else
                {
                    return ((int)Math.Floor(price / 1000)).ToString() + " tỉ " + symbol;
                }
            }
            else
            {
                return ((int)price).ToString() + " triệu " + symbol;
            }
        }
    }
}
