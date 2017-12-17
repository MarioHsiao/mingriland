// --------------------------
//   Author => Lex XIAO
// --------------------------

#region
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
#endregion

namespace TLAuto.Base.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        /// ���ڴ����ķ�ʽʵ�ֵ����
        /// </summary>
        /// <typeparam name="TObject">��������</typeparam>
        /// <param name="obj">��Ҫ�����Ķ���</param>
        /// <returns></returns>
        public static TObject DeepClone<TObject>(this TObject obj) where TObject: class
        {
            return obj.ToStream().ToData<TObject>();
        }

        public static Task<int> ReadAllBytesAsync(this Stream stream, out byte[] buffer)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            var initialCapacity = stream.CanSeek ? (int)stream.Length : 0;
            buffer = new byte[initialCapacity];
            return Task<int>.Factory.FromAsync(stream.BeginRead, stream.EndRead, buffer, 0, initialCapacity, null);
        }

        #region ���л��ڷ����л�
        /// <summary>
        /// �Ӷ���������ȡ����
        /// </summary>
        /// <typeparam name="TObject">��Ҫת���Ķ�������</typeparam>
        /// <param name="stream">��</param>
        /// <returns></returns>
        public static TObject ToData<TObject>(this MemoryStream stream) where TObject: class
        {
            using (stream)
            {
                stream.Position = 0;
                var deserializer = new BinaryFormatter();
                var temp = deserializer.Deserialize(stream);
                return temp as TObject;
            }
        }

        /// <summary>
        /// �Ӷ���������ȡ����
        /// </summary>
        /// <typeparam name="TObject">��Ҫת���Ķ�������</typeparam>
        /// <param name="stream">��</param>
        /// <returns></returns>
        public static TObject ToDataAsStruct<TObject>(this MemoryStream stream) where TObject: struct
        {
            using (stream)
            {
                stream.Position = 0;
                var deserializer = new BinaryFormatter();
                var temp = deserializer.Deserialize(stream);
                return (TObject)temp;
            }
        }

        /// <summary>
        /// ������תΪ��������
        /// </summary>
        /// <param name="obj">��Ҫת���Ķ���</param>
        /// <typeparam name="TObject">��������</typeparam>
        /// <returns></returns>
        public static MemoryStream ToStream<TObject>(this TObject obj)
        {
            var serializer = new BinaryFormatter();
            var stream = new MemoryStream();
            serializer.Serialize(stream, obj);
            return stream;
        }
        #endregion
    }
}