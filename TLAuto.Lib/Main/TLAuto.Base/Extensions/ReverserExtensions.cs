// --------------------------
//   Author => Lex XIAO
// --------------------------

#region
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#endregion

namespace TLAuto.Base.Extensions
{
    /// <summary>
    /// �̳�IComparer�ӿڣ�ʵ��ͬһ�Զ������͡�����Ƚ�
    /// </summary>
    /// <typeparam name="T">TΪ��������</typeparam>
    public sealed class Reverser<T> : IComparer<T>
    {
        private readonly Type _type;
        private ReverserInfo _info;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="type">���бȽϵ�������</param>
        /// <param name="name">���бȽ϶������������</param>
        /// <param name="direction">�ȽϷ���(����/����)</param>
        public Reverser(Type type, string name, ReverserInfo.Direction direction)
        {
            _type = type;
            _info.name = name;
            if (direction != ReverserInfo.Direction.ASC)
            {
                _info.direction = direction;
            }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="className">���бȽϵ�������</param>
        /// <param name="name">���бȽ϶������������</param>
        /// <param name="direction">�ȽϷ���(����/����)</param>
        public Reverser(string className, string name, ReverserInfo.Direction direction)
        {
            try
            {
                _type = Type.GetType(className, true);
                _info.name = name;
                _info.direction = direction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="t">���бȽϵ����͵�ʵ��</param>
        /// <param name="name">���бȽ϶������������</param>
        /// <param name="direction">�ȽϷ���(����/����)</param>
        public Reverser(T t, string name, ReverserInfo.Direction direction)
        {
            _type = t.GetType();
            _info.name = name;
            _info.direction = direction;
        }

        //���룡ʵ��IComparer<T>�ıȽϷ�����
        int IComparer<T>.Compare(T t1, T t2)
        {
            var x = _type.InvokeMember(_info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t1, null);
            var y = _type.InvokeMember(_info.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty, null, t2, null);
            if (_info.direction != ReverserInfo.Direction.ASC)
            {
                Swap(ref x, ref y);
            }
            return new CaseInsensitiveComparer().Compare(x, y);
        }

        //����������
        private void Swap(ref object x, ref object y)
        {
            object temp = null;
            temp = x;
            x = y;
            y = temp;
        }
    }

    /// <summary>
    /// ����Ƚ�ʱʹ�õ���Ϣ��
    /// </summary>
    public struct ReverserInfo
    {
        /// <summary>
        /// �Ƚϵķ������£�
        /// ASC������
        /// DESC������
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// ����
            /// </summary>
            ASC = 0,

            /// <summary>
            /// ����
            /// </summary>
            DESC = 1
        }

        /// <summary>
        /// </summary>
        public enum Target
        {
            /// <summary>
            /// </summary>
            CUSTOMER = 0,

            /// <summary>
            /// </summary>
            FORM,

            /// <summary>
            /// </summary>
            FIELD,

            /// <summary>
            /// </summary>
            SERVER
        }

        /// <summary>
        /// </summary>
        public string name;

        /// <summary>
        /// </summary>
        public Direction direction;

        /// <summary>
        /// </summary>
        public Target target;
    }
}