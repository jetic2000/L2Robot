using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace L2Robot
{
    public partial class FormMain : Form
    {
        public SmartTimer timer_instances;
        public int selectedClient = -1;
        //public ArrayList listView_instances_items;

        public FormMain()
        {
            InitializeComponent();

            Globals.l2net_home = this;

            //Init all timers and etc
            timer_instances = new SmartTimer();
            timer_instances.Interval = 1500;
            timer_instances.OnTimerTick += timer_instances_Tick;

            //UpdateInstancesListInternal();
            //listView_instances_items = new ArrayList();
            //timer_instances.Start();
        }

        private void buttonIGStart_Click(object sender, EventArgs e)
        {
            //Globals.l2net_home.timer_instances.Start();
            LoginServer.IG_Init();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonLoadScript_Click(object sender, EventArgs e)
        {
            if (selectedClient == -1)
            {
                MessageBox.Show("请先在左边列表中一个游戏进程");
                return;
            }

            this.openFileDialogScript.Filter = "脚本文件(*.txt)|*.txt";
            if (this.openFileDialogScript.ShowDialog() == DialogResult.OK)
            {
                if (selectedClient != -1)
                {
                    FileInfo myFile = new FileInfo(openFileDialogScript.FileName);
                    this.textBoxScriptFile.Text = myFile.Name;

                    GameData gamedata = Globals.Games[selectedClient];
                    gamedata.Script_File = openFileDialogScript.FileName;
                    this.buttonRunScript.Enabled = true;
                }
            }
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            GameData gamedata = Globals.Games[selectedClient];
            //GameData gamedata = new GameData();
            //gamedata.running = true;

            if (gamedata.running)
            {
                //need to toggle scripting on and off
                if (gamedata.CurrentScriptState == ScriptState.Stopped || gamedata.CurrentScriptState == ScriptState.Finished)
                {
                    try
                    {
                        //add reset here... to start the script over always...
                        gamedata.scriptthread.Reset_Script();
                        //gamedata.Script_File = "test.txt";
                        //start the thread
                        gamedata.scriptthread.scriptthread.Start();
                        gamedata.CurrentScriptState = ScriptState.Running;//running
                        gamedata.BOTTING = true;
                        this.buttonRunScript.Text = "点击停止";
                        //menuItem_startscript.Text = Globals.m_ResourceManager.GetString("menuItem_stopscript");
                        //startScriptToolStripMenuItem.Text = Globals.m_ResourceManager.GetString("menuItem_stopscript");
                    }
                    catch
                    {
                        //this.Add_Error("Failed to START scripting thread" + Environment.NewLine + "Try to reset the script first?");
                    }
                }
                else
                {
                    try
                    {
                        gamedata.scriptthread.scriptthread.Abort();
                        gamedata.CurrentScriptState = ScriptState.Stopped;//stopped
                        gamedata.BOTTING = false;
                        this.buttonRunScript.Text = "点击开始";
                        //menuItem_startscript.Text = Globals.m_ResourceManager.GetString("menuItem_startscript");
                        //startScriptToolStripMenuItem.Text = Globals.m_ResourceManager.GetString("menuItem_startscript");
                    }
                    catch
                    {
                        //this.Add_Error("Failed to STOP scripting thread");
                    }
                }
            }
        }

        private void listView_instances_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (listView_instances.SelectedItems.Count > 0)
            {
                selectedClient = int.Parse(listView_instances.SelectedItems[0].SubItems[1].Text.ToString());
                Console.WriteLine("Selected Item : {0}", selectedClient);
                GameData gamedata = Globals.Games[selectedClient];
                if (gamedata.CurrentScriptState == ScriptState.Stopped || gamedata.CurrentScriptState == ScriptState.Finished)
                {
                    this.buttonRunScript.Text = "点击开始";
                }
                else
                {
                    this.buttonRunScript.Text = "点击停止";
                }
            }
        }

        private void buttonOOG_Click(object sender, EventArgs e)
        {
            if (selectedClient == -1)
            {
                MessageBox.Show("请先在左边列表中一个游戏进程");
                return;
            }
            GameData gamedata = Globals.Games[selectedClient];
            gamedata.clientthread.readthread.Abort();
            gamedata.clientthread.sendthread.Abort();
        }
    }
}
