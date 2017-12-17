// --------------------------
//   Author => Lex XIAO
// --------------------------

#region
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace TLAuto.Base.Extensions
{
    public static class StringExtensions
    {
        #region �����ٶ�ת��
        /// <summary>
        /// �����ٶ�ת�����ַ�������ʽ
        /// </summary>
        /// <param name="speed">�����ٶȣ��ֽ�Ϊ��λ��</param>
        /// <returns></returns>
        public static string ConvertDownloadStr(this double speed)
        {
            var mStrSize = "";
            var factSize = speed;
            if (factSize < 1024.00)
            {
                mStrSize = factSize.ToString("F2") + " Byte/s";
            }
            else
            {
                if ((factSize >= 1024.00) && (factSize < 1048576))
                {
                    mStrSize = (factSize / 1024.00).ToString("F2") + " K/s";
                }
                else
                {
                    if ((factSize >= 1048576) && (factSize < 1073741824))
                    {
                        mStrSize = (factSize / 1024.00 / 1024.00).ToString("F2") + " M/s";
                    }
                    else
                    {
                        if (factSize >= 1073741824)
                        {
                            mStrSize = (factSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G/s";
                        }
                    }
                }
            }
            return mStrSize;
        }
        #endregion

        #region �ַ���������ȡ
        /// <summary>
        /// ˫�ֽ��ж�
        /// </summary>
        private static readonly Regex DoubleByteRegex = new Regex("[^\x00-\xff]");

        /// <summary>
        /// ��Сд�ж�
        /// </summary>
        private static readonly Regex CaseRegex = new Regex("[A-H]|[M-Z]");

        /// <summary>
        /// �õ��ַ�����ʵ����, 1�����ֳ���Ϊ2,һ����д��1.5��
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <param name="nums">�ַ�����</param>
        /// <returns>�ַ�����</returns>
        public static int GetStringLength(this string input, out char[] nums)
        {
            double num = 0;
            nums = input.ToCharArray();
            for (var i = 0; i < nums.Length; i++)
            {
                var m = DoubleByteRegex.Match(nums[i].ToString());
                if (m.Success)
                {
                    num += 2;
                }
                else
                {
                    var m2 = CaseRegex.Match(nums[i].ToString());
                    if (m2.Success)
                    {
                        num += 1.5;
                    }
                    else
                    {
                        num++;
                    }
                }
            }
            return (int)num;
        }

        /// <summary>
        /// �ַ����ض�
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <param name="length">����</param>
        /// <param name="substitute">�������ȵı�ʾ���硰...��������Ͳ����ٺ�����</param>
        /// <returns></returns>
        public static string SubCutString(this string input, int length, string substitute = "...")
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            //��ʼ���ַ������ַ�����
            char[] arr;
            //����ַ�������ʵ����С�ڻ��ߵ���Ҫ��ȡ���ַ������Ⱦ�ֱ�ӷ���Դ�ַ���
            if (GetStringLength(input, out arr) <= length)
            {
                return input;
            }
            //��ȡ������substitute�ĳ���
            if (string.IsNullOrEmpty(substitute))
            {
                length -= Encoding.UTF8.GetBytes("...").Length;
            }
            else
            {
                length -= Encoding.UTF8.GetBytes(substitute).Length;
            }
            //�����������ַ��ֽڳ���
            double byteLength = 0;
            //ƴװ�ַ�����
            var subBuilder = new StringBuilder();
            //ѭ���ж�ÿ���ַ�������˫�ֽڻ��ǵ��ֽ�(��дӢ����ĸΪ1.5���ֽ�)
            for (var i = 0; i < arr.Length; i++)
            {
                //�ж�˫�ֽ�
                var doubleByteMatch = DoubleByteRegex.Match(arr[i].ToString());
                if (doubleByteMatch.Success)
                {
                    byteLength += 2;
                }
                else
                {
                    //�ж��Ƿ�Ϊ��д
                    var caseMatch = CaseRegex.Match(arr[i].ToString());
                    if (caseMatch.Success)
                    {
                        byteLength += 1.5;
                    }
                    else
                    {
                        byteLength++;
                    }
                }
                if (byteLength > length)
                {
                    break;
                }
                subBuilder.Append(arr[i]);
            }
            subBuilder.Append(substitute);
            return subBuilder.ToString();
        }

        /// <summary>
        /// �ַ����ض�
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <param name="length">����</param>
        /// <param name="substitute">�������ȵı�ʾ���硰...��������Ͳ����ٺ�����</param>
        /// <returns></returns>
        public static string SubCutString2(this string input, int length, string substitute = "...")
        {
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var tempString = "";
            var s = ascii.GetBytes(input);
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                try
                {
                    tempString += input.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (tempLen > length)
                {
                    break;
                }
            }
            var mybyte = Encoding.Default.GetBytes(input);
            if (mybyte.Length > length)
            {
                tempString += substitute;
            }
            return tempString;
        }

        /// <summary>
        /// ��ĩβɾ��ָ�����ȵ��ַ���
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <param name="delLength">ɾ������</param>
        /// <returns></returns>
        public static string DeleteEndString(this string input, int delLength)
        {
            return input.Length > delLength
                       ? input.Substring(0, input.Length - delLength)
                       : input;
        }
        #endregion

        #region ���ȫ��ת��
        /// <summary>
        /// ת��ǵĺ���(SBC case)
        /// </summary>
        /// <param name="input">����</param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if ((c[i] > 65535) && (c[i] < 65375))
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

        /// <summary>
        /// תȫ�ǵĺ���(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            //���תȫ�ǣ�
            var c = input.ToCharArray();
            for (var i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }
        #endregion

        #region ��ȡ����ƴ������ĸ
        /// <summary>
        /// ��ָ�����ַ����б�cnStr�м�������ƴ�������ַ���
        /// </summary>
        /// <param name="cnStr">�����ַ���</param>
        /// <returns>���Ӧ�ĺ���ƴ������ĸ��</returns>
        public static string GetSpellCode(this string cnStr)
        {
            var strTemp = string.Empty;
            var iLen = cnStr.Length;
            for (var i = 0; i <= iLen - 1; i++)
            {
                strTemp += GetCharSpellCode(cnStr.Substring(i, 1));
            }
            return strTemp;
        }

        /// <summary>
        /// �õ�һ�����ֵ�ƴ����һ����ĸ�������һ��Ӣ����ĸ��ֱ�ӷ��ش�д��ĸ
        /// </summary>
        /// <param name="cnChar">��������</param>
        /// <returns>������д��ĸ</returns>
        private static string GetCharSpellCode(this string cnChar)
        {
            var zw = Encoding.Default.GetBytes(cnChar);
            //�������ĸ����ֱ�ӷ��� 
            if (zw.Length == 1)
            {
                return cnChar.ToUpper();
            }
            //��ȡ��һ�ַ����ֽ�����
            int i1 = zw[0];
            int i2 = zw[1];
            long iCnChar = (i1 * 256) + i2;
            //expresstion 
            //table of the constant list 
            // 'A'; //45217..45252 
            // 'B'; //45253..45760 
            // 'C'; //45761..46317 
            // 'D'; //46318..46825 
            // 'E'; //46826..47009 
            // 'F'; //47010..47296 
            // 'G'; //47297..47613 

            // 'H'; //47614..48118 
            // 'J'; //48119..49061 
            // 'K'; //49062..49323 
            // 'L'; //49324..49895 
            // 'M'; //49896..50370 
            // 'N'; //50371..50613 
            // 'O'; //50614..50621 
            // 'P'; //50622..50905 
            // 'Q'; //50906..51386 

            // 'R'; //51387..51445 
            // 'S'; //51446..52217 
            // 'T'; //52218..52697 
            //û��U,V 
            // 'W'; //52698..52979 
            // 'X'; //52980..53640 
            // 'Y'; //53689..54480 
            // 'Z'; //54481..55289 

            // iCnChar match the constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }
            if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            return "?";
        }
        #endregion

        #region ������ʽ
        /// <summary>
        /// ָʾ������ʽʹ��pattern�������ƶ���������ʽ�Ƿ��������ַ������ҵ�ƥ����
        /// </summary>
        /// <param name="input">Ҫ����ƥ������ַ���</param>
        /// <param name="pattern">Ҫƥ���������ʽģʽ</param>
        /// <returns></returns>
        public static bool IsMatch(this string input, string pattern)
        {
            return !input.IsNullOrEmpty() && Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// ��ָ���������ַ���������pattern�������ṩ��������ʽ��ƥ����
        /// </summary>
        /// <param name="input">Ҫ����ƥ������ַ���</param>
        /// <param name="pattern">Ҫƥ���������ʽģʽ</param>
        /// <returns></returns>
        public static string Match(this string input, string pattern)
        {
            return input.IsNullOrEmpty()
                       ? string.Empty
                       : Regex.Match(input, pattern).Value;
        }

        /// <summary>
        /// ��ָ���������ַ����ڣ�ʹ��ָ�����滻�ַ����滻��ָ��������ʽƥ��������ַ���
        /// </summary>
        /// <param name="input">Ҫ����ƥ������ַ���</param>
        /// <param name="pattern">Ҫƥ���������ʽģʽ</param>
        /// <param name="replacement">�滻�ַ���</param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement)
        {
            return input.IsNullOrEmpty()
                       ? string.Empty
                       : Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// ��ָ���������ַ����ڣ�ʹ��ָ�����滻�ַ����滻��ָ��������ʽƥ��������ַ���
        /// </summary>
        /// <param name="input">Ҫ����ƥ������ַ���</param>
        /// <param name="pattern">Ҫƥ���������ʽģʽ</param>
        /// <param name="replacement">�滻�ַ���</param>
        /// <param name="options">�ṩ��������������ʽѡ���ö��ֵ</param>
        /// <returns></returns>
        public static string Replace(this string input, string pattern, string replacement, RegexOptions options)
        {
            return input.IsNullOrEmpty()
                       ? string.Empty
                       : Regex.Replace(input, pattern, replacement, options);
        }

        /// <summary>
        /// ��ָ���������ַ����ڣ��滻��ʼ�ͽ����ַ����м��ֵ
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="replacement">�滻�ַ���</param>
        /// <returns></returns>
        public static string ReplaceStartToEnd(this string input, string start, string end, string replacement)
        {
            return input.Replace("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", replacement, RegexOptions.Multiline | RegexOptions.Singleline);
        }

        /// <summary>
        /// ����ַ����п�ʼ�ͽ����ַ����м��ֵ
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <param name="start">��ʼ</param>
        /// <param name="end">����</param>
        /// <returns></returns>
        public static string GetStartToEnd(this string input, string start, string end)
        {
            var rg = new Regex("(?<=(" + start + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(input).Value;
        }

        /// <summary>
        /// ��֤�����ַ����Ƿ��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchNumeric(this string input)
        {
            return input.IsMatch(@"^[-]?\d+[.]?\d*$");
        }

        /// <summary>
        /// ��֤�����ַ����Ƿ������ĸ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchLetter(this string input)
        {
            return input.IsMatch(@"^[A-Za-z]+$");
        }

        /// <summary>
        /// ��֤�����ַ����Ƿ�������ֻ���ĸ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchNumericAndLetter(this string input)
        {
            return input.IsMatch(@"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// ��֤�����ַ����Ƿ�������ֻ���ĸ��С����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchNumericAndLetterAndSign(this string input)
        {
            //������ſ���������ӣ���\.λ�ú���
            return input.IsMatch(@"^[A-Za-z0-9\.]+$");
        }

        /// <summary>
        /// ��֤�����ַ����Ƿ��ǺϷ�IP(�������˿ں�)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMatchIpAddressWithOutPort(this string input)
        {
            return input.IsMatch(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }

        /// <summary>
        /// �ж�һ���ַ����Ƿ��������͵��ַ���
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns>���򷵻�true�����򷵻�false</returns>
        public static bool IsUnsign(this string input)
        {
            return Regex.IsMatch(input, @"^[1-9]\d*$");
        }
        #endregion

        #region ��������ת������֤
        /// <summary>
        /// ָʾָ���Ķ����Ƿ���Int32
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns></returns>
        public static bool IsInt32(this string input)
        {
            int i;
            return int.TryParse(input, out i);
        }

        /// <summary>
        /// ָʾָ���Ķ����Ƿ���Int64��long��
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns></returns>
        public static bool IsInt64(this string input)
        {
            long i;
            return long.TryParse(input, out i);
        }

        /// <summary>
        /// ָʾָ���Ķ����Ƿ���DateTime
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns></returns>
        public static bool IsDateTime(this string input)
        {
            DateTime dateTime;
            return DateTime.TryParse(input, out dateTime);
        }

        /// <summary>
        /// ָʾָ���Ķ����Ƿ���Boolean
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns></returns>
        public static bool IsBoolean(this string input)
        {
            bool b;
            return bool.TryParse(input, out b);
        }

        /// <summary>
        /// ָʾָ���Ķ����Ƿ���Decimal
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns></returns>
        public static bool IsDecimal(this string input)
        {
            decimal de;
            return decimal.TryParse(input, out de);
        }

        /// <summary>
        /// ָʾָ���Ķ����Ƿ���Double
        /// </summary>
        /// <param name="input">�ַ���</param>
        /// <returns></returns>
        public static bool IsDouble(this string input)
        {
            double d;
            return double.TryParse(input, out d);
        }

        /// <summary>
        /// ��ָ����Base64�ַ���ת��Ϊ�ֽ�����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] FromBase64String(this string input)
        {
            return Convert.FromBase64String(input);
        }

        /// <summary>
        /// ��֤Email��ʽ
        /// </summary>
        /// <param name="email">�ʼ���ַ</param>
        /// <returns></returns>
        public static bool IsValidEmailAddress(this string email)
        {
            return email.IsMatch(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        }
        #endregion

        #region ��˹��������������
        /// <summary>
        /// ת��Ϊ��������������һ����������ĸСд����
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <returns></returns>
        public static string ToCamel(this string input)
        {
            if (input.IsNullOrEmpty())
            {
                return input;
            }
            return input[0].ToString().ToLower() + input.Substring(1);
        }

        /// <summary>
        /// ת��Ϊ��˹������������һ����������ĸ��д����
        /// </summary>
        /// <param name="input">Դ�ַ���</param>
        /// <returns></returns>
        public static string ToPascal(this string input)
        {
            if (input.IsNullOrEmpty())
            {
                return input;
            }
            return input[0].ToString().ToUpper() + input.Substring(1);
        }
        #endregion

        #region �㷨
        /// <summary>
        /// �����ַ����� MD5 ��ϣ�����ַ���Ϊ�գ��򷵻ؿգ����򷵻ؼ�������
        /// </summary>
        public static string ComputeMd5Hash(this string str)
        {
            var hash = str;
            if (str != null)
            {
                var md5 = new MD5CryptoServiceProvider();
                var data = Encoding.ASCII.GetBytes(str);
                data = md5.ComputeHash(data);
                hash = "";
                for (var i = 0; i < data.Length; i++)
                {
                    hash += data[i].ToString("x2");
                }
            }
            return hash;
        }

        /// <summary>
        /// ʮ�����ƴ�С�Ƚ�
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>-1 a ��;0 ���;1 b ��</returns>
        public static int CompareToBase16(string a, string b)
        {
            var isAempty = string.IsNullOrEmpty(a);
            var isBempty = string.IsNullOrEmpty(b);
            if (isAempty && isBempty)
            {
                return 0;
            }

            if (isAempty)
            {
                return 1;
            }

            if (isBempty)
            {
                return -1;
            }

            if (a.ToLower().IndexOf("0x", StringComparison.InvariantCulture) >= 0)
            {
                a = a.Substring(2);
            }
            if (b.ToLower().IndexOf("0x", StringComparison.InvariantCulture) >= 0)
            {
                b = b.Substring(2);
            }
            var aa = Convert.ToInt64(a, 16);
            var bb = Convert.ToInt64(b, 16);
            if (aa > bb)
            {
                return -1;
            }
            if (aa == bb)
            {
                return 0;
            }
            return 1;
        }
        #endregion

        #region ������չ����
        /// <summary>
        /// Indicates whether the specified string is null or an Empty string.
        /// </summary>
        /// <param name="s">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string (""); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// �ӵ�ǰ System.String �����Ƴ�����ǰ���հ��ַ���β���հ��ַ���(�Զ��ж��ַ����Ƿ�Ϊ�գ����Ϊ���򷵻ؿ��ַ���)
        /// </summary>
        /// <param name="s">�ַ���</param>
        /// <returns></returns>
        public static string ToTrim(this string s)
        {
            return s.IsNullOrEmpty()
                       ? string.Empty
                       : s.Trim();
        }

        /// <summary>
        /// ��ָ���ַ����еĸ�ʽ���滻Ϊָ����������Ӧ����ʵ����ֵ���ı���Ч��
        /// </summary>
        /// <param name="format">���ϸ�ʽ�ַ���</param>
        /// <param name="args">Ҫ��ʽ���Ĳ���</param>
        /// <returns></returns>
        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// ɾ������β��ָ���ַ�����ַ�
        /// </summary>
        /// <param name="s">Դ�ַ���</param>
        /// <param name="strchar">�ָ���</param>
        /// <returns>ȥ����β�ַ�����ַ���</returns>
        public static string DelLastChar(this string s, string strchar)
        {
            return s.Substring(0, s.LastIndexOf(strchar));
        }

        /// <summary>
        /// �ַ����Ƚϣ����Դ�Сд
        /// </summary>
        /// <param name="strA">�ַ���A</param>
        /// <param name="strB">�ַ���B</param>
        /// <returns>�ȽϽ��</returns>
        public static bool CompareWith(this string strA, string strB)
        {
            return string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) == 1;
        }

        /// <summary>
        /// "abc\0\0" => "abc"
        /// </summary>
        public static string TrimEndZero(this string s)
        {
            if (s == null)
            {
                return null;
            }

            var indexOf = s.IndexOf('\0');
            if (indexOf == -1)
            {
                return s;
            }

            return s.Substring(0, indexOf);
        }

        public static string Reverse(this string original)
        {
            var arr = original.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        #endregion
    }
}