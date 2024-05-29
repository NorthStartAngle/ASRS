using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public enum CTXType
    {
        RDS,
        RTS,
        WTK,
        RTK,
        WNT,
        RNT,
        WMC,
        RMC,
        CKF
    }

    public class WMSG //RDS
    {
        public ushort msgId { get; protected set; }
        public int STX = 0x02;
        public int ETX = 0x03;
        public CTXType CTX;

        public virtual string Create()
        {
            msgId = Common.unique1();
            CTX = CTXType.RDS;
            string strMsgId = msgId.ToString("D10");
            ushort crc16 = Common.ComputeCRC(Encoding.ASCII.GetBytes(nameof(CTX) + strMsgId));
            return $"{STX.ToString("X2")}|{crc16}|{nameof(CTX)}|{msgId.ToString("D10")}|{ETX.ToString("X2")}";
        }
    }

    public class WTK :  WMSG
    {
        public int taskMode;//2
        public ushort taskId;//10
        public int fromRow;//2
        public int fromCol;//4

        public int fromColOffsetDir;//1
        public int fromColOffset;//4
        public int fromLayer;//2
        public int fromDepthMax;//1
        public int fromDepth;//1
        public int toRow;//2
        public int toCol;//4
        public int toColOffsetDir;//1
        public int toColOffset;//4

        public int toLayer;//2
        public int toDepthMax;//1
        public int toDelth;//1
        public int boxLength;//4
        public int boxWidth;//4

        public int taskReserved5;//2
        public int taskReserved4;//2
        public int boxHeight;//4

        public override string Create()
        {
            msgId = Common.unique1();
            CTX = CTXType.WTK;
            string strMsgId = msgId.ToString("D10");

            string strData = msgId.ToString("D10") + taskMode.ToString("D2") + taskId.ToString("D10") + fromRow.ToString("D2") + fromCol.ToString("D4") + fromColOffsetDir.ToString("D1") + fromColOffset.ToString("D4") + fromLayer.ToString("D2") + fromDepthMax.ToString("D1") + fromDepth.ToString("D1") + toRow.ToString("D2") + toCol.ToString("D4") + toColOffsetDir.ToString("D1") + toColOffset.ToString("D4") + toLayer.ToString("D2") + toDepthMax.ToString("D1") + toDelth.ToString("D1") + boxLength.ToString("D4") + boxWidth.ToString("D4") + taskReserved5.ToString("D2") + taskReserved4.ToString("D2") + boxHeight.ToString("D4");

            ushort crc16 = Common.ComputeCRC(Encoding.ASCII.GetBytes(nameof(CTX) + strData));
            return $"{STX.ToString("X2")}|{crc16}|{nameof(CTX)}|{strData}|{ETX.ToString("X2")}";
        }

        public string CreateByParam(ushort[] param, bool workMode)
        {
            if(workMode) // Auto
            {
                taskId = Common.unique3();
            }
            else // Manual
            {
                taskId= Common.unique2();
            }
            taskMode = param[0];
            fromRow = param[1];
            fromCol = param[2];
            fromColOffsetDir = param[3];
            fromColOffset = param[4];
            fromLayer = param[5];
            fromDepthMax = param[6];
            fromDepth = param[7];
            toRow = param[8];
            toCol = param[9];
            toColOffsetDir = param[10];
            toColOffset = param[11];
            toLayer = param[12];
            toDepthMax = param[13];
            toDelth = param[14];
            boxLength = param[15];
            boxWidth = param[16];
            taskReserved5 = param[17];
            taskReserved4 = param[18];
            boxHeight = param[19];
            
            return Create();
        }
    }

    internal class RTS
    {
        public RTS() { }
        
        public ushort msgId;//10
        public int workMode;
        public int pickStatus;
        public int putStatus;
        public int noteReserved2;
        public int noteReserved1;
        public int taskMode;//2
        public ushort taskId;//10
        public int fromRow;//2
        public int fromCol;//4
        public int fromColOffsetDir;
        public int fromColOffset;//4
        public int fromLayer;//2
        public int fromDepthMax;
        public int fromDepth;
        public int toRow;//2
        public int toCol;//4
        public int toColOffsetDir;
        public int toColOffset;//4
        public int toLayer;//2
        public int toDepthMax;
        public int boxLength;//4
        public int boxWidth;//4
        public int taskStatus;
        public int taskReserved5;//2
        public int taskReserved4;//2
        public int boxHeight;//4
        public int taskReserved2;//4
        public int taskReserved1;//4
        public int manualCmd;//2
        public int manualStatus;
        public int deviceStatus;//4
        public int actionStatus;//2

        public bool status;
        public static RTS Parse(byte[] data)
        {
            RTS _rts = new RTS();
            try
            {
                _rts.msgId = Common.ReadFromArray(data, 0, 10);
                _rts.workMode = Common.ReadFromArray(data, 10, 1);
                _rts.pickStatus = Common.ReadFromArray(data, 11, 1);
                _rts.putStatus = Common.ReadFromArray(data, 12, 1);
                _rts.noteReserved2 = Common.ReadFromArray(data, 13, 1);
                _rts.noteReserved1 = Common.ReadFromArray(data, 14, 1);
                _rts.taskMode = Common.ReadFromArray(data, 15, 2);
                _rts.taskId = Common.ReadFromArray(data, 17, 10);
                _rts.fromRow = Common.ReadFromArray(data, 27, 2);
                _rts.fromCol = Common.ReadFromArray(data, 29, 4);
                _rts.fromColOffsetDir = Common.ReadFromArray(data, 33, 1);
                _rts.fromColOffset = Common.ReadFromArray(data, 34, 4);
                _rts.fromLayer = Common.ReadFromArray(data, 38, 2);
                _rts.fromDepthMax = Common.ReadFromArray(data, 40, 1);
                _rts.fromDepth = Common.ReadFromArray(data, 41, 1);
                _rts.toRow = Common.ReadFromArray(data, 42, 2);
                _rts.toCol = Common.ReadFromArray(data, 44, 4);
                _rts.toColOffsetDir = Common.ReadFromArray(data, 48, 1);
                _rts.toColOffset = Common.ReadFromArray(data, 49, 4);
                _rts.toLayer = Common.ReadFromArray(data, 53, 2);
                _rts.toDepthMax = Common.ReadFromArray(data, 55, 1);
                _rts.boxLength = Common.ReadFromArray(data, 56, 4);
                _rts.boxWidth = Common.ReadFromArray(data, 60, 4);
                _rts.taskStatus = Common.ReadFromArray(data, 64, 1);
                _rts.taskReserved5 = Common.ReadFromArray(data, 65, 2);
                _rts.taskReserved4 = Common.ReadFromArray(data, 67, 2);
                _rts.boxHeight = Common.ReadFromArray(data, 69, 4);
                _rts.taskReserved2 = Common.ReadFromArray(data, 73, 4);
                _rts.taskReserved1 = Common.ReadFromArray(data, 77, 4);
                _rts.manualCmd = Common.ReadFromArray(data, 81, 2);
                _rts.manualStatus = Common.ReadFromArray(data, 83, 1);
                _rts.deviceStatus = Common.ReadFromArray(data, 84, 4);
                _rts.actionStatus = Common.ReadFromArray(data, 88, 2);

                _rts.status = true;

            }
            catch (Exception)
            {
                _rts.status = false;
            }

            return _rts;
        }
    }

    internal class RTK
    {
        public RTK() { }

        public ushort msgId;//10
        public ushort taskId;//10
        public int recvResult;

        public bool status;

        public static RTK Parse(byte[] data)
        {
            RTK _rtk = new RTK();
            try
            {
                _rtk.msgId = Common.ReadFromArray(data, 0, 10);
                _rtk.taskId = Common.ReadFromArray(data, 10, 10);
                _rtk.recvResult = Common.ReadFromArray(data, 20, 1);

                _rtk.status = true;
            }
            catch (Exception e)
            {
                _rtk.status = false;
            }

            return _rtk;
        }
    }
}
