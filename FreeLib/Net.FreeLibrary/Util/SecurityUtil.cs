using System;

namespace Net.FreeLibrary.Util
{
    public class SecurityUtil
    {
        internal static string EncodeString(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data) == false)
                {
                    string encodedData = Encode(data);
                    int shiftCount = 3;
                    string shifted = ShiftAString(encodedData, shiftCount);
                    return Encode(string.Concat(shifted, (char)shiftCount));
                }

                return string.Empty;

            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static string DecodeString(string data)
        {
            try
            {
                if (string.IsNullOrEmpty(data) == false)
                {
                    string firstDecode = Decode(data);
                    /*
                    int unshiftCount = firstDecode[firstDecode.Length - 1].Char2Int();
                    firstDecode = firstDecode.Substring(0, firstDecode.Length - 1);
                    firstDecode = UnShiftString(firstDecode, unshiftCount);
                    string lastDecode = Decode(firstDecode);
                    */
                    firstDecode = UnShiftString(firstDecode, 3);
                    string lastDecode = Decode(firstDecode);
                    return lastDecode;
                }

                return string.Empty;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] toDecodeByte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(toDecodeByte, 0, toDecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(toDecodeByte, 0, toDecodeByte.Length, decodedChar, 0);
                string result = new string(decodedChar);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string Encode(string data)
        {
            try
            {
                Byte[] encDataByte = new Byte[data.Length];
                encDataByte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = System.Convert.ToBase64String(encDataByte);
                return encodedData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static string ShiftAString(string shifted, int shiftCount)
        {
            string result = "";
            try
            {
                if (string.IsNullOrEmpty(shifted) == false)
                {
                    int lenStr = shifted.Length;
                    // StringBuilder strBuilder = new StringBuilder();
                    for (int lenCounter = 0; lenCounter < lenStr; lenCounter++)
                    {
                        result = string.Concat(result, shifted[(lenCounter + shiftCount) % lenStr]);
                        //strBuilder.Append(shifted[(lenCounter + shiftCount) % lenStr]);
                    }
                    //return strBuilder.ToString();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        internal static string UnShiftString(string unshifted, int unshiftCount)
        {
            string result = "";
            try
            {
                if (string.IsNullOrEmpty(unshifted) == false)
                {
                    int lenStr = unshifted.Length;
                    //StringBuilder strBuilder = new StringBuilder();
                    for (int lenCounter = 0; lenCounter < lenStr; lenCounter++)
                    {
                        result = string.Concat(result, unshifted[(lenCounter + lenStr - unshiftCount) % lenStr]);
                        //strBuilder.Append(unshifted[(lenCounter + lenStr - unshiftCount) % lenStr]);
                    }
                    //return strBuilder.ToString();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}
