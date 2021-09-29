using System.Globalization;
using System.Text.RegularExpressions;

namespace JustSellIt.Web.Helpers
{
    public static class StringHelper
    {
        public static string CapitalizeFirstLetter(string str)
        {
            TextInfo textInfo = new CultureInfo("pl-PL", false).TextInfo;

            return Regex.Replace(textInfo.ToTitleCase(str), " {2,}", " ").TrimStart().TrimEnd();
        }
    }
}
