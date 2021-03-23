using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Threading;

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

    public static class Globals
    {
        //public static L2NET l2net_home;

        //1234567890123456
        public const string AES_Key = "#V^yw45?YLV$5wYa";
        public const string Code_Key = "$V%YWTaedcfwef0-";

        public const string Map_Key = "pK.+3!x8XzSW?@,B";
        public const string Map_Salt = "QaABcmPq$]@H+2u4NXxG";

        public const string Name = "L2.Net";
        public const string Version = "June 3, 2018";
        public const string VersionLetter = "https://github.com/devmvalvm/L2Net";
        public const int MinDataPack = 392;
        public static string PATH = "";
        public const string SOUND_NAMESPACE = "L2_login.Sounds.";
        public const int LOAD_BUFFER = 6600000;

        public const int TimeOut = int.MaxValue;

        public static Dictionary<int, GameData> Games = new Dictionary<int, GameData>();


        //public static System.Threading.Thread GG_sendthread;

        //public static Thread gamedrawthread;
        //public static Thread directinputthread;

        //public static SortedList Login_Servers;
        //public static uint Login_SelectedServer = 0;
    
        public static string SecurityPin = "";

        public static bool FulllyIn = false;
        public static bool OOG = false;
        public static bool show_active_skills = true;

        //anti-assist/skills when picking up
        public static bool picking_up_items = false;

        //public static DX_Keyboard Keyboard;

        public static StreamWriter text_out;

#if false
        public static Login login_window;
        public static Map map_window;
        public static BotOptionsScreen botoptionsscreen;
        public static Overlay overlaywindow;
        public static ShortCutBar shortcutwindow;
        public static Setup setupwindow;
        public static ScriptDebugger scriptdebugwindow;
        public static ActionWindow actionwindow;
        public static TradeWindow tradewindow;
        public static PrivateStoreSellWindow privatestoresellwindow;
        public static TargetInfoScreen targetinfoscreen;
        public static PetWindow petwindow;
        public static PetWindowGive petwindowgive;
        public static PetWindowTake petwindowtake;
        public static Captcha captchawindow;
        public static GameGuardServer ggwindow;
        public static GameGuardClient ggclientwindow;

        public static ExtendedActionWindow extendedactionwindow;
        public static MailboxWindow mailboxwindow;

#endif
        public static bool enterworld_check = false;

        //used to know if we are in the gameserver yet
        public static bool enterworld_sent = false;

        public static string enterworld_custom = "";
        //adifenix(obce)
        public static bool pre_unknow_blowfish = false;
        public static bool pre_proxy_serv = false;
        public static string pre_game_srv_listen_prt = "";

        public static bool unknow_blowfish = false;
        public static string game_srv_listen_prt = "";

        public static bool pre_enterworld_ip = false;
        public static string[] pre_enterworld_ip_tab = new string[20];
        public static bool enterworld_ip = false;
        public static bool proxy_serv = false; // work as proxy server -blowfishless method
        public static byte[] proxy_serv_ip = new byte[4]; // ip from proxy serv
        public static byte[] proxy_serv_port = new byte[2]; // port from proxy server
        public static ArrayList ew_con_array = new ArrayList();
        public static ArrayList ew_chc_ed_array = new ArrayList();
        // packet window stuff ...------------------------------------------------
        //public static pck_window_thr pck_thread = new pck_window_thr();
        //--------------------------------------------------------------------------
        public static bool Send_Blank_GG = false;
        public static bool Hide_Message_Boxes = false;

        public static string DirectInputKey = "-none-";
        public static bool DirectInputLast = false;
        public static bool DirectInputSetup = false;
        public static string DirectInputSetupValue = "";

        public static string DirectInputKey2 = "-none-";
        public static bool DirectInputLast2 = false;
        public static bool DirectInputSetup2 = false;
        public static string DirectInputSetupValue2 = "";

        public static bool DownloadNewCrests = false;
        public static bool SocialNpcs = false;
        public static bool NpcSay = false;
        public static bool SocialSelf = false;
        public static bool SocialPcs = false;
        public static bool ShowNamesNpcs = true;
        public static bool ShowNamesPcs = true;
        public static bool ShowNamesItems = true;
        public static bool IgnoreExitConf = false;
        public static bool ToggleBottingifGMAction = false;
        public static bool ToggleBottingifTeleported = false;

        public static string Script_MainFile = "";
        public static bool Script_Debugging = false;
        public const long Script_Ticks_Per_Switch = TimeSpan.TicksPerMillisecond * 1;
        public static string BotOptionsFile = "";

        public const int SLEEP_ClientSendThread = 75;
        public const int SLEEP_GameSendThread = 75;

        public const int SLEEP_Script_Reset = 500;
        public const int SLEEP_Script_Reset2 = 50;
        public const int SLEEP_WhileScript = 1;//sleep while paused... TODO: for infinite loops as well

        public const int SLEEP_ProcessDataThread = 10;//50
        public const long SLEEP_Animate = TimeSpan.TicksPerMillisecond * 120;
        public const long SLEEP_Sound_Alerts = TimeSpan.TicksPerMillisecond * 500;

        public const int SLEEP_BotAI = 100;
        //Oddi: Sleep_followrest..
        public const int SLEEP_FollowRestThread = 500;
        public const int SLEEP_DrawGameThread = 100;

        public const int SLEEP_WaitIGConnection = 1;
        public const int SLEEP_Load = 25;
        public const int SLEEP_LoginDelay = 250;

        public const int SLEEP_DirectInputDelay = 100;
        public const int SLEEP_SoundEngine = 100;

        public const int SLEEP_BotAIDelay = 500;
        public const int SLEEP_BotAIDelay_Target = 2000;
        public const int SLEEP_BotAIDelay_TargetInc = 25;
        public const int SLEEP_BotAIDelay_TargetSame = 250;
        //public const int SLEEP_BotAIDelay_Pickup = 1500;
        public const int SLEEP_BotAIDelay_PickupInc = 250;
        public const int PICKUP_Z_Diff = 50;

        public const int SLEEP_KillReset = 250;

        public const string SCRIPT_OUT_VAR = "@@@OUT";

        public const long MAX_SOUND_DELAY = TimeSpan.TicksPerSecond * 3;

        public static bool Use_Direct_Sound = true;
        public static bool Use_Hardware_Acceleration = true;
        public static int Texture_Mode = 1;
        public static int ViewRange = 0;
        public static bool White_Names = false;
        public static bool Suppress_Quakes = false;
        public static bool Popup_Captcha = false;
        public static bool AutolearnSkills = false;
        public static bool LogWriting = true;


        public static string Captcha_HTML1 = "captcha ";
        public static string Captcha_HTML2 = "";

        public const float Difficulty_Balance = 12;
        public const float Average_Word_Length = 5.10F;

        public static Random Rando = new Random(DateTime.Now.Millisecond);

        public const int THREAD_WAIT_DX = 75;//wait to read from players/npcs/items to draw on the map //don't want to wait too long... the gui thread will get locked
        public const int THREAD_WAIT_GUI = 200;//interaction with the gui //don't want to wait too long... the gui thread will get locked

        public const int MAX_LINES = 200;//max lines for the text box
        public const int MAX_LINES_PASS = 250;//max printed lines per pass
        public const int MAX_LINES_BYPASS = 500;//max printed lines per pass

        public const int MESSAGE_RESIZE_TIMER = 150;

        public const int MAX_MESSAGE_LEN = 128;

        public const int COUNT_SERVERNAME = 127;
        public const int COUNT_SYSTEMMSG = 3120;
        public const int COUNT_HENNAGROUP = 190;
        public const int COUNT_NPCNAME = 10090;
        public const int COUNT_ITEMNAME = 18100;
        public const int COUNT_CLASSES = 110;
        public const int COUNT_RACES = 6;
        public const int COUNT_SKILLS = 7460;
        public const int COUNT_ACTIONS = 190;
        public const int COUNT_QUESTS = 2690;
        public const int COUNT_ZONES = 500;
        public const int COUNT_NPCSTRING = 5460;

        public const float UNITS = 32768.0F;
        public const float ModX = UNITS * 20;
        public const float ModY = UNITS * 18;
        public const int ZRANGE_DIFF = 100;
        public const long SLEEP_TEXTURE = 1500 * TimeSpan.TicksPerMillisecond;
        public const long MAP_HOLD_STREAM = 5 * TimeSpan.TicksPerMinute;
        public const long MAP_HOLD_TEXTURE = 60 * TimeSpan.TicksPerMinute;

        public const int MIN_RADIUS = 6;
        public const float THRESHOLD = 5.0F;
        public const float THRESHOLD_L2NET = 50.0F;
        public const float INV_THOUSAND = 1.0F / 1000.0F;



        public const int BUFF_COUNT = 40;
        public const int ITEM_COUNT = 20;
        public const int COMBAT_COUNT = 10;

        public const int Skills_PerPage = 12;
        public const int Skills_Pages = 12;//normally 10

        public const uint Skill_SWEEP = 42;
        public const uint Skill_SPOIL = 254;
        public const uint Skill_SPOILCRUSH = 384;
        public const uint Skill_PLUNDER = 10548;

        public const uint ID_OFFSET = 1000;//multiply id by this and add quest prog to get the index in the array
        public const uint NPC_OFF = 1000000;

        public const int BUFFER_MAX = 131072;
        public const int BUFFER_PACKET = 65535;
        public const int BUFFER_NETWORK = 131072;

        public const int UDP_Port = 33801;

        public const string UnknownItem = "-unknown item-";
        public const string UnknownItemDesc = "-unknown item description-";
        public const string UnknownItemImagePath = "-no item image-";
        public const string UnknownNPC = "-unknown npc-";
        public const string UnknownTitle = "-unknown title-";
        public const string UnknownRace = "-unknown race-";
        public const string UnknownClass = "-unknown class-";
        public const string UnknownServer = "-unknown server-";

        public const long FAILED_BUFF = TimeSpan.TicksPerSecond * 4;
        public const double SKILL_MIN_REUSE = 100;
        public const double SKILL_INIT_REUSE = 2000;

        //real npc removal seems to be at 2048... or at least that is when we stop getting delete item packets for npcs
        public const int REMOVE_RANGE = 4000;
        public const int REMOVE_RANGE_INNER = 2048;
        //3584 loses shit...
        //3840 loses shit...
        //4096 is just barely too far
        public const long NPC_RemoveAtActive = TimeSpan.TicksPerDay * 60;//System.TimeSpan.TicksPerSecond * 300;//5 min
        public const long NPC_RemoveAtInvin = TimeSpan.TicksPerDay * 60;//2 months
        public const long NPC_RemoveAtDead = TimeSpan.TicksPerSecond * 15;//15 seconds
        public const long CLEAN_TIMER = TimeSpan.TicksPerSecond * 45;//45 seconds
        public const int CHAT_TIMER = 250;
        public const int PLAYERS_TIMER = 1500;
        public const int ITEMS_TIMER = 1500;
        public const int NPCS_TIMER = 1500;
        public const int INVENTORY_TIMER = 1000;
        public const int MYBUFFS_TIMER = 1500;

        public static string NumberGroupSeparator = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        public static char NumberDecimalSeparator = System.Globalization.CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0];

        public static System.Drawing.Brush Red = System.Drawing.Brushes.Red;
        public static System.Drawing.Brush Salmon = System.Drawing.Brushes.Salmon;
        public static System.Drawing.Brush White = System.Drawing.Brushes.White;
        public static System.Drawing.Brush Gray = System.Drawing.Brushes.Gray;
        public static System.Drawing.Brush Yellow = System.Drawing.Brushes.Yellow;
        public static System.Drawing.Brush Blue = System.Drawing.Brushes.Blue;
        public static System.Drawing.Brush Cyan = System.Drawing.Brushes.Cyan;
        public static Brush Pink = System.Drawing.Brushes.Pink;
        public static System.Drawing.Brush Orange = System.Drawing.Brushes.OrangeRed;
        public static System.Drawing.Brush Gold = System.Drawing.Brushes.Gold;
        public static System.Drawing.Brush Green = System.Drawing.Brushes.Lime;
        public static System.Drawing.Brush LightYellow = System.Drawing.Brushes.LightGoldenrodYellow;

        public static System.Drawing.SolidBrush Tell_Brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(253, 0, 253));
        public static System.Drawing.SolidBrush Party_Brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(0, 251, 0));
        public static System.Drawing.SolidBrush Clan_Brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(125, 117, 253));
        public static System.Drawing.SolidBrush Trade_Brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(234, 162, 245));
        public static System.Drawing.SolidBrush Ally_Brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(119, 251, 153));
        public static System.Drawing.SolidBrush Announcement_Brush = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(127, 249, 253));

        //option stuff
        private static string _ProductKey = "LOVELKQKMGBOGNET";
        public static bool LoadedInterface = false;

        public static int Voice = 0;
        public static bool MinimizeToTray = false;
        public static bool DumpModeClient = false;
        public static bool DumpModeServer = false;
        public static bool AllowFiles = true;
        public static bool ScriptCompatibilityv386 = false;
        public static string L2Path = "";
        public static System.Windows.Forms.MainMenu back_menu;
        //end of option stuff

        public static ArrayList Bannedkeys = new ArrayList();

        //public static GameData gamedata;

        public static SortedList servername;
        public static SortedList systemmsg;
        public static SortedList hennagrp;
        public static SortedList npcname;
        public static SortedList itemname;
        public static SortedList classes;
        public static SortedList races;
        public static SortedList skilllist;
        public static SortedList actionlist;
        public static SortedList questlist;
        public static SortedList zonelist;
        public static SortedList levelexp;
        public static SortedList npcstring;

        public static byte[] Login_GG_Reply;
        public static SortedList GG_List;

        public static ArrayList requested_clancrests = new ArrayList();
        public static ArrayList crestids = new ArrayList();
        public static SortedList clanlist = new SortedList();

        public static ResourceManager m_ResourceManager = new ResourceManager("L2_login.Languages.English", System.Reflection.Assembly.GetExecutingAssembly());
        public static int LanguageSet = 0;

        //public static uint LootType = 1;//1 - random
        public static uint ClanOnline = 0;
        public static uint ClanMembers = 0;
        public static uint LastRezz1 = 0;
        public static uint LastRezz2 = 0;

        public static bool CanPrint = false;
        public static bool Got_Skills = false;

        public static ReaderWriterLockSlim NPCLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim InventoryLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim PetInventoryLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim ItemLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim PlayerLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim BuyListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim MyBuffsListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim MyselfLock = new ReaderWriterLockSlim();

        public static ReaderWriterLockSlim PartyLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim BuffsGivenLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim ClanListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim SkillListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim BuffListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim ItemListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim CombatListLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim DoNotItemLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim DoNotNPCLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim RestBelowLock = new ReaderWriterLockSlim();

        public static ReaderWriterLockSlim MobListLock = new ReaderWriterLockSlim();

        public static readonly object ItemImagesLock = new object();
        public static readonly object SkillImagesLock = new object();

        public static ReaderWriterLockSlim GameSendQueueLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim ClientSendQueueLock = new ReaderWriterLockSlim();
        public static ReaderWriterLockSlim GameReadQueueLock = new ReaderWriterLockSlim();

        public static ReaderWriterLockSlim ChatLock = new ReaderWriterLockSlim();

        //only for debugging
#if false
        public static AstarNode debugNode;
        public static AstarNode debugNode2;
        public static AstarNode debugNode3;
#endif
        public static ArrayList debugPath;

        public static string ProductKey
        {
            get
            {
                return _ProductKey;
            }
            set
            {
                _ProductKey = value.Replace("-", "").ToUpperInvariant();
            }
        }
    }
}
