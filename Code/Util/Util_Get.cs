namespace L2Robot
{
    public static partial class Util
    {
        static public void ClearAllNearby(GameData gamedata)
        {
            //Globals.l2net_home.listView_npc_data.VirtualListSize = 0;
            //Globals.l2net_home.listView_items_data.VirtualListSize = 0;
            //Globals.l2net_home.listView_players_data.VirtualListSize = 0;

            //lets get rid of all the mobs/npcs that are in our list
            Globals.ItemLock.EnterWriteLock();
            try
            {
                gamedata.nearby_items.Clear();
                //Globals.l2net_home.listView_inventory_items.Clear();
            }
            finally
            {
                Globals.ItemLock.ExitWriteLock();
            }

            Globals.NPCLock.EnterWriteLock();
            try
            {
                gamedata.nearby_npcs.Clear();
                //Globals.l2net_home.listView_npc_data_items.Clear();
            }
            finally
            {
                Globals.NPCLock.ExitWriteLock();
            }

            Globals.PlayerLock.EnterWriteLock();
            try
            {
                gamedata.nearby_chars.Clear();
                //Globals.l2net_home.listView_players_data_items.Clear();
            }
            finally
            {
                Globals.PlayerLock.ExitWriteLock();
            }

            /*Globals.l2net_home.timer_players.Start();
            Globals.l2net_home.timer_items.Start();
            Globals.l2net_home.timer_npcs.Start();*/
        }

        static public void GetLoc(GameData gamedata, uint id, ref int x, ref int y, ref int z)
        {
            //is self?
            if (gamedata.my_char.ID == id)
            {
                x = Float_Int32(gamedata.my_char.X);
                y = Float_Int32(gamedata.my_char.Y);
                z = Float_Int32(gamedata.my_char.Z);
                return;
            }

            //is in the list of chars?
            Globals.PlayerLock.EnterReadLock();
            try
            {
                CharInfo player = GetChar(gamedata, id);

                if (player != null)
                {
                    x = Float_Int32(player.X);
                    y = Float_Int32(player.Y);
                    z = Float_Int32(player.Z);
                    return;
                }
            }
            finally
            {
                Globals.PlayerLock.ExitReadLock();
            }

            Globals.NPCLock.EnterReadLock();
            try
            {
                NPCInfo npc = GetNPC(gamedata, id);

                if (npc != null)
                {
                    x = Float_Int32(npc.X);
                    y = Float_Int32(npc.Y);
                    z = Float_Int32(npc.Z);
                    return;
                }
            }
            finally
            {
                Globals.NPCLock.ExitReadLock();
            }
        }

        public static TargetType GetType(GameData gamedata, uint id)
        {
            if (id == 0)
            {
                return TargetType.NONE;
            }
            if (gamedata.my_char.ID == id)
            {
                return TargetType.SELF;
            }

            Globals.PlayerLock.EnterReadLock();
            try
            {
                if (isChar(gamedata, id))
                {
                    return TargetType.PLAYER;
                }
            }
            finally
            {
                Globals.PlayerLock.ExitReadLock();
            }

            Globals.NPCLock.EnterReadLock();
            try
            {
                if (isNPC(gamedata, id))
                {
                    return TargetType.NPC;
                }
            }
            finally
            {
                Globals.NPCLock.ExitReadLock();
            }

            return TargetType.ERROR;
        }

        public static bool isChar(GameData gamedata, uint id)
        {

            if (gamedata.nearby_chars.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static CharInfo GetChar(GameData gamedata, uint id)
        {
            try
            {
                return (CharInfo)gamedata.nearby_chars[id];
            }
            catch
            {
            }
            return null;
        }

        public static CharInfo GetChar(GameData gamedata, string str)
        {
            str = str.ToUpperInvariant();

            foreach (CharInfo player in gamedata.nearby_chars.Values)
            {
                if (System.String.Equals(player.Name.ToUpperInvariant(), str))
                {
                    return player;
                }
            }

            return null;
        }

        public static bool isNPC(GameData gamedata, uint id)
        {
            if (gamedata.nearby_npcs.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static NPCInfo GetNPC(GameData gamedata, uint id)
        {
            try
            {
                return (NPCInfo)gamedata.nearby_npcs[id];
            }
            catch
            {
                return null;
            }
        }

        public static void IgnoreNPC(GameData gamedata, uint id, bool ignore)
        {
            Globals.NPCLock.EnterReadLock();
            try
            {
                ((NPCInfo)gamedata.nearby_npcs[id]).Ignore = ignore;
            }
            catch
            {
            }
            finally
            {
                Globals.NPCLock.ExitReadLock();
            }
        }
    }
}
