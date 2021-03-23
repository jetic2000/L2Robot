using System.Collections;
using System.Windows.Forms;
using System;
using System.Drawing;

namespace L2Robot
{
    public partial class FormMain
    {
        //All delegate Functions

        public void timer_Tick_UpdateGUI()
        {
            //need to update the items list
            timer_gui.Stop();
            UpdateGUI();
        }

        delegate void UpdateGUI_Callback();
        public void UpdateGUI()
        {
            //we only need to check invoke on 1 chat box... because they are all on the same form
            if (this.buttonLoadScript.InvokeRequired)
            {
                UpdateGUI_Callback d = new UpdateGUI_Callback(UpdateGUI);
                buttonLoadScript.Invoke(d);
                return;
            }

            this.buttonLoadScript.Enabled = true;
        }

        
        public void timer_Tick_Instances()
        {
            //need to update the items list
            Console.WriteLine("timer_instances_Tick()");
            timer_instances.Stop();
            UpdateInstancesList();
        }

        private void RemoveInstance(GameData gamedata)
        {
            Console.WriteLine("RemoveInstance: {0}", gamedata.Client_Port);
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

        private void UpdateInstancesListInternal()
        {
            
            ArrayList dirty_items = new ArrayList();
            for (int i = 0; i < listView_instances.Items.Count; i++)
            {
                int id = Util.GetInt32(listView_instances.Items[i].SubItems[1].Text);
                if (Globals.Games.ContainsKey(id))
                {

                }
                else
                {
                    dirty_items.Add(i);
                }
            }

            //need to remove all dirty items now
            for (int i = dirty_items.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("++++++++++++++++Remove something?");
                listView_instances.Items.RemoveAt((int)dirty_items[i]);
            }

            dirty_items.Clear();

            foreach (GameData gamedata in Globals.Games.Values)
            {
                //find the item in our arraylist...
                if (gamedata.InList == false)
                {
                    //add it
                    ListViewItem ObjListItem;
                    ObjListItem = new ListViewItem();
                    ObjListItem.Text = "";
                    //ObjListItem.SubItems.Add(gamedata.Client_Port.ToString());
                    ObjListItem.SubItems.Add(gamedata.Client_Port.ToString());
                    Console.WriteLine("++++++++++++++++");
                    listView_instances.Items.Add(ObjListItem);
                    gamedata.InList = true;
                }
            }

            foreach (GameData gamedata in Globals.Dirty_Games)
            {
                RemoveInstance(gamedata);
            }
            Globals.Dirty_Games.Clear();
        }

        public delegate void UpdateInstancesListDelegate();
        private void UpdateInstancesList()
        {
            if (listView_instances.InvokeRequired)
            {
                Console.WriteLine("InvokeRequired");
                UpdateInstancesListDelegate d = new UpdateInstancesListDelegate(UpdateInstancesList);
                listView_instances.Invoke(d);
                return;
            }

            try
            {
                Console.WriteLine("Invoked");
                Globals.InstancesLock.EnterWriteLock();
                try
                {
                    UpdateInstancesListInternal();
                }
                finally
                {
                    Globals.InstancesLock.ExitWriteLock();
                }
                //listView_instances.VirtualListSize = listView_instances_items.Count;
            }
            catch
            {
                //failed to acquire the lock...
            }
            finally
            {
                //listView_instances.EndUpdate();
                //listView_instances.Refresh();
            }

        }

        //Log
        delegate void UpdateLog_Callback(string msg);
        public void UpdateLog(string msg)
        {
            //we only need to check invoke on 1 chat box... because they are all on the same form
            if (this.textBoxLog.InvokeRequired)
            {
                UpdateLog_Callback d = new UpdateLog_Callback(UpdateLog);
                textBoxLog.Invoke(d, new object[] { msg });
                return;
            }

            try
            {
                Console.WriteLine("Invoked");
                Globals.LogLock.EnterWriteLock();
                try
                {
                    if (this.textBoxLog.GetLineFromCharIndex(this.textBoxLog.Text.Length) > 2000)
                    {
                        this.textBoxLog.Text = "";
                    }
                    this.textBoxLog.AppendText(msg + "\r\n");
                }
                finally
                {
                    Globals.LogLock.ExitWriteLock();
                }
                //listView_instances.VirtualListSize = listView_instances_items.Count;
            }
            catch
            {

            }
            finally
            {

            }

        }



    }
}
