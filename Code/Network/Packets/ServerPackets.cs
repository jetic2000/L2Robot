using System.Globalization;
using System;
using System.Threading;
using System.Text;
using System.Collections;
using System.Collections.Generic;

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
            Globals.PlayerLock.EnterWriteLock();
            player.Load(buffe);
            if (player.Name == "")
            {
                Console.WriteLine("---Player without Name, Ignore, ID: 0x{0:x}", player.ID);
            }
            else
            {
                {
                    PlayerMsg msg = new PlayerMsg();
                    msg.PlayerName = player.Name;
                    msg.PlayerID = player.ID;
                    msg.msg = String.Format("[Player] {0}[0x{1:x}] ({2},{3},{4})", 
                        player.Name, player.ID, player.Current_Pos.X, player.Current_Pos.Y, player.Current_Pos.Z);
                    Globals.l2net_home.UpdateLog(msg);
                }

                AddInfo.Add_CharInfo(gamedata, player);
                //Console.WriteLine("Num of near char : {0}", gamedata.nearby_chars.Count);
            }
            Globals.PlayerLock.ExitWriteLock();
        }

        public static void ExSetCompassZoneCode(GameData gamedata, ByteBuffer buff)
        {
            uint type = buff.ReadUInt32();//
            gamedata.cur_zone = type;
            Console.WriteLine("---ExSetCompassZoneCode: 0x{0:x}", gamedata.cur_zone);
        }


        public static void NetPing(GameData gamedata, ByteBuffer buffe)
        {

        }

        public static void DeleteItem(GameData gamedata, ByteBuffer buffe)
        {
            uint dead_object = buffe.ReadUInt32();//System.BitConverter.ToInt32(buffe,1);
            switch (Util.GetType(gamedata, dead_object))
            {
                case TargetType.PLAYER:
                    Globals.PlayerLock.EnterWriteLock();
                    AddInfo.Remove_CharInfo(gamedata, dead_object);
                    Globals.PlayerLock.ExitWriteLock();
                    //Globals.l2net_home.timer_players.Start();
                    break;
            }

            //need to check if anything had this targeted and set it's Dest_ to the current location
        }

        public static void MoveToPawn(GameData gamedata, ByteBuffer buffe)
        {
            uint data1 = buffe.ReadUInt32();

            buffe.SetIndex(25);
            float dx = buffe.ReadInt32();
            float dy = buffe.ReadInt32();
            float dz = buffe.ReadInt32();
            Coordinate c = new Coordinate();
            TargetType type = Util.GetType(gamedata, data1);

            //byte[] buff;

            //buff = buffe.Get_ByteArray();
            //Console.WriteLine("[MoveToPawn]:" + BitConverter.ToString(buff, 0).Replace("-", string.Empty).ToLower());

            switch (type)
            {
                case TargetType.SELF:
                    Globals.MyselfLock.EnterWriteLock();
                    gamedata.my_char.Moving = false;
                    gamedata.my_char.MoveTarget = 0;
                    gamedata.my_char.MoveTargetType = TargetType.NONE;
                    c.X = dx;
                    c.Y = dy;
                    c.Z = dz;
                    gamedata.my_char.Current_Pos = c;
                    {
                        PlayerMsg msg = new PlayerMsg();
                        msg.PlayerName = gamedata.my_char.Name;
                        msg.PlayerID = gamedata.my_char.ID;
                        msg.msg = String.Format("[MoveToPawn] {0}[0x{1:x}] ({2},{3},{4})", 
                                    gamedata.my_char.Name, gamedata.my_char.ID,
                                    gamedata.my_char.Current_Pos.X, gamedata.my_char.Current_Pos.Y, gamedata.my_char.Current_Pos.Z);
                        //Console.WriteLine(h);
                        Globals.l2net_home.UpdateLog(msg);
                    }
                    {
                        PlayerInstance me = new PlayerInstance();
                        me.PlayerID = gamedata.my_char.ID;
                        me.PlayerName = gamedata.my_char.Name;
                        me.X = gamedata.my_char.Current_Pos.X;
                        me.Y = gamedata.my_char.Current_Pos.Y;
                        me.Z = gamedata.my_char.Current_Pos.Z;
                        Console.WriteLine("[ME]MoveToPawn");
                        Globals.l2net_home.UpdateInstanceList(me);
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
                            c.X = dx;
                            c.Y = dy;
                            c.Z = dz;
                            player.Current_Pos = c;
                            {
                                PlayerMsg msg = new PlayerMsg();
                                msg.PlayerName = gamedata.my_char.Name;
                                msg.PlayerID = gamedata.my_char.ID;
                                msg.msg = String.Format("[MoveToPawn] {0}[0x{1:x}] ({2},{3},{4})", 
                                        player.Name, player.ID,
                                        player.Current_Pos.X, player.Current_Pos.Y, player.Current_Pos.Z);
                                //Console.WriteLine(h);
                                Globals.l2net_home.UpdateLog(msg);
                            }
                            AddInfo.Add_CharInfo(gamedata, player);
                        }
                    }
                    finally
                    {
                        Globals.PlayerLock.ExitWriteLock();
                    }
                    break;
            }
        }

        public static void UserInfo(GameData gamedata, ByteBuffer buffe)
        {
            //Call Only once for char login
            Console.WriteLine("[UserInfo]:" + BitConverter.ToString(buffe._data, 0).Replace("-", string.Empty).ToLower());
            int loc = buffe.GetIndex();
            uint ID = buffe.ReadUInt32();

            Globals.MyselfLock.EnterWriteLock();
            if ((gamedata.my_char.ID == 0) || (ID == gamedata.my_char.ID))
            {
                gamedata.my_char.ID = ID;
                gamedata.my_char.Load_User(buffe);
            }
            else
            {
                Console.WriteLine("++++Warning: Update other than user");
            }
            Globals.MyselfLock.ExitWriteLock();
        }

        public static void MagicSkillUser(GameData gamedata, ByteBuffer buffe)
        {
            
        }

        public static void StopMove(GameData gamedata, ByteBuffer buffe)
        {
            uint data1 = buffe.ReadUInt32();
            float dx = buffe.ReadInt32();
            float dy = buffe.ReadInt32();
            float dz = buffe.ReadInt32();
            Coordinate c = new Coordinate();

            TargetType type = Util.GetType(gamedata, data1);

            switch (type)
            {
                case TargetType.SELF:
                    Globals.MyselfLock.EnterWriteLock();
                    gamedata.my_char.Moving = false;
                    gamedata.my_char.MoveTarget = 0;
                    gamedata.my_char.MoveTargetType = TargetType.NONE;
                    c.X = dx;
                    c.Y = dy;
                    c.Z = dz;
                    gamedata.my_char.Current_Pos = c;

                    c.X = dx;
                    c.Y = dy;
                    c.Z = dz;
                    gamedata.my_char.Dest_Pos = c;

                    {
                        PlayerMsg msg = new PlayerMsg();
                        msg.PlayerName = gamedata.my_char.Name;
                        msg.PlayerID = gamedata.my_char.ID;
                        msg.msg = String.Format("[StopMove] {0}[0x{1:x}] ({2},{3},{4})", 
                                    gamedata.my_char.Name, gamedata.my_char.ID,
                                    gamedata.my_char.Current_Pos.X, gamedata.my_char.Current_Pos.Y, gamedata.my_char.Current_Pos.Z);
                        //Console.WriteLine(h);
                        Globals.l2net_home.UpdateLog(msg);
                    }
                    {
                        PlayerInstance me = new PlayerInstance();
                        me.PlayerID = gamedata.my_char.ID;
                        me.PlayerName = gamedata.my_char.Name;
                        me.X = gamedata.my_char.Current_Pos.X;
                        me.Y = gamedata.my_char.Current_Pos.Y;
                        me.Z = gamedata.my_char.Current_Pos.Z;
                        Console.WriteLine("[ME]Update StopMove");
                        Globals.l2net_home.UpdateInstanceList(me);
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

                            c.X = dx;
                            c.Y = dy;
                            c.Z = dz;
                            player.Current_Pos = c;

                            c.X = dx;
                            c.Y = dy;
                            c.Z = dz;
                            player.Dest_Pos = c;

                            //player.X = dx;
                            //player.Y = dy;
                            //player.Z = dz;
                            //player.Dest_X = dx;
                            //player.Dest_Y = dy;
                            //player.Dest_Z = dz;
                            {
                                PlayerMsg msg = new PlayerMsg();
                                msg.PlayerName = gamedata.my_char.Name;
                                msg.PlayerID = gamedata.my_char.ID;
                                msg.msg = String.Format("[StopMove] {0}[0x{1:x}] ({2},{3},{4})", 
                                        player.Name, player.ID,
                                        player.Current_Pos.X, player.Current_Pos.Y, player.Current_Pos.Z);
                                //Console.WriteLine(h);
                                Globals.l2net_home.UpdateLog(msg);
                            }
                            AddInfo.Add_CharInfo(gamedata, player);
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
            Coordinate c = new Coordinate();

            TargetType type = Util.GetType(gamedata, data1);

            if (dx == ox && dy == oy && dz == oz)
            {
                switch (type)
                {
                    case TargetType.SELF:
                        Globals.MyselfLock.EnterWriteLock();
                        //Globals.gamedata.my_char.Clear_Botting_Buffing(false);
                        c.X = ox;
                        c.Y = oy;
                        c.Z = oz;
                        gamedata.my_char.Current_Pos = c;

                        c.X = dx;
                        c.Y = dy;
                        c.Z = dz;
                        gamedata.my_char.Dest_Pos = c;

                        //gamedata.my_char.X = ox;
                        //gamedata.my_char.Y = oy;
                        //gamedata.my_char.Z = oz;
                        //gamedata.my_char.Dest_X = dx;
                        //gamedata.my_char.Dest_Y = dy;
                        //gamedata.my_char.Dest_Z = dz;
                        gamedata.my_char.Moving = false;
                        gamedata.my_char.MoveTarget = 0;
                        gamedata.my_char.MoveTargetType = 0;
                        gamedata.my_char.lastMoveTime = DateTime.Now;

                        {
                            PlayerInstance me = new PlayerInstance();
                            me.PlayerID = gamedata.my_char.ID;
                            me.PlayerName = gamedata.my_char.Name;
                            me.X = gamedata.my_char.Current_Pos.X;
                            me.Y = gamedata.my_char.Current_Pos.Y;
                            me.Z = gamedata.my_char.Current_Pos.Z;
                            Console.WriteLine("[ME]Update MoveToLocation1");
                            Globals.l2net_home.UpdateInstanceList(me);
                        }

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
                                c.X = ox;
                                c.Y = oy;
                                c.Z = oz;
                                player.Current_Pos = c;

                                c.X = dx;
                                c.Y = dy;
                                c.Z = dz;
                                player.Dest_Pos = c;
                                //player.X = ox;
                                //player.Y = oy;
                                //player.Z = oz;
                                //player.Dest_X = dx;
                                //player.Dest_Y = dy;
                                //player.Dest_Z = dz;
                                player.Moving = false;
                                player.MoveTarget = 0;
                                player.MoveTargetType = 0;
                                player.lastMoveTime = DateTime.Now;
                                //Console.WriteLine("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                                //player.Name, player.ID,
                                //player.X, player.Y, player.Z,
                                //player.Dest_X, player.Dest_Y, player.Dest_Z);
                                AddInfo.Add_CharInfo(gamedata, player);
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
                        c.X = ox;
                        c.Y = oy;
                        c.Z = oz;
                        gamedata.my_char.Current_Pos = c;

                        c.X = dx;
                        c.Y = dy;
                        c.Z = dz;
                        gamedata.my_char.Dest_Pos = c;
                        //gamedata.my_char.X = ox;
                        //gamedata.my_char.Y = oy;
                        //gamedata.my_char.Z = oz;
                        //gamedata.my_char.Dest_X = dx;
                        //gamedata.my_char.Dest_Y = dy;
                        //gamedata.my_char.Dest_Z = dz;
                        gamedata.my_char.Moving = true;
                        gamedata.my_char.MoveTarget = 0;
                        gamedata.my_char.MoveTargetType = 0;
                        gamedata.my_char.lastMoveTime = DateTime.Now;
                        {
                            PlayerMsg msg = new PlayerMsg();
                            msg.PlayerName = gamedata.my_char.Name;
                            msg.PlayerID = gamedata.my_char.ID;
                            msg.msg = String.Format("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                                gamedata.my_char.Name, gamedata.my_char.ID,
                                gamedata.my_char.Current_Pos.X, gamedata.my_char.Current_Pos.Y, gamedata.my_char.Current_Pos.Z,
                                gamedata.my_char.Dest_Pos.X, gamedata.my_char.Dest_Pos.Y, gamedata.my_char.Dest_Pos.Z);
                            //Console.WriteLine(h);
                            Globals.l2net_home.UpdateLog(msg);
                        }
                        
                        {
                            PlayerInstance me = new PlayerInstance();
                            me.PlayerID = gamedata.my_char.ID;
                            me.PlayerName = gamedata.my_char.Name;
                            me.X = gamedata.my_char.Current_Pos.X;
                            me.Y = gamedata.my_char.Current_Pos.Y;
                            me.Z = gamedata.my_char.Current_Pos.Z;
                            Globals.l2net_home.UpdateInstanceList(me);
                            Console.WriteLine("[ME]Update MoveToLocation2");
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
                                c.X = ox;
                                c.Y = oy;
                                c.Z = oz;
                                player.Current_Pos = c;

                                c.X = dx;
                                c.Y = dy;
                                c.Z = dz;
                                player.Dest_Pos = c;
                                //player.X = ox;
                                //player.Y = oy;
                                //player.Z = oz;
                                //player.Dest_X = dx;
                                //player.Dest_Y = dy;
                                //player.Dest_Z = dz;
                                player.Moving = true;
                                player.MoveTarget = 0;
                                player.MoveTargetType = 0;
                                player.lastMoveTime = DateTime.Now;
                                {
                                    PlayerMsg msg = new PlayerMsg();
                                    msg.PlayerName = gamedata.my_char.Name;
                                    msg.PlayerID = gamedata.my_char.ID;
                                    msg.msg = String.Format("[MoveToLocation] {0}[0x{1:x}] ({2},{3},{4}) -> ({5},{6},{7})", 
                                        player.Name, player.ID,
                                        player.Current_Pos.X, player.Current_Pos.Y, player.Current_Pos.Z,
                                        player.Dest_Pos.X, player.Dest_Pos.Y, player.Dest_Pos.Z);
                                    //Console.WriteLine(h);
                                    Globals.l2net_home.UpdateLog(msg);
                                }
                                AddInfo.Add_CharInfo(gamedata, player);                                
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
            Coordinate c = new Coordinate();

            TargetType type = Util.GetType(gamedata, id);

            switch (type)
            {
                case TargetType.SELF:
                    Globals.MyselfLock.EnterWriteLock();
                    c.X = _x;
                    c.Y = _y;
                    c.Z = _z;
                    gamedata.my_char.Current_Pos = c;

                    c.X = _x;
                    c.Y = _y;
                    c.Z = _z;
                    gamedata.my_char.Dest_Pos = c;
                    //gamedata.my_char.X = _x;
                    //gamedata.my_char.Y = _y;
                    //gamedata.my_char.Z = _z;
                    //gamedata.my_char.Dest_X = _x;
                    //gamedata.my_char.Dest_Y = _y;
                    //gamedata.my_char.Dest_Z = _z;
                    gamedata.teleported = true;
                    {
                        PlayerMsg msg = new PlayerMsg();
                        msg.PlayerName = gamedata.my_char.Name;
                        msg.PlayerID = gamedata.my_char.ID;
                        msg.msg = String.Format("[Teleport] {0}[0x{1:x}] -> ({2},{3},{4})", 
                                gamedata.my_char.Name, gamedata.my_char.ID,
                                gamedata.my_char.Current_Pos.X, gamedata.my_char.Current_Pos.Y, gamedata.my_char.Current_Pos.Z);
                        //Console.WriteLine(h);
                        Globals.l2net_home.UpdateLog(msg);
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
                            c.X = _x;
                            c.Y = _y;
                            c.Z = _z;
                            player.Current_Pos = c;

                            c.X = _x;
                            c.Y = _y;
                            c.Z = _z;
                            player.Dest_Pos = c;
                            //player.X = _x;
                            //player.Y = _y;
                            //player.Z = _z;
                            //player.Dest_X = _x;
                            //player.Dest_Y = _y;
                            //player.Dest_Z = _z;
                            {
                                PlayerMsg msg = new PlayerMsg();
                                msg.PlayerName = gamedata.my_char.Name;
                                msg.PlayerID = gamedata.my_char.ID;
                                msg.msg = String.Format("[Teleport] {0}[0x{1:x}] -> ({2},{3},{4})", 
                                player.Name, player.ID,
                                player.Current_Pos.X, player.Current_Pos.Y, player.Current_Pos.Z);
                                //Console.WriteLine(h);
                                Globals.l2net_home.UpdateLog(msg);
                            }
                            AddInfo.Add_CharInfo(gamedata, player);
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
