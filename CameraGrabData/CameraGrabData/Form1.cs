using MvCodeReaderSDKNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraGrabData
{
    public partial class Form1 : Form
    {
      
        MvCodeReader.MV_CODEREADER_DEVICE_INFO_LIST m_stDeviceList = new MvCodeReader.MV_CODEREADER_DEVICE_INFO_LIST();
        private MvCodeReader m_MyCamera = new MvCodeReader();

        //start
        bool m_bGrabbing = false;
        Thread m_hReceiveThread = null;
        MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2 m_stFrameInfo = new MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2();

        //thread
        byte[] m_BufForDriver = new byte[1024 * 1024 * 20];
        Bitmap bmp = null;
        //Graphics gra = null;
        Pen pen = new Pen(Color.Blue, 3);                   // 画笔颜色
        Point[] stPointList = new Point[4];                 // 条码位置的4个点坐标
        GraphicsPath WayShapePath = new GraphicsPath();     // 图形路径，内部变量 
        GraphicsPath OcrShapePath = new GraphicsPath();     // 图形路径，内部变量
        Matrix stRotateWay = new Matrix();
        Matrix stRotateM = new Matrix();
        Pen penOcr = new Pen(Color.Yellow, 3);
        Pen penWay = new Pen(Color.Red, 3);

        public Form1()
        {
            InitializeComponent();
        }

        private void txtSearchDevice_Click(object sender, EventArgs e)
        {
            // ch:创建设备列表 | en:Create Device List
            System.GC.Collect();
            //cbDeviceList.Items.Clear();
            m_stDeviceList.nDeviceNum = 0;
            int nRet = MvCodeReader.MV_CODEREADER_EnumDevices_NET(ref m_stDeviceList, MvCodeReader.MV_CODEREADER_GIGE_DEVICE);
            if (0 != nRet)
            {
                MessageBox.Show("Enumerate devices fail!", nRet.ToString());
                return;
            }

            if (0 == m_stDeviceList.nDeviceNum)
            {
                MessageBox.Show("None Device!");
                return;
            }

            // ch:在窗体列表中显示设备名 | en:Display device name in the form list
            for (int i = 0; i < m_stDeviceList.nDeviceNum; i++)
            {
                MvCodeReader.MV_CODEREADER_DEVICE_INFO device = (MvCodeReader.MV_CODEREADER_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[i], typeof(MvCodeReader.MV_CODEREADER_DEVICE_INFO));
                if (device.nTLayerType == MvCodeReader.MV_CODEREADER_GIGE_DEVICE)
                {
                    IntPtr buffer = Marshal.UnsafeAddrOfPinnedArrayElement(device.SpecialInfo.stGigEInfo, 0);
                    MvCodeReader.MV_CODEREADER_GIGE_DEVICE_INFO gigeInfo = (MvCodeReader.MV_CODEREADER_GIGE_DEVICE_INFO)Marshal.PtrToStructure(buffer, typeof(MvCodeReader.MV_CODEREADER_GIGE_DEVICE_INFO));
                    if (gigeInfo.chUserDefinedName != "")
                    {
                        MessageBox.Show("Get Device 1");
                        //cbDeviceList.Items.Add("GEV: " + gigeInfo.chUserDefinedName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                    else
                    {
                        MessageBox.Show("Get Device 2");
                        //cbDeviceList.Items.Add("GEV: " + gigeInfo.chManufacturerName + " " + gigeInfo.chModelName + " (" + gigeInfo.chSerialNumber + ")");
                    }
                }
            }

            //// ch:选择第一项 | en:Select the first item
            //if (m_stDeviceList.nDeviceNum != 0)
            //{
            //    cbDeviceList.SelectedIndex = 0;
            //}
        }

        private void btnOpenDevice_Click(object sender, EventArgs e)
        {
            if (m_stDeviceList.nDeviceNum == 0)
            {
                MessageBox.Show("No device, please select");
                return;
            }

            // ch:获取选择的设备信息 | en:Get selected device information
            MvCodeReader.MV_CODEREADER_DEVICE_INFO device =
                (MvCodeReader.MV_CODEREADER_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[0],
                                                              typeof(MvCodeReader.MV_CODEREADER_DEVICE_INFO));

            // ch:打开设备 | en:Open device
            if (null == m_MyCamera)
            {
                m_MyCamera = new MvCodeReader();
                if (null == m_MyCamera)
                {
                    return;
                }
            }

            int nRet = m_MyCamera.MV_CODEREADER_CreateHandle_NET(ref device);
            if (MvCodeReader.MV_CODEREADER_OK != nRet)
            {
                return;
            }

            nRet = m_MyCamera.MV_CODEREADER_OpenDevice_NET();
            if (MvCodeReader.MV_CODEREADER_OK != nRet)
            {
                m_MyCamera.MV_CODEREADER_DestroyHandle_NET();
                MessageBox.Show("Device open fail!", nRet.ToString());
                return;
            }

            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            m_MyCamera.MV_CODEREADER_SetEnumValue_NET("TriggerMode", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_MODE.MV_CODEREADER_TRIGGER_MODE_OFF);

            //bnGetParam_Click(null, null);// ch:获取参数 | en:Get parameters

            // ch:控件操作 | en:Control operation
            //SetCtrlWhenOpen();
        }

        private void btnSetTrigger_Click(object sender, EventArgs e)
        {
            int nRet = m_MyCamera.MV_CODEREADER_SetEnumValue_NET("TriggerMode", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_MODE.MV_CODEREADER_TRIGGER_MODE_ON);
            if (MvCodeReader.MV_CODEREADER_OK != nRet)
            {
                MessageBox.Show("Set TriggerMode On Fail!", nRet.ToString());
                //bnContinuesMode.Checked = true;
                return;
            }

            // ch:触发源选择:0 - Line0; | en:Trigger source select:0 - Line0;
            //           1 - Line1;
            //           2 - Line2;
            //           3 - Line3;
            //           4 - Counter;
            //           7 - Software;
            //if (cbSoftTrigger.Checked)
            //{
            //    nRet = m_MyCamera.MV_CODEREADER_SetEnumValue_NET("TriggerSource", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_SOURCE.MV_CODEREADER_TRIGGER_SOURCE_SOFTWARE);
            //    if (MvCodeReader.MV_CODEREADER_OK != nRet)
            //    {
            //        ShowErrorMsg("Set TriggerMode Source SoftWare Fail!", nRet);
            //        return;
            //    }

            //    if (m_bGrabbing)
            //    {
            //        bnTriggerExec.Enabled = true;
            //    }
            //}
            //else
            //{
            nRet = m_MyCamera.MV_CODEREADER_SetEnumValue_NET("TriggerSource", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_SOURCE.MV_CODEREADER_TRIGGER_SOURCE_LINE0);
            if (MvCodeReader.MV_CODEREADER_OK != nRet)
            {
                MessageBox.Show("Set TriggerMode Source Line0 Fail!", nRet.ToString());
                return;

            }
            //}
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // ch:标志位置位true | en:Set position bit true
            m_bGrabbing = true;

            m_hReceiveThread = new Thread(ReceiveThreadProcess);
            m_hReceiveThread.Start();

            m_stFrameInfo.nFrameLen = 0;//取流之前先清除帧长度
            m_stFrameInfo.enPixelType = MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Undefined;
            // ch:开始采集 | en:Start Grabbing
            int nRet = m_MyCamera.MV_CODEREADER_StartGrabbing_NET();
            if (MvCodeReader.MV_CODEREADER_OK != nRet)
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join();
                MessageBox.Show("Start Grabbing Fail!", nRet.ToString());
                return;
            }

            //    // ch:控件操作 | en:Control Operation
            //    SetCtrlWhenStartGrab();
            //    bnContinuesMode.Enabled = false;
            //    bnTriggerMode.Enabled = false;
            //}
        }
        public void ReceiveThreadProcess()
        {
            MvCodeReader.MV_CODEREADER_INTVALUE_EX stParam = new MvCodeReader.MV_CODEREADER_INTVALUE_EX();
            int nRet = MvCodeReader.MV_CODEREADER_OK;

            IntPtr pData = IntPtr.Zero;
            MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2 stFrameInfo = new MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2();
            IntPtr pstFrameInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2)));
            Marshal.StructureToPtr(stFrameInfo, pstFrameInfo, false);

            while (m_bGrabbing)
            {
                nRet = m_MyCamera.MV_CODEREADER_GetOneFrameTimeoutEx2_NET(ref pData, pstFrameInfo, 100);
                if (nRet == MvCodeReader.MV_CODEREADER_OK)
                {
                    stFrameInfo = (MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2)Marshal.PtrToStructure(pstFrameInfo, typeof(MvCodeReader.MV_CODEREADER_IMAGE_OUT_INFO_EX2));
                    m_stFrameInfo = stFrameInfo;
                }

                if (nRet == MvCodeReader.MV_CODEREADER_OK)
                {
                    if (0 >= stFrameInfo.nFrameLen)
                    {
                        continue;
                    }
                    MessageBox.Show("get an Image2");
                    // 绘制图像
                    Marshal.Copy(pData, m_BufForDriver, 0, (int)stFrameInfo.nFrameLen);
                    if (stFrameInfo.enPixelType == MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Mono8)
                    {
                        IntPtr pImage = Marshal.UnsafeAddrOfPinnedArrayElement(m_BufForDriver, 0);
                        bmp = new Bitmap(stFrameInfo.nWidth, stFrameInfo.nHeight, stFrameInfo.nWidth, PixelFormat.Format8bppIndexed, pImage);
                        ColorPalette cp = bmp.Palette;
                        for (int i = 0; i < 256; i++)
                        {
                            cp.Entries[i] = Color.FromArgb(i, i, i);
                        }
                        bmp.Palette = cp;

                        //pictureBox1.Image = (Image)bmp;
                        MessageBox.Show("get an Image");
                    }
                    else if (stFrameInfo.enPixelType == MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Jpeg)
                    {
                        GC.Collect();
                        MemoryStream ms = new MemoryStream();
                        ms.Write(m_BufForDriver, 0, (int)stFrameInfo.nFrameLen);

                        //pictureBox1.Image = Image.FromStream(ms);
                        MessageBox.Show("get an Image");
                    }

                    //MvCodeReader.MV_CODEREADER_RESULT_BCR_EX stBcrResult = (MvCodeReader.MV_CODEREADER_RESULT_BCR_EX)Marshal.PtrToStructure(stFrameInfo.pstCodeListEx, typeof(MvCodeReader.MV_CODEREADER_RESULT_BCR_EX));

                    //pictureBox1.Refresh();
                    //for (int i = 0; i < stBcrResult.nCodeNum; ++i)
                    //{
                    //    for (int j = 0; j < 4; ++j)
                    //    {
                    //        stPointList[j].X = (int)(stBcrResult.stBcrInfoEx[i].pt[j].x * (float)(pictureBox1.Size.Width) / stFrameInfo.nWidth);
                    //        stPointList[j].Y = (int)(stBcrResult.stBcrInfoEx[i].pt[j].y * (float)(pictureBox1.Size.Height) / stFrameInfo.nHeight);
                    //    }
                    //    gra.DrawPolygon(pen, stPointList);
                    //}

                    //MvCodeReader.MV_CODEREADER_WAYBILL_LIST stWayList = (MvCodeReader.MV_CODEREADER_WAYBILL_LIST)Marshal.PtrToStructure(stFrameInfo.pstWaybillList, typeof(MvCodeReader.MV_CODEREADER_WAYBILL_LIST));

                    //for (int i = 0; i < stWayList.nWaybillNum; ++i)
                    //{
                    //    float fWayX = (float)(stWayList.stWaybillInfo[i].fCenterX * (float)(pictureBox1.Size.Width) / stFrameInfo.nWidth);
                    //    float fWayY = (float)(stWayList.stWaybillInfo[i].fCenterY * (float)(pictureBox1.Size.Height) / stFrameInfo.nHeight);
                    //    float fWayW = (float)(stWayList.stWaybillInfo[i].fWidth * (float)(pictureBox1.Size.Width) / stFrameInfo.nWidth);
                    //    float fWayH = (float)(stWayList.stWaybillInfo[i].fHeight * (float)(pictureBox1.Size.Height) / stFrameInfo.nHeight);

                    //    WayShapePath.Reset();
                    //    WayShapePath.AddRectangle(new RectangleF(fWayX - fWayW / 2, fWayY - fWayH / 2, fWayW, fWayH));

                    //    stRotateWay.Reset();
                    //    PointF stCenPoint = new PointF(fWayX, fWayY);
                    //    stRotateWay.RotateAt(stWayList.stWaybillInfo[i].fAngle, stCenPoint);
                    //    WayShapePath.Transform(stRotateWay);
                    //    gra.DrawPath(penWay, WayShapePath);
                    //}

                    //MvCodeReader.MV_CODEREADER_OCR_INFO_LIST stOcrInfo = (MvCodeReader.MV_CODEREADER_OCR_INFO_LIST)Marshal.PtrToStructure(stFrameInfo.UnparsedOcrList.pstOcrList, typeof(MvCodeReader.MV_CODEREADER_OCR_INFO_LIST));

                    //for (int i = 0; i < stOcrInfo.nOCRAllNum; ++i)
                    //{
                    //    float fOcrInfoX = (float)(stOcrInfo.stOcrRowInfo[i].nOcrRowCenterX * (float)(pictureBox1.Size.Width) / stFrameInfo.nWidth);
                    //    float fOcrInfoY = (float)(stOcrInfo.stOcrRowInfo[i].nOcrRowCenterY * (float)(pictureBox1.Size.Height) / stFrameInfo.nHeight);
                    //    float fOcrInfoW = (float)(stOcrInfo.stOcrRowInfo[i].nOcrRowWidth * (float)(pictureBox1.Size.Width) / stFrameInfo.nWidth);
                    //    float fOcrInfoH = (float)(stOcrInfo.stOcrRowInfo[i].nOcrRowHeight * (float)(pictureBox1.Size.Height) / stFrameInfo.nHeight);

                    //    OcrShapePath.Reset();
                    //    OcrShapePath.AddRectangle(new RectangleF(fOcrInfoX - fOcrInfoW / 2, fOcrInfoY - fOcrInfoH / 2, fOcrInfoW, fOcrInfoH));

                    //    stRotateM.Reset();
                    //    PointF stCenPoint = new PointF(fOcrInfoX, fOcrInfoY);
                    //    stRotateM.RotateAt(stOcrInfo.stOcrRowInfo[i].fOcrRowAngle, stCenPoint);
                    //    OcrShapePath.Transform(stRotateM);
                    //    gra.DrawPath(penOcr, OcrShapePath);
                    //}
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int val = 0;
            for (int i=0;i<=50;i++)
            {
                if(0==0){
                    if (0 >= val)
                    {
                        continue;
                    }
                }
               
            }
            

            int res = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAllInOne_Click(object sender, EventArgs e)
        {
            #region Search Device
            //search device
            System.GC.Collect();
            //cbDeviceList.Items.Clear();
            m_stDeviceList.nDeviceNum = 0;
            int nRet = MvCodeReader.MV_CODEREADER_EnumDevices_NET(ref m_stDeviceList, MvCodeReader.MV_CODEREADER_GIGE_DEVICE);
            if (0 != nRet)
            {
                MessageBox.Show("Enumerate devices fail!", nRet.ToString());
                return;
            }

            if (0 == m_stDeviceList.nDeviceNum)
            {
                MessageBox.Show("None Device!");
                return;
            }

            #endregion

            #region Open Device
            //open device
            if (m_stDeviceList.nDeviceNum == 0)
            {
                MessageBox.Show("No device, please select");
                return;
            }

            // ch:获取选择的设备信息 | en:Get selected device information
            MvCodeReader.MV_CODEREADER_DEVICE_INFO device =
                (MvCodeReader.MV_CODEREADER_DEVICE_INFO)Marshal.PtrToStructure(m_stDeviceList.pDeviceInfo[0],
                                                              typeof(MvCodeReader.MV_CODEREADER_DEVICE_INFO));

            // ch:打开设备 | en:Open device
            if (null == m_MyCamera)
            {
                m_MyCamera = new MvCodeReader();
                if (null == m_MyCamera)
                {
                    return;
                }
            }

            int nRet2 = m_MyCamera.MV_CODEREADER_CreateHandle_NET(ref device);
            if (MvCodeReader.MV_CODEREADER_OK != nRet2)
            {
                return;
            }

            nRet2 = m_MyCamera.MV_CODEREADER_OpenDevice_NET();
            if (MvCodeReader.MV_CODEREADER_OK != nRet2)
            {
                m_MyCamera.MV_CODEREADER_DestroyHandle_NET();
                MessageBox.Show("Device open fail!", nRet2.ToString());
                return;
            }
            #endregion

            #region Trigger Mode
            //trigger Mode on
            int nRet3 = m_MyCamera.MV_CODEREADER_SetEnumValue_NET("TriggerMode", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_MODE.MV_CODEREADER_TRIGGER_MODE_ON);
            if (MvCodeReader.MV_CODEREADER_OK != nRet3)
            {
                MessageBox.Show("Set TriggerMode On Fail!", nRet3.ToString());
                //bnContinuesMode.Checked = true;
                return;
            }

            nRet3 = m_MyCamera.MV_CODEREADER_SetEnumValue_NET("TriggerSource", (uint)MvCodeReader.MV_CODEREADER_TRIGGER_SOURCE.MV_CODEREADER_TRIGGER_SOURCE_LINE0);
            if (MvCodeReader.MV_CODEREADER_OK != nRet3)
            {
                MessageBox.Show("Set TriggerMode Source Line0 Fail!", nRet3.ToString());
                return;

            }
            #endregion

            #region Start
            // ch:标志位置位true | en:Set position bit true
            m_bGrabbing = true;

            m_hReceiveThread = new Thread(ReceiveThreadProcess);
            m_hReceiveThread.Start();

            m_stFrameInfo.nFrameLen = 0;//取流之前先清除帧长度
            m_stFrameInfo.enPixelType = MvCodeReader.MvCodeReaderGvspPixelType.PixelType_CodeReader_Gvsp_Undefined;
            // ch:开始采集 | en:Start Grabbing
            int nRet4 = m_MyCamera.MV_CODEREADER_StartGrabbing_NET();
            if (MvCodeReader.MV_CODEREADER_OK != nRet4)
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join();
                MessageBox.Show("Start Grabbing Fail!", nRet4.ToString());
                return;
            }
            #endregion

        }

        private void btnCloseDevice_Click(object sender, EventArgs e)
        {
            // ch:取流标志位清零 | en:Reset flow flag bit
            if (m_bGrabbing == true)
            {
                m_bGrabbing = false;
                m_hReceiveThread.Join();

                // ch:停止采集 | en:Stop Grabbing
                int nRet = m_MyCamera.MV_CODEREADER_StopGrabbing_NET();
                if (nRet != MvCodeReader.MV_CODEREADER_OK)
                {
                    MessageBox.Show("Stop Grabbing Fail!", nRet.ToString());
                }
            }

            // ch:关闭设备 | en:Close Device
            m_MyCamera.MV_CODEREADER_CloseDevice_NET();
            m_MyCamera.MV_CODEREADER_DestroyHandle_NET();

            //// ch:控件操作 | en:Control Operation
            //SetCtrlWhenClose();
        }
    }
}
