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
            Globals.PlayerLock.EnterWriteLock();

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
            }
            catch
            {
                //oh well
            }
            finally
            {
                Globals.PlayerLock.ExitWriteLock();
            }
        }

        public static void Remove_CharInfo(GameData gamedata, uint id)
        {
            try
            {
                if (gamedata.nearby_chars.ContainsKey(id))
                {
                    gamedata.nearby_chars.Remove(id);
                }
            }
            catch
            {
                //oh well
            }
        }

        public static void CleanUp_Char(GameData gamedata)
        {
            ArrayList remove = new ArrayList();

            Globals.PlayerLock.EnterWriteLock();
            try
            {
                foreach (CharInfo player in gamedata.nearby_chars.Values)
                {
                    int dist = Util.Distance(gamedata.my_char.X, gamedata.my_char.Y, gamedata.my_char.Z, player.X, player.Y, player.Z);

                    if (dist >= Globals.REMOVE_RANGE)
                    {
                        remove.Add(player.ID);
                    }
                }

                if (remove.Count > 0)
                {
                    foreach (uint id in remove)
                    {
                        //Globals.l2net_home.Add_OnlyDebug("Player Cleanup - Removing: " + id.ToString());
                        //Console.WriteLine("Player Cleanup - Removing: " + id.ToString());
                        Remove_CharInfo(gamedata, id);
                    }
                }
            }
            catch
            {
                //oh well
            }
            finally
            {
                Globals.PlayerLock.ExitWriteLock();
            }

        }
    }//end of class
}//end of namespace