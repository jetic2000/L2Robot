using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace L2Robot
{
    public class CharInfo : Object_Base
    {
        //public float _X = 0;
        //public float _Y = 0;
        //public float _Z = 0;
        private string _Name = "";

        private Coordinate _Current_Pos;
        private Coordinate _Dest_Pos;
        //public float _Dest_X = 0;
        //public float _Dest_Y = 0;
        //public float _Dest_Z = 0;
        public volatile bool Moving = false;
        private System.DateTime _lastMoveTime = System.DateTime.Now;
        public volatile uint MoveTarget = 0;
        public volatile TargetType MoveTargetType = TargetType.NONE;

        private uint _TargetID = 0;
        public volatile TargetType CurrentTargetType = TargetType.NONE;

        public volatile float Cur_HP = 0;
        public volatile float Max_HP = 0;
        public volatile float Cur_MP = 0;
        public volatile float Max_MP = 0;
        public volatile float Cur_CP = 0;
        public volatile float Max_CP = 0;

        private readonly object lastMoveTimeLock = new object();
        private readonly object CharInfoLock = new object();

        public string Name
        {
            get
            {
                string tmp;
                lock (CharInfoLock)
                {
                    tmp = this._Name;
                }
                return tmp;
            }
            set
            {
                lock (CharInfoLock)
                {
                    _Name = value;
                }
            }
        }

        public uint TargetID
        {
            get
            {
                uint tmp;
                lock (CharInfoLock)
                {
                    tmp = this._TargetID;
                }
                return tmp;
            }
            set
            {
                lock (CharInfoLock)
                {
                    _TargetID = value;
                }
            }
        }

        public Coordinate Current_Pos
        {
            get
            {
                Coordinate tmp;
                lock (CharInfoLock)
                {
                    tmp.X = _Current_Pos.X;
                    tmp.Y = _Current_Pos.Y;
                    tmp.Z = _Current_Pos.Z;
                }
                return tmp;
            }
            set
            {
                lock (CharInfoLock)
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
                lock (CharInfoLock)
                {
                    tmp.X = _Dest_Pos.X;
                    tmp.Y = _Dest_Pos.Y;
                    tmp.Z = _Dest_Pos.Z;
                }
                return tmp;
            }
            set
            {
                lock (CharInfoLock)
                {
                    _Dest_Pos.X = value.X;
                    _Dest_Pos.Y = value.Y;
                    _Dest_Pos.Z = value.Z;
                }
            }
        }

        public System.DateTime lastMoveTime
        {
            get
            {
                System.DateTime tmp;
                lock (lastMoveTimeLock)
                {
                    tmp = this._lastMoveTime;
                }
                return tmp;
            }
            set
            {
                lock (lastMoveTimeLock)
                {
                    _lastMoveTime = value;
                }
            }
        }
 
        public CharInfo()
        {
            Moving = false;
        }

        public void Load(ByteBuffer buff)
        {
            int loc = buff.GetIndex();
            int size = buff.ReadInt16(); //size
            //Console.WriteLine("Loc: {0}, Load: {1}", loc, size);
            ID = buff.ReadUInt32();//A0 B9 B0 49
            buff.SetIndex(loc + size);
            buff.ReadInt32();
            Coordinate c = new Coordinate();
            c.X = buff.ReadInt32();
            c.Y = buff.ReadInt32();
            c.Z = buff.ReadInt32();
            Current_Pos = c;

            buff.ReadInt32();
            int namesize = buff.ReadInt16();
            List<byte> unicodeChars = new List<byte>();
            for (int j = 0; j < namesize; j++)
            {
                unicodeChars.Add(buff.ReadByte());
                unicodeChars.Add(buff.ReadByte());
            }
            byte[] PlayerName = unicodeChars.ToArray();
            Name = Encoding.Unicode.GetString(PlayerName);
            //Console.WriteLine("Char {0}[0x{1:x}] -> ({2},{3},{4})", Name, ID, X, Y, Z);
        }

        public void CopyNew(CharInfo ch_inf)
        {
            Current_Pos = ch_inf.Current_Pos;
        }
    }//end of player info
}
