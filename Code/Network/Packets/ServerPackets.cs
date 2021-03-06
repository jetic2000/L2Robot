using System;
using System.Threading;

namespace L2Robot
{
    /// <summary>
    /// Summary description for GamePackets.
    /// </summary>
    static public class ServerPackets
    {
        public static void StatusUpdate(GameData gamedata, ByteBuffer DataBuffer)
        {
            uint EntityID = DataBuffer.ReadUInt32();

        }

        public static void EXUserInfo(GameData gamedata, ByteBuffer buffe)
        {
            CharInfo player = new CharInfo();
            player.Load(buffe);
            AddInfo.Add_CharInfo(gamedata, player);
            Console.WriteLine("Num of near char : {0}", gamedata.nearby_chars.Count);
        }

        public static void ExSetCompassZoneCode(GameData gamedata, ByteBuffer buff)
        {
            uint type = buff.ReadUInt32();//
            gamedata.cur_zone = type;
        }


        public static void NetPing(GameData gamedata, ByteBuffer buffe)
        {

        }

        public static void DeleteItem(GameData gamedata, ByteBuffer buffe)
        {
            uint dead_object = buffe.ReadUInt32();//System.BitConverter.ToInt32(buffe,1);
            //buffe.ReadUInt32();
            //4 bytes of poop after the objid
            
            /* Add back later: Jack
            if (gamedata.my_char.MoveTarget == dead_object)
            {
                gamedata.my_char.Moving = false;
                gamedata.my_char.MoveTarget = 0;
                gamedata.my_char.MoveTargetType = TargetType.NONE;
                gamedata.my_char.Dest_X = gamedata.my_char.X;
                gamedata.my_char.Dest_Y = gamedata.my_char.Y;
                gamedata.my_char.Dest_Z = gamedata.my_char.Z;
            }

            Globals.PlayerLock.EnterReadLock();
            try
            {
                foreach (CharInfo player in gamedata.nearby_chars.Values)
                {
                    if (player.MoveTarget == dead_object)
                    {
                        player.Moving = false;
                        player.MoveTarget = 0;
                        player.MoveTargetType = TargetType.NONE;
                        player.Dest_X = player.X;
                        player.Dest_Y = player.Y;
                        player.Dest_Z = player.Z;
                    }
                }
            }
            catch
            {
                //oops
            }
            finally
            {
                Globals.PlayerLock.ExitReadLock();
            }

            Globals.NPCLock.EnterReadLock();
            try
            {
                foreach (NPCInfo npc in gamedata.nearby_npcs.Values)
                {
                    if (npc.MoveTarget == dead_object)
                    {
                        npc.Moving = false;
                        npc.MoveTarget = 0;
                        npc.MoveTargetType = TargetType.NONE;
                        npc.Dest_X = npc.X;
                        npc.Dest_Y = npc.Y;
                        npc.Dest_Z = npc.Z;
                    }
                }
            }
            catch
            {
                //oops
            }
            finally
            {
                Globals.NPCLock.ExitReadLock();
            }
            */

            switch (Util.GetType(gamedata, dead_object))
            {
                case TargetType.PLAYER:
                    AddInfo.Remove_CharInfo(gamedata, dead_object);
                    //Globals.l2net_home.timer_players.Start();
                    break;
                case TargetType.NPC:
                    AddInfo.Remove_NPCInfo(gamedata, dead_object);
                    //Globals.l2net_home.timer_npcs.Start();
                    break;
            }

            //need to check if anything had this targeted and set it's Dest_ to the current location
        }

    }//end of class
}
