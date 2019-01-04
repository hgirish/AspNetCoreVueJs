using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AspNetCoreVueJs.Web.Infrastructure
{
    public static class StringExtensions
    {
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveDiacritics().ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            str = Regex.Replace(str, @"\s+", " ").Trim();

            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-");

            return str;

        }

        private static string RemoveDiacritics(this string text)
        {
            var str = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());



            return str.Normalize(NormalizationForm.FormC);
        }
    }
}
