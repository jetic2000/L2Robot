using System.Collections;
using System;

namespace L2Robot
{
    /// <summary>
    /// Summary description for AddInfo.
    /// </summary>
    public static class AddInfo
    {
        public static void Add_CharInfo(GameData gamedata, CharInfo ch_inf)
        {
            //lets check thru the list view to see if the char is there... if so lets update instead of add
            //Globals.PlayerLock.EnterWriteLock();
            //Globals.MyselfLock.EnterWriteLock();
            try
            {
                if (gamedata.nearby_chars.ContainsKey(ch_inf.ID))
                {
                    //in the array already
                    //Globals.gamedata.nearby_chars[ch_inf.ID] = ch_inf;
                    ((CharInfo)gamedata.nearby_chars[ch_inf.ID]).CopyNew(ch_inf);
                }
                else
                {
                    gamedata.nearby_chars.Add(ch_inf.ID, ch_inf);
                }

                FishFlyPlayer p = new FishFlyPlayer();
                p.PlayerName = ch_inf.Name;
                p.PlayerID = ch_inf.ID;
                //Globals.MyselfLock.ExitWriteLock();
                //Globals.MyselfLock.EnterWriteLock();
                p.OwenID = gamedata.my_char.ID;
                Globals.l2net_home.UpdateDataGridView(p);
                //Console.WriteLine("gamedata.fishflyplayerqueue.Enqueue()");

                Globals.FishFlyPlayerQueueLock.EnterWriteLock();
                gamedata.fishflyplayerqueue.Enqueue(p);
                Globals.FishFlyPlayerQueueLock.ExitWriteLock();
            }
            catch
            {
                //oh well
            }
            finally
            {
                //Globals.MyselfLock.ExitWriteLock();
                //Globals.PlayerLock.ExitWriteLock();
            }
        }

        public static void Remove_CharInfo(GameData gamedata, uint id)
        {
            //Globals.PlayerLock.EnterWriteLock();
            //Globals.MyselfLock.EnterWriteLock();
            try
            {
                if (gamedata.nearby_chars.ContainsKey(id))
                {
                    gamedata.nearby_chars.Remove(id);
                }
                FishFlyPlayer p = new FishFlyPlayer();
                p.PlayerName = "";
                p.PlayerID = id;
                //Globals.MyselfLock.EnterWriteLock();
                p.OwenID = gamedata.my_char.ID;
                Globals.l2net_home.UpdateDataGridView(p);
            }
            catch
            {
                //oh well
            }
            finally
            {
                //Globals.MyselfLock.ExitWriteLock();
                //Globals.PlayerLock.ExitWriteLock();
            }
        }

        public static void CleanUp_Char(GameData gamedata)
        {
            ArrayList remove = new ArrayList();

            //Globals.PlayerLock.EnterWriteLock();
            try
            {
                foreach (CharInfo player in gamedata.nearby_chars.Values)
                {
                    int dist = Util.Distance(gamedata.my_char.Current_Pos, player.Current_Pos);

                    if (dist >= Globals.REMOVE_RANGE)
                    {
                        remove.Add(player.ID);
                        //Console.WriteLine("++++++++++++++++++++++++++++++++++++++");
                        //Console.WriteLine("Remove ID:{0}", player.ID);
                    }
                }
            }
            catch
            {
                //oh well
            }
            finally
            {
                //Globals.PlayerLock.ExitWriteLock();
            }


            if (remove.Count > 0)
            {
                foreach (uint id in remove)
                {
                    //Globals.l2net_home.Add_OnlyDebug("Player Cleanup - Removing: " + id.ToString());
                    //Console.WriteLine("Player Cleanup - Removing: " + id.ToString());
                    Remove_CharInfo(gamedata, id);
                    //Console.WriteLine("++++++++++++++++++++++++++++++++++++++");
                    Console.WriteLine("Remove_CharInfo ID:{0}", id);
                }
            }


        }
    }//end of class
}//end of namespace