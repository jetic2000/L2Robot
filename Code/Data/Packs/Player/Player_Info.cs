using System;
using System.Collections;

namespace L2Robot
{
    /// <summary>
    /// Summary description for Char_Info.
    /// </summary>
    public class Player_Info : Object_Base
    {
        private string _Name = "";
        private string _Title = "";
        public volatile uint Sex = 0;
        public volatile uint Race = 0;
        public volatile uint Class = 0;
        public volatile uint BaseClass = 0;
        public volatile uint ActiveClass = 0;
        public volatile uint Active = 0;
        public volatile float X = 0;
        public volatile float Y = 0;
        public volatile float Z = 0;
        public volatile float Cur_HP = 0;
        public volatile float Cur_MP = 0;
        public volatile uint SP = 0;
        private ulong _XP = 0;
        public volatile float XPPercent = 0;

        public volatile uint Level = 0;
        public volatile uint INT = 0;
        public volatile uint STR = 0;
        public volatile uint CON = 0;
        public volatile uint MEN = 0;
        public volatile uint DEX = 0;
        public volatile uint WIT = 0;
        public volatile uint charID = 0;

        public volatile float Max_HP = 0;
        public volatile float Max_MP = 0;

        public volatile float Cur_CP = 0;
        public volatile float Max_CP = 0;

        public volatile float Dest_X = 0;
        public volatile float Dest_Y = 0;
        public volatile float Dest_Z = 0;
        public volatile bool Moving = false;
        private DateTime _lastMoveTime = DateTime.Now;
        public volatile uint MoveTarget = 0;
        public volatile TargetType MoveTargetType = TargetType.NONE;
        private DateTime _lastVerifyTime = DateTime.Now;

        public volatile int Heading = 0;

        public volatile uint Weapon_Equipped = 0;
        public volatile int Fame = 0;
        public volatile int Allow_Minimap = 0;
        public volatile int Vitality_Points = 0;
        public volatile uint ExtendedEffects = 0;

        public volatile uint Cur_Load = 0;
        public volatile uint Max_Load = 0;

        public volatile uint obj_Under = 0;
        public volatile uint obj_REar = 0;
        public volatile uint obj_LEar = 0;
        public volatile uint obj_Neck = 0;
        public volatile uint obj_RFinger = 0;
        public volatile uint obj_LFinger = 0;
        public volatile uint obj_Head = 0;
        public volatile uint obj_RHand = 0;
        public volatile uint obj_LHand = 0;
        public volatile uint obj_Gloves = 0;
        public volatile uint obj_Chest = 0;
        public volatile uint obj_Legs = 0;
        public volatile uint obj_Feet = 0;
        public volatile uint obj_Back = 0;
        public volatile uint obj_LRHand = 0;
        public volatile uint obj_Hair = 0;
        public volatile uint obj_Face = 0;

        public volatile uint itm_Under = 0;
        public volatile uint itm_REar = 0;
        public volatile uint itm_LEar = 0;
        public volatile uint itm_Neck = 0;
        public volatile uint itm_RFinger = 0;
        public volatile uint itm_LFinger = 0;
        public volatile uint itm_Head = 0;
        public volatile uint itm_RHand = 0;
        public volatile uint itm_LHand = 0;
        public volatile uint itm_Gloves = 0;
        public volatile uint itm_Chest = 0;
        public volatile uint itm_Legs = 0;
        public volatile uint itm_Feet = 0;
        public volatile uint itm_Back = 0;
        public volatile uint itm_LRHand = 0;
        public volatile uint itm_Hair = 0;
        public volatile uint itm_Face = 0;
        public volatile uint itm_rbracelet = 0;
        public volatile uint itm_lbracelet = 0;
        public volatile uint itm_talisman1 = 0;
        public volatile uint itm_talisman2 = 0;
        public volatile uint itm_talisman3 = 0;
        public volatile uint itm_talisman4 = 0;
        public volatile uint itm_talisman5 = 0;
        public volatile uint itm_talisman6 = 0;


        public volatile uint aug_Under = 0;
        public volatile uint aug_REar = 0;
        public volatile uint aug_LEar = 0;
        public volatile uint aug_Neck = 0;
        public volatile uint aug_RFinger = 0;
        public volatile uint aug_LFinger = 0;
        public volatile uint aug_Head = 0;
        public volatile uint aug_RHand = 0;
        public volatile uint aug_LHand = 0;
        public volatile uint aug_Gloves = 0;
        public volatile uint aug_Chest = 0;
        public volatile uint aug_Legs = 0;
        public volatile uint aug_Feet = 0;
        public volatile uint aug_Back = 0;
        public volatile uint aug_LRHand = 0;
        public volatile uint aug_Hair = 0;
        public volatile uint aug_Face = 0;
        public volatile uint aug_rbracelet = 0;
        public volatile uint aug_lbracelet = 0;
        public volatile uint aug_talisman1 = 0;
        public volatile uint aug_talisman2 = 0;
        public volatile uint aug_talisman3 = 0;
        public volatile uint aug_talisman4 = 0;
        public volatile uint aug_talisman5 = 0;
        public volatile uint aug_talisman6 = 0;

        public volatile uint Patk = 0;
        public volatile uint PatkSpeed = 0;
        public volatile uint PDef = 0;
        public volatile uint Evasion = 0;
        public volatile uint Accuracy = 0;
        public volatile uint Focus = 0;
        public volatile uint Matk = 0;
        public volatile uint MatkSpeed = 0;
        public volatile uint MDef = 0;

        public volatile uint PvPFlag = 0;
        public volatile int Karma = 0;

        public volatile float RunSpeed = 0;
        public volatile float WalkSpeed = 0;
        public volatile uint SwimRunSpeed = 0;
        public volatile uint SwimWalkSpeed = 0;
        public volatile uint flRunSpeed = 0;
        public volatile uint flWalkSpeed = 0;
        public volatile uint FlyRunSpeed = 0;
        public volatile uint FlyWalkSpeed = 0;

        public volatile float MoveSpeedMult = 0;
        public volatile float AttackSpeedMult = 0;
        public volatile float CollisionRadius = 0;
        public volatile float CollisionHeight = 0;

        public volatile uint HairSytle = 0;
        public volatile uint HairColor = 0;
        public volatile uint Face = 0;
        public volatile uint AccessLevel = 0;

        public volatile uint ClanID = 0;
        public volatile uint ClanCrestID = 0;
        public volatile uint AllyID = 0;
        public volatile uint AllyCrestID = 0;
        public volatile uint isClanLeader = 0;
        public volatile uint MountType = 0;//byte
        public volatile uint PrivateStoreType = 0;//byte
        public volatile uint hasDwarfCraft = 0;//byte
        public volatile uint PKCount = 0;
        public volatile uint PvPCount = 0;

        public volatile uint CubicCount = 0;//ushort
        private ArrayList _Cubics = new ArrayList();

        private ArrayList _AbnEffects = new ArrayList();

        public volatile uint FindParty = 0;//byte

        public volatile uint AbnormalEffects = 0;
        private ulong _ClanPrivileges = 0;
        public volatile uint isRunning = 1;
        public volatile uint isSitting = 1;
        public volatile uint isAlikeDead = 0;

        public volatile uint RecLeft = 0;//ushort
        public volatile uint RecAmount = 0;//0 = white | 255 = blue//ushort
        public volatile uint InventoryLimit = 0;//ushort
        public volatile uint SpecialEffects = 0;
        public volatile uint EnchantAmount = 0;//byte

        public volatile uint TeamCircle = 0;//1= Blue, 2 = red//byte

        public volatile uint ClanCrestIDLarge = 0;

        public volatile uint HeroIcon = 0;//byte
        public volatile uint HeroGlow = 0;//byte

        public volatile uint isFishing = 0;//0x01 - fishing//byte
        public volatile int FishX = 0;
        public volatile int FishY = 0;
        public volatile int FishZ = 0;

        public volatile uint NameColor = 0;
        public volatile uint TitleColor = 0;
        public volatile uint PledgeClass = 0;
        public volatile uint DemonSword = 0;

        public volatile int Transform_ID = 0;
        public volatile int Agathon_ID = 0;

        public volatile uint Symbol1 = 0;
        public volatile uint Symbol2 = 0;
        public volatile uint Symbol3 = 0;
        public volatile uint MaxTats = 0;

        public volatile uint TargetID = 0;
        public volatile uint TargetColor = 0;//ushort
        public volatile TargetType CurrentTargetType = TargetType.NONE;
        public volatile bool TargetSpoiled = false;
        public volatile bool CannotSeeTarget = false;

        public volatile uint LastTarget = 0;
        public volatile uint BuffTarget = 0;
        public volatile uint BuffTargetLast = 0;
        public volatile uint BuffNeedTarget = 0;
        public volatile uint BuffSkillID = 0;
        private DateTime _lastbufftime;

        public volatile uint isInCombat = 0;//byte
        public volatile bool isAttacking = false;

        public volatile uint Charges = 0;
        public volatile uint Souls = 0;
        public volatile uint DeathPenalty = 0;

        public volatile uint HitTime = 0;
        public double ExpireFactor = 1.0;
        public long ExpiresTime = 0;
        public volatile uint Resisted = 0;

        public volatile int AtkAttrib = 0;
        public volatile int AtkAttribVal = 0;

        public volatile int DefAttribFire = 0;
        public volatile int DefAttribWater = 0;
        public volatile int DefAttribWind = 0;
        public volatile int DefAttribEarth = 0;
        public volatile int DefAttribHoly = 0;
        public volatile int DefAttribUnholy = 0;
        public volatile uint MAccuracy = 0;
        public volatile uint MEvasion = 0;
        public volatile uint MCritical = 0;

        //need to keep track of:
        //sit/stand
        //in party
        //party leader
        //loot type

        private readonly object lastbufftimeLock = new object();
        private readonly object lastVerifyTimeLock = new object();
        private readonly object lastMoveTimeLock = new object();
        private readonly object ClanPrivilegesLock = new object();
        private readonly object XPLock = new object();
        private readonly object CubicsLock = new object();
        private readonly object AbnEffectsLock = new object();
        private readonly object TitleLock = new object();
        private readonly object NameLock = new object();

        public bool HasEffect(AbnormalEffects test)
        {
            return AbnEffects.IndexOf((uint)test) != -1;
        }

        public bool HasExtendedEffect(ExtendedEffects test)
        {
            return (ExtendedEffects & (uint)test) != 0;
        }

        public bool MyCharRelation(MyRelation test)
        {
            return (isClanLeader & (uint)test) != 0;
        }

        public string Name
        {
            get
            {
                string tmp;
                lock (NameLock)
                {
                    tmp = this._Name;
                }
                return tmp;
            }
            set
            {
                lock (NameLock)
                {
                    _Name = value;
                }
                //Globals.l2net_home.SetName();
            }
        }
        public string Title
        {
            get
            {
                string tmp;
                lock (TitleLock)
                {
                    tmp = this._Title;
                }
                return tmp;
            }
            set
            {
                lock (TitleLock)
                {
                    _Title = value;
                }
            }
        }
        public ArrayList Cubics
        {
            get
            {
                ArrayList tmp;
                lock (CubicsLock)
                {
                    tmp = this._Cubics;
                }
                return tmp;
            }
            set
            {
                lock (CubicsLock)
                {
                    _Cubics = value;
                }
            }
        }

        public ArrayList AbnEffects
        {
            get
            {
                ArrayList tmp;
                lock (AbnEffectsLock)
                {
                    tmp = this._AbnEffects;
                }
                return tmp;
            }
            set
            {
                lock (AbnEffectsLock)
                {
                    _AbnEffects = value;
                }
            }
        }

        public ulong XP
        {
            get
            {
                ulong tmp;
                lock (XPLock)
                {
                    tmp = this._XP;
                }
                return tmp;
            }
            set
            {
                lock (XPLock)
                {
                    _XP = value;
                }
            }
        }

        public ulong ClanPrivileges
        {
            get
            {
                ulong tmp;
                lock (ClanPrivilegesLock)
                {
                    tmp = this._ClanPrivileges;
                }
                return tmp;
            }
            set
            {
                lock (ClanPrivilegesLock)
                {
                    _ClanPrivileges = value;
                }
            }
        }
        public DateTime LastBuffTime
        {
            get
            {
                DateTime tmp;
                lock (lastbufftimeLock)
                {
                    tmp = this._lastbufftime;
                }
                return tmp;
            }
            set
            {
                lock (lastbufftimeLock)
                {
                    _lastbufftime = value;
                }
            }
        }
        public DateTime lastMoveTime
        {
            get
            {
                DateTime tmp;
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
        public DateTime lastVerifyTime
        {
            get
            {
                DateTime tmp;
                lock (lastVerifyTimeLock)
                {
                    tmp = this._lastVerifyTime;
                }
                return tmp;
            }
            set
            {
                lock (lastVerifyTimeLock)
                {
                    _lastVerifyTime = value;
                }
            }
        }

        public bool CanBuff()
        {
            if (BuffNeedTarget == 0)
            {
                return true;
            }
            if (BuffTarget == TargetID)
            {
                return true;
            }
            return false;
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
    }//end of class
}
