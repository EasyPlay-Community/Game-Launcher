using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using SysNet = System.Net;
using System.Windows.Forms;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Threading;
using CustomMessageBox;


namespace MGL
{
    public partial class Main : Form
    {
        #region -- Инициализировать компоненты --
        public Main()
        {
            InitializeComponent();
            button5.Enabled = false;
            button8.Enabled = false;
            button11.Enabled = false;
            label3.Visible = false;
            timer1.Enabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            backgroundWorker1.RunWorkerAsync();
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatStyle = FlatStyle.Flat;
            button10.FlatAppearance.BorderSize = 0;
            button10.FlatStyle = FlatStyle.Flat;
            label22.Text = "Проверка доступных обновлений...";
        }
        #endregion

        #region -- Делает форму перетаскиваемой --
        private void Form1_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            base.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }
        #endregion

        #region -- Загрузчик обновлений клиента Mindustry & Foo's Client --
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            FileStream GameFs = null;
            if (!File.Exists("GameVersion"))
            {
                using (GameFs = File.Create("GameVersion"))
                {

                }

                using (StreamWriter sw = new StreamWriter("GameVersion"))
                {
                    sw.Write("1");
                }
            }
            FileStream FooFS = null;
            if (!File.Exists("Foo'sVersion"))
            {
                using (FooFS = File.Create("Foo'sVersion"))
                {

                }

                using (StreamWriter sw = new StreamWriter("Foo'sVersion"))
                {
                    sw.Write("1");
                }
            }

            //Ссылка на последний релиз .jar файла Mindustry на GitHub
            string GameUrl = "https://github.com/Anuken/Mindustry/releases/latest/download/Mindustry.jar";
            //Ссылка на последний релиз .jar файла Foo's Client на GitHub
            string FooUrl = "https://github.com/mindustry-antigrief/mindustry-client-v7-builds/releases/latest/download/desktop.jar";
            //Путь к папке, в которую будет скачиваться .jar файл Mindustry
            string GamePath = AppDomain.CurrentDomain.BaseDirectory + "/Mindustry/jre/";
            //Путь к папке, в которую будет скачиваться .jar файл Foo's Client
            string FooPath = AppDomain.CurrentDomain.BaseDirectory + "/Mindustry/Foo'sClient/jre/";
            //Путь к дирректории лаунчера
            string BasePath = AppDomain.CurrentDomain.BaseDirectory;

            // Получаем текущую версию игры из файла GameVersion
            string currentGameVersion = File.ReadAllText(BasePath + "GameVersion");
            // Получаем текущую версию игры из файла Foo'sVersion
            string currentFooVersion = File.ReadAllText(BasePath + "Foo'sVersion");

            // Проверяем последний релиз Mindustry на GitHub
            CheckGithubMindustry githubCheckerMindustry = new CheckGithubMindustry();
            string latestGameVersion = githubCheckerMindustry.CheckLatestRelease();

            // Проверяем последний релиз Foo's на GitHub
            CheckGithubFooClient githubCheckerFoo = new CheckGithubFooClient();
            string latestFooVersion = githubCheckerFoo.CheckLatestRelease();

            //Скачиваем клиент Mindustry если версия с GitHub новее.
            string sUrlToReadFileFrom = GameUrl;
            string sFilePathToWriteFileTo = GamePath + "Mindustry.jar";

            //Скачиваем клиент Foo's если версия с GitHub новее.
            string FooUrlToReadFileFrom = FooUrl;
            string FooFilePathToWriteFileTo = FooPath + "Foo'sClient.jar";

            //Цвет текста downloadLbl
            string hexColor = "#F4D280";
            Color textColorDwnld = ColorTranslator.FromHtml(hexColor);

            if (currentGameVersion != latestGameVersion)
            {
                downloadLbl.ForeColor = textColorDwnld;
                downloadLbl.Text = "Скачивается обновление Mindustry, ожидайте пожалуйста......";
                Uri url2 = new Uri(sUrlToReadFileFrom);
                SysNet.HttpWebRequest request = (SysNet.HttpWebRequest)SysNet.WebRequest.Create(GameUrl);
                SysNet.HttpWebResponse response = (SysNet.HttpWebResponse)request.GetResponse();
                response.Close();

                Int64 iSize = response.ContentLength;

                Int64 iRunningByteTotal = 0;

                using (SysNet.WebClient client = new SysNet.WebClient())
                {
                    using (System.IO.Stream streamRemote = client.OpenRead(new Uri(sUrlToReadFileFrom)))
                    {
                        using (Stream streamLocal = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int iByteSize = 0;
                            byte[] byteBuffer = new byte[iSize];
                            while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                streamLocal.Write(byteBuffer, 0, iByteSize);
                                iRunningByteTotal += iByteSize;

                                double dIndex = (double)(iRunningByteTotal);
                                double dTotal = (double)byteBuffer.Length;
                                double dProgressPercentage = (dIndex / dTotal);
                                int iProgressPercentage = (int)(dProgressPercentage * 100);

                                backgroundWorker1.ReportProgress(iProgressPercentage);
                            }

                            streamLocal.Close();
                        }

                        streamRemote.Close();
                    }
                }
                //Записывем актуальную версию в version
                File.WriteAllText(BasePath + "GameVersion", latestGameVersion);
            }

            Thread.Sleep(500);
            if (currentFooVersion != latestFooVersion)
            {
                downloadLbl.ForeColor = textColorDwnld;
                downloadLbl.Text = "Скачивается обновление Foo's Client, ожидайте пожалуйста......";
                Uri url3 = new Uri(FooUrlToReadFileFrom);
                SysNet.HttpWebRequest request = (SysNet.HttpWebRequest)SysNet.WebRequest.Create(GameUrl);
                SysNet.HttpWebResponse response = (SysNet.HttpWebResponse)request.GetResponse();
                response.Close();

                Int64 iSize = response.ContentLength;

                Int64 iRunningByteTotal = 0;

                using (SysNet.WebClient client = new SysNet.WebClient())
                {
                    using (System.IO.Stream streamRemote = client.OpenRead(new Uri(FooUrlToReadFileFrom)))
                    {
                        using (Stream streamLocal = new FileStream(FooFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int iByteSize = 0;
                            byte[] byteBuffer = new byte[iSize];
                            while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                streamLocal.Write(byteBuffer, 0, iByteSize);
                                iRunningByteTotal += iByteSize;

                                double dIndex = (double)(iRunningByteTotal);
                                double dTotal = (double)byteBuffer.Length;
                                double dProgressPercentage = (dIndex / dTotal);
                                int iProgressPercentage = (int)(dProgressPercentage * 100);

                                backgroundWorker1.ReportProgress(iProgressPercentage);
                            }

                            streamLocal.Close();
                        }

                        streamRemote.Close();
                    }
                }
                //Записывем актуальную версию в version
                File.WriteAllText(BasePath + "Foo'sVersion", latestFooVersion);
            }
        }
        #endregion

        #region -- Загрузка обновлений --
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //this.Size = new Size(175, 125);
            downloadLbl.Visible = true;
            button8.Enabled = false;
            button4.Enabled = false;
            button10.Enabled = false;
            button11.Enabled = false;
            label22.Visible = false;
            progressBar1.ForeColor = Color.Blue;
            progressBar1.Value = e.ProgressPercentage;
            label19.Text = (e.ProgressPercentage.ToString() + "%");
        }
        #endregion

        #region -- Загрузка Обновлений не требуется --
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invalidate();
            timer1.Enabled = true;
            timer1.Interval = 1;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            button5.Enabled = true;
            button11.Enabled = true;
            button10.Enabled = true;
            label19.Visible = false;
            label22.Visible = false;
            this.downloadLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            downloadLbl.Text = "Обновлений для игры нет.";
            label1.Text = System.IO.File.ReadAllText(Application.StartupPath + "/GameVersion");
            FileStream fs = null;
            if (!File.Exists("updater"))
            {
                using (fs = File.Create("updater"))
                {

                }
                using (StreamWriter sw = new StreamWriter("updater"))
                {
                    sw.Write("0");
                }
            }
            label5.Text = System.IO.File.ReadAllText(Application.StartupPath + "/updater");
        }
        #endregion

        #region -- Таймер при запуске формы --
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Maximum == progressBar1.Value)
            {
                timer1.Enabled = false;
                progressBar1.Visible = false; //Видимость progressBar После загрузки и проверки файлов
                downloadLbl.Visible = true;
                label3.Visible = false;
                button8.Enabled = true;
                button11.Enabled = true;
                button4.Enabled = true;
            }
            else
            {
                downloadLbl.Visible = false;
                label3.Visible = true;
                progressBar1.Value++;
                button8.Enabled = false;
                button11.Enabled = false;
                button4.Enabled = false;
            }
        }
        #endregion

        #region -- Проверить пинг и статус -- 
        private void OnlStatus_Tick(object sender, EventArgs e)
        {
            TimerOnlStatus.Interval = 20000;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Check Online
            IPStatus status = IPStatus.TimedOut;
            try
            {
                Ping ping1 = new Ping();
                PingReply reply = ping1.Send(@"update.easyplay.su");
                status = reply.Status;
            }
            catch { }
            if (status != IPStatus.Success)
            {
                pictureBox6.BackgroundImage = global::MGL.Properties.Resources.server_offline;
                return;
            }
            else
            {
                pictureBox6.BackgroundImage = global::MGL.Properties.Resources.server_online;
                var content = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/EasyPlay.su.json");
                var result = content;
                label11.Text = result;

                var content1 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result1 = content1;
                label12.Text = result1;

                var content2 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result2 = content2;
                label13.Text = result2;

                var content3 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result3 = content3;
                label14.Text = result3;

                var content4 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result4 = content4;
                label15.Text = result4;

                var content5 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result5 = content5;
                label16.Text = result5;

                var content6 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result6 = content6;
                label17.Text = result6;

                var content7 = new WebClient { Encoding = Encoding.UTF8 }.DownloadString("http://update.easyplay.su/OnlStatus/status/ComingSoon.json");
                var result7 = content7;
                label21.Text = result7;
            }
            //End Check Online

            //Ping
            List<string> serversList = new List<string>();
            serversList.Add("EasyPlay.su"); //address
            Ping ping = new SysNet.NetworkInformation.Ping();
            PingReply pingReply = null;

            foreach (string server in serversList)
            {
                pingReply = ping.Send(server);

                if (pingReply.Status != IPStatus.TimedOut)
                {
                    label18.Text = pingReply.RoundtripTime.ToString();
                }

            }
            //End ping 

            //Check online Status EasyPlay.Su
            var client = new TcpClient();
            if (client.ConnectAsync("EasyPlay.su", 6567).Wait(200))
            {
                pictureBox1.Image = global::MGL.Properties.Resources.Sonline;
                label2.Text = "Сервер включен.";
            }
            else
            {
                pictureBox1.Image = global::MGL.Properties.Resources.Soffline;
                label2.Text = "Сервер выключен.";
            }
            //End Check online Status


            //Check online Status Server2
            var client1 = new TcpClient();
            if (client1.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox2.Image = global::MGL.Properties.Resources.Scs;
                label4.Text = "Сервер включен.";
            }
            else
            {
                pictureBox2.Image = global::MGL.Properties.Resources.Scs;
                label4.Text = "Сервер выключен.";
            }
            //End Check online Status

            //Check online Status Server3
            var client2 = new TcpClient();
            if (client2.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox3.Image = global::MGL.Properties.Resources.Scs;
                label6.Text = "Сервер включен.";
            }
            else
            {
                pictureBox3.Image = global::MGL.Properties.Resources.Scs;
                label6.Text = "Сервер выключен.";
            }
            //End Check online Status 

            //Check online Status Server4
            var client3 = new TcpClient();
            if (client3.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox4.Image = global::MGL.Properties.Resources.Scs;
                label7.Text = "Сервер включен.";
            }
            else
            {
                pictureBox4.Image = global::MGL.Properties.Resources.Scs;
                label7.Text = "Сервер выключен.";
            }
            //End Check online Status

            //Check online Status Server5
            var client4 = new TcpClient();
            if (client4.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox5.Image = global::MGL.Properties.Resources.Scs;
                label8.Text = "Сервер включен.";
            }
            else
            {
                pictureBox5.Image = global::MGL.Properties.Resources.Scs;
                label8.Text = "Сервер выключен.";
            }
            //End Check online Status

            //Check online Status Server6
            var client5 = new TcpClient();
            if (client5.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox7.Image = global::MGL.Properties.Resources.Scs;
                label9.Text = "Сервер включен.";
            }
            else
            {
                pictureBox7.Image = global::MGL.Properties.Resources.Scs;
                label9.Text = "Сервер выключен.";
            }
            //End Check online Status

            //Check online Status Server7
            var client6 = new TcpClient();
            if (client6.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox8.Image = global::MGL.Properties.Resources.Scs;
                label10.Text = "Сервер включен.";
            }
            else
            {
                pictureBox8.Image = global::MGL.Properties.Resources.Scs;
                label10.Text = "Сервер выключен.";
            }
            //End Check online Status

            //Check online Status Server8
            var client7 = new TcpClient();
            if (client7.ConnectAsync("127.0.0.1", 1).Wait(0))
            {
                pictureBox9.Image = global::MGL.Properties.Resources.Scs;
                label20.Text = "Сервер включен.";
            }
            else
            {
                pictureBox9.Image = global::MGL.Properties.Resources.Scs;
                label20.Text = "Сервер выключен.";
            }
            //End Check online Status

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        #endregion

        #region -- Проверить, запущен ли Mindustry.exe --
        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }

            }
            return false;
        }
        #endregion

        #region -- Загрузить форму --
        public void Open_OfflineMode(object obj)
        {
            Application.Run(new OfflineMode());
        }
        public void Custom_MSG(object obj)
        {
            Application.Run(new CustomDialog());
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            downloadLbl.Visible = false;
            TimerOnlStatus.Enabled = true;
            SetRoundedShape(progressBar1, 7);
            ToolTip t = new ToolTip();
            IPStatus status = IPStatus.TimedOut;
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(@"update.easyplay.su");
                status = reply.Status;
            }
            catch { }
            if (status != IPStatus.Success)
            {
                Thread CustomMSG;
                this.Close();
                CustomMSG = new Thread(Custom_MSG);
                CustomMSG.SetApartmentState(ApartmentState.STA);
                CustomMSG.Start();
                //DialogResult dialogResult = MessageBox.Show("У лаунчера нет доступа к севреру обновлений.\nПроверьте подключение к интернету\nИли свяжитесь с Админитрацией в Discord\nКанал: 🎮┇game-launcher\n\n\nПродолжить работу автономно?", "No Interner Connection", MessageBoxButtons.YesNo);
                //if (dialogResult == DialogResult.Yes)
                //{
                //    Thread offlineMode;
                //    //pictureBox6.BackgroundImage = global::MGL.Properties.Resources.server_offline;
                //    //webBrowser1.Visible = false;
                //    //OfflineMode offlineForm = new OfflineMode();
                //    //offlineForm.Show();
                //    this.Close();
                //    offlineMode = new Thread(Open_OfflineMode);
                //    offlineMode.SetApartmentState(ApartmentState.STA);
                //    offlineMode.Start();


                //}
                //else if (dialogResult == DialogResult.No)
                //{
                //    Application.Exit();
                //}
            }
            else
            {
                pictureBox6.BackgroundImage = global::MGL.Properties.Resources.server_online;
            }
            t.SetToolTip(button6, "Откроет папку с модификациями игры");
            t.SetToolTip(button7, "Откроет папку с картами игры");
            t.SetToolTip(button4, "Функция ремонта и восстановления клиента в исходное состояние");
            t.SetToolTip(button1, "Свернуть лаунчер");
            t.SetToolTip(button5, "Закрыть лаунчер");
            t.SetToolTip(pictureBox6, "Статус подключения к серверу обновлений.");
            t.SetToolTip(label18, "Ping до серверов EasyPlay.su");
        }
        #endregion

        #region -- Кнопки --

        #region -- Кнопка Свернуть лаунчер --
        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 1;
            button1.FlatStyle = FlatStyle.Popup;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка Закрыть лаунчер --
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.FlatAppearance.BorderSize = 1;
            button5.FlatStyle = FlatStyle.Popup;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка Сайт --
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы хотите перейти перейти на сайт?", "Подтвердите действие!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://easyplay.su/");
            }
            else if (dialogResult == DialogResult.No)
            {
                //
            }
        }
        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.FlatAppearance.BorderSize = 1;
            button3.FlatStyle = FlatStyle.Popup;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка Discord --
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы хотите перейти в Discord EasyPlay?", "Подтвердите действие!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://ds.easyplay.su/");
            }
            else if (dialogResult == DialogResult.No)
            {
                //
            }
        }
        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.FlatAppearance.BorderSize = 1;
            button2.FlatStyle = FlatStyle.Popup;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка карты --
        private void button7_Click(object sender, EventArgs e)
        {
            {
                string dirName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Mindustry/maps/";
                if (Directory.Exists(dirName))
                {
                    Process.Start(dirName);
                }
                else
                {
                    MessageBox.Show("У вас не создана папка с картами, повторите попытку позже.");
                }
            }
        }
        private void button7_MouseEnter(object sender, EventArgs e)
        {
            button7.FlatAppearance.BorderSize = 1;
            button7.FlatStyle = FlatStyle.Popup;
        }

        private void button7_MouseLeave(object sender, EventArgs e)
        {
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка моды --
        private void button6_Click(object sender, EventArgs e)
        {
            {
                string dirName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Mindustry/mods/";
                if (Directory.Exists(dirName))
                {
                    Process.Start(dirName);
                }
                else
                {
                    MessageBox.Show("У вас не создана папка с модификациями, повторите попытку позже.");
                }
            }
        }
        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.FlatAppearance.BorderSize = 1;
            button6.FlatStyle = FlatStyle.Popup;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка ремонта --
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы запустили средство восстановления клиента\n\nВы уверены что хотите заново переустановить клиент?", "Внимание!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete(Application.StartupPath + "/version");
                MessageBox.Show("Лаунчер будет перезагружен!\n\nПроизойдет переустановка клиента.", "Внимание!");
                Application.Restart();
            }
            else if (dialogResult == DialogResult.No)
            {
                //
            }
        }
        private void button4_MouseEnter(object sender, EventArgs e)
        {
            button4.FlatAppearance.BorderSize = 1;
            button4.FlatStyle = FlatStyle.Popup;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка настроек --
        private void button10_Click(object sender, EventArgs e)
        {
            /*       ------Функция временно отключена------
                {
                string dirName1 = (Application.StartupPath + "/Settings//Mindustry.ru.crt");
                string dirName = (Application.StartupPath + "/Settings//");
                if (Directory.Exists(dirName) && File.Exists(dirName1) == true)
                {
                DialogResult dialogResult = MessageBox.Show("Сейчас вам будет предложено установить сертификат безопасности Mindustry.ru \nУстановить?", "Внимание!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                Process.Start(Application.StartupPath + "//Settings//Mindustry.ru.crt");
                }
                else
                {
                //
                }
                }
                else
                {
                DialogResult dialogResult = MessageBox.Show("Лаунчер не может найти Сертификат безопасности \nХотите ли вы запустить средство устранения ошибок?", "Внимание!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                if (!File.Exists("Updater.exe"))
                {
                MessageBox.Show("Лаунчер не может найти средство устранения ошибок\nПереустановите программу.", "Внимание!");
                Application.Exit();
                }
                else
                {
                File.Delete(Application.StartupPath + "/updater");
                Process.Start(Application.StartupPath + "/Updater.exe");
                Application.Exit();
                }
                }
                else if (dialogResult == DialogResult.No)
                {
                //
                }
                }
                }
            */
        }
        private void button10_MouseEnter(object sender, EventArgs e)
        {
            button10.FlatAppearance.BorderSize = 1;
            button10.FlatStyle = FlatStyle.Popup;
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            button10.FlatAppearance.BorderSize = 0;
            button10.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка запуска игры --
        private void button8_Click(object sender, EventArgs e)
        {
            if (IsProcessOpen("Mindustry"))
            {
                MessageBox.Show("Клиент уже запущен!");
            }
            else
            {
                string dirName1 = (Application.StartupPath + "/Mindustry//Mindustry.exe");
                string dirName = (Application.StartupPath + "/Mindustry//");
                if (Directory.Exists(dirName) && File.Exists(dirName1) == true)
                {
                    Process.Start(Application.StartupPath + "//Mindustry//Mindustry.exe");
                    Application.Exit();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Лаунчер не может найти каталог игры \nХотите ли вы запустить средство устранения ошибок?", "Внимание!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {

                        if (!File.Exists("Updater.exe"))
                        {
                            MessageBox.Show("Лаунчер не может найти средство устранения ошибок\nПереустановите программу.", "Внимание!");
                            Application.Exit();
                        }
                        else
                        {
                            File.Delete(Application.StartupPath + "/GameVersion");
                            File.Delete(Application.StartupPath + "/Foo'sVersion");
                            File.Delete(Application.StartupPath + "/updater");
                            File.Delete(Application.StartupPath + "/Launcher.exe.config");
                            Directory.Delete(Application.StartupPath + "/Mindustry", true);
                            Directory.Delete(Application.StartupPath + "/libs", true);
                            Application.Exit();
                        }


                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //
                    }
                }
            }
        }
        #endregion

        #region -- Кнопка GitHub --
        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы хотите перейти перейти в GitHub?", "Подтвердите действие!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://github.com/EasyPlay-Community/Game-Launcher");
            }
            else if (dialogResult == DialogResult.No)
            {
                //
            }
        }
        private void button9_MouseEnter(object sender, EventArgs e)
        {
            button9.FlatAppearance.BorderSize = 1;
            button9.FlatStyle = FlatStyle.Popup;
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            button9.FlatAppearance.BorderSize = 0;
            button9.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #region -- Кнопка FooClient --
        private void button11_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Вы хотите запустить Mindustry Foo's клиент?", "Подтвердите действие!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (IsProcessOpen("Mindustry"))
                {
                    MessageBox.Show("Клиент уже запущен!");
                }
                else
                {
                    string dirName1 = (Application.StartupPath + "/Mindustry/Foo'sClient//Mindustry.exe");
                    string dirName = (Application.StartupPath + "/Mindustry/Foo'sClient//");
                    if (Directory.Exists(dirName) && File.Exists(dirName1) == true)
                    {
                        Process.Start(Application.StartupPath + "//Mindustry//Foo'sClient//Mindustry.exe");
                        Application.Exit();
                    }
                    else
                    {
                        DialogResult dialogResult1 = MessageBox.Show("Лаунчер не может найти каталог клиента \nХотите ли вы запустить средство устранения ошибок?", "Внимание!", MessageBoxButtons.YesNo);
                        if (dialogResult1 == DialogResult.Yes)
                        {
                            File.Delete(Application.StartupPath + "/GameVersion");
                            File.Delete(Application.StartupPath + "/Foo'sVersion");
                            File.Delete(Application.StartupPath + "/updater");
                            File.Delete(Application.StartupPath + "/Launcher.exe.config");
                            Directory.Delete(Application.StartupPath + "/Mindustry", true);
                            Directory.Delete(Application.StartupPath + "/libs", true);
                            Application.Exit();
                        }
                        else
                        {
                            //
                        }
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //
            }
        }
        private void button11_MouseEnter(object sender, EventArgs e)
        {
            button11.FlatAppearance.BorderSize = 1;
            button11.FlatStyle = FlatStyle.Popup;
        }

        private void button11_MouseLeave(object sender, EventArgs e)
        {
            button11.FlatAppearance.BorderSize = 0;
            button11.FlatStyle = FlatStyle.Flat;
        }
        #endregion

        #endregion

        #region -- Закругление progressBar --
        public static void SetRoundedShape(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.AddLine(0, control.Height - radius, 0, radius);
            path.AddArc(0, 0, radius, radius, 180, 90);
            control.Region = new Region(path);
        }
        #endregion
    }

}