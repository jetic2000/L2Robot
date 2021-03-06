using System;

namespace L2Robot
{
    public partial class ScriptEngine
    {
        private ScriptVariable Get_Value(string oname)
        {
            string name = oname.ToUpperInvariant();

            ScriptVariable scr_var = new ScriptVariable();

            switch (name)
            {
                case "VOID":
                    scr_var.Name = "VOID";
                    scr_var.Type = Var_Types.NULL;
                    scr_var.Value = 0L;
                    break;
                case "ZERO":
                    scr_var.Name = "ZERO";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "ONE":
                    scr_var.Name = "ONE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "TWO":
                    scr_var.Name = "TWO";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "FALSE":
                    scr_var.Name = "FALSE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "TRUE":
                    scr_var.Name = "TRUE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "TOWN":
                    scr_var.Name = "TOWN";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "CLANHALL":
                    scr_var.Name = "CLANHALL";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "CASTLE":
                    scr_var.Name = "CASTLE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "SIEGEHQ":
                    scr_var.Name = "SIEGEHQ";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "FORTRESS":
                    scr_var.Name = "FORTRESS";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "WALKING":
                    scr_var.Name = "WALKING";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "RUNNING":
                    scr_var.Name = "RUNNING";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "SITTING":
                    scr_var.Name = "SITTING";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "STANDING":
                    scr_var.Name = "STANDING";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "START_FAKEDEATH":
                    scr_var.Name = "START_FAKEDEATH";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "STOP_FAKEDEATH":
                    scr_var.Name = "STOP_FAKEDEATH";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "ALIVE":
                    scr_var.Name = "ALIVE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "DEAD":
                    scr_var.Name = "DEAD";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "CHANNEL_ALL":
                    scr_var.Name = "CHANNEL_ALL";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "CHANNEL_SHOUT":
                    scr_var.Name = "CHANNEL_SHOUT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "CHANNEL_PRIVATE":
                    scr_var.Name = "CHANNEL_PRIVATE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "CHANNEL_PARTY":
                    scr_var.Name = "CHANNEL_PARTY";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "CHANNEL_CLAN":
                    scr_var.Name = "CHANNEL_CLAN";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 4L;
                    break;
                case "CHANNEL_GM":
                    scr_var.Name = "CHANNEL_GM";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 5L;
                    break;
                case "CHANNEL_PETITION":
                    scr_var.Name = "CHANNEL_PETITION";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 6L;
                    break;
                case "CHANNEL_PETITIONREPLY":
                    scr_var.Name = "CHANNEL_PETITIONREPLY";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 7L;
                    break;
                case "CHANNEL_TRADE":
                    scr_var.Name = "CHANNEL_TRADE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 8L;
                    break;
                case "CHANNEL_ALLY":
                    scr_var.Name = "CHANNEL_ALLY";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 9L;
                    break;
                case "CHANNEL_ANNOUNCEMENT":
                    scr_var.Name = "CHANNEL_ANNOUNCEMENT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 10L;
                    break;
                case "CHANNEL_BOAT":
                    scr_var.Name = "CHANNEL_BOAT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 11L;
                    break;
                case "CHANNEL_PARTYROOM":
                    scr_var.Name = "CHANNEL_PARTYROOM";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 15L;
                    break;
                case "CHANNEL_PARTYCOMMANDER":
                    scr_var.Name = "CHANNEL_PARTYCOMMANDER";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 16L;
                    break;
                case "CHANNEL_HERO":
                    scr_var.Name = "CHANNEL_HERO";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 17L;
                    break;
                case "TYPE_ERROR":
                    scr_var.Name = "TYPE_ERROR";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = -1L;
                    break;
                case "TYPE_NONE":
                    scr_var.Name = "TYPE_NONE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "TYPE_SELF":
                    scr_var.Name = "TYPE_SELF";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "TYPE_PLAYER":
                    scr_var.Name = "TYPE_PLAYER";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "TYPE_NPC":
                    scr_var.Name = "TYPE_NPC";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "SHORTCUT_ITEM":
                    scr_var.Name = "SHORTCUT_ITEM";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "SHORTCUT_SKILL":
                    scr_var.Name = "SHORTCUT_SKILL";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "SHORTCUT_ACTION":
                    scr_var.Name = "SHORTCUT_ACTION";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "SHORTCUT_MACRO":
                    scr_var.Name = "SHORTCUT_MACRO";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 4L;
                    break;
                case "SHORTCUT_RECIPE":
                    scr_var.Name = "SHORTCUT_RECIPE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 5L;
                    break;
                case "TICKS_PER_MS":
                    scr_var.Name = "TICKS_PER_MS";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 10000L;
                    break;
                case "TICKS_PER_S":
                    scr_var.Name = "TICKS_PER_S";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 10000000L;
                    break;
                case "TICKS_PER_M":
                    scr_var.Name = "TICKS_PER_M";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 600000000L;
                    break;
                case "TICKS_PER_H":
                    scr_var.Name = "TICKS_PER_H";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 36000000000L;
                    break;
                case "TICKS_PER_D":
                    scr_var.Name = "TICKS_PER_D";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 864000000000L;
                    break;
                case "RANDI":
                    scr_var.Name = "RANDI";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)Globals.Rando.Next(0, 100);
                    break;
                case "RANDD":
                    scr_var.Name = "RANDD";
                    scr_var.Type = Var_Types.DOUBLE;
                    scr_var.Value = Globals.Rando.NextDouble();
                    break;
                case "NULL":
                    scr_var.Name = "NULL";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "INT":
                    scr_var.Name = "INT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "DOUBLE":
                    scr_var.Name = "DOUBLE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "STRING":
                    scr_var.Name = "STRING";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "FILEWRITER":
                    scr_var.Name = "FILEWRITER";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 4L;
                    break;
                case "FILEREADER":
                    scr_var.Name = "FILEREADER";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 5L;
                    break;
                case "ARRAYLIST":
                    scr_var.Name = "ARRAYLIST";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 6L;
                    break;
                case "SORTEDLIST":
                    scr_var.Name = "SORTEDLIST";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 7L;
                    break;
                case "STACK":
                    scr_var.Name = "STACK";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 8L;
                    break;
                case "QUEUE":
                    scr_var.Name = "QUEUE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 9L;
                    break;
                case "CLASS":
                    scr_var.Name = "CLASS";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 10L;
                    break;
                case "BYTEBUFFER":
                    scr_var.Name = "BYTEBUFFER";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 11L;
                    break;
                case "WINDOW":
                    scr_var.Name = "WINDOW";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 12L;
                    break;
                case "THREAD":
                    scr_var.Name = "THREAD";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 13L;
                    break;
                case "PUBLIC":
                    scr_var.Name = "PUBLIC";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "PRIVATE":
                    scr_var.Name = "PRIVATE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "PROTECTED":
                    scr_var.Name = "PROTECTED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "PI":
                    scr_var.Name = "PI";
                    scr_var.Type = Var_Types.DOUBLE;
                    scr_var.Value = Math.PI;
                    break;
                case "E":
                    scr_var.Name = "E";
                    scr_var.Type = Var_Types.DOUBLE;
                    scr_var.Value = Math.E;
                    break;
                case "CMD":
                    scr_var.Name = "CMD";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "GUI":
                    scr_var.Name = "GUI";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "HTML":
                    scr_var.Name = "HTML";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "GDI":
                    scr_var.Name = "GDI";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 4L;
                    break;
                case "ASTERISK":
                    scr_var.Name = "ASTERISK";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 0L;
                    break;
                case "ERROR":
                    scr_var.Name = "ERROR";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 1L;
                    break;
                case "EXCLAMATION":
                    scr_var.Name = "EXCLAMATION";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 2L;
                    break;
                case "HAND":
                    scr_var.Name = "HAND";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 3L;
                    break;
                case "INFORMATION":
                    scr_var.Name = "INFORMATION";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 4L;
                    break;
                case "NONE":
                    scr_var.Name = "NONE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 5L;
                    break;
                case "QUESTION":
                    scr_var.Name = "QUESTION";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 6L;
                    break;
                case "STOP":
                    scr_var.Name = "STOP";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 7L;
                    break;
                case "WARNING":
                    scr_var.Name = "WARNING";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = 8L;
                    break;
                case "SCRIPTEVENT_CHAT":
                    scr_var.Name = "SCRIPTEVENT_CHAT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.Chat;
                    break;
                case "SCRIPTEVENT_SELFDIE":
                    scr_var.Name = "SCRIPTEVENT_SELFDIE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfDie;
                    break;
                case "SCRIPTEVENT_SELFREZ":
                    scr_var.Name = "SCRIPTEVENT_SELFREZ";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfRez;
                    break;
                case "SCRIPTEVENT_SELFENTERCOMBAT":
                    scr_var.Name = "SCRIPTEVENT_SELFENTERCOMBAT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfEnterCombat;
                    break;
                case "SCRIPTEVENT_SELFLEAVECOMBAT":
                    scr_var.Name = "SCRIPTEVENT_SELFLEAVECOMBAT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfLeaveCombat;
                    break;
                case "SCRIPTEVENT_SELFSTOPMOVE":
                    scr_var.Name = "SCRIPTEVENT_SELFSTOPMOVE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfStopMove;
                    break;
                case "SCRIPTEVENT_SELFTARGETED":
                    scr_var.Name = "SCRIPTEVENT_SELFTARGETED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfTargeted;
                    break;
                case "SCRIPTEVENT_SELFUNTARGETED":
                    scr_var.Name = "SCRIPTEVENT_SELFUNTARGETED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfUnTargeted;
                    break;
                case "SCRIPTEVENT_TARGETDIE":
                    scr_var.Name = "SCRIPTEVENT_TARGETDIE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.TargetDie;
                    break;
                case "SCRIPTEVENT_CHATTOBOT":
                    scr_var.Name = "SCRIPTEVENT_CHATTOBOT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.ChatToBot;
                    break;
                case "SCRIPTEVENT_UDPRECEIVE":
                    scr_var.Name = "SCRIPTEVENT_UDPRECEIVE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.UDPReceive;
                    break;
                case "SCRIPTEVENT_SKILLUSED":
                    scr_var.Name = "SCRIPTEVENT_SKILLUSED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SkillUsed;
                    break;
                case "SCRIPTEVENT_SKILLLAUNCHED":
                    scr_var.Name = "SCRIPTEVENT_SKILLLAUNCHED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SkillLaunched;
                    break;
                case "SCRIPTEVENT_SKILLCANCELED":
                    scr_var.Name = "SCRIPTEVENT_SKILLCANCELED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SkillCanceled;
                    break;
                case "SCRIPTEVENT_OTHERSKILLUSED":
                    scr_var.Name = "SCRIPTEVENT_OTHERSKILLUSED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.OtherSkillUsed;
                    break;
                case "SCRIPTEVENT_OTHERSKILLLAUNCHED":
                    scr_var.Name = "SCRIPTEVENT_OTHERSKILLLAUNCHED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.OtherSkillLaunched;
                    break;
                case "SCRIPTEVENT_OTHERSKILLCANCELED":
                    scr_var.Name = "SCRIPTEVENT_OTHERSKILLCANCELED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.OtherSkillCanceled;
                    break;
                case "SCRIPTEVENT_JOINPARTY":
                    scr_var.Name = "SCRIPTEVENT_JOINPARTY";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.JoinParty;
                    break;
                case "SCRIPTEVENT_LEAVEPARTY":
                    scr_var.Name = "SCRIPTEVENT_LEAVEPARTY";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.LeaveParty;
                    break;
                case "SCRIPTEVENT_UDPRECEIVEBB":
                    scr_var.Name = "SCRIPTEVENT_UDPRECEIVEBB";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.UDPReceiveBB;
                    break;
                case "SCRIPTEVENT_SERVERPACKET":
                    scr_var.Name = "SCRIPTEVENT_SERVERPACKET";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.ServerPacket;
                    break;
                case "SCRIPTEVENT_SERVERPACKETEX":
                    scr_var.Name = "SCRIPTEVENT_SERVERPACKETEX";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.ServerPacketEX;
                    break;
                case "SCRIPTEVENT_SYSTEMMESSAGE":
                    scr_var.Name = "SCRIPTEVENT_SYSTEMMESSAGE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SystemMessage;
                    break;
                case "SCRIPTEVENT_PARTYINVITE":
                    scr_var.Name = "SCRIPTEVENT_PARTYINVITE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.PartyInvite;
                    break;
                case "SCRIPTEVENT_TRADEINVITE":
                    scr_var.Name = "SCRIPTEVENT_TRADEINVITE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.TradeInvite;
                    break;
                case "SCRIPTEVENT_CLIENTPACKET":
                    scr_var.Name = "SCRIPTEVENT_CLIENTPACKET";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.ClientPacket;
                    break;
                case "SCRIPTEVENT_CLIENTPACKETEX":
                    scr_var.Name = "SCRIPTEVENT_CLIENTPACKETEX";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.ClientPacketEX;
                    break;
                case "SCRIPTEVENT_SELFPACKET":
                    scr_var.Name = "SCRIPTEVENT_SELFPACKET";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfPacket;
                    break;
                case "SCRIPTEVENT_SELFPACKETEX":
                    scr_var.Name = "SCRIPTEVENT_SELFPACKETEX";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.SelfPacketEX;
                    break;
                case "SCRIPTEVENT_AGGRO":
                    scr_var.Name = "SCRIPTEVENT_AGGRO";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.Aggro;
                    break;
                case "SCRIPTEVENT_CHAREFFECT":
                    scr_var.Name = "SCRIPTEVENT_CHAREFFECT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.CharEffect;
                    break;
                case "SCRIPTEVENT_PARTYEFFECT":
                    scr_var.Name = "SCRIPTEVENT_PARTYEFFECT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)EventType.PartyEffect;
                    break;
                case "SYSTEM_THREADCOUNT":
                    scr_var.Name = "SYSTEM_THREADCOUNT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)Threads.Count;
                    break;
                case "SYSTEM_CURRENTFILE":
                    scr_var.Name = "SYSTEM_CURRENTFILE";
                    scr_var.Type = Var_Types.STRING;
                    //Replace the escape character \ with the realbackslash \\
                    scr_var.Value = ((ScriptThread)Threads[CurrentThread]).Current_File;//.Replace("\\", "\\\\");
                    break;
                case "SYSTEM_HANDLECOUNT":
                    scr_var.Name = "SYSTEM_HANDLECOUNT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)System.Diagnostics.Process.GetCurrentProcess().HandleCount;
                    break;
                case "SYSTEM_MEMORYUSAGE":
                    scr_var.Name = "SYSTEM_MEMORYUSAGE";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = GC.GetTotalMemory(false);
                    break;
                case "SYSTEM_MEMORYALLOCATED":
                    scr_var.Name = "SYSTEM_MEMORYALLOCATED";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = Environment.WorkingSet;
                    break;
                case "SYSTEM_STACKHEIGHT":
                    scr_var.Name = "SYSTEM_STACKHEIGHT";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = (long)StackHeight;
                    break;
                case "SYSTEM_VERSION":
                    scr_var.Name = "SYSTEM_VERSION";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = Util.GetInt64(Globals.Version);
                    break;
                case "ENV_MACHINENAME":
                    scr_var.Name = "ENV_MACHINENAME";
                    scr_var.Type = Var_Types.STRING;
                    scr_var.Value = Environment.MachineName;
                    break;
                case "ENV_USERNAME":
                    scr_var.Name = "ENV_USERNAME";
                    scr_var.Type = Var_Types.STRING;
                    scr_var.Value = Environment.UserName;
                    break;
                case "ENV_PATH":
                    scr_var.Name = "ENV_PATH";
                    scr_var.Type = Var_Types.STRING;
                    scr_var.Value = Globals.PATH;
                    break;
                case "NOW":
                    scr_var.Name = "NOW";
                    scr_var.Type = Var_Types.INT;
                    scr_var.Value = DateTime.Now.Ticks;
                    break;
                default:
                    if (name.StartsWith("#I"))
                    {
                        scr_var.Name = name;
                        scr_var.Type = Var_Types.INT;
                        scr_var.Value = Util.GetInt64(oname.Substring(2, oname.Length - 2));
                    }
                    else if (name.StartsWith("#D"))
                    {
                        scr_var.Name = name;
                        scr_var.Type = Var_Types.DOUBLE;
                        scr_var.Value = Util.GetDouble(oname.Substring(2, oname.Length - 2));
                    }
                    else if (name.StartsWith("#$"))
                    {
                        scr_var.Name = name;
                        scr_var.Type = Var_Types.STRING;
                        scr_var.Value = oname.Substring(2, oname.Length - 2);
                    }
                    else if (name.StartsWith("\"#$"))
                    {
                        scr_var.Name = name;
                        scr_var.Type = Var_Types.STRING;
                        scr_var.Value = oname.Substring(3, oname.Length - 4);
                    }
                    else
                    {
                        //try to create a dynamic variable from this...
                        try
                        {
                            if (oname.StartsWith("0x"))
                            {
                                //trying to get a hex value
                                scr_var.Name = oname;
                                scr_var.Type = Var_Types.INT;
                                scr_var.Value = long.Parse(oname.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
                            }
                            else if (oname.Contains("."))
                            {
                                //TODO : this doesn't work because the . is interpreted as a class operator

                                //must be a double
                                scr_var.Name = oname;
                                scr_var.Type = Var_Types.DOUBLE;
                                scr_var.Value = double.Parse(oname, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                            }
                            else
                            {
                                scr_var.Name = oname;

                                //try
                                //{
                                //must be an integer
                                scr_var.Type = Var_Types.INT;
                                scr_var.Value = long.Parse(oname, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
                                /*}
                                catch
                                {
                                    //failed to cast to an int... must be a string
                                    scr_var.Type = Var_Types.STRING;
                                    scr_var.Value = oname;
                                }*/

                                //if we catch here it will always succeed and never give an error that the variable couldn't be found...
                            }
                        }
                        catch
                        {
                            scr_var.Name = name;
                            scr_var.Type = Var_Types.STRING;
                            scr_var.Value = "ERROR - VARIABLE NOT FOUND AT LINE " + Line_Pos.ToString();
                            Script_Error("VARIABLE " + name + " IS UNDEFINED");
                        }
                    }
                    break;
            }

            return scr_var;
        }
    }
}
