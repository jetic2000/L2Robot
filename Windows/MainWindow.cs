using System.Linq.Expressions;
using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Linq;

namespace L2Robot
{
    public partial class FormMain : Form
    {
        public SmartTimer timer_instances;
        public SmartTimer timer_gui;
        public int selectedClient = -1;
        //public ArrayList listView_instances_items;

        public FormMain()
        {
            InitializeComponent();

            Globals.l2net_home = this;

            //Init all timers and etc
            timer_instances = new SmartTimer();
            timer_instances.Interval = 1500;
            timer_instances.OnTimerTick += timer_Tick_Instances;

            timer_gui = new SmartTimer();
            timer_gui.Interval = 1000;
            timer_gui.OnTimerTick += timer_Tick_UpdateGUI;
        }

        private void buttonIGStart_Click(object sender, EventArgs e)
        {
            //Globals.l2net_home.timer_instances.Start();
            LoginServer.IG_Init();
            this.buttonIGStart.Enabled = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonLoadScript_Click(object sender, EventArgs e)
        {
            // if (selectedClient == -1)
            // {
            //     MessageBox.Show("请先在左边列表中一个游戏进程");
            //     return;
            // }

            this.openFileDialogScript.Filter = "脚本文件(*.txt)|*.txt";
            if (this.openFileDialogScript.ShowDialog() == DialogResult.OK)
            {
                // if (selectedClient != -1)
                // {
                //     FileInfo myFile = new FileInfo(openFileDialogScript.FileName);
                //     this.textBoxScriptFile.Text = myFile.Name;

                //     GameData gamedata = Globals.Games[selectedClient];
                //     gamedata.Script_File = openFileDialogScript.FileName;
                //     this.buttonRunScript.Enabled = true;
                // }
                FileInfo myFile = new FileInfo(openFileDialogScript.FileName);
                this.textBoxScriptFile.Text = myFile.Name;

                Globals.Script_File = openFileDialogScript.FileName;
                this.buttonRunScript.Enabled = true;
            }
        }

        public void SetStartScript()
        {
            this.buttonRunScript.Text = "点击停止";
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            GameData gamedata;
            Boolean isAllStopped = false;

            Globals.InstancesLock.EnterWriteLock();

            if (this.buttonRunScript.Text == "点击开始")
            {
                while(true)
                {
                    for (int i = 0; i < Globals.Games.Count; i++)
                    {
                        gamedata = Globals.Games.ElementAt(i).Value;
                        Console.WriteLine("+++GameData Port：{0}, ScriptStatus: {1}", gamedata.Client_Port, gamedata.CurrentScriptState);
                        if (gamedata.CurrentScriptState == ScriptState.Stopped || gamedata.CurrentScriptState == ScriptState.Finished)
                        {
                            try
                            {
                                //add reset here... to start the script over always...
                                gamedata.scriptthread.Reset_Script();
                                gamedata.Script_File = Globals.Script_File;
                                gamedata.CurrentScriptState = ScriptState.Running;//running
                                gamedata.BOTTING = true;
                                gamedata.scriptthread.scriptthread.Start();
                                //this.buttonRunScript.Text = "点击停止";
                            }
                            catch
                            {
                                //this.Add_Error("Failed to START scripting thread" + Environment.NewLine + "Try to reset the script first?");
                            }
                        }
                    }

                    isAllStopped = false;
                    for (int i = 0; i < Globals.Games.Count; i++)
                    {
                        gamedata = Globals.Games.ElementAt(i).Value;
                        if (gamedata.CurrentScriptState == ScriptState.Stopped || gamedata.CurrentScriptState == ScriptState.Finished)
                        {
                            isAllStopped = true;
                        }
                    }
                    if (isAllStopped == false)
                    {
                        this.buttonRunScript.Text = "点击停止";
                        break;
                    }
                }
            }
            else
            {
                while(true)
                {
                    for (int i = 0; i < Globals.Games.Count; i++)
                    {
                        try
                        {
                            gamedata = Globals.Games.ElementAt(i).Value;
                            gamedata.scriptthread.scriptthread.Abort();
                            gamedata.CurrentScriptState = ScriptState.Stopped;//stopped
                            gamedata.BOTTING = false;
                            //this.buttonRunScript.Text = "点击开始";
                            //menuItem_startscript.Text = Globals.m_ResourceManager.GetString("menuItem_startscript");
                            //startScriptToolStripMenuItem.Text = Globals.m_ResourceManager.GetString("menuItem_startscript");
                        }
                        catch
                        {
                            //this.Add_Error("Failed to STOP scripting thread");
                        }
                    }

                    isAllStopped = true;
                    for (int i = 0; i < Globals.Games.Count; i++)
                    {
                        gamedata = Globals.Games.ElementAt(i).Value;
                        if (gamedata.CurrentScriptState == ScriptState.Running)
                        {
                            isAllStopped = false;
                        }
                    }
                    if (isAllStopped == true)
                    {
                        this.buttonRunScript.Text = "点击开始";
                        break;
                    }
                }
            }

            Globals.InstancesLock.ExitWriteLock();

/*
                else
                {
                    try
                    {
                        gamedata.scriptthread.scriptthread.Abort();
                        gamedata.CurrentScriptState = ScriptState.Stopped;//stopped
                        gamedata.BOTTING = false;
                        //this.buttonRunScript.Text = "点击开始";
                        //menuItem_startscript.Text = Globals.m_ResourceManager.GetString("menuItem_startscript");
                        //startScriptToolStripMenuItem.Text = Globals.m_ResourceManager.GetString("menuItem_startscript");
                    }
                    catch
                    {
                        //this.Add_Error("Failed to STOP scripting thread");
                    }
                }
            }、
*/
        }

        private void listView_instances_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
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

        private void button1_Click(object sender, EventArgs e)
        {
            Globals.PATH = Environment.CurrentDirectory;
            GameData gamedata = new GameData();
            gamedata.scriptthread = new ScriptEngine(gamedata);
            gamedata.Script_File = Globals.PATH + "\\Scripts\\test.txt";
            //gamedata.Script_File = "test.txt";
            gamedata.BOTTING = true;
            gamedata.running = true;
            gamedata.CurrentScriptState = ScriptState.Running;
            gamedata.scriptthread.scriptthread.Start();
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

                    if (gamedata.scriptthread.scriptthread != null)
                    {
                        gamedata.scriptthread.scriptthread.Abort();
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
    }
}
