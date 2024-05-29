using System;
using System.Text;

namespace LIB
{
    internal static class Common
    {
        static ushort unique_n1 = 0;
        static ushort unique_n2 = 0;
        static ushort unique_n3 = 0;

        internal static Object parseDATA(Byte[] data)
        {
            RTS _rts = RTS.Parse(data);
            if(_rts.status)
            {
                return _rts;
            }

            RTK _rtk = RTK.Parse(data);
            if(_rtk.status)
            {
                return _rtk;
            }

            return null;
        }

        internal static ushort unique1(bool isRepeat = false) 
        {
            if (isRepeat) { return unique_n1; }
            unique_n1 += 1;
            if ((long)unique_n1 > 9999999999) { unique_n1 = 1; }
            return unique_n1; 
        }

        internal static ushort unique2(bool isRepeat = false)
        {
            if (isRepeat) { return unique_n2; }

            unique_n2 += 1;
            if (unique_n2 > 200) { unique_n2 = 1; }
            return unique_n2;
        }

        internal static ushort unique3(bool isRepeat = false)
        {
            if (isRepeat) { return unique_n3; }

            unique_n3 += 1;
            if ((long)unique_n3 > 9999999999) { unique_n3 = 201; }
            return unique_n3;
        }

        internal static void ParseIpPort(string ipPort, out string ip, out int port) 
        {
            if (string.IsNullOrEmpty(ipPort)) throw new ArgumentNullException(nameof(ipPort));

            ip = null;
            port = -1;

            int colonIndex = ipPort.LastIndexOf(':');
            if (colonIndex != -1)
            {
                ip = ipPort.Substring(0, colonIndex);
                port = Convert.ToInt32(ipPort.Substring(colonIndex + 1));
            }
        }

        internal static ushort ReadFromArray(byte[] bytes,int start, int len) {
            if( bytes.Length > start+len )
            {
                byte[] arr = new byte[len];
                for(int i = 0; i < len;i++)
                {
                    arr[i] = bytes[start+i];
                }

                string s = Encoding.ASCII.GetString(arr);
                return (ushort)Convert.ToUInt64(s);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        internal static ushort ComputeCRC(byte[] data)
        {
            ushort crc = 0xFFFF;
            const ushort polynomial = 0xA001;
            foreach (byte b in data)
            {
                crc ^= b;

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 1) == 1)
                    {
                        crc = (ushort)((crc >> 1) ^ polynomial);
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            return crc;
        }
    }
}
