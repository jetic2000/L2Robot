using System.Linq.Expressions;
using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;

namespace L2Robot
{
    public partial class FormMain : Form
    {
        //public SmartTimer timer_instances;
        //public SmartTimer timer_gui;
        public int selectedClient = -1;

        //public ArrayList listView_instances_items;

        //public BindingList<FishFlyPlayer> fp = new BindingList<FishFlyPlayer>();
        public Dictionary<uint, BindingList<FishFlyPlayer>> FishFlyPlayerLists = new Dictionary<uint, BindingList<FishFlyPlayer>>();

        [DllImport("user32.dll")]       
        public static extern int GetCursorPos(ref Point lpPoint);  //获取鼠标坐标，该坐标是光标所在的屏幕坐标位置
        [DllImport("user32.dll")]        
        public static extern int WindowFromPoint(int xPoint,int yPoint); 
        public Button lvBtn;

        public FormMain()
        {
            InitializeComponent();

            Globals.l2net_home = this;
            this.dataGridViewNearPlayer.AutoGenerateColumns = false;
            this.buttonGetHwd.Enabled = false;
            
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 25);
            this.listView_instances.SmallImageList = imgList;

            lvBtn = new Button();
            listView_instances.Controls.Add(lvBtn);
            lvBtn.Visible = false;
            lvBtn.Click += new EventHandler(lvBtn_Click);

            //Init all timers and etc
            //timer_instances = new SmartTimer();
            //timer_instances.Interval = 1500;
            //timer_instances.OnTimerTick += timer_Tick_Instances;

            //timer_gui = new SmartTimer();
            //timer_gui.Interval = 1000;
            //timer_gui.OnTimerTick += timer_Tick_UpdateGUI;
        }

        private void buttonIGStart_Click(object sender, EventArgs e)
        {
            //Globals.l2net_home.timer_instances.Start();
            LoginServer.IG_Init();
            this.buttonIGStart.Enabled = false;
        }

        private void listView_instances_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            GameData gamedata;
            try
            {       
                    if (this.listView_instances.SelectedItems.Count > 0)
                    {
                        this.lvBtn.Visible = false;
                        uint ID = uint.Parse((this.listView_instances.SelectedItems[0].SubItems[4].Text));
                        string PlayerName = this.listView_instances.SelectedItems[0].Text;
                        Console.WriteLine("-------ID:{0}", ID);
                        this.dataGridViewNearPlayer.DataSource = FishFlyPlayerLists[ID];
                        this.buttonGetHwd.Enabled = true;

                        this.lvBtn.Size = new Size(listView_instances.SelectedItems[0].SubItems[3].Bounds.Width,
                            listView_instances.SelectedItems[0].SubItems[3].Bounds.Height);
                        this.lvBtn.Location = new Point(listView_instances.SelectedItems[0].SubItems[3].Bounds.Left,
                                                listView_instances.SelectedItems[0].SubItems[3].Bounds.Top);

                        gamedata = GetCurrentGameData(PlayerName);
                        if (gamedata.fishflythread.bRunning == false)
                        {
                            this.lvBtn.Text = "未激活";
                        }
                        else
                        {
                            this.lvBtn.Text = "点击停止";
                        }
                        this.lvBtn.Visible = true;
                    }
                    else
                    {
                        this.buttonGetHwd.Enabled = false;
                    }
            }
            catch
            {

            }

            // if (listView_instances.SelectedItems.Count > 0)
            // {
            //     selectedClient = int.Parse(listView_instances.SelectedItems[0].SubItems[1].Text.ToString());
            //     Console.WriteLine("Selected Item : {0}", selectedClient);
            //     GameData gamedata = Globals.Games[selectedClient];
            //     if (gamedata.CurrentScriptState == ScriptState.Stopped || gamedata.CurrentScriptState == ScriptState.Finished)
            //     {
            //         this.buttonRunScript.Text = "点击开始";
            //     }
            //     else
            //     {
            //         this.buttonRunScript.Text = "点击停止";
            //     }
            // }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameData gamedata;

            Globals.running = false;

            Globals.InstancesLock.EnterWriteLock();

            for (int i = 0; i < Globals.Games.Count; i++)
            {
                Console.WriteLine("game clean up");
                gamedata = Globals.Games.ElementAt(i).Value;

                gamedata.running = false;

                try {
                    if (gamedata.gameprocessdatathread != null)
                    {
                        gamedata.gameprocessdatathread.processthread.Abort();
                    }

                    if (gamedata.clientthread != null)
                    {
                        gamedata.clientthread.sendthread.Abort();
                        gamedata.clientthread.readthread.Abort();
                    }

                    if (gamedata.gamethread != null)
                    {
                        gamedata.gamethread.sendthread.Abort();
                        gamedata.gamethread.readthread.Abort();
                    }

                    if (gamedata.Game_ClientLink != null)
                    {
                        gamedata.Game_ClientLink.Stop();
                    }
                    gamedata.Game_ClientLink = null;

                    if (gamedata.Game_ClientSocket != null)
                    {
                        gamedata.Game_ClientSocket.Close();
                    }
                    gamedata.Game_ClientSocket = null;

                    if (gamedata.Game_GameSocket != null)
                    {
                        gamedata.Game_GameSocket.Close();
                    }
                    gamedata.Game_GameSocket = null;

                    
                }
                catch
                {
                    Console.WriteLine("Error during clean up");
                }
            }
            Globals.InstancesLock.ExitWriteLock();

            if (Globals.waitingGameData != null)
            {
                if (Globals.waitingGameData.Game_ClientLink != null)
                {
                    Globals.waitingGameData.Game_ClientLink.Stop();
                }
                Globals.waitingGameData.Game_ClientLink = null;

                if (Globals.waitingGameData.Game_ClientSocket != null)
                {
                    Globals.waitingGameData.Game_ClientSocket.Close();
                }
            }
        }

        private void buttonGetHwd_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void buttonGetHwd_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void buttonGetHwd_MouseUp(object sender, MouseEventArgs e)
        {
            Point pi = new Point();
            int hwnd;
            GameData gamedata;
            GetCursorPos(ref pi); //获取鼠标坐标值
            Console.WriteLine( "坐标：X={0}, Y={1}", pi.X, pi.Y);
            hwnd = WindowFromPoint(pi.X, pi.Y);
            Console.WriteLine( "窗口句柄：Hwnd=0x{0:X}", hwnd);
            Cursor = Cursors.Default;
            
            if (this.listView_instances.Items.Count > 0)
            {
                if (this.listView_instances.SelectedItems.Count == 1)
                {
                    this.listView_instances.SelectedItems[0].SubItems[1].Text = hwnd.ToString();
                    GetBlackList();

                    Globals.InstancesLock.EnterWriteLock();
                    for (int j = 0; j < Globals.Games.Count; j++)
                    {
                        gamedata = Globals.Games.ElementAt(j).Value;
                        if (gamedata.my_char.Name == this.listView_instances.SelectedItems[0].Text)
                        {
                            gamedata.processPid = hwnd;
                            break;
                        }
                    }
                    Globals.InstancesLock.ExitWriteLock();
                }
            }
        }

        private void GetBlackList()
        {
            List<String> p;
            GameData gamedata;
            string selectedPlayer = "";

            this.listViewBlackList.Items.Clear();
            if (this.listView_instances.Items.Count > 0)
            {
                if (this.listView_instances.SelectedItems.Count == 1)
                {
                    selectedPlayer = this.listView_instances.SelectedItems[0].Text;
                }
            }
            Console.WriteLine("Selected Player{0}", selectedPlayer);

            if (selectedPlayer != "")
            { 
                for (int i = 0; i < Globals.Games.Count; i++)
                {
                    gamedata = Globals.Games.ElementAt(i).Value;
                    if (gamedata.my_char.Name == selectedPlayer)
                    {
                        Console.WriteLine("Find Player:{0}", selectedPlayer);
                        string cfg = selectedPlayer + ".txt";
                        gamedata.blacklist_chars.UpdataBlackList(cfg);
                        p = gamedata.blacklist_chars.GetBlackList();

                        foreach(string tmp in p)
                        {
                            //Console.WriteLine(tmp);
                            this.listViewBlackList.Items.Add(tmp);
                        }

                        break;
                    }
                }
            }
        }

        private GameData GetCurrentGameData(String PlayerName)
        {
            bool found = false;
            GameData gamedata = null;

            if (PlayerName != "")
            { 
                for (int i = 0; i < Globals.Games.Count; i++)
                {
                    gamedata = Globals.Games.ElementAt(i).Value;
                    if (gamedata.my_char.Name == PlayerName)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (found == true)
            {
                return gamedata;
            }
            else
            {
                return null;
            }
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            /*
            Point pi = new Point();
            int hwnd;
            GameData gamedata;
            GetCursorPos(ref pi); //获取鼠标坐标值
            Console.WriteLine("坐标：X={0}, Y={1}", pi.X, pi.Y);
            hwnd = WindowFromPoint(pi.X, pi.Y);
            Console.WriteLine("窗口句柄：Hwnd=0x{0:X}", hwnd);
            Cursor = Cursors.Default;

            BackgroundKey key = new BackgroundKey(hwnd, 0);
            key.PressKey();
            */
        }

        private void lvBtn_Click(object sender, EventArgs e)
        {
            string PlayerName = "";
            GameData gamedata;
            if (this.listView_instances.SelectedItems.Count > 0)
            {
                Console.WriteLine("lvBtn_Clicked, Select:{0}", listView_instances.SelectedItems[0].Text);
                PlayerName = listView_instances.SelectedItems[0].Text;

                gamedata = GetCurrentGameData(PlayerName);
                if (gamedata.fishflythread.bRunning == false)
                {
                    this.lvBtn.Text = "点击停止";
                    gamedata.fishflythread.bRunning = true;
                    gamedata.fishflythread.bGetData = true;
                }
                else
                {
                    this.lvBtn.Text = "未激活";
                    gamedata.fishflythread.bRunning = false;
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListViewItem p0 = new ListViewItem();
            p0.Text = "哈哈哈";
            p0.SubItems.Add("1111");
            p0.SubItems.Add("2222");
            p0.SubItems.Add("");
            p0.SubItems.Add("3333");
            this.listView_instances.Items.Add(p0);

            ListViewItem p1 = new ListViewItem();
            p1.Text = "乐乐乐";
            p1.SubItems.Add("4444");
            p1.SubItems.Add("5555");
            p1.SubItems.Add("");
            p1.SubItems.Add("6666");
            this.listView_instances.Items.Add(p1);
        }

        private void listView_instances_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = this.listView_instances.Columns[e.ColumnIndex].Width;
        }
    }
}
