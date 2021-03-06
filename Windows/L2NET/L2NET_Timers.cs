using System.Collections;
using System.Windows.Forms;
using System;
using System.Drawing;

namespace L2Robot
{
    public partial class FormMain
    {
        //All delegate Functions
        
        public void timer_instances_Tick()
        {
            //need to update the items list
            Console.WriteLine("timer_instances_Tick()");
            timer_instances.Stop();
            UpdateInstancesList();
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
                Globals.InstancesLock.EnterReadLock();
                try
                {
                    UpdateInstancesListInternal();
                }
                finally
                {
                    Globals.InstancesLock.ExitReadLock();
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


    }
}
