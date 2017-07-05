using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using mshtml;
using CefSharp;
using CefSharp.WinForms;
using System.Globalization;
using NAudio.Wave;
using System.Timers;
using System.IO;
using System.Net.Http;
using System.Collections.Specialized;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Gma.System.MouseKeyHook;

namespace SmartTV
{

    public partial class Form1 : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        HttpFileUpload httpFileUpload;


        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        static string endpoint = "http://140.124.182.77";
        static string personalPagePath = "/personal_page.html";
        static string voiceRecognitionPath = "/voice_recognition.html";
        static string questionPath = "/question.html";
        static string viewVideoPath = "/view_video.html";
        static string selectProgramPath = "/select_program.html";

        static string voiceEndpoint = "http://140.124.182.49";
        static string recognition_request_api = voiceEndpoint + ":8008/Home/Upload";
        static string recognition_response_api = voiceEndpoint + ":8008/api/values";
        static string question_request_api = voiceEndpoint + ":8009/Home/Upload";
        static string question_response_api = voiceEndpoint + ":8009/api/values";
        static string keyword_request_api = voiceEndpoint + ":8010/Home/Upload";
        static string keyword_response_api = voiceEndpoint + ":8010/api/values";
        string voice_type = "recognition";
        
        int nfc_id = -1;

        string file = "temp.txt";

        Process vision;
        Process vision_back;

        Boolean listenCommand = true;
        private string lastCommand = "";

        private string keywords = "";

        private int _period = 1000;
        private int _countDown = 0;
        private int _cursorStallTime = 0;
        private System.Timers.Timer _timer;

        public WaveInEvent waveSource = null;
        public WaveFileWriter waveFile = null;

        private int recordPeriod = 0;
        private System.Timers.Timer aTimer;
        private string recordPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/temp_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".wav";

        private bool mouseOver = false;
        private bool menuOpen = false;

        enum FaceStatus {NotDetect = 0, Detect};
        enum FacePosition {Center, Near, Left, Right };
        private FaceStatus faceStatus;
        private FacePosition facePosition;
        enum HandStatus {NotDetect = 0, Detect}
        enum HandCalibrationStatus {NotCalibrated, Calibrated}
        enum HandPosition {inside, outside};
        private HandStatus leftHandStatus, rightHandStatus;
        private HandCalibrationStatus handCalibrationStatus;
        private HandPosition handPosition;
        

        private bool _CursorShown = true;
        public bool CursorShown
        {
            get
            {
                return _CursorShown;
            }
            set
            {
                if (value == _CursorShown)
                {
                    return;
                }

                if (value)
                {
                    System.Windows.Forms.Cursor.Show();
                }
                else
                {
                    System.Windows.Forms.Cursor.Hide();
                }

                _CursorShown = value;
            }
        }

        void timer_Elapsed(object sender, EventArgs e)
        {
            listenCommand = false;
            _countDown -= (int)_timer.Interval;
            if (_countDown <= 0)
            {
                listenCommand = true;
                _timer.Stop();
            }
        }

        public ChromiumWebBrowser browser;

        public void InitBrowser()
        {
            CefSettings browser_setting = new CefSettings();
            browser_setting.SetOffScreenRenderingBestPerformanceArgs();
            Cef.Initialize(browser_setting);
            browser = new ChromiumWebBrowser(endpoint);
            this.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            browser.FrameLoadEnd += new EventHandler<FrameLoadEndEventArgs>(FrameLoadEnd);
            browser.LoadingStateChanged += OnLoadingStateChanged;
            //browser.Select();
            //SendKeys.SendWait("{ENTER}");
            invokeSendKeys += SendKeys.SendWait;
        }

        //A very basic example
        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            Console.WriteLine("OnLoadingStateChanged");
            if (browser.Address == endpoint + viewVideoPath)
            {
                if (args.IsLoading == false)
                {
                    const string script = "get_keyword();";

                    browser.EvaluateScriptAsync(script).ContinueWith(x =>
                    {
                        var response = x.Result;

                        if (response.Success && response.Result != null)
                        {

                            var keywords = (List<Object>)response.Result;
                            string digit = "$digit =";
                            for (int i = 0; i < keywords.Count(); i++)
                            {
                                digit += Regex.Replace(keywords[i].ToString(), @"\s+", "").ToUpper();
                                if (i < keywords.Count() - 1) digit += "|";
                            }
                            digit += "|bghmm;";
                            Console.WriteLine(digit);

                            //string file = "temp.txt";

                            //if (!File.Exists(file))
                            //{

                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }

                            using (StreamWriter sw = File.CreateText(file))
                            {
                                string preamp = "$preamp =I|PLEASE|MAY|CAN|WHAT'S|WHAT;";
                                string subject = "$subject =I|YOU|ME;";
                                string inquire = "$inquire =WANT|SEARCH|SHOW|GIVE|TALE|TELL|EXPLAIN|SEE|IS|WANNA;";
                                string postamp = "$postamp =THANKYOU|THANKS|PLEASE;";
                                string filler = "$filler =bghmm;";
                                sw.WriteLine(preamp);
                                sw.WriteLine(subject);
                                sw.WriteLine(inquire);
                                sw.WriteLine(digit);
                                sw.WriteLine(postamp);
                                sw.WriteLine(filler);
                                sw.WriteLine("( SIL ( [$preamp] [$inquire] [$subject] $digit [$postamp] ) SIL)");
                                sw.Close();
                            }
                                
                            NameValueCollection nvc = new NameValueCollection();
                            
                            /*
                            var result = httpFileUpload.UploadFileByHttpWebRequest("http://140.124.182.49:8010/Home/Upload_key",
                                file, "file", "text/plain", nvc);
                            Console.WriteLine("Result : " + result);
                            Console.WriteLine("send file");*/
                            //File.Copy(file, backupFile, true);


                                
                            //}
                            //--
                        }
                    });
                }
            }
        }

        public void FrameLoadEnd(object obj, FrameLoadEndEventArgs args)
        {
            browser.ExecuteScriptAsync("function injectJquery(){var e=document.createElement(\"script\");e.setAttribute(\"type\",\"text/javascript\"),e.setAttribute(\"src\",\"https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js\"),document.getElementsByTagName(\"head\")[0].appendChild(e)}");
            browser.ExecuteScriptAsync("injectJquery();");
            browser.ExecuteScriptAsync("function sendKeyCode(e){if (typeof $ !== 'function') return; var n=$.Event(\"keydown\");n.which=e,$(document).trigger(n)}");
            //browser.ShowDevTools();
            if (browser.Address == endpoint + voiceRecognitionPath)
            {
                Console.WriteLine(browser.Address);
                voice_type = "recognition";
                this.Invoke(this.invokeStatus, "openvoice");
                //startVoice();
            }
            else if (browser.Address == endpoint + viewVideoPath)
            {
                Console.WriteLine(browser.Address);
                voice_type = "keyword";

            }
            else if (browser.Address == endpoint + questionPath)
            {
                Console.WriteLine(browser.Address);
                voice_type = "question";
            }
        }

        public void startVoice()
        {
            if (aTimer != null && aTimer.Enabled == true) return;
            aTimer = new System.Timers.Timer();
        
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 100;
            aTimer.Enabled = true;
            //pictureBoxVoice.Visible = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (recordPeriod == 0)
            {
                waveSource = new WaveInEvent();
                waveSource.WaveFormat = new WaveFormat(16000, 8, 2);
                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
                waveSource.StartRecording();
            }
            if (recordPeriod == 3000)
            {
                waveSource.StopRecording();
                aTimer.Enabled = false;
                this.Invoke(this.invokeStatus, "openvoice end");
                recordPeriod = 0;
            }
            else
            {
                recordPeriod += 100;
            }
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            
            if (waveFile != null)
            {
                try
                {
                    waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                    waveFile.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (waveSource != null)
            {         
                recordPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/temp_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".wav";
                waveFile = new WaveFileWriter(recordPath, waveSource.WaveFormat);
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            //recordPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"/temp_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".wav";
            //waveFile = new WaveFileWriter(recordPath, waveSource.WaveFormat);
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile.Close();
                waveFile = null;
            }

            try
            {
                saveLog("send");

                if (File.Exists(recordPath))
                {
                    Console.WriteLine(voice_type);
                    if (voice_type == "recognition")
                    {
                        NameValueCollection nvc = new NameValueCollection();

                        var result = httpFileUpload.UploadFileByHttpWebRequest(recognition_request_api,
                            recordPath, "file", "audio/wav", nvc);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(recognition_response_api);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream resStream = response.GetResponseStream();
                        StreamReader objReader = new StreamReader(resStream);
                        String res = objReader.ReadToEnd();
                        //Console.Write(res);
                        saveLog(res);

                        dynamic test = JsonConvert.DeserializeObject(res);
                        if (test.status != null)
                        {
                            this.Invoke(this.invokeGoBack);
                        }
                        else
                        {
                            Console.Write(test.NFC_ID);
                            nfc_id = (int)test.NFC_ID;
                            this.Invoke(this.invokeKeyDown, nfc_id);
                        }
                    }
                    else if (voice_type == "question")
                    {
                        NameValueCollection nvc = new NameValueCollection();

                        var result = httpFileUpload.UploadFileByHttpWebRequest(question_request_api,
                            recordPath, "file", "audio/wav", nvc);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(question_response_api + '/' + nfc_id);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream resStream = response.GetResponseStream();
                        StreamReader objReader = new StreamReader(resStream);
                        String res = objReader.ReadToEnd();
                        Console.WriteLine(res);
                        saveLog(res);

                        dynamic test = JsonConvert.DeserializeObject(res);
                        if (test.status == null)
                        {
                            if (test.answer == "o" || test.answer == "O")
                            {
                                this.Invoke(this.invokeKeyDown, 38);
                            }
                            else if (test.answer == "A" || test.answer == "ONE")
                            {
                                this.Invoke(this.invokeKeyDown, 65);
                            }
                            else if (test.answer == "B" || test.answer == "TWO")
                            {
                                this.Invoke(this.invokeKeyDown, 66);
                            }
                            else if (test.answer == "C" || test.answer == "THREE")
                            {
                                this.Invoke(this.invokeKeyDown, 67);
                            }
                            else if (test.answer == "D" || test.answer == "FOUR")
                            {
                                this.Invoke(this.invokeKeyDown, 68);
                            }
                        }
                        else
                        {
                            this.Invoke(this.invokeKeyDown, 78);
                        }
                    }
                    else if (voice_type == "keyword")
                    {
                        //test
                        //this.Invoke(this.invokeKeyword, "basement");

                        NameValueCollection nvc = new NameValueCollection();

                        var result = httpFileUpload.UploadFileByHttpWebRequest(keyword_request_api,
                            recordPath, "file", "audio/wav", nvc);

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(keyword_response_api);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        Stream resStream = response.GetResponseStream();
                        StreamReader objReader = new StreamReader(resStream);
                        String res = objReader.ReadToEnd();
                        Console.WriteLine("Result : " + res);
                        saveLog(res);

                        dynamic test = JsonConvert.DeserializeObject(res);
                        if (test.status == null)
                        {
                            string keyword = test.keyword.GetType() == typeof(Newtonsoft.Json.Linq.JArray) ? test.keyword[0] : test.keyword;
                            //Console.WriteLine("Keyword : " + keyword);
                            this.Invoke(this.invokeKeyword, keyword);
                        }

                        var keywordResult = httpFileUpload.UploadFileByHttpWebRequest("http://140.124.182.49:8010/Home/Upload_key",
                        file, "file", "text/plain", nvc);
                        Console.WriteLine("send file keyword");
                    }



                    
                    //File.Delete(recordPath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void saveLog(string responseText)
        {
            string file = "audio_log.csv";
            string log = "";
            if (voice_type == "recognition")
            {
                log += "login";
            }
            else if (voice_type == "question")
            {
                log += "QA";
            }
            else
            {
                log += voice_type;
            }
            log += "," + recordPath + "," + DateTime.Now.ToString("yyyyMMddHHmmssffff") + "," + responseText;
            if (!File.Exists(file))
            {
                using (StreamWriter sw = File.CreateText(file))
                {
                    sw.WriteLine(log);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine(log);
                }
            }
            Console.WriteLine(log);
        }

        /// <summary>
        /// 上傳檔案至 Server 透過HttpWebRequest
        /// </summary>
        /// <param name="url">上傳網址</param>
        /// <param name="file">檔案位置</param>
        /// <param name="paramName">該control name</param>
        /// <param name="contentType">image type </param>
        /// <param name="nvc">其他參數</param>
        /// <returns></returns>
        

        private IKeyboardMouseEvents m_GlobalHook;

        public void Subscribe()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            m_GlobalHook = Hook.GlobalEvents();

            m_GlobalHook.KeyDown += GlobalHookKeyDown;
        }


        //Not working
        private void GlobalHookKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.R)
            {
                startVoice();
            }
            if (e.Control && e.KeyCode == Keys.M)
            {
                mouseOver = !mouseOver;
            }
        }

        public void Unsubscribe()
        {
            m_GlobalHook.KeyDown -= GlobalHookKeyDown;

            //It is recommened to dispose it
            m_GlobalHook.Dispose();
        }

        public Form1()
        {
            InitializeComponent();
            httpFileUpload = new HttpFileUpload();
            panel1.BackColor = Color.FromArgb(44, 46, 49);
            this.BackColor = Color.FromArgb(44, 46, 49);
            //pictureBoxRightHand.BackColor = Color.FromArgb(44, 46, 49);
            InitBrowser();
            //Subscribe();
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void Form1_Load(object sender, EventArgs ea)
        {
            //Start Guesture
            vision = new Process();
            vision.StartInfo.FileName = "../../../Vision/Vision.exe";
            //compiler.StartInfo.Arguments = "/r:System.dll /out:sample.exe stdstr.cs";
            vision.StartInfo.UseShellExecute = false;
            vision.StartInfo.CreateNoWindow = true;
            vision.StartInfo.RedirectStandardOutput = true;
            vision.OutputDataReceived += HandleGesture;
            vision.Start();
            
            //hread.Sleep(500);
            //SetParent(vision.MainWindowHandle, panel1.Handle);
            vision.BeginOutputReadLine();

            vision_back = new Process();
            vision_back.StartInfo.FileName = "../../../Vision-back/Vision.exe";
            vision_back.StartInfo.UseShellExecute = false;
            vision_back.StartInfo.CreateNoWindow = true;
            vision_back.StartInfo.RedirectStandardOutput = true;
            vision_back.OutputDataReceived += HandleGesture;
            vision_back.Start();
            vision_back.BeginOutputReadLine();

            invokeKeyDown += this.invokeKeyDownEvent;
            invokeGoBack += this.invokeGoBackEvent;
            invokeGoHome += this.invokeGoHomeEvent;
            invokeStatus += this.invokeStatusEvent;
            invokeKeyword += this.invokeKeywordEvent;
            invokeMouseClick += this.invokeMouseClickEvent;
            invokeShowMenu += this.invokeShowMenuEvent;
            invokeShowMsg += this.invokeShowMsgEvent;
            invokeShowHandsMsg += this.invokeShowHandsMsgEvent;
            invokeShowMask += this.invokeShowMaskEvent;

            menuPanel.Location = new Point((this.ClientSize.Width - menuPanel.Size.Width) / 2, (this.ClientSize.Height - menuPanel.Size.Height) / 2);
            Rectangle r = new Rectangle(0, 0, menuPanel.Width, menuPanel.Height);
            System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
            int d = menuPanel.Height;
            gp.AddArc(r.X, r.Y, d, d, 180, 90);
            gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
            gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
            gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);
            menuPanel.Region = new Region(gp);

            CursorShown = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (vision != null && !vision.HasExited)
                vision.Kill();
            if (vision_back != null && !vision_back.HasExited)
                vision_back.Kill();
            //Unsubscribe();
        }

        private delegate void invokeSendKeysThreadMethod(string keys);
        private event invokeSendKeysThreadMethod invokeSendKeys;

        private delegate void invokeThisThreadMethod(int keycode);
        private event invokeThisThreadMethod invokeKeyDown;

        private delegate void invokeThisThreadMethod2();
        private event invokeThisThreadMethod2 invokeGoBack;

        private delegate void invokeThisThreadMethod3();
        private event invokeThisThreadMethod3 invokeGoHome;

        private delegate void invokeThisThreadMethod4(string status);
        private event invokeThisThreadMethod4 invokeStatus;

        private delegate void invokeThisThreadMethod5(string keyword);
        private event invokeThisThreadMethod5 invokeKeyword;

        private delegate void invokeThisThreadMethod6(string mouse);
        private event invokeThisThreadMethod6 invokeMouseClick;

        private delegate void invokeThisThreadMethod7(bool enable);
        private event invokeThisThreadMethod7 invokeShowMenu;

        private delegate void invokeThisThreadMethod8(string msg);
        private event invokeThisThreadMethod8 invokeShowMsg;

        private delegate void invokeThisThreadMethodForHands(string msg);
        private event invokeThisThreadMethodForHands invokeShowHandsMsg;

        private delegate void invokeThisThreadMethod9(bool visible);
        private event invokeThisThreadMethod9 invokeShowMask;

        Guid showMenuTimer;
        private void keepShowMenu()
        {
            ClearTimeout(showMenuTimer);
            showMenuTimer = SetTimeout(() =>
            {
                this.Invoke(this.invokeShowMenu, false);
            }, 6000, this);
        }

        private void invokeShowMenuEvent(bool enable)
        {
            if (enable)
            {
                menuPanel.Enabled = true;
                menuPanel.Visible = true;
                menuOpen = true;

                this.Invoke(this.invokeShowMask, true);

                if (!menuOpen) CursorShown = true;

                ClearTimeout(showMenuTimer);
                showMenuTimer = SetTimeout(() =>
                {
                    this.Invoke(this.invokeShowMenu, false);
                }, 6000, this);
            }
            else
            {
                menuPanel.Enabled = false;
                menuPanel.Visible = false;
                menuOpen = false;

                this.Invoke(this.invokeShowMask, false);

                if (mouseOver)
                {
                    CursorShown = true;
                }
                else
                {
                    CursorShown = false;
                }
            }
        }

        private void invokeShowMsgEvent(string msg)
        {
            faceMsgLabel.Text = msg;
        }

        private void invokeShowHandsMsgEvent(string msg)
        {
            handsMsgLabel.Text = msg;
        }

        private void invokeShowMaskEvent(bool visible)
        {
            if (visible)
            {
                if (!browser.IsLoading)
                    browser.ExecuteScriptAsync("show_mask();");
            }
            else
            {
                if (!browser.IsLoading)
                    browser.ExecuteScriptAsync("close_mask();");
            }
        }

        private void invokeKeyDownEvent(int keycode)
        {
            if (mouseOver || menuOpen)
            {
                if (keycode == 13)
                {
                    this.Invoke(this.invokeMouseClick, "left");
                    return;
                }
                else if (!menuOpen && (keycode == 38 || keycode == 40))
                {
                    //na
                }
                else
                {
                    browser.ExecuteScriptAsync("sendKeyCode(" + keycode + ");");
                }
            }
            Console.WriteLine(keycode);
            if (!browser.IsLoading)
                browser.ExecuteScriptAsync("sendKeyCode(" + keycode + ");");
        }

        private void invokeGoBackEvent()
        {
            if (mouseOver) return;
            Console.WriteLine("GoBack");
            //browser.Back();
            if (!browser.IsLoading)
                browser.ExecuteScriptAsync("sendKeyCode(27);");
        }

        private void invokeGoHomeEvent()
        {
            if (mouseOver) return;
            Console.WriteLine("GoHome");
            if (browser.Address == endpoint + viewVideoPath)
                browser.Load(endpoint + personalPagePath);
            else
                browser.Load(endpoint + selectProgramPath);
        }
        private void invokeKeywordEvent(string keyword)
        {
            if (!browser.IsLoading)
                //Console.WriteLine("pop_keyword" + keyword);
                //keyword.ToLower();
                if (keyword == "bghmm")
                {
                    browser.ExecuteScriptAsync("sendKeyCode(69);");
                    Thread.Sleep(3000);
                    browser.ExecuteScriptAsync("sendKeyCode(68);");
                }
                else
                {
                    
                    browser.ExecuteScriptAsync("pop_keyword(" + "\"" + keyword + "\"" + ");");
                }
                
        }

        private void invokeMouseClickEvent(string mouse)
        {
            if (mouse == "left")
            {
                Console.WriteLine("left mouse click");
                uint X = (uint) Cursor.Position.X;
                uint Y = (uint) Cursor.Position.Y;
                mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
                SetTimeout(() =>
                {
                    mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                }, 100);
            }
        }

        private float fist_x = 0f;
        private float fist_y = 0f;
        private int fistOutCounter = 0;
        private System.Timers.Timer cursorStallTimer;
        private int cursorStallTime = 0;
        private DateTime lastStallTime;

        private void invokeStatusEvent(string status)
        {
            //Console.WriteLine("STATUS :" + status);
            string faceChineseMsg = "";
            string handChineseMsg = "";


            if (fistOutCounter <= 0)
            {
                fistOutCounter = 0;
            }
            else
            {
                fistOutCounter--;
            }
            switch (status)
            {
                case "openvoice":
                    startVoice();
                    pictureBoxVoice.Visible = true;
                    break;
                case "openvoice end":
                    pictureBoxVoice.Visible = false;
                    break;
                case "fist":
                    fistOutCounter = 2;
                    break;
                case "Face is detected":
                    //pictureBoxFace.Visible = true;
                    pictureBoxFace.Image = SmartTV.Properties.Resources.face_ac;
                    faceStatus = FaceStatus.Detect;
                    facePosition = FacePosition.Center;
                    break;
                case "Face is not detected":
                    //pictureBoxFace.Visible = false;
                    pictureBoxFace.Image = SmartTV.Properties.Resources.face;
                    faceStatus = FaceStatus.NotDetect;               
                    break;
                case "Face is too near":
                    faceStatus = FaceStatus.Detect;
                    facePosition = FacePosition.Near;
                    break;
                case "Face is too left":
                    facePosition = FacePosition.Left;
                    break;
                case "Face is too right":
                    facePosition = FacePosition.Right;
                    break;
                case "Left hand is detected":
                    //pictureBoxLeftHand.Visible = true;
                    //pictureBoxRightHand.Visible = false;
                    pictureBoxLeftHand.Image = SmartTV.Properties.Resources.l_hand_ac;
                    pictureBoxRightHand.Image = SmartTV.Properties.Resources.r_hand;
                    leftHandStatus = HandStatus.Detect;
                    rightHandStatus = HandStatus.NotDetect;
                    break;
                case "Right hand is detected":
                    //pictureBoxLeftHand.Visible = false;
                    //pictureBoxRightHand.Visible = true;
                    pictureBoxLeftHand.Image = SmartTV.Properties.Resources.l_hand;
                    pictureBoxRightHand.Image = SmartTV.Properties.Resources.r_hand_ac;
                    rightHandStatus = HandStatus.Detect;
                    leftHandStatus = HandStatus.NotDetect;
                    break;
                case "Hand Detected, Hand Not Calibrated, Hand Inside Borders,":
                    handCalibrationStatus = HandCalibrationStatus.NotCalibrated;
                    //handPosition = HandPosition.inside;
                    break;
                case "Hand Detected, Hand Not Calibrated, Hand Out Of Borders,":
                    handCalibrationStatus = HandCalibrationStatus.NotCalibrated;
                    //handPosition = HandPosition.outside;
                    break;
                case "Hand Calibrated,":
                    handCalibrationStatus = HandCalibrationStatus.Calibrated;
                    break;
                case "Hand Out Of Borders,":
                    Console.WriteLine("OUT ");
                    handCalibrationStatus = HandCalibrationStatus.Calibrated;
                    handPosition = HandPosition.outside;
                    break;
                case "Hand Inside Borders,":
                    handCalibrationStatus = HandCalibrationStatus.Calibrated;
                    //handPosition = HandPosition.inside;
                    break;
                case "Both hands are detected":
                    //pictureBoxLeftHand.Visible = true;
                    //pictureBoxRightHand.Visible = true;
                    pictureBoxLeftHand.Image = SmartTV.Properties.Resources.l_hand_ac;
                    pictureBoxRightHand.Image = SmartTV.Properties.Resources.r_hand_ac;
                    leftHandStatus = HandStatus.Detect;
                    rightHandStatus = HandStatus.Detect;
                    break;
                case "No hand is detected":
                    lastCommand = "";
                    //pictureBoxLeftHand.Visible = false;
                    //pictureBoxRightHand.Visible = false;
                    pictureBoxLeftHand.Image = SmartTV.Properties.Resources.l_hand;
                    pictureBoxRightHand.Image = SmartTV.Properties.Resources.r_hand;
                    leftHandStatus = HandStatus.NotDetect;
                    rightHandStatus = HandStatus.NotDetect;
                    break;
                default:
                    if (status != null && status.StartsWith("point:"))
                    {
                        string[] coords = status.Substring(6).Split(',');
                        float new_fist_x = float.Parse(coords[0], CultureInfo.InvariantCulture.NumberFormat);
                        float new_fist_y = float.Parse(coords[1], CultureInfo.InvariantCulture.NumberFormat);
                        float diff_fist_x = (new_fist_x - fist_x);
                        float diff_fist_y = (new_fist_y - fist_y);
                        //Console.WriteLine("x: " + diff_fist_x);
                        //Console.WriteLine("y: " + diff_fist_y);
                        if (fistOutCounter > 0)
                        {
                            if (diff_fist_x <= -5) this.Invoke(this.invokeKeyDown, 39); // left
                            else if (diff_fist_x >= 5) this.Invoke(this.invokeKeyDown, 37); // right
                            else if (diff_fist_y <= -5) this.Invoke(this.invokeKeyDown, 38); // up
                            else if (diff_fist_y >= 5) this.Invoke(this.invokeKeyDown, 40); // down
                        }
                        fist_x = new_fist_x;
                        fist_y = new_fist_y;
                        if (Math.Abs(diff_fist_x) < 1 && Math.Abs(diff_fist_y) < 1)
                        {
                            lastStallTime = DateTime.Now;
                            if (cursorStallTimer == null)
                            {
                                cursorStallTimer = new System.Timers.Timer();
                                cursorStallTimer.Elapsed += new ElapsedEventHandler(OnCursorTimerEvent);
                                cursorStallTimer.Interval = 100;
                                cursorStallTimer.Enabled = true;
                                Console.WriteLine("cursor stall");
                            }
                            else
                            {
                                cursorStallTimer.Enabled = true;
                            }
                        }
                        else
                        {
                            if (cursorStallTimer != null)
                            {
                                cursorStallTimer.Enabled = false;
                                cursorStallTimer = null;
                                cursorStallTime = 0;
                                Console.WriteLine("cursor not stall");
                            }
                        }
                        if (menuOpen)
                        {
                            int X = Cursor.Position.X - (int)diff_fist_x * 2;
                            if (X < menuPanel.Location.X) X = menuPanel.Location.X;
                            else if (X > (menuPanel.Location.X + menuPanel.Width)) X = menuPanel.Location.X + menuPanel.Width;
                            Cursor.Position = new Point(X, menuPanel.Location.Y + menuPanel.Height / 2);
                            lastCommand = "";
                        } else if (mouseOver)
                            Cursor.Position = new Point(Cursor.Position.X - (int)diff_fist_x * 10, Cursor.Position.Y + (int)diff_fist_y * 10);
                    }
                    break;
            }

            if (faceStatus == FaceStatus.Detect)
            {
                if (facePosition == FacePosition.Center)
                {
                    faceChineseMsg = "偵測到您的臉！";
                }
                else if (facePosition == FacePosition.Near)
                {
                    faceChineseMsg = "偵測到您的臉，請將臉與攝影機保持適當距離！";
                }
                else if (facePosition == FacePosition.Left)
                {
                    faceChineseMsg = "偵測到您的臉，太左邊了！";
                }
                else if (facePosition == FacePosition.Right)
                {
                    faceChineseMsg = "偵測到您的臉！太右邊了！";
                }
            }
            else if (faceStatus == FaceStatus.NotDetect)
            {
                faceChineseMsg = "臉與攝影機請保持一個手臂的距離並對準鏡頭。";
            }

            if (faceStatus == FaceStatus.Detect && (leftHandStatus == HandStatus.NotDetect || rightHandStatus == HandStatus.NotDetect))
            {
                handChineseMsg = "未偵測到您的手！";
                handsMsgLabel.ForeColor = System.Drawing.Color.Red;
            }

            if (leftHandStatus == HandStatus.Detect)
            {
                handChineseMsg = "偵測到您的左手！";
                handsMsgLabel.ForeColor = System.Drawing.Color.White;
                /*if (handPosition == HandPosition.outside)
                {
                    handChineseMsg = "偵測到您的左手！ 請將手靠近攝影機...";
                }*/
                /*if (handPosition == HandPosition.inside)
                {
                    handChineseMsg = "偵測到您的左手！ 已校準...";
                }*/
            }
            else if (rightHandStatus == HandStatus.Detect)
            {
                handChineseMsg = "偵測到您的右手！";
                handsMsgLabel.ForeColor = System.Drawing.Color.White;
                /*if (handPosition == HandPosition.outside)
                {
                    handChineseMsg = "偵測到您的右手！ 請將手靠近攝影機...";
                }*/
                /*if (handPosition == HandPosition.inside)
                {
                    handChineseMsg = "偵測到您的右手！ 已校準...";
                }*/
            }

            if(leftHandStatus == HandStatus.Detect && rightHandStatus == HandStatus.Detect)
            {
                handChineseMsg = "偵測到您的雙手！";
                handsMsgLabel.ForeColor = System.Drawing.Color.White;
            }


            this.Invoke(this.invokeShowMsg, faceChineseMsg);
            this.Invoke(this.invokeShowHandsMsg, handChineseMsg);
        }

        private void OnCursorTimerEvent(object source, ElapsedEventArgs e)
        {
            if (lastStallTime.AddSeconds(1) < DateTime.Now || menuOpen)
            {
                cursorStallTimer.Enabled = false;
                cursorStallTimer = null;
                _cursorStallTime = 0;
                return;
            }
            if (_cursorStallTime > 3000) //4s
            {
                Console.WriteLine("menu open");
                cursorStallTimer.Enabled = false;
                cursorStallTimer = null;
                _cursorStallTime = 0;
                this.Invoke(this.invokeShowMenu, true);
            }
            else
            {
                _cursorStallTime += 100;
            }
        }

        #region SetTimeout/ClearTimeout Simulation
        //Dictionary for running setTimeout
        static Dictionary<Guid, Thread> _setTimeoutHandles =
            new Dictionary<Guid, Thread>();
        //SetTimeout for no UI Thread issue
        static Guid SetTimeout(Action cb, int delay)
        {
            return SetTimeout(cb, delay, null);
        }
        //Javascript-style SetTimeout function
        //remember to set uiForm argument when there cb is trying
        //to change UI controls in window form
        //it will return a GUID as handle for cancelling
        static Guid SetTimeout(Action cb, int delay, Form uiForm)
        {
            Guid g = Guid.NewGuid();
            Thread t = new Thread(() =>
            {
                Thread.Sleep(delay);
                _setTimeoutHandles.Remove(g);
                if (uiForm != null)
                    //use Invoke() to avoid threading issue
                    //Ref: http://tinyurl.com/yjckzhz
                    uiForm.Invoke(cb);
                else
                    cb();
            });
            _setTimeoutHandles.Add(g, t);
            t.Start();
            return g;
        }
        //Javascript-style ClearTimeout
        static void ClearTimeout(Guid g)
        {
            if (!_setTimeoutHandles.ContainsKey(g))
                return;
            _setTimeoutHandles[g].Abort();
            _setTimeoutHandles.Remove(g);
        }
        #endregion

        private void HandleGesture(object sender, DataReceivedEventArgs e)
        {
            Boolean isCommand = false;

            

            if (listenCommand)
            {
                //Console.WriteLine(e.Data);
                if (e.Data != null && (e.Data.StartsWith("Hand ") || e.Data.StartsWith("Face ")))
                {
                    
                    //string chineseMsg = "";
                    /*if (e.Data == "Face is too near")
                    {
                        chineseMsg = "請將臉與攝影機保持一個手臂的距離，並對準鏡頭。";
                    }
                    else if (e.Data == "Face is detected")
                    {
                        chineseMsg = "已偵測到您的臉！";
                    }
                    else if (e.Data.StartsWith("Hand Detected"))
                    {
                        chineseMsg = "已偵測到您的手！";
                    }
                    else if (e.Data == "Hand Out Of Borders,")
                    {
                        chineseMsg = "請將您的手";
                    }*/

                    this.Invoke(this.invokeStatus, e.Data);
                }

                if (mouseOver)
                {
                    if(e.Data == "cursor_hand_closing")
                    {
                        //if (lastCommand != e.Data)
                        //{
                        _cursorStallTime = 0;
                        Console.WriteLine(e.Data.ToString());
                        this.Invoke(this.invokeKeyDown, 13);
                        isCommand = true;
                        //}
                    }
                }
                else
                {
                    if(e.Data == "thumb_up")
                    {
                        if (lastCommand != e.Data)
                        {
                            this.Invoke(this.invokeKeyDown, 13);
                            isCommand = true;
                        }
                    }
                }

                switch (e.Data)
                {
                    case "two_fingers_pinch_open":
                        Console.WriteLine("FINGERS!!!!");
                        this.Invoke(this.invokeKeyDown, 67);
                        this.Invoke(this.invokeKeyDown, 84);
                        isCommand = true;
                        break;
                    case "swipe_right":
                    case "wave-right":
                        this.Invoke(this.invokeKeyDown, 39);
                        isCommand = true;
                        break;
                    case "swipe_left":
                    case "wave-left":
                        this.Invoke(this.invokeKeyDown, 37);
                        isCommand = true;
                        break;
                    case "swipe_up":
                    case "wave-up":
                        this.Invoke(this.invokeKeyDown, 38);
                        isCommand = true;
                        break;
                    case "swipe_down":
                    case "wave-down":
                        this.Invoke(this.invokeKeyDown, 40);
                        isCommand = true;
                        break;
                    case "tap":
                        //Console.WriteLine("tap");
                        //if (mouseOver || menuOpen)
                        //    this.Invoke(this.invokeMouseClick, "left");
                        break;
                    case "home":
                        this.Invoke(this.invokeGoHome);
                        isCommand = true;
                        break;
                    case "back":
                    case "wave":
                        this.Invoke(this.invokeGoBack);
                        isCommand = true;
                        break;
                    case "openvoice":
                        this.Invoke(this.invokeStatus, "openvoice");
                        isCommand = true;
                        break;
                    case "mute":
                        if (lastCommand != "mute")
                        {
                            this.Invoke(this.invokeKeyDown, 77);
                            isCommand = true;
                        }
                        break;
                    default:
                        try
                        {
                            this.Invoke(this.invokeStatus, e.Data);
                        }
                        catch (Exception exception)
                        {

                        }
                        break;
                }
            }
            else
            {
                try
                {
                    this.Invoke(this.invokeStatus, e.Data);
                }
                catch (Exception exception)
                {

                }
            }

            if (isCommand)
            {
                //Console.WriteLine(e.Data);
                _countDown = _period;
                listenCommand = false;
                _timer = new System.Timers.Timer();
                _timer.Interval = 100;
                _timer.Enabled = true;
                _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                _timer.Start();

                lastCommand = e.Data;
            }
        }

        private void mouseOverPictureBox_Click(object sender, EventArgs e)
        {
            Console.WriteLine("mouse over");
            mouseOver = !mouseOver;
            mouseOverPictureBox.Refresh();
        }

        private void mouseOverPictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            string text = "";
            if (mouseOver) text = "Enabled";

            SizeF textSize = e.Graphics.MeasureString(text, Font);
            PointF locationToDraw = new PointF();
            locationToDraw.X = (mouseOverPictureBox.Width / 2) - (textSize.Width / 2);
            locationToDraw.Y = (mouseOverPictureBox.Height / 2) - (textSize.Height / 2);

            e.Graphics.DrawString(text, Font, Brushes.Black, locationToDraw);
        }

        private void voiceMenuPictureBox_Click(object sender, EventArgs e)
        {
            startVoice();
            this.Invoke(this.invokeShowMenu, false);
        }

        private void homeMenuPictureBox_Click(object sender, EventArgs e)
        {
            if (browser.Address == endpoint + viewVideoPath)
                browser.Load(endpoint + personalPagePath);
            else
                browser.Load(endpoint + selectProgramPath);
            this.Invoke(this.invokeShowMenu, false);
        }

        private void backMenuPictureBox_Click(object sender, EventArgs e)
        {
            if (!browser.IsLoading)
                browser.ExecuteScriptAsync("sendKeyCode(27);");
            this.Invoke(this.invokeShowMenu, false);
        }

        private void backMenuPictureBox_MouseEnter(object sender, EventArgs e)
        {
            backMenuPictureBox.Image = SmartTV.Properties.Resources.ic_back_hover;
            keepShowMenu();
        }

        private void backMenuPictureBox_MouseLeave(object sender, EventArgs e)
        {
            backMenuPictureBox.Image = SmartTV.Properties.Resources.ic_back1;
        }

        private void backMenuPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            backMenuPictureBox.Image = SmartTV.Properties.Resources.ic_back_click;
        }

        private void backMenuPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            backMenuPictureBox.Image = SmartTV.Properties.Resources.ic_back_hover;
        }

        private void homeMenuPictureBox_MouseEnter(object sender, EventArgs e)
        {
            homeMenuPictureBox.Image = SmartTV.Properties.Resources.ic_home_hover;
            keepShowMenu();
        }

        private void homeMenuPictureBox_MouseLeave(object sender, EventArgs e)
        {
            homeMenuPictureBox.Image = SmartTV.Properties.Resources.ic_home1;
        }

        private void homeMenuPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            homeMenuPictureBox.Image = SmartTV.Properties.Resources.ic_home_click;
        }

        private void homeMenuPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            homeMenuPictureBox.Image = SmartTV.Properties.Resources.ic_home_hover;
        }

        private void mouseOverPictureBox_MouseEnter(object sender, EventArgs e)
        {
            mouseOverPictureBox.Image = SmartTV.Properties.Resources.ic_cursor_hover;
            keepShowMenu();
        }

        private void mouseOverPictureBox_MouseLeave(object sender, EventArgs e)
        {
            mouseOverPictureBox.Image = SmartTV.Properties.Resources.ic_cursor1;
        }

        private void mouseOverPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseOverPictureBox.Image = SmartTV.Properties.Resources.ic_cursor_click;
        }

        private void mouseOverPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseOverPictureBox.Image = SmartTV.Properties.Resources.ic_cursor_hover;
        }

        private void questionMenuPictureBox_MouseEnter(object sender, EventArgs e)
        {
            questionMenuPictureBox.Image = SmartTV.Properties.Resources.ic_question_hover;
        }

        private void questionMenuPictureBox_MouseLeave(object sender, EventArgs e)
        {
            questionMenuPictureBox.Image = SmartTV.Properties.Resources.ic_question;
        }

        private void logoutMenuPictureBox_MouseEnter(object sender, EventArgs e)
        {
            logoutMenuPictureBox.Image = SmartTV.Properties.Resources.ic_logout_hover;
        }

        private void logoutMenuPictureBox_MouseLeave(object sender, EventArgs e)
        {
            logoutMenuPictureBox.Image = SmartTV.Properties.Resources.ic_logout;
        }

        private void questionMenuPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!browser.IsLoading)
                browser.ExecuteScriptAsync("sendKeyCode(82);");
        }

        private void logoutMenuPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            this.Invoke(this.invokeGoBack);
        }
    }
}