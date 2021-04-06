using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace komm_rein.ui.web.Pages
{
    public static class QueryStringHelper
    {
        public static NameValueCollection QueryString(this NavigationManager navigationManager)
        {
            return HttpUtility.ParseQueryString(new Uri(navigationManager.Uri).Query);
        }

        public static string QueryString(this NavigationManager navigationManager, string key)
        {
            return navigationManager.QueryString()[key];
        }

        public static bool TryGetQueryString<T>(this NavigationManager navManager, string key, out T value)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valuesFromQueryString))
            {
                var valueFromQueryString = string.Join(", ", valuesFromQueryString.ToArray());

                if (typeof(T) == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
                {
                    value = (T)(object)valueAsInt;
                    return true;
                }

                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)valueFromQueryString.ToString();
                    return true;
                }

                if (typeof(T) == typeof(DateTime) && DateTime.TryParse(valueFromQueryString, out var valueAsDatetime))
                {
                    value = (T)(object)valueAsDatetime;
                    return true;
                }

                if (typeof(T) == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
                {
                    value = (T)(object)valueAsDecimal;
                    return true;
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Wraps NavigationManager.NavigateTo and appends one query paparmeter, ONLY if value is not NULL or Empty
        /// </summary>
        /// <param name="navigationManager"></param>
        /// <param name="uri"></param>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public static void NavigateTo(this NavigationManager navigationManager, string uri, string parameter, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                uri = QueryHelpers.AddQueryString(uri, parameter, value);
            }

            navigationManager.NavigateTo(uri);
        }
    }
}
