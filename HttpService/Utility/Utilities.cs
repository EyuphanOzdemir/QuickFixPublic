using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Utility
{
    public static class Utilities
    {
        public static string GetQueryString<T>(T obj)
        {
            var queryStringBuilder = new StringBuilder();

            foreach (var property in typeof(T).GetProperties())
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    if (queryStringBuilder.Length > 0)
                        queryStringBuilder.Append("&");

                    queryStringBuilder.Append(Uri.EscapeDataString(property.Name));
                    queryStringBuilder.Append("=");
                    queryStringBuilder.Append(Uri.EscapeDataString(value.ToString()));
                }
            }

            return queryStringBuilder.ToString();
        }
    }
}
