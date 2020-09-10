using System;

namespace Net.FreeLibrary.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object o)
        {
            return o == null;
        }

        public static bool IsNullOrDbNull(this object obj)
        {
            return (null == obj || obj == DBNull.Value);
        }

        public static string ToStr(this object obj)
        {
            return obj.IsNullOrDbNull() == true ? string.Empty : obj.ToString();
        }

        public static int ToInt(this object obj)
        {
            try
            {
                return obj.ToStr().Str2Int();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int Str2Int(this string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int Char2Int(this char ch)
        {
            try
            {
                return Convert.ToInt32(ch);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool IsNullOrEmpty(this string str)
        {
            if (str == null)
            {
                return true;
            }
            else
            {
                return str.Length == 0;
            }
        }

        public static bool IsNullOrSpace(this string str)
        {
            if (str == null)
            {
                return true;
            }
            else
            {
                return str.Trim().Length == 0;
            }
        }

        public static int FirstIndexOf(this string str, char ch)
        {
            try
            {
                if (str.IsNullOrEmpty())
                    return -1;

                if (ch.IsNull())
                    return -1;

                char[] chs = str.ToCharArray();
                int _index = -1;
                for (int charCounter = 0; charCounter < chs.Length; charCounter++)
                {
                    if (string.Format("{0}", ch).Equals(string.Format("{0}", chs[charCounter])))
                    {
                        _index = charCounter;
                        break;
                    }
                }
                return _index;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
