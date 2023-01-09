using System;
using System.Text;

namespace TDL.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static bool ContainInvariant(this string text, string keyword)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
		/// GetUserNameFromMailAddress
		/// </summary>
		public static string GetUserNameFromMailAddress(this string input)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.User;
            }
            catch
            {
                return input;
            }
        }

        public static bool EqualsInvariant(this string s, string value)
        {
            return !string.IsNullOrEmpty(s) &&
                s.Equals(value, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsEmoty(this string input)
        {
            return input.Equals("[]");
        }

        public static string DecodeBase64(this string value)
        {
            var valueBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}
