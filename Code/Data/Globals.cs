using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

namespace L2Robot
{

    public enum TargetType : byte
    {
        //0 - none | 1 - player | 2 - other players | 3 - npc | 4 - item
        ERROR = 255,
        NONE = 0,
        SELF = 1,
        PLAYER = 2,
        NPC = 3,
        ITEM = 4,
        MYPET = 5,
        MYPET1 = 6,
        MYPET2 = 7,
        MYPET3 = 8
    }

    public struct Coordinate
    {
        public float X;
        public float Y;
        public float Z;
    }


    public enum TextType : byte
    {
        LOCAL = 0,
        TRADE = 1,
        PARTY = 2,
        CLAN = 4,
        ALLY = 5,
        SYSTEM = 6,
        BOT = 7,
        ALL = 8,
        HERO = 9
    }

    /// <summary>
    /// 扩展BindingList，防止增加、删除项时自动更新界面而不出现“跨线程操作界面控件”异常
    /// </summary>
    public class UiBindList<T> : BindingList<T>
    {
        /// <summary>
        /// 界面同步上下文
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }
 
        /// <summary>
        /// 使用此方法执行一切操作上下文相关的操作
        /// </summary>
        private void Excute(Action action, object state = null)
        {
            if (SynchronizationContext == null)
                action();
            else
                SynchronizationContext.Post(d => action(), state);
 
        }
 
        public new void Add(T item)
        {
            Excute(() => base.Add(item));
        }
 
        public new void Remove(T item)
        {
            Excute(() => base.Remove(item));
        }
    }

    public static class Globals
    {
        public static Thread IGListener;
        public static GameData waitingGameData;
        public static bool running;
        public static FormMain l2net_home;

        public const string Name = "L2.Net";
        public const string Version = "June 3, 2018";
        public static string PATH = "";

        public const int TimeOut = int.MaxValue;

        public static Dictionary<int, GameData> Games = new Dictionary<int, GameData>();

        public static byte[] proxy_serv_ip = new byte[4]; // ip from proxy serv
        public static byte[] proxy_serv_port = new byte[2]; // port from proxy server

        public const int SLEEP_ClientSendThread = 75;
        public const int SLEEP_GameSendThread = 75;
        public const int SLEEP_FishFLyThread = 1000;


        public const int SLEEP_ProcessDataThread = 10;//50

        //Oddi: Sleep_followrest..
        

        public const int BUFFER_MAX = 131072;
        public const int BUFFER_PACKET = 65535;
        public const int BUFFER_NETWORK = 131072;


        //real npc removal seems to be at 2048... or at least that is when we stop getting delete item packets for npcs
        public const int REMOVE_RANGE = 30000;
        public const long CLEAN_TIMER = TimeSpan.TicksPerSecond * 3;//45 seconds

        public static string NumberGroupSeparator = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        public static char NumberDecimalSeparator = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0];


        public static ReaderWriterLockSlim InstancesLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim PlayerLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim MyselfLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim GameSendQueueLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim ClientSendQueueLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim GameReadQueueLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim LogLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim FishFlyPlayerQueueLock = new ReaderWriterLockSlim();

        public class Games_Instances
        {
            public string name
            {
                get
                {
                    return string.Format("{0} ({1} port {2})", this.process_name, this.protocol, this.port_number);
                }
                set { }
            }
            public string port_number { get; set; }
            public string process_name { get; set; }
            public string protocol { get; set; }
        }
    }
}
