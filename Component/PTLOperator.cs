using ASRS.libs;
using LIBS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASRS.Component
{
    public partial class PTLOperator : UserControl
    {
        public OutboundOperatorEvents dispatcher;

        DateTime pressedDt = DateTime.Now;
        int pressedBayID = -1;
        bool on_off = false;
        int ptl_flash_counter = 0;
        
        private string originalLocation ="";
        public List<MSGs> instructions = new List<MSGs>();

        Task _idleMSGProcessor = null;
        CancellationTokenSource idleTokenSource = null;
        CancellationToken _idletoken;

        System.Windows.Forms.Timer PTL_Flash_timer = new System.Windows.Forms.Timer();

        public PTLOperator()
        {
            InitializeComponent();

            PTL_Flash_timer.Interval = 1000;
            PTL_Flash_timer.Tick += PTL_Flash;

            idleTokenSource = new CancellationTokenSource();
            _idletoken = idleTokenSource.Token;
            _idletoken.Register(() =>
            {
                try
                {
                    _idleMSGProcessor.Dispose();
                }
                catch (Exception)
                {

                }
            });
            _idleMSGProcessor = Task.Run(IdleProcessing, _idletoken);
        }  

        public void PTLSwitch_Pressed(int BayID)
        {
            originalLocation = "";
            Invoke((Action)(() =>
            {
                lblPTL_Switch.Text = $"OUTBOUND PTL_ID is {BayID.ToString()}";
                if (pressedBayID == BayID && (DateTime.Now - pressedDt).TotalSeconds < 10)
                {
                    PTL_LIGHT_OFF(pressedBayID);
                    PTL_Flash_timer.Stop();
                    return;
                }

                pressedBayID = BayID;
                PTL_Bay ptl = Manager.AppOwner.ptls.Find(x => x.BayID == BayID.ToString());
                if (ptl != null)
                {
                    var inventory = Manager.AppOwner.inventorys.FindAll(x => x.getSKU() == ptl.SKU_Assigned).OrderBy(I => I.getTimeIN()).FirstOrDefault();

                    if (inventory != null)
                    {
                        originalLocation = inventory.getLocation_RowCol();
                        PTL_Flash_timer.Stop();
                        PTL_LIGHT_ON(BayID, 1, 20);
                    }
                    else
                    {
                        ptl_flash_counter = 0;
                        on_off = false;

                        PTL_Flash_timer.Start();
                    }
                }
            }));
        }

        public void Sensor_Lane_FULL()
        {
            //back bring box to lane.
            MSGs curMSGGROUP = instructions.Find(i => i.status == 1);
            if(curMSGGROUP != null)
            {
                
                WTK w = createWTK(Manager.AppOwner.subSystem.lanes[curMSGGROUP.lane],Common.ConvertToPos(curMSGGROUP.location));
                curMSGGROUP.Add(w);
            }
        }

        public void PTLdeviceButtonPressed(int laneIndex)
        {
            if(originalLocation!= "")
            {
                MSGs mg = new MSGs();
                mg.location = originalLocation;
                mg.lane = laneIndex;

                WTK w = createWTK(Common.ConvertToPos(originalLocation), Manager.AppOwner.subSystem.lanes[laneIndex]);

                mg.Add(w); mg.totalMSG = 1;
                instructions.Add(mg);
            }
        }

        public void Gecko_StatusChanged(object sender, RTS e)
        {
            MSGs curMSGGROUP = instructions.Find(i => i.status == 2);
            if (curMSGGROUP == null) return;

            MSG curMSG = curMSGGROUP.Find(i => ((WTK)i).taskId == e.taskId);
            if (curMSG == null) return;

            if (e.taskStatus == 4)//completed
            {
                curMSGGROUP.Remove(curMSG);

                if (curMSGGROUP.Count() == 0)
                {
                    instructions.Remove(curMSGGROUP);
                }
            }

            dispatcher?.HandleStorageChanged(this, new OutboundStatus(curMSGGROUP.location, curMSGGROUP.lane, e.taskStatus + 1));
        }

        public void Gecko_RecvWTK(object sender, RTK e)
        {
            MSGs curMSGGROUP = instructions.Find(i => i.status == 1);
            MSG curMSG = curMSGGROUP.Find(i => ((WTK)i).taskId == e.taskId);

            //curMSGGROUP.Remove(curMSG);

            if (e.recvResult > 0)
            {
                curMSGGROUP.status = 3; //Error while direct to Gecko

                dispatcher?.HandleStorageChanged(this, new OutboundStatus(curMSGGROUP.location, curMSGGROUP.lane,10));
                instructions.Remove(curMSGGROUP);
            }
            else
            {
                curMSGGROUP.status = 2;
                dispatcher?.HandleStorageChanged(this, new OutboundStatus(curMSGGROUP.location, curMSGGROUP.lane, 1));
            }
        }

        private async void IdleProcessing()
        {
            while (!_idletoken.IsCancellationRequested)
            {
                await Task.Delay(100, _idletoken).ConfigureAwait(false);
                if (Manager.AppOwner.gecko.isWTKAvaiable())
                {
                    if (instructions.FindAll(item => item.status > 0).Count() == 0)
                    {
                        foreach (MSGs mg in instructions)
                        {
                            if (mg.Count > 0)
                            {
                                mg.status = 1;
                                Manager.AppOwner.gecko.setWTK((WTK)mg[0]);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void PTL_Flash(object sender, EventArgs e)
        {
            if (ptl_flash_counter++ < 20)
            {
                on_off = !on_off;
                if (on_off)
                    PTL_LIGHT_ON(pressedBayID, 2, 1);
                else
                    PTL_LIGHT_OFF(pressedBayID);

            }
            else
            {
                PTL_Flash_timer.Stop();
            }
        }
        
        private void PTL_LIGHT_ON(int BayID, int Color, int delay)
        {
            if (Color == 1)
                lblPTLSwitch_Status.Image = global::ASRS.Properties.Resources.normal;
            else if (Color == 2)
                lblPTLSwitch_Status.Image = global::ASRS.Properties.Resources.fault;

            Invalidate();
        }

        private void PTL_LIGHT_OFF(int BayID)
        {
            lblPTLSwitch_Status.Image = global::ASRS.Properties.Resources.no_action;

            Invalidate();
        }
        private WTK createWTK(Pos toDest)
        {
            return createWTK(new Pos()
            {
                row = 0,
                col = 0,
                deep = 1
            }, toDest);
        }

        private WTK createWTK(Pos fromSrc, Pos toDest)
        {
            WTK wtk = new WTK()
            {
                taskMode = 2,
                taskId = Common.unique2(),
                fromRow = fromSrc.row,
                fromCol = fromSrc.col,
                fromColOffsetDir = 0,
                fromColOffset = 0,
                fromLayer = 1,
                fromDepthMax = 0,
                fromDepth = fromSrc.deep,
                toRow = toDest.row,
                toCol = toDest.col,
                toColOffsetDir = 0,
                toColOffset = 0,
                toLayer = 1,
                toDepthMax = 2,
                toDepth = toDest.deep,
                boxLength = 18,
                boxWidth = 36,
                taskReserved5 = 0,
                taskReserved4 = 0,
                boxHeight = 44,
                dt = DateTime.Now,
            };

            return wtk;
        }
    }
}
