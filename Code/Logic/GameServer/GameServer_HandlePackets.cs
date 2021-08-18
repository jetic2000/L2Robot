using System;

namespace L2Robot
{
    public partial class GameServer
    {
        private void HandlePackets()
        {
            ByteBuffer buffe;
            uint b0 = 0, b1 = 0;//byte
            string last_p = "";//, last_p2 = "";
            //int index = 0;

            while (this.gamedata.GetCount_DataToBot() > 0)
            {
                try
                {
                    //buffe = null;

                    Globals.GameReadQueueLock.EnterWriteLock();
                    try
                    {
                        //Console.WriteLine("-gamereadqueue.Dequeue()");
                        buffe = (ByteBuffer)this.gamedata.gamereadqueue.Dequeue();
                        {
                            byte[] buff;
                            buff = buffe.Get_ByteArray();
                            Console.WriteLine("[S]:" + BitConverter.ToString(buff, 0).Replace("-", string.Empty).ToLower());
                        }

                    }
                    catch (System.Exception e)
                    {
                        //Globals.l2net_home.Add_Error("Packet Error Reading Queue : " + e.Message);
                        break;
                    }
                    finally
                    {
                        Globals.GameReadQueueLock.ExitWriteLock();
                    }

                    //buffe contains unencoded data
                    b0 = buffe.ReadByte();
                    //last_p2 = last_p;
                    //last_p = b0.ToString("X2");
                    //Console.WriteLine("Packet: {0}", last_p);

                    //do we have an event for this packet?
                    /*
                    if (ScriptEngine.ServerPacketsContainsKey((int)b0))
                    {
                        ScriptEvent sc_ev = new ScriptEvent();
                        sc_ev.Type = EventType.ServerPacket;
                        sc_ev.Type2 = (int)b0;
                        sc_ev.Variables.Add(new ScriptVariable(buffe, "PACKET", Var_Types.BYTEBUFFER, Var_State.PUBLIC));
                        sc_ev.Variables.Add(new ScriptVariable(System.DateTime.Now.Ticks, "TIMESTAMP", Var_Types.INT, Var_State.PUBLIC));
                        ScriptEngine.SendToEventQueue(sc_ev);
                    }*/

                    switch ((PServer)b0)
                    {
                        case PServer.MTL:
                            ServerPackets.MoveToLocation(this.gamedata, buffe);
                            break;
                        case PServer.CI:
                            //Console.WriteLine("[S]:PServer.CI");
                            //ServerPackets.CharInfo(this.gamedata, buffe);
                            break;
                        case PServer.UI:
                            //Console.WriteLine("[S]:PServer.UI");
                            ServerPackets.UserInfo(this.gamedata, buffe);
                            break;
                        case PServer.StopMove:
                            ServerPackets.StopMove(this.gamedata, buffe);
                            break;
                        case PServer.Attack:
                            //ClientPackets.Attack_Packet(buffe);
                            break;
                        case PServer.Die:
                            //ClientPackets.Die_Packet(buffe);
                            break;
                        case PServer.NetPing:
                            //This is tmp solution
                            //ClientPackets.NetPing(this.gamedata, buffe);
                            break;
                        case PServer.StatusUpdate:
                            //Console.WriteLine("[S]:PServer.StatusUpdate");
                            //ServerPackets.StatusUpdate(this.gamedata, buffe);
                            break;
                        case PServer.TeleportToLocation:
                            ServerPackets.Teleport(this.gamedata, buffe);
                            break;
                        case PServer.DeleteObject:
                            ServerPackets.DeleteItem(this.gamedata, buffe);
                            break;
                        case PServer.MoveToPawn:
                            ServerPackets.MoveToPawn(this.gamedata, buffe);
                            break;
                        case PServer.MagicSkillUser:
                            ServerPackets.MagicSkillUser(this.gamedata, buffe);
                            break;
                        case PServer.EXPacket:
                            b1 = buffe.ReadUInt16();
                            last_p = last_p + " " + b1.ToString("X2");
                            switch ((PServerEX)b1)
                            {
                                case PServerEX.ExUserinfoStats:
                                    //ClientPackets.ExUserInfoStats(buffe);
                                    break;
                                case PServerEX.ExUserInfo:
                                    //Console.WriteLine("[S]:PServerEX.ExUserInfo");
                                    ServerPackets.EXUserInfo(this.gamedata, buffe);
                                    break;
                                case PServerEX.ExSetCompassZoneCode:
                                    ServerPackets.ExSetCompassZoneCode(this.gamedata, buffe);
                                    break;
                            }
                            break;
                    }//end of switch on packet type

                }
                catch (System.Exception e)
                {
                    //Globals.l2net_home.Add_Error("Packet Error: " + last_p + " Previous Packet: " + last_p2 + " : " + e.Message);
                    //Globals.l2net_home.Add_Error("Packet Error: " + last_p + " :: " + e.Message);
                }
            }//end of loop to handle queue data
        }//end of HandlePackets

    }
}
