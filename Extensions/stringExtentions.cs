using System;
using System.Text.RegularExpressions;

namespace WJ_HustleForProfit_003.Extensions
{
    public static class stringExtentions
    {
        public static int WordCount(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return 0;

            string[] words = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        public static int CharCount(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            // Return the length of the string
            return str.Length;
        }

        public static int CharCountNoHTML(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            // Remove HTML tags using a regular expression
            string noHtmlStr = Regex.Replace(str, "<.*?>", string.Empty);

            // Return the length of the string without HTML tags
            return noHtmlStr.Length;
        }

        public static int CharCountNoSpaces(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            // Remove spaces from the string
            string noSpaceStr = str.Replace(" ", string.Empty);

            // Return the length of the string without spaces
            return noSpaceStr.Length;
        }
    }
}
