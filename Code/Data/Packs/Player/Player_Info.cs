using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace L2Robot
{
    /// <summary>
    /// Summary description for Char_Info.
    /// </summary>
    public class Player_Info : Object_Base
    {
        private string _Name = "";
        private Coordinate _Current_Pos;
        private Coordinate _Dest_Pos;
        //public volatile float X = 0;
        //public volatile float Y = 0;
        //public volatile float Z = 0;
        //public volatile float Dest_X = 0;
        //public volatile float Dest_Y = 0;
        //public volatile float Dest_Z = 0;
        public volatile bool Moving = false;
        private DateTime _lastMoveTime = DateTime.Now;
        public volatile uint MoveTarget = 0;
        public volatile TargetType MoveTargetType = TargetType.NONE;
        private DateTime _lastVerifyTime = DateTime.Now;
        private ArrayList _AbnEffects = new ArrayList();
        public volatile bool TargetSpoiled = false;
        public volatile uint BuffTarget = 0;
        public volatile uint BuffTargetLast = 0;
        //private readonly object lastVerifyTimeLock = new object();
        //private readonly object lastMoveTimeLock = new object();
        private readonly object PlayerInfoLock = new object();

        public string Name
        {
            get
            {
                string tmp;
                lock (PlayerInfoLock)
                {
                    tmp = this._Name;
                }
                return tmp;
            }
            set
            {
                lock (PlayerInfoLock)
                {
                    _Name = value;
                }
                //Globals.l2net_home.SetName();
            }
        }

        public Coordinate Current_Pos
        {
            get
            {
                Coordinate tmp;
                lock (PlayerInfoLock)
                {
                    tmp.X = _Current_Pos.X;
                    tmp.Y = _Current_Pos.Y;
                    tmp.Z = _Current_Pos.Z;
                }
                return tmp;
            }
            set
            {
                lock (PlayerInfoLock)
                {
                    _Current_Pos.X = value.X;
                    _Current_Pos.Y = value.Y;
                    _Current_Pos.Z = value.Z;
                }
            }
        }

        public Coordinate Dest_Pos
        {
            get
            {
                Coordinate tmp;
                lock (PlayerInfoLock)
                {
                    tmp.X = _Dest_Pos.X;
                    tmp.Y = _Dest_Pos.Y;
                    tmp.Z = _Dest_Pos.Z;
                }
                return tmp;
            }
            set
            {
                lock (PlayerInfoLock)
                {
                    _Dest_Pos.X = value.X;
                    _Dest_Pos.Y = value.Y;
                    _Dest_Pos.Z = value.Z;
                }
            }
        }

        public DateTime lastMoveTime
        {
            get
            {
                DateTime tmp;
                lock (PlayerInfoLock)
                {
                    tmp = this._lastMoveTime;
                }
                return tmp;
            }
            set
            {
                lock (PlayerInfoLock)
                {
                    _lastMoveTime = value;
                }
            }
        }
        public DateTime lastVerifyTime
        {
            get
            {
                DateTime tmp;
                lock (PlayerInfoLock)
                {
                    tmp = this._lastVerifyTime;
                }
                return tmp;
            }
            set
            {
                lock (PlayerInfoLock)
                {
                    _lastVerifyTime = value;
                }
            }
        }

        public Player_Info()
        {
            Moving = false;
            TargetSpoiled = false;
            lastVerifyTime = DateTime.Now;
            //Globals.gamedata.BOT_STATE = BotState.Nothing;
            BuffTarget = 0;
            BuffTargetLast = 0;
            //Clear_Skills();
            //Clear_MyBuffs();
            //Clear_Party();
        }

        
        public void Load_User(ByteBuffer buff)
        {
            int length = 0;
            int loc = buff.GetIndex();
            byte Mask1 = 0x0;
            byte Mask2 = 0x0;
            byte Mask3 = 0x0;
            byte Mask4 = 0x0;
            Int16 sub_length = 0;
            UInt16 sub_cmd = 0x0;
            int content_index = 0;

            length = buff.ReadInt32(); //Total content length
            //Console.WriteLine("UserInfo Load length: {0}",length);
            loc += 4;
            sub_cmd = buff.ReadUInt16();
            loc += 2;
            if (sub_cmd != 0x1c)
            {
                Console.WriteLine("Error for UserInfo");
                return;
            }
            Mask1 = buff.ReadByte();
            Mask2 = buff.ReadByte();
            Mask3 = buff.ReadByte();
            Mask4 = buff.ReadByte();
            //Console.WriteLine("UserInfo Load mask: 0x{0:x}{1:x}{2:x}{3:x}", Mask1, Mask2, Mask3, Mask4);
            loc += 4;

            if ((Mask1 & 0x80) != 0)
            {
                buff.SetIndex(loc);
                buff.ReadUInt32(); //Fix Length
                loc += 4;
            }

            //Handle Player Name and Etc
            if ((Mask1 & 0x40) != 0)
            {
                //Globals.MyselfLock.EnterWriteLock();
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                int namesize = buff.ReadInt16();
                List<byte> unicodeChars = new List<byte>();
                for (int j = 0; j < namesize; j++)
                {
                    unicodeChars.Add(buff.ReadByte());
                    unicodeChars.Add(buff.ReadByte());
                }
                byte[] PlayerName = unicodeChars.ToArray();
                Name = Encoding.Unicode.GetString(PlayerName);
                Console.WriteLine("User Name: {0}", Name);
                //Globals.MyselfLock.ExitWriteLock();

                loc += sub_length;
            }

            if ((Mask1 & 0x20) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask1 & 0x10) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            
            if ((Mask1 & 0x08) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask1 & 0x04) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask1 & 0x02) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask1 & 0x01) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask2 & 0x80) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask2 & 0x40) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            //Handle User Position
            if ((Mask2 & 0x20) != 0)
            {
                //Globals.MyselfLock.EnterWriteLock();
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                Coordinate c = new Coordinate();
                c.X = buff.ReadInt32();
                c.Y = buff.ReadInt32();
                c.Z = buff.ReadInt32();
                Current_Pos = c;

                Console.WriteLine("UserInfo：{0}[0x{1:x}] -> ({2},{3},{4})", Name, ID, Current_Pos.X, Current_Pos.Y, Current_Pos.Z);
                PlayerInstance player = new PlayerInstance();

                player.PlayerID = ID;
                player.PlayerName = Name;
                player.X = Current_Pos.X;
                player.Y = Current_Pos.Y;
                player.Z = Current_Pos.Z;
                Globals.l2net_home.UpdateInstanceList(player);
                //Globals.MyselfLock.ExitWriteLock();

                loc += sub_length;
            }

            if ((Mask2 & 0x10) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask2 & 0x08) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            
            if ((Mask2 & 0x04) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            
            if ((Mask2 & 0x02) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            
            if ((Mask2 & 0x01) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x80) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x40) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x20) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x10) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x08) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x04) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x02) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask3 & 0x01) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x80) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x40) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x20) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x10) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x08) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x04) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x02) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            if ((Mask4 & 0x01) != 0)
            {
                content_index += 1;
                buff.SetIndex(loc);
                sub_length = buff.ReadInt16();
                loc += sub_length;
            }

            //Console.WriteLine("UserInfo Load content_index: {0}, loc: {1}", content_index, loc);    

        }
    }//end of class
}
