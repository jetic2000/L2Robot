using System.Collections;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.ComponentModel;

namespace L2Robot
{
    public partial class FormMain
    {
        //All delegate Functions

        //Log
        delegate void UpdateLog_Callback(PlayerMsg msg);
        public void UpdateLog(PlayerMsg msg)
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
                //Console.WriteLine("Invoked");
                //Globals.LogLock.EnterWriteLock();
                try
                {
                    if (this.textBoxLog.GetLineFromCharIndex(this.textBoxLog.Text.Length) > 10000)
                    {
                        this.textBoxLog.Text = "";
                    }

                    /*
                    if (this.listView_instances.SelectedItems.Count > 0)
                    {
                        if ((msg.isForRel == true)&&(this.listView_instances.SelectedItems[0].Text == msg.PlayerName))
                            this.textBoxLog.AppendText(msg.msg + "\r\n");
                    }
                    */
                    if (msg.isForRel == true)
                    {
                        this.textBoxLog.AppendText(msg.msg + "\r\n");
                    }

                }
                finally
                {
                    //Globals.LogLock.ExitWriteLock();
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
    
        //ListView_Instances Update
        delegate void UpdateListViewInstance_Callback(PlayerInstance p);

        public void UpdateInstanceList(PlayerInstance p)
        {
            //we only need to check invoke on 1 chat box... because they are all on the same form
            if (this.listView_instances.InvokeRequired)
            {
                UpdateListViewInstance_Callback d = new UpdateListViewInstance_Callback(UpdateInstanceList);
                listView_instances.Invoke(d, new object[] { p });
                return;  
            }

            try
            {
                this.listView_instances.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

                bool found = false;
                //string backup_processid = "";

                foreach (ListViewItem i in this.listView_instances.Items)
                {
                    if (i.Text == p.PlayerName)
                    {
                        found = true;
                        //backup_processid = i.SubItems[1].Text;
                        //this.listView_instances.Items.Remove(i);
                        if (p.toInit == false)
                        {
                            i.SubItems[2].Text = "(" + p.X.ToString() + "," + p.Y.ToString() + "," + p.Z.ToString() + ")" ;
                            i.SubItems[3].Text = "";
                            i.SubItems[4].Text = p.PlayerID.ToString();
                        }
                        else
                        {
                            //found same name but need to init means process id may change, 
                            //need to put empty to ask user to reconnect
                            this.listView_instances.Items.Remove(i);
                            //i.SubItems[1].Text = "未获取";
                            //i.SubItems[2].Text = "(" + p.X.ToString() + "," + p.Y.ToString() + "," + p.Z.ToString() + ")" ;
                            //i.SubItems[3].Text = "";
                            //i.SubItems[4].Text = p.PlayerID.ToString();
                        }
                        //this.listView_instances.Items.Add(lvi);
                        break;
                    }
                }

                if ((found == false) && (p.toInit == false))
                {
                    //Init FishFly Binding List for this user
                    //Console.WriteLine("+Add Binding List for new found user");
                    BindingList<FishFlyPlayer> fp = new BindingList<FishFlyPlayer>();
                    FishFlyPlayerLists.Add(p.PlayerID, fp);
                    //Console.WriteLine("-Add Binding List for new found user");

                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = p.PlayerName;
                    lvi.SubItems.Add("未获取");
                    lvi.SubItems.Add("(" + p.X.ToString() + "," + p.Y.ToString() + "," + p.Z.ToString() + ")" );
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add(p.PlayerID.ToString());
                    this.listView_instances.Items.Add(lvi);
                }

                /*
                if (p.toInit == false)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = p.PlayerName;
                    //lvi.SubItems.Add(p.ProcessPid.ToString());
                    if (backup_processid == "")
                    {
                        lvi.SubItems.Add("未获取");
                    }
                    else
                    {
                        lvi.SubItems.Add(backup_processid);
                    }
                    //lvi.SubItems.Add("未获取");
                    lvi.SubItems.Add("(" + p.X.ToString() + "," + p.Y.ToString() + "," + p.Z.ToString() + ")" );
                    lvi.SubItems.Add("");
                    lvi.SubItems.Add(p.PlayerID.ToString());
                    this.listView_instances.Items.Add(lvi);
                }
                else
                {
                    this.lvBtn.Text = "未激活";
                    
                }
                */
                
                this.listView_instances.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }
            catch
            {

            }
            finally
            {

            }
        }


        delegate void UpdateDataGridView_Callback(FishFlyPlayer p);
        public void UpdateDataGridView(FishFlyPlayer p)
        {
            BindingList<FishFlyPlayer> fp;

            //we only need to check invoke on 1 chat box... because they are all on the same form
            if (this.dataGridViewNearPlayer.InvokeRequired)
            {
                UpdateDataGridView_Callback d = new UpdateDataGridView_Callback(UpdateDataGridView);
                listView_instances.Invoke(d, new object[] { p });
                return;
            }

            try
            {
                //Update BindingList
                //Console.WriteLine("+Get Right Binding List, p.PlayerID:{0}, p.OwenID:{0}", p.PlayerID, p.OwenID);
                if (p.toInit == true)
                {
                    FishFlyPlayerLists.Remove(p.RemoveID);
                    fp = new BindingList<FishFlyPlayer>();
                    FishFlyPlayerLists.Add(p.PlayerID, fp);
                }
                else
                {

                    fp = FishFlyPlayerLists[p.OwenID]; 
                    //Console.WriteLine("-Get Right Binding List");
                    if (p.PlayerName == "")
                    {
                        //Remove
                        for (int i = 0; i < fp.Count; i++)
                        {
                            if (fp[i].PlayerID == p.PlayerID)
                            {
                                //Console.WriteLine("Remove FP Player ID：{0}", p.PlayerID);
                                fp.Remove(fp[i]);
                            }
                        }
                    }
                    else
                    {
                        //Add
                        if (fp.Contains(p))
                        {
                            //fp.Remove(p);
                        }
                        else
                        {
                            fp.Add(p);
                        }

                        //Console.WriteLine("Add/Update FP Player ID：{0}", p.PlayerID);
                        //fp.Add(p);
                        //fp.Insert(0, p);
                    } 
                }
            }
            catch
            {

            }
            finally
            {

            }
        }

        delegate void ClearBlackList_Callback();
        public void ClearBlackList()
        {
            //we only need to check invoke on 1 chat box... because they are all on the same form
            if (this.listViewBlackList.InvokeRequired)
            {
                ClearBlackList_Callback d = new ClearBlackList_Callback(ClearBlackList);
                listViewBlackList.Invoke(d);
                return;
            }

            this.listViewBlackList.Clear();
        }

    }
}
