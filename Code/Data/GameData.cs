using System;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;

namespace L2Robot
{
    public enum DeviceState : int
    {
        Lost = 0,
        Ready = 1,
        Broke = 2,
        Fixed = 3,
    }

    public struct Vertex
    {
        //public SlimDX.Vector4 Position;
        public int Color;
        public float Tu;
        public float Tv;
    }

    public struct FishFlyPlayer
    {
        public string PlayerName
        {
            get {return _PlayerName;}
            set { _PlayerName = value; }
        }
        public uint OwenID
        {
            get { return _OwenID; }
            set { _OwenID = value; }
        }

        public uint PlayerID
        {
            get { return _PlayerID; }
            set { _PlayerID = value; }
        }

        public bool toInit
        {
            get { return _toInit; }
            set { _toInit = value; }
        }
        public uint RemoveID
        {
            get { return _RemoveID; }
            set { _RemoveID = value; }
        }
        private string _PlayerName;
        private uint _PlayerID;
        private uint _OwenID;
        private uint _RemoveID;
        private bool _toInit;
    }

    public struct PlayerInstance
    {
        public string PlayerName
        {
            get {return _PlayerName;}
            set { _PlayerName = value; }
        }

        public uint PlayerID
        {
            get { return _PlayerID; }
            set { _PlayerID = value; }
        }

        public uint LocalPort
        {
            get { return _LocalPort; }
            set { _LocalPort = value; }
        }

        public int ProcessPid
        {
            get { return _ProcessPid; }
            set { _ProcessPid = value; }
        }

        public float X
        {
            get { return _X; }
            set { _X = value; }
        }

        public float Y
        {
            get { return _Y; }
            set { _Y = value; }
        }

        public float Z
        {
            get { return _Z; }
            set { _Z = value; }
        }

        public bool toInit
        {
            get { return _toInit; }
            set { _toInit = value; }
        }

        private string _PlayerName;
        private uint _PlayerID;
        private uint _LocalPort;
        private int _ProcessPid;
        private float _X;
        private float _Y;
        private float _Z;
        private bool _toInit;

    }

    public class PlayerMsg
    {
        public string PlayerName;
        public uint PlayerID;
        public string msg;
        public bool isForRel;

        public PlayerMsg()
        {
            isForRel = false;
        }
    }

    public class Server
    {
        public uint ID = 0;
        public string IP = "0.0.0.0";
        public int Port = 0;
    }

    public enum Chronicle : byte
    {
        Prelude = 0,
        Chronicle_1 = 1,
        Chronicle_2 = 2,
        Chronicle_3 = 3,
        Chronicle_4 = 4,
        Chronicle_5 = 5,
        Interlude = 6,
        CT1 = 7,
        CT1_5 = 8,
        CT2_1 = 9,
        CT2_2 = 10,
        CT2_3 = 11,
        CT2_4 = 12,
        CT2_5 = 13,
        CT2_6 = 14,
        CT3_0 = 15,
        CT3_1 = 16,
        CT3_2 = 17,
        CT4_0 = 18
    }

    public enum AbnormalEffects : uint
    {
        //BLEEDING = 0x000001,
        //POISON = 0x000002,
        //REDCIRCLE = 0x000004,
        //ICE = 0x000008,
        //WIND = 0x000010,
        //FEAR = 0x000020,
        //STUN = 0x000040,
        //SLEEP = 0x000080,
        //MUTED = 0x000100,
        //ROOT = 0x000200,
        //HOLD_1 = 0x000400,  // paralysis
        //HOLD_2 = 0x000800,  // Petrified
        //UNKNOWN_13 = 0x001000,
        //BIG_HEAD = 0x002000,
        //FLAME = 0x004000,
        //UNKNOWN_16 = 0x008000,
        //GROW = 0x010000,
        //FLOATING_ROOT = 0x020000,  // Frintezza Stun
        //DANCE_STUNNED = 0x040000,  // Frintezza Stun
        //FIREROOT_STUN = 0x080000,
        //STEALTH = 0x100000,
        //IMPRISIONING_1 = 0x200000,
        //IMPRISIONING_2 = 0x400000,
        //MAGIC_CIRCLE = 0x800000,  // SoE
        //ICE2 = 0x1000000,
        //EARTHQUAKE = 0x2000000,
        //UNKNOWN_27 = 0x4000000,
        //INVULNERABLE = 0x8000000,
        //VITALITY = 0x10000000,
        //REAL_TARGET = 0x20000000,
        //DEATH_MARK = 0x40000000,
        //SKULL_FEAR = 0x80000000,
        //ARCANE_SHIELD = 0x008000,
        //CONFUSED = 0x0020,


        /*These are wrong */
        ICE = 0x98,
        WIND = 0x97,
        MUTED = 0x96,
        HOLD_1 = 0x95,
        INVULNERABLE = 0x94,
        FLOATING_ROOT = 0x93,
        DANCE_STUNNED = 0x92,
        FIREROOT_STUN = 0x91,
        SKULL_FEAR = 0x90,
        STEALTH = 0x8F,
        REDCIRCLE = 0x8E,
        FLAME = 0x8D,
        IMPRISIONING_1 = 0x8C,
        IMPRISIONING_2 = 0x8B,
        MAGIC_CIRCLE = 0x8A,
        ICE2 = 0x89,
        EARTHQUAKE = 0x88,
        VITALITY = 0x87,
        REAL_TARGET = 0x86,
        DEATH_MARK = 0x85,
        CONFUSED = 0x84,
        /*End bad section */

        TOGGLE_BUFF = 0x00,
        BLEEDING = 0x01,
        POISON = 0x02,
        STUN = 0x07,
        SLEEP = 0x08,
        SILENCE = 0x09,
        ROOT = 0x0A,
        PETRIFIED = 0x0C,
        VITALITY_HERB = 0x1D,
        FEAR = 0x20,
        XP_BUFF = 0x28, //mentees appreciation etc
        POLYMORPHED = 0x32,
        HERB_OF_POWER = 0x45,
        HERB_OF_MAGIC = 0x46,
    }

    public enum ExtendedEffects : uint
    {
        INVINCIBLE = 0x000001,
        AIR_STUN = 0x000002,
        AIR_ROOT = 0x000004,
        BAGUETTE_SWORD = 0x000008,
        YELLOW_AFFRO = 0x000010,
        PINK_AFFRO = 0x000020,
        BLACK_AFFRO = 0x000040,
        UNKNOWN8 = 0x000080,
        STIGMA_SHILIEN = 0x000100,
        STAKATOROOT = 0x000200,
        FREEZING = 0x000400,
        VESPER = 0x000800,
    }

    public enum MyRelation : uint
    {
        LEADER_RIGHTS = 0x40,
        DEFENDER = 0x80,
        ATTACKER = 0x180,
        CROWN = 0xC0,
        FLAG = 0x1C0,
    }

    public enum RelationStates : uint
    {
        PARTY1 = 0x00001, // party member
        PARTY2 = 0x00002, // party member
        PARTY3 = 0x00004, // party member
        PARTY4 = 0x00008, // party member
        PARTYLEADER = 0x00010, // true if is party leader
        HAS_PARTY = 0x00020, // true if is in party
        CLAN_MEMBER = 0x00040, // true if is in clan
        LEADER = 0x00080, // true if is clan leader
        CLAN_MATE = 0x00100, // true if is in same clan
        INSIEGE = 0x00200, // true if in siege
        ATTACKER = 0x00400, // true when attacker
        ALLY = 0x00800, // blue siege icon, cannot have if red
        ENEMY = 0x01000, // true when red icon, doesn't matter with blue
        MUTUAL_WAR = 0x04000, // double fist
        ONESIDED_WAR = 0x08000, // single fist
        ALLY_MEMBER = 0x10000, // clan is in alliance
        TERRITORY_WAR = 0x80000, // show Territory War icon		
    }

    //Hold all data for one instance
    public class GameData
    {
        private int _Process_Pid;
        private Socket _Game_GameSocket;
        private TcpListener _Game_ClientLink;
        private Socket _Game_ClientSocket;
        private MixedPackets _Mixer = null;
        private ServerThread _gamethread;
        private ClientThread _clientthread;
        private FishFlyThread _fishflythread;
        private GameServer _gameprocessdatathread;
        private Queue _gamesendqueue = new Queue();
        private Queue _gamereadqueue = new Queue();
        private Queue _clientsendqueue = new Queue();
        private Queue _fishflyplayerqueue = new Queue();
        private Player_Info _my_char = new Player_Info();
        private BlackListPlayers _blacklist_chars = new BlackListPlayers();
        private SortedList _nearby_chars = new SortedList();
        private byte[] _game_key = new byte[16];
        private byte[] _gameguardInit = new byte[16];
        private Crypt _crypt_out;// = new Crypt();
        private Crypt _crypt_in;// = new Crypt();
        private Crypt _crypt_clientout;// = new Crypt();
        private Crypt _crypt_clientin;// = new Crypt();

        //public volatile bool OOG = true;
        public volatile bool teleported = false;
        //public volatile BotState BOT_STATE = BotState.Nothing;
        //public volatile bool autoreply = false;
        //public volatile bool autoreplyPM = false;

        public volatile int Server_ID = 0;
        public volatile int Obfuscation_Key = 0;

        //public volatile uint yesno_state = 0;

        public volatile int Client_Port;

        public volatile string Game_IP;
        public volatile int Game_Port;
        public volatile int Game_Port_Ext;

        public volatile string IG_Local_IP = "127.0.0.1";
        public volatile string GG_IP = "127.0.0.1";
        public volatile string Override_Game_IP;
        public volatile int Override_Game_Port;
        public volatile int Override_Game_Port_Ext;

        public volatile Chronicle Chron;
        public volatile bool logged_in = false;
        public volatile bool running = false;
        //public volatile uint cur_zone = 0;
        private readonly object MixerLock = new object();
        private readonly object processPidLock = new object();
        private readonly object gamethreadLock = new object();
        private readonly object clientthreadLock = new object();
        private readonly object fishflythreadLock = new object();
        private readonly object gameprocessdatathreadLock = new object();
        //private readonly object InListLock = new object();
        private readonly object Game_ClientLinkLock = new object();
        private readonly object Game_GameSocketLock = new object();
        private readonly object Game_ClientSocketLock = new object();

        private readonly object game_keyLock = new object();
        private readonly object crypt_outLock = new object();
        private readonly object crypt_inLock = new object();
        private readonly object crypt_clientoutLock = new object();
        private readonly object crypt_clientinLock = new object();
        private readonly object gamesendqueueLock = new object();
        private readonly object gamereadqueueLock = new object();
        private readonly object clientsendqueueLock = new object();
        private readonly object nearby_charsLock = new object();
        private readonly object fishflyplayerqueueLock = new object();
        private readonly object mycharLock = new object();
        private readonly object blcharsLock = new object();

        public GameData()
        {
            CreateCrypt();
           //ig_listener = new Thread(new ThreadStart(LoginServer.IG_Listener));
        }

        public void ReLogin()
        {
            //Stop Fish Check
            //Clear UI
            FishFlyPlayer fp = new FishFlyPlayer();
            fp.toInit = true; //Speical OwenID = 0 to notifiy init
            fp.RemoveID = this.my_char.ID;
            Globals.l2net_home.UpdateDataGridView(fp);

            PlayerInstance p = new PlayerInstance();
            p.PlayerName = this.my_char.Name;
            p.toInit = true;
            Globals.l2net_home.UpdateInstanceList(p);

            //this.fishflythread.bRunning = false;
            this.fishflythread.bGetData = false;

            my_char = new Player_Info();
            nearby_chars = new SortedList();
            blacklist_chars = new BlackListPlayers();
            Globals.l2net_home.ClearBlackList();
        }

        public void CreateCrypt()
        {
            crypt_in = new Crypt();
            crypt_out = new Crypt();
            crypt_clientin = new Crypt();
            crypt_clientout = new Crypt();
        }

        public void SendToGameServerNF(ByteBuffer buff)
        {
            Globals.GameSendQueueLock.EnterWriteLock();
            try
            {
                gamesendqueue.Enqueue(buff);
            }
            finally
            {
                Globals.GameSendQueueLock.ExitWriteLock();
            }


            if (gamethread.sendthread.ThreadState == ThreadState.WaitSleepJoin)
            {
                try
                {
                    gamethread.sendthread.Interrupt();
                }
                catch (ThreadInterruptedException)
                {
                    //everything worked perfect
                }
                catch
                {
                    //Globals.l2net_home.Add_Error("SendToGameServerNF error");
                }
            }
        }

        public void SendToClient(ByteBuffer buff)
        {
            Globals.ClientSendQueueLock.EnterWriteLock();
            try
            {
                clientsendqueue.Enqueue(buff);
            }
            finally
            {
                Globals.ClientSendQueueLock.ExitWriteLock();
            }

            if (clientthread.sendthread.ThreadState == ThreadState.WaitSleepJoin)
            {
                try
                {
                    clientthread.sendthread.Interrupt();
                }
                catch (ThreadInterruptedException)
                {
                    //everything worked perfect
                }
                catch
                {
                    //Globals.l2net_home.Add_Error("SendToClient error");
                }
            }
        }

        public void SendToBotRead(ByteBuffer buff)
        {
            //Console.WriteLine("+SendToBotRead");
            Globals.GameReadQueueLock.EnterWriteLock();
            try
            {
                gamereadqueue.Enqueue(buff);
            }
            finally
            {
                Globals.GameReadQueueLock.ExitWriteLock();
            }

            if (gameprocessdatathread.processthread.ThreadState == ThreadState.WaitSleepJoin)
            {
                try
                {
                    gameprocessdatathread.processthread.Interrupt();
                }
                catch (ThreadInterruptedException)
                {
                    //everything worked perfect
                }
                catch
                {
                    //Globals.l2net_home.Add_Error("SendToBotRead error");
                }
            }
        }

        public int GetCount_DataToServer()
        {
            Globals.GameSendQueueLock.EnterWriteLock();
            try
            {
                return gamesendqueue.Count;
            }
            finally
            {
                Globals.GameSendQueueLock.ExitWriteLock();
            }
        }

        public int GetCount_DataToClient()
        {
            Globals.ClientSendQueueLock.EnterWriteLock();
            try
            {
                return clientsendqueue.Count;
            }
            finally
            {
                Globals.ClientSendQueueLock.ExitWriteLock();
            }
        }

        public int GetCount_DataToBot()
        {
            Globals.GameReadQueueLock.EnterWriteLock();
            try
            {
                return gamereadqueue.Count;
            }
            finally
            {
                Globals.GameReadQueueLock.ExitWriteLock();
            }
        }
        public int GetCount_DataToFlyFish()
        {
            Globals.FishFlyPlayerQueueLock.EnterWriteLock();
            try
            {
                return fishflyplayerqueue.Count;
            }
            finally
            {
                Globals.FishFlyPlayerQueueLock.ExitWriteLock();
            }
        }

        public Socket Game_GameSocket
        {
            get
            {
                Socket tmp;
                lock (Game_GameSocketLock)
                {
                    tmp = this._Game_GameSocket;
                }
                return tmp;
            }
            set
            {
                lock (Game_GameSocketLock)
                {
                    _Game_GameSocket = value;
                }
            }
        }

        public TcpListener Game_ClientLink
        {
            get
            {
                TcpListener tmp;
                lock (Game_ClientLinkLock)
                {
                    tmp = this._Game_ClientLink;
                }
                return tmp;
            }
            set
            {
                lock (Game_ClientLinkLock)
                {
                    _Game_ClientLink = value;
                }
            }
        }

        public Socket Game_ClientSocket
        {
            get
            {
                Socket tmp;
                lock (Game_ClientSocketLock)
                {
                    tmp = this._Game_ClientSocket;
                }
                return tmp;
            }
            set
            {
                lock (Game_ClientSocketLock)
                {
                    _Game_ClientSocket = value;
                }
            }
        }

        public MixedPackets Mixer
        {
            get
            {
                MixedPackets tmp;
                lock (MixerLock)
                {
                    tmp = this._Mixer;
                }
                return tmp;
            }
            set
            {
                lock (MixerLock)
                {
                    _Mixer = value;
                }
            }
        }

        public ServerThread gamethread
        {
            get
            {
                ServerThread tmp;
                lock (gamethreadLock)
                {
                    tmp = this._gamethread;
                }
                return tmp;
            }
            set
            {
                lock (gamethreadLock)
                {
                    _gamethread = value;
                }
            }
        }

        public ClientThread clientthread
        {
            get
            {
                ClientThread tmp;
                lock (clientthreadLock)
                {
                    tmp = this._clientthread;
                }
                return tmp;
            }
            set
            {
                lock (clientthreadLock)
                {
                    _clientthread = value;
                }
            }
        }
        public FishFlyThread fishflythread
        {
            get
            {
                FishFlyThread tmp;
                lock (fishflythreadLock)
                {
                    tmp = this._fishflythread;
                }
                return tmp;
            }
            set
            {
                lock (fishflythreadLock)
                {
                    _fishflythread = value;
                }
            }
        }

        public int processPid
        {
            get
            {
                int tmp;
                lock (processPidLock)
                {
                    tmp = this._Process_Pid;
                }
                return tmp;
            }
            set
            {
                lock (processPidLock)
                {
                    _Process_Pid = value;
                }
            }
        }

        public GameServer gameprocessdatathread
        {
            get
            {
                GameServer tmp;
                lock (gameprocessdatathreadLock)
                {
                    tmp = this._gameprocessdatathread;
                }
                return tmp;
            }
            set
            {
                lock (gameprocessdatathreadLock)
                {
                    _gameprocessdatathread = value;
                }
            }
        }

        public SortedList nearby_chars
        {
            get
            {
                SortedList tmp;
                lock (nearby_charsLock)
                {
                    tmp = this._nearby_chars;
                }
                return tmp;
            }
            set
            {
                lock (nearby_charsLock)
                {
                    _nearby_chars = value;
                }
            }
        }

        public Queue fishflyplayerqueue
        {
            get
            {
                Queue tmp;
                lock (fishflyplayerqueueLock)
                {
                    tmp = this._fishflyplayerqueue;
                }
                return tmp;
            }
            set
            {
                lock (fishflyplayerqueueLock)
                {
                    _fishflyplayerqueue = value;
                }
            }
        }


        public Queue clientsendqueue
        {
            get
            {
                Queue tmp;
                lock (clientsendqueueLock)
                {
                    tmp = this._clientsendqueue;
                }
                return tmp;
            }
            set
            {
                lock (clientsendqueueLock)
                {
                    _clientsendqueue = value;
                }
            }
        }
        public Queue gamereadqueue
        {
            get
            {
                Queue tmp;
                lock (gamereadqueueLock)
                {
                    tmp = this._gamereadqueue;
                }
                return tmp;
            }
            set
            {
                lock (gamereadqueueLock)
                {
                    _gamereadqueue = value;
                }
            }
        }
        public Queue gamesendqueue
        {
            get
            {
                Queue tmp;
                lock (gamesendqueueLock)
                {
                    tmp = this._gamesendqueue;
                }
                return tmp;
            }
            set
            {
                lock (gamesendqueueLock)
                {
                    _gamesendqueue = value;
                }
            }
        }

        public Crypt crypt_clientin
        {
            get
            {
                Crypt tmp;
                lock (crypt_clientinLock)
                {
                    tmp = this._crypt_clientin;
                }
                return tmp;
            }
            set
            {
                lock (crypt_clientinLock)
                {
                    _crypt_clientin = value;
                }
            }
        }
        public Crypt crypt_clientout
        {
            get
            {
                Crypt tmp;
                lock (crypt_clientoutLock)
                {
                    tmp = this._crypt_clientout;
                }
                return tmp;
            }
            set
            {
                lock (crypt_clientoutLock)
                {
                    _crypt_clientout = value;
                }
            }
        }
        public Crypt crypt_in
        {
            get
            {
                Crypt tmp;
                lock (crypt_inLock)
                {
                    tmp = this._crypt_in;
                }
                return tmp;
            }
            set
            {
                lock (crypt_inLock)
                {
                    _crypt_in = value;
                }
            }
        }
        public Crypt crypt_out
        {
            get
            {
                Crypt tmp;
                lock (crypt_outLock)
                {
                    tmp = this._crypt_out;
                }
                return tmp;
            }
            set
            {
                lock (crypt_outLock)
                {
                    _crypt_out = value;
                }
            }
        }

        public byte[] game_key
        {
            get
            {
                byte[] tmp;
                lock (game_keyLock)
                {
                    tmp = this._game_key;
                }
                return tmp;
            }
            set
            {
                lock (game_keyLock)
                {
                    _game_key = value;
                }
            }
        }

        public Player_Info my_char
        {
            get
            {
                Player_Info tmp;
                lock (mycharLock)
                {
                    tmp = _my_char;
                }
                return tmp;
            }
            set
            {
                lock (mycharLock)
                {
                    _my_char = value;
                }
            }
        }

        public BlackListPlayers blacklist_chars
        {
            get
            {
                BlackListPlayers tmp;
                lock (blcharsLock)
                {
                    tmp = _blacklist_chars;
                }
                return tmp;
            }
            set
            {
                lock (blcharsLock)
                {
                    _blacklist_chars = value;
                }
            }
        }

    }//end of class
}
