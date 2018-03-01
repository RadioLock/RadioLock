using System;
using System.Text;

namespace LLock
{
    public class Helper
    {
        public static char[] FromHex(string hex)
        {
            char[] raw = new char[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = (char)Convert.ToInt32(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        public static char[] CharFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new char[1];
            return ASCIIEncoding.ASCII.GetChars(ASCIIEncoding.ASCII.GetBytes(str));
        }

        public static DateTime StringToDate(string str)
        {
            int year = int.Parse(str.Substring(0, 4));
            int month = int.Parse(str.Substring(4, 2));
            int day = int.Parse(str.Substring(6, 2));
            int hour = int.Parse(str.Substring(8, 2));
            int min = int.Parse(str.Substring(10, 2));
            return new DateTime(year, month, day, hour, min, 0);
        }

        public static string CharToString(char[] arr)
        {
            string str = new string(arr);
            int pos = str.IndexOf('\0');
            if (pos >= 0)
                str = str.Substring(0, pos);
            return str;
        }
    }
}