using System.Globalization;
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
            if (player.Name == "")
            {
                Console.WriteLine("---Player without Name, Ignore, ID: 0x{0:x}", player.ID);
            }
            else
            {
                {
                    string h = String.Format("[Player] {0}[0x{1:x}] ({2},{3},{4})", 
                        player.Name, player.ID, player.X, player.Y, player.Z);
                    //Console.WriteLine(h);
                    Globals.l2net_home.UpdateLog(h);
                }

                AddInfo.Add_CharInfo(gamedata, player);
                //Console.WriteLine("Num of near char : {0}", gamedata.nearby_chars.Count);
            }
        }

        public static void ExSetCompassZoneCode(GameData gamedata, ByteBuffer buff)
        {
            uint type = buff.ReadUInt32();//
            gamedata.cur_zone = type;
            //Console.WriteLine("Cur_Zone: {0}", gamedata.cur_zone);
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
            }

            //need to check if anything had this targeted and set it's Dest_ to the current location
        }

        public static void UserInfo(GameData gamedata, ByteBuffer buffe)
        {
            Console.WriteLine("[UserInfo]:" + BitConverter.ToString(buffe._data, 0).Replace("-", string.Empty).ToLower());
            int loc = buffe.GetIndex();
            uint ID = buffe.ReadUInt32();

            if ((gamedata.my_char.ID == 0) || (ID == gamedata.my_char.ID))
            {
                gamedata.my_char.ID = ID;
                gamedata.my_char.Load_User(buffe);

                {
                    string h = String.Format("[ME] {0}[0x{1:x}] ({2},{3},{4})",
                        gamedata.my_char.Name, gamedata.my_char.ID, gamedata.my_char.X, gamedata.my_char.Y, gamedata.my_char.Z);
                    //Console.WriteLine(h);
                    Globals.l2net_home.UpdateLog(h);
                }

                if (gamedata.teleported)
                {
                    gamedata.teleported = false;
                    GameServer.CleanUp(gamedata);
                }
            }
            else
            {
                Console.WriteLine("++++Warning: Update other than user");
            }
        }

        public static void StopMove(GameData gamedata, ByteBuffer buffe)
        {
            uint data1 = buffe.ReadUInt32();
            int dx = buffe.ReadInt32();
            int dy = buffe.ReadInt32();
            int dz = buffe.ReadInt32();

            TargetType type = Util.GetType(gamedata, data1);

            switch (type)
            {
                case TargetType.SELF:
                    Globals.MyselfLock.EnterWriteLock();
                    gamedata.my_char.Moving = false;
                    gamedata.my_char.MoveTarget = 0;
                    gamedata.my_char.MoveTargetType = TargetType.NONE;
                    gamedata.my_char.X = dx;
                    gamedata.my_char.Y = dy;
                    gamedata.my_char.Z = dz;
                    gamedata.my_char.Dest_X = dx;
                    gamedata.my_char.Dest_Y = dy;
                    gamedata.my_char.Dest_Z = dz;
                    {
                        string h = String.Format("[StopMove] {0}[0x{1:x}] ({2},{3},{4})", 
                                    gamedata.my_char.Name, gamedata.my_char.ID,
                                    gamedata.my_char.X, gamedata.my_char.Y, gamedata.my_char.Z);
                        //Console.WriteLine(h);
                        Globals.l2net_home.UpdateLog(h);
                    }
                    Globals.MyselfLock.ExitWriteLock();
                    break;
                case TargetType.PLAYER:
                    Globals.PlayerLock.EnterWriteLock();
                    try
                    {
                        CharInfo player = Util.GetChar(gamedata, data1);

                        if (player != null)
                        {
                            player.Moving = false;
                            player.MoveTarget = 0;
                            player.MoveTargetType = TargetType.NONE;
                            player.X = dx;
                            player.Y = dy;
                            player.Z = dz;
                            player.Dest_X = dx;
                            player.Dest_Y = dy;
                            player.Dest_Z = dz;
                            {
                                string h = String.Format("[StopMove] {0}[0x{1:x}] ({2},{3},{4})", 
                                        player.Name, player.ID,
                                        player.X, player.Y, player.Z);
                                //Console.WriteLine(h);
                                Globals.l2net_home.UpdateLog(h);
                            }
                        }
                    }
                    finally
                    {
                        Globals.PlayerLock.ExitWriteLock();
                    }
                    break;
            }
        }

        public static void MoveToLocation(GameData gamedata, ByteBuffer buffe)
        {
            //Console.WriteLine("[MoveToLocation]:" + BitConverter.ToString(buffe._data, 0).Replace("-", string.Empty).ToLower());
            uint data1 = buffe.ReadUInt32();
            int dx = buffe.ReadInt32();
            int dy = buffe.ReadInt32();
            int dz = buffe.ReadInt32();
            int ox = buffe.ReadInt32();
            int oy = buffe.ReadInt32();
            int oz = buffe.ReadInt32();

            TargetType type = Util.GetType(gamedata, data1);

            if (dx == ox && dy == oy && dz == oz)
            {
                switch (type)
                {
                    case TargetType.SELF:
                        Globals.MyselfLock.EnterWriteLock();
                        //Globals.gamedata.my_char.Clear_Botting_Buffing(false);
                        gamedata.my_char.X = ox;
                        gamedata.my_char.Y = oy;
                        gamedata.my_char.Z = oz;
                        gamedata.my_char.Dest_X = dx;
                        gamedata.my_char.Dest_Y = dy;
                        gamedata.my_char.Dest_Z = dz;
                        gamedata.my_char.Moving = false;
                        gamedata.my_char.MoveTarget = 0;
                        gamedata.my_char.MoveTargetType = 0;
                        gamedata.my_char.lastMoveTime = DateTime.Now;

                        // Console.WriteLine("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                        //         gamedata.my_char.Name, gamedata.my_char.ID,
                        //         gamedata.my_char.X, gamedata.my_char.Y, gamedata.my_char.Z,
                        //         gamedata.my_char.Dest_X, gamedata.my_char.Dest_Y, gamedata.my_char.Dest_Z);
                        Globals.MyselfLock.ExitWriteLock();
                        break;
                    case TargetType.PLAYER:
                        Globals.PlayerLock.EnterWriteLock();
                        try
                        {
                            CharInfo player = Util.GetChar(gamedata, data1);

                            if (player != null)
                            {
                                player.X = ox;
                                player.Y = oy;
                                player.Z = oz;
                                player.Dest_X = dx;
                                player.Dest_Y = dy;
                                player.Dest_Z = dz;
                                player.Moving = false;
                                player.MoveTarget = 0;
                                player.MoveTargetType = 0;
                                player.lastMoveTime = DateTime.Now;

                                //Console.WriteLine("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                                //player.Name, player.ID,
                                //player.X, player.Y, player.Z,
                                //player.Dest_X, player.Dest_Y, player.Dest_Z);
                            }
                        }//unlock
                        finally
                        {
                            Globals.PlayerLock.ExitWriteLock();
                        }
                        break;
                }
            }
            else
            {
                switch (type)
                {
                    case TargetType.SELF:
                        //Globals.gamedata.my_char.Clear_Botting_Buffing(false);
                        Globals.MyselfLock.EnterWriteLock();
                        gamedata.my_char.X = ox;
                        gamedata.my_char.Y = oy;
                        gamedata.my_char.Z = oz;
                        gamedata.my_char.Dest_X = dx;
                        gamedata.my_char.Dest_Y = dy;
                        gamedata.my_char.Dest_Z = dz;
                        gamedata.my_char.Moving = true;
                        gamedata.my_char.MoveTarget = 0;
                        gamedata.my_char.MoveTargetType = 0;
                        gamedata.my_char.lastMoveTime = DateTime.Now;
                        {
                            string h = String.Format("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                                gamedata.my_char.Name, gamedata.my_char.ID,
                                gamedata.my_char.X, gamedata.my_char.Y, gamedata.my_char.Z,
                                gamedata.my_char.Dest_X, gamedata.my_char.Dest_Y, gamedata.my_char.Dest_Z);
                            //Console.WriteLine(h);
                            Globals.l2net_home.UpdateLog(h);
                        }
                        Globals.MyselfLock.ExitWriteLock();
                        break;
                    case TargetType.PLAYER:
                        Globals.PlayerLock.EnterWriteLock();
                        try
                        {
                            CharInfo player = Util.GetChar(gamedata, data1);

                            if (player != null)
                            {
                                player.X = ox;
                                player.Y = oy;
                                player.Z = oz;
                                player.Dest_X = dx;
                                player.Dest_Y = dy;
                                player.Dest_Z = dz;
                                player.Moving = true;
                                player.MoveTarget = 0;
                                player.MoveTargetType = 0;
                                player.lastMoveTime = DateTime.Now;
                                {
                                    string h = String.Format("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                                    player.Name, player.ID,
                                    player.X, player.Y, player.Z,
                                    player.Dest_X, player.Dest_Y, player.Dest_Z);
                                    //Console.WriteLine(h);
                                    Globals.l2net_home.UpdateLog(h);
                                }                                
                            }
                        }//unlock
                        finally
                        {
                            Globals.PlayerLock.ExitWriteLock();
                        }
                        break;
                }
            }
        }


        public static void Teleport(GameData gamedata, ByteBuffer buff)
        {
            uint id = buff.ReadUInt32();
            int _x = buff.ReadInt32();
            int _y = buff.ReadInt32();
            int _z = buff.ReadInt32();

            TargetType type = Util.GetType(gamedata, id);

            switch (type)
            {
                case TargetType.SELF:
                    Globals.MyselfLock.EnterWriteLock();
                    gamedata.my_char.X = _x;
                    gamedata.my_char.Y = _y;
                    gamedata.my_char.Z = _z;
                    gamedata.my_char.Dest_X = _x;
                    gamedata.my_char.Dest_Y = _y;
                    gamedata.my_char.Dest_Z = _z;
                    gamedata.teleported = true;
                    {
                        string h = String.Format("[Teleport] {0}[0x{1:x}] -> ({2},{3},{4})", 
                                gamedata.my_char.Name, gamedata.my_char.ID,
                                gamedata.my_char.X, gamedata.my_char.Y, gamedata.my_char.Z);
                        //Console.WriteLine(h);
                        Globals.l2net_home.UpdateLog(h);
                    }

                    Globals.MyselfLock.ExitWriteLock();
                    break;
                case TargetType.PLAYER:
                    Globals.PlayerLock.EnterWriteLock();
                    try
                    {
                        CharInfo player = Util.GetChar(gamedata, id);

                        if (player != null)
                        {
                            player.X = _x;
                            player.Y = _y;
                            player.Z = _z;
                            player.Dest_X = _x;
                            player.Dest_Y = _y;
                            player.Dest_Z = _z;
                            {
                                string h = String.Format("[Teleport] {0}[0x{1:x}] -> ({2},{3},{4})", 
                                player.Name, player.ID,
                                player.X, player.Y, player.Z);
                                //Console.WriteLine(h);
                                Globals.l2net_home.UpdateLog(h);
                            }
                        }
                    }//unlock
                    finally
                    {
                        Globals.PlayerLock.ExitWriteLock();
                    }
                    break;
            }
        }

    }//end of class
}
