using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LIBS
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
	
	public enum Status
	{
		Accepting,
		Accept,
		Pending,
		Completed,
		Error
	}


    public struct Pos
    {
        public int row;
        public int col;
        public int deep;
    }

    public class MSG
    {
        public ushort msgId;
        public int STX = 0x02;
        public int ETX = 0x03;
        public CTXType CTX;
		
		public Status status;
		public DateTime dt;
		public int Interval=300;
		
        public MSG(CTXType type)
        {
            CTX = type;
			status = Status.Accepting;
			dt = DateTime.Now;
        }
		
		public MSG(MSG other)
		{
			msgId = other.msgId;
			STX = other.STX;
			ETX = other.ETX;
			CTX=other.CTX;
			
			status=other.status;
			dt = other.dt;
			Interval=other.Interval;
		}

		public MSG NewID()
		{
			msgId = Common.unique1();
			return this;
		}
		
		public MSG SetStatus(Status _status)
		{
			status = _status;
			return this;
		}
		
		public MSG Update(DateTime _dt)
		{
			dt = _dt;return this;
		}
		
        public override string ToString()
        {
            return "";
        }
    }

    public class RDS : MSG
    {
		public int consecutiveInterval = 100; 
        public RDS() : base(CTXType.RDS)
        {
        }
		
		public RDS(RDS other):base(other)
        {
			consecutiveInterval = other.consecutiveInterval;
        }
		
        public override string ToString()
        {
            string strMsgId = msgId.ToString("D10");
            ushort crc16 = Common.ComputeCRC(Encoding.ASCII.GetBytes(nameof(CTX) + strMsgId));
            return $"{STX.ToString("X2")}|{crc16}|{nameof(CTX)}|{msgId.ToString("D10")}|{ETX.ToString("X2")}";
        }
    }
    public class WTK : MSG
    {
        public int taskMode;//2
        public ushort taskId = 0;//10
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
        public int toDepth;//1
        public int boxLength;//4
        public int boxWidth;//4

        public int taskReserved5;//2
        public int taskReserved4;//2
        public int boxHeight;//4

        public byte workMode = 0;
		
        public WTK() : base(CTXType.WTK)
        {
			Interval = 3000;
        }

		public WTK(WTK other) : base(other)
		{
			taskMode = other.taskMode;//2
			taskId =  other.taskId;//10
			fromRow= other.fromRow;//2
			fromCol= other.fromCol;//4

			fromColOffsetDir= other.fromColOffsetDir;//1
			fromColOffset= other.fromColOffset;//4
			fromLayer= other.fromLayer;//2
			fromDepthMax= other.fromDepthMax;//1
			fromDepth= other.fromDepth;//1
			toRow= other.toRow;//2
			toCol= other.toCol;//4
			toColOffsetDir= other.toColOffsetDir;//1
			toColOffset= other.toColOffset;//4

			toLayer= other.toLayer;//2
			toDepthMax= other.toDepthMax;//1
			toDepth= other.toDepth;//1
			boxLength= other.boxLength;//4
			boxWidth= other.boxWidth;//4

			taskReserved5= other.taskReserved5;//2
			taskReserved4= other.taskReserved4;//2
			boxHeight= other.boxHeight;//4

			workMode= other.workMode;
		}
		
		public WTK assignTaskId()
		{
			if(workMode == 0)
			{
				taskId = Common.unique2(); 
			}else{
				taskId = Common.unique3(); 
			}
			
			return this;
		}
		
        public override string ToString()
        {
            string strMsgId = msgId.ToString("D10");

            string strData = msgId.ToString("D10") + taskMode.ToString("D2") + taskId.ToString("D10") + fromRow.ToString("D2") + fromCol.ToString("D4") + fromColOffsetDir.ToString("D1") + fromColOffset.ToString("D4") + fromLayer.ToString("D2") + fromDepthMax.ToString("D1") + fromDepth.ToString("D1") + toRow.ToString("D2") + toCol.ToString("D4") + toColOffsetDir.ToString("D1") + toColOffset.ToString("D4") + toLayer.ToString("D2") + toDepthMax.ToString("D1") + toDepth.ToString("D1") + boxLength.ToString("D4") + boxWidth.ToString("D4") + taskReserved5.ToString("D2") + taskReserved4.ToString("D2") + boxHeight.ToString("D4");

            ushort crc16 = Common.ComputeCRC(Encoding.ASCII.GetBytes(nameof(CTX) + strData));
            return $"{STX.ToString("X2")}|{crc16}|{nameof(CTX)}|{strData}|{ETX.ToString("X2")}";
        }
    }
    
    public class ResponseMSG
    {
        public ushort msgId;
        public DateTime dt;
        public bool isCorrect;
		
        public ResponseMSG()
        {
            dt = DateTime.Now;
        }

        public  static ResponseMSG Parse(byte[] data)
        {
			ResponseMSG msg = new ResponseMSG();
			
			try{
				msg.msgId = Common.ReadFromArray(data, 0, 10);
                msg.isCorrect = true;
			}catch(Exception)
			{
                msg.isCorrect = false;
			}           
            
            return msg;
        }
    }

    public class RTS : ResponseMSG
    {
        public RTS():base()
        {
        }
        
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

				_rts.dt = DateTime.Now;
                _rts.isCorrect = true;

            }
            catch (Exception)
            {
                _rts.isCorrect = false;
            }

            return _rts;
        }
    }

    public class RTK : ResponseMSG
    {
        public RTK() : base() { }

        public ushort taskId;//10
        public int recvResult;


        public static RTK Parse(byte[] data)
        {
            RTK _rtk = new RTK();
            try
            {
                _rtk.msgId = Common.ReadFromArray(data, 0, 10);
                _rtk.taskId = Common.ReadFromArray(data, 10, 10);
                _rtk.recvResult = Common.ReadFromArray(data, 20, 1);
				
				_rtk.dt = DateTime.Now;
                _rtk.isCorrect = true;
            }
            catch (Exception e)
            {
                _rtk.isCorrect = false;
            }

            return _rtk;
        }
    }

	public class MSG_GROUP : List<MSG>{
		public ProductLookup product;
		public int status=0;
		public int totalMSG =0;
	}

    public class MSGs : List<MSG>
    {
        public string location;
        public int status = 0;
        public int lane;
        public int totalMSG = 0;
    }

    public class InboundStatus : ProductLookup{
		public InboundStatus(){}
		public InboundStatus(ProductLookup other, int _status,int _subProcess,int _remainsProcess){
            status = _status;
			subProcess = _subProcess;
			curProcess=_subProcess - _remainsProcess;
			
			ID = other.ID;
			ProductID = other.ProductID;
			Image = other.Image;
			SKU =other.SKU;
			Barcodes = other.Barcodes;
		}
		public int status=0; //0:init,1:Accept,2:Accept new task,3:confirmed,4:progress,5:complete,10:error in accepting,20:error in working
		public int subProcess;
		public int curProcess;
	}

    public class OutboundStatus
    {
        public OutboundStatus() { 
            
        }
        public OutboundStatus(string _loc, int _laneIndex,int _status)
        {
            location = _loc;
            laneIndex = _laneIndex;
            status = _status;
        }
        public string location;
        public int laneIndex;
        public int status = 0; 
    }
}
