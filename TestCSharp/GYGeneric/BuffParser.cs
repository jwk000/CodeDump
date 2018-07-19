using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GYGeneric
{
    public class BuffParser
    {
        public BuffParser(byte[] buff)
        {
            mBuff = buff;
        }

        public string PopString(int len)
        {
            string ret = null;
            if (len == 0) return null;
            if (check_buff(len))
            {
                ret = Encoding.UTF8.GetString(mBuff, mIndex, len);
				//ret = ret.Remove(ret.IndexOf('\0'));
                mIndex += len;
            }
            return ret;
        }

        public bool PopBool()
        {
            bool b = BitConverter.ToBoolean(mBuff, mIndex);
            mIndex += sizeof(bool);
            return b;
        }

        public byte PopByte()
        {
            return mBuff[mIndex++];
        }

        public int PopInt()
        {
            int i = BitConverter.ToInt32(mBuff, mIndex);
            mIndex += sizeof(int);
            return i;
        }

        public short PopShort()
        {
            short s = BitConverter.ToInt16(mBuff, mIndex);
            mIndex += sizeof(short);
            return s;
        }

        public long PopLong()
        {
            long l = BitConverter.ToInt64(mBuff, mIndex);
            mIndex += sizeof(long);
            return l;
        }

        public Int16 PopInt16()
        {
            return PopShort();
        }

        public Int32 PopInt32()
        {
            return PopInt();
        }

        public Int64 PopInt64()
        {
            return PopLong();
        }

        public uint PopUint()
        {
            uint u = BitConverter.ToUInt32(mBuff, mIndex);
            mIndex += sizeof(uint);
            return u;
        }

        public ushort PopUshort()
        {
            ushort u = BitConverter.ToUInt16(mBuff, mIndex);
            mIndex += sizeof(ushort);
            return u;
        }

		public char PopChar()
		{
			char c = BitConverter.ToChar (mBuff, mIndex);
			mIndex += sizeof(char);
			return c;
		}

        public ulong PopUlong()
        {
            ulong u = BitConverter.ToUInt64(mBuff, mIndex);
            mIndex += sizeof(ulong);
            return u;
        }

        public UInt16 PopUint16()
        {
            return PopUshort();
        }

        public UInt32 PopUint32()
        {
            return PopUint();
        }

        public UInt64 PopUint64()
        {
            return PopUlong();
        }

        public float PopFloat()
        {
            float f = BitConverter.ToSingle(mBuff, mIndex);
            mIndex += sizeof(float);
            return f;
        }

        public double PopDouble()
        {
            double d = BitConverter.ToDouble(mBuff, mIndex);
            mIndex += sizeof(double);
            return d;
        }

        public byte[] PopBytes(int len)
        {
            byte[] ret = null;
            if (check_buff(len))
            {
                ret = new byte[len];
                Array.Copy(mBuff, mIndex, ret, 0, len);
                mIndex += len;
            }
            return ret;
        }


        private bool check_buff(int len)
        {
            if (mBuff == null) return false;
            if (mBuff.Length < len) return false;
            return true;
        }
        private byte[] mBuff;
        private int mIndex = 0;
    }
}
