// --------------------------
//   Author => Lex XIAO
// --------------------------

#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
#endregion

namespace TLAuto.Base.Extensions
{
    public static class ByteExtensions
    {
        #region Hash
        /// <summary>
        /// ʹ��ָ���㷨Hash
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashName"></param>
        /// <returns></returns>
        public static byte[] Hash(this byte[] data, string hashName = null)
        {
            var algorithm = hashName.IsNullOrEmpty()
                                ? HashAlgorithm.Create()
                                : HashAlgorithm.Create(hashName);
            return algorithm.ComputeHash(data);
        }
        #endregion

        #region ����
        /// <summary>
        /// ����byte���鵽ָ��byte���顣
        /// </summary>
        /// <param name="src">Դ���顣</param>
        /// <param name="target">Ŀ�����顣</param>
        /// <param name="srcIndex">��ʼ�������š�</param>
        /// <returns>����Ŀ�����顣</returns>
        public static void CopyIndex(this byte[] src, byte[] target, long srcIndex)
        {
            var p = 0;
            for (var i = srcIndex; i < src.Length; i++)
            {
                target[p] = src[i];
                p++;
            }
        }
        #endregion

        #region ת��Ϊʮ�������ַ���
        /// <summary>
        /// ת��Ϊʮ�������ַ���
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }

        public static string ToBin(this byte b)
        {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }

        /// <summary>
        /// ת��Ϊʮ�������ַ�����ÿ���ַ�������ģ����Ҫ�����Ķ�����ô��ַ������Ĵ˷�����
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this IEnumerable<byte> bytes)
        {
            return ToHex(bytes, string.Empty);
        }

        /// <summary>
        /// ת��Ϊʮ�������ַ���
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="split">�ָ���</param>
        /// <returns></returns>
        public static string ToHex(this IEnumerable<byte> bytes, string split)
        {
            var isNull = split.IsNullOrEmpty();
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2"));
                if (!isNull)
                {
                    sb.Append(split);
                }
            }
            return isNull
                       ? sb.ToString()
                       : sb.ToString().DelLastChar(split);
        }

        /// <summary>
        /// 16�����ַ���ת�ֽ�����
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="split">�ָ���</param>
        /// <returns></returns>
        public static byte[] HexStrToBytes(this string hexString, string split)
        {
            hexString = hexString.Replace(split, "");

            if (hexString.Length % 2 != 0)
            {
                hexString += " ";
            }

            var returnBytes = new byte[hexString.Length / 2];

            for (var i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }
        #endregion

        #region λ����
        /// <summary>
        /// index��0��ʼ
        /// ��ȡȡ��index�Ƿ�Ϊ1
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool GetBit(this byte b, int index)
        {
            return (b & (1 << index)) > 0;
        }

        /// <summary>
        /// ����indexλ��Ϊ1
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte SetBit(this byte b, int index)
        {
            b |= (byte)(1 << index);
            return b;
        }

        /// <summary>
        /// ����indexλ��Ϊ0
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte ClearBit(this byte b, int index)
        {
            b &= (byte)((1 << 8) - 1 - (1 << index));
            return b;
        }

        /// <summary>
        /// ����indexλȡ��
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte ReverseBit(this byte b, int index)
        {
            b ^= (byte)(1 << index);
            return b;
        }
        #endregion

        #region ��������ת��
        /// <summary>
        /// ��ָ���ַ���ת��ָ�����ȵ�bytes��
        /// </summary>
        public static byte[] ToBytes(this string str, int size, Encoding encoding)
        {
            var bytes = encoding.GetBytes(str);
            var ret = new byte[size];
            bytes.CopyTo(ret, 0);
            return ret;
        }

        /// <summary>
        /// ��ָ���ַ���ת��ָ�����ȵ�bytes��
        /// </summary>
        public static byte[] ToBytes(this string str, int size)
        {
            return str.ToBytes(size, Encoding.Default);
        }

        /// <summary>
        /// ��ָ���ַ���ת��ָ�����ȵ�bytes��
        /// </summary>
        public static byte[] ToBytesUTF8(this string str, int size)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            var ret = new byte[size];
            bytes.CopyTo(ret, 0);
            return ret;
        }

        /// <summary>
        /// ��ָ�����ֽ�����ת��ΪBase64�ַ���
        /// </summary>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string ToDefaultCodingString(this byte[] bytes)
        {
            var s = Encoding.Default.GetString(bytes);
            return s.TrimEndZero();
        }

        public static string ToDefaultCodingString(this ushort[] bytes)
        {
            var bs = new byte[bytes.Length];
            bytes.CopyTo(bs, 0);
            var s = Encoding.Default.GetString(bs);
            return s.TrimEndZero();
        }

        /// <summary>
        /// ת��Ϊָ��������ַ���
        /// </summary>
        /// <param name="data">�ֽ�����</param>
        /// <param name="encoding">�ַ�����</param>
        /// <returns></returns>
        public static string Decode(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }

        /// <summary>
        /// ת��Ϊ�ڴ���
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static MemoryStream ToMemoryStream(this byte[] data)
        {
            return new MemoryStream(data);
        }

        /// <summary>
        /// ����ָ���ĳ��Ƚ����Զ���λ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static short ConvertInt16(this byte[] data)
        {
            var datas = ConvertBytesToLength(data, 2);
            return BitConverter.ToInt16(datas, 0);
        }

        /// <summary>
        /// ����ָ���ĳ��Ƚ����Զ���λ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int ConvertInt32(this byte[] data)
        {
            var datas = ConvertBytesToLength(data, 4);
            return BitConverter.ToInt32(datas, 0);
        }

        /// <summary>
        /// ����ָ���ĳ��Ƚ����Զ���λ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static long ConvertInt64(this byte[] data)
        {
            var datas = ConvertBytesToLength(data, 8);
            return BitConverter.ToInt64(datas, 0);
        }

        /// <summary>
        /// ���ݳ����Զ���λ
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataSize"></param>
        /// <returns></returns>
        private static byte[] ConvertBytesToLength(byte[] data, int dataSize)
        {
            if (data.Length == dataSize)
            {
                return data;
            }
            var bytes = new byte[dataSize];
            for (var i = 0; i < dataSize; i++)
            {
                if (data.Length == i + 1)
                {
                    bytes[i] = data[i];
                }
                else
                {
                    bytes[i] = 0;
                }
            }
            return bytes;
        }
        #endregion

        #region ���л��ͷ����л�
        /// <summary>
        /// ������תΪ�ֽ�����
        /// </summary>
        /// <typeparam name="TObject">����</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBytes<TObject>(this TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                return stream.ToArray();
            }
        }

        /// <summary>
        /// ���ֽ�����ת��Ϊ����
        /// </summary>
        /// <typeparam name="TObject">��Ҫת���Ķ�������</typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static TObject ToObject<TObject>(this byte[] bytes) where TObject: class
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length, false))
            {
                var formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                stream.Flush();
                return data as TObject;
            }
        }

        /// <summary>
        /// ���ֽ�����ת��Ϊ����(struct)
        /// </summary>
        /// <typeparam name="TObject">��Ҫת���Ķ�������</typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static TObject ToObjectAsStruct<TObject>(this byte[] bytes) where TObject: struct
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length, false))
            {
                var formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                stream.Flush();
                return (TObject)data;
            }
        }
        #endregion

        #region �ṹ����ֽ������ת��
        /// <summary>
        /// �ṹ��תbyte����
        /// </summary>
        /// <param name="structObj">Ҫת���Ľṹ��</param>
        /// <returns>ת�����byte����</returns>
        public static byte[] StructToBytes<TObject>(this TObject structObj) where TObject: struct
        {
            //�õ��ṹ��Ĵ�С
            var size = Marshal.SizeOf(structObj);
            //����byte����
            var bytes = new byte[size];
            //����ṹ���С���ڴ�ռ�
            var structPtr = Marshal.AllocHGlobal(size);
            //���ṹ�忽������õ��ڴ�ռ�
            Marshal.StructureToPtr(structObj, structPtr, false);
            //���ڴ�ռ俽��byte����
            Marshal.Copy(structPtr, bytes, 0, size);
            //�ͷ��ڴ�ռ�
            Marshal.FreeHGlobal(structPtr);
            //����byte����
            return bytes;
        }

        /// <summary>
        /// byte����ת�ṹ��
        /// </summary>
        /// <param name="bytes">byte����</param>
        /// <param name="type">�ṹ������</param>
        /// <returns>ת����Ľṹ��</returns>
        public static object BytesToStuct(this byte[] bytes, Type type)
        {
            //�õ��ṹ��Ĵ�С
            var size = Marshal.SizeOf(type);
            //byte���鳤��С�ڽṹ��Ĵ�С
            if (size > bytes.Length)
            {
                //���ؿ�
                return null;
            }
            //����ṹ���С���ڴ�ռ�
            var structPtr = Marshal.AllocHGlobal(size);
            //��byte���鿽������õ��ڴ�ռ�
            Marshal.Copy(bytes, 0, structPtr, size);
            //���ڴ�ռ�ת��ΪĿ��ṹ��
            var obj = Marshal.PtrToStructure(structPtr, type);
            //�ͷ��ڴ�ռ�
            Marshal.FreeHGlobal(structPtr);
            //���ؽṹ��
            return obj;
        }
        #endregion
    }
}