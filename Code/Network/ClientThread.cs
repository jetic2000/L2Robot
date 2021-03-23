using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace L2Robot
{
    /// <summary>
    /// Sends packets between the bot and the client
    /// </summary>
    public class ClientThread
    {
        public Thread sendthread;
        public Thread readthread;
        public GameData gamedata;

        public ClientThread(GameData gamedata)
        {
            this.gamedata = gamedata;
            Init();
        }

        public void Init()
        {
            sendthread = new Thread(new ThreadStart(ClientSendThread));
            readthread = new Thread(new ThreadStart(ClientReadThread));

            sendthread.IsBackground = true;
            readthread.IsBackground = true;

            sendthread.Priority = ThreadPriority.Highest;
            readthread.Priority = ThreadPriority.Highest;
        }

        public void network_exception()
        {
            Globals.InstancesLock.EnterWriteLock();
            if (Globals.Games.ContainsKey(this.gamedata.Client_Port))
            {
                Globals.Games.Remove(this.gamedata.Client_Port);
                Globals.l2net_home.timer_instances.Start();
                Globals.Dirty_Games.Add(this.gamedata);
            }
            Globals.InstancesLock.ExitWriteLock();
            Globals.l2net_home.timer_instances.Start();
        }

        private void ClientSendThread()
        {
            byte[] b2 = new byte[2];
            byte[] buff;
            byte[] buffe;

            ByteBuffer bbuffer0;

            try
            {
                while (this.gamedata.running)
                {
                    while (this.gamedata.GetCount_DataToClient() > 0)
                    {
                        bbuffer0 = null;

                        Globals.ClientSendQueueLock.EnterWriteLock();
                        try
                        {
                            bbuffer0 = (ByteBuffer)this.gamedata.clientsendqueue.Dequeue();
                        }
                        finally
                        {
                            Globals.ClientSendQueueLock.ExitWriteLock();
                        }

                        //Get Decrpted Client Data
                        buff = bbuffer0.Get_ByteArray();
                        this.gamedata.crypt_clientout.encrypt(buff);
                        buffe = new byte[2 + buff.Length];
                        b2 = BitConverter.GetBytes((short)buffe.Length);
                        buffe[0] = b2[0];
                        buffe[1] = b2[1]; ;
                        buff.CopyTo(buffe, 2);
                        this.gamedata.Game_ClientSocket.Send(buffe);//,0,buffe.Length);

                        //release the crap we dont need anymore
                        buff = null;
                        buffe = null;
                    }

                    Thread.Sleep(Globals.SLEEP_ClientSendThread);//sleep for 100 mili seconds; this is okay because a new send should wake the thread up
                }//end of while running
            }
            catch (Exception e)
            {
                //Globals.l2net_home.Add_Error("crash: ClientSendThread : " + e.Message);
                network_exception();
            }
        }

        private void ClientReadThread()
        {
            byte[] buffread = new byte[Globals.BUFFER_MAX];
            byte[] buffpacket;
            byte[] buffpacketin;

            int cnt = 0;
            int size = 0;
            bool forward = true;
            int recv_len = 0;

            ByteBuffer bbuffer0;
            try
            {
                while (this.gamedata.running)
                {
                    if (this.gamedata.Game_ClientSocket.Poll(-1, SelectMode.SelectRead))
                    {
                        try
                        {
                            recv_len = this.gamedata.Game_ClientSocket.Receive(buffread, cnt, Globals.BUFFER_PACKET - cnt, SocketFlags.None);
                            if (recv_len == 0)
                            {
                                Console.WriteLine("Client disconnected");
                                network_exception();
                                return;
                            }
                            else
                            {
                                cnt += recv_len;
                                size = BitConverter.ToUInt16(buffread, 0);
                            }
                        }
                        catch
                        {
                                Console.WriteLine("Client disconnected Error");
                                network_exception();
                                return;
                        }
                    }

                    while (cnt >= size && cnt > 2)
                    {
                        //if we got partial shit we cant use, read some more until it is full
                        while (size > cnt)
                        {
                            if (this.gamedata.Game_ClientSocket.Poll(-1, SelectMode.SelectRead))
                            {
                                try
                                {
                                    recv_len = this.gamedata.Game_ClientSocket.Receive(buffread, cnt, Globals.BUFFER_PACKET - cnt, SocketFlags.None);
                                    if (recv_len == 0)
                                    {
                                        Console.WriteLine("Client disconnected");
                                        network_exception();
                                        return;
                                    }
                                    else
                                    {
                                        cnt += recv_len;
                                    }
                                }
                                catch
                                {
                                    Console.WriteLine("Client disconnected Error");
                                    network_exception();
                                    return;
                                }
                            }                                       
                        }

                        buffpacketin = new byte[size - 2];
                        buffpacket = new byte[size - 2];
                        Array.Copy(buffread, 2, buffpacketin, 0, size - 2);
                        

                        this.gamedata.crypt_clientin.decrypt(buffpacketin);

                        //Console.WriteLine("[C]:" + BitConverter.ToString(buffpacketin, 0).Replace("-", string.Empty).ToLower());
                        Array.Copy(buffpacketin, 0, buffpacket, 0, size - 2);

                        //TODO: What is Mixer?
                        if (this.gamedata.Mixer != null)
                        {
                            this.gamedata.Mixer.Decrypt0(buffpacket);
                        }

                        Console.WriteLine("[C]:" + BitConverter.ToString(buffpacket, 0).Replace("-", string.Empty).ToLower());

                        //shift the data over by size for next loop
                        for (uint i = 0; i < cnt - size; i++)
                        {
                            buffread[i] = buffread[size + i];
                        }

                        cnt -= size;

                        if (buffpacket.Length > 0)
                        {
                            forward = true;

                            if (this.gamedata.CurrentScriptState == ScriptState.Running)
                            {
                                if ((PClient)buffpacket[0] == PClient.EXPacket)
                                {
                                    if (this.gamedata.scriptthread.Blocked_ClientPacketsEX.ContainsKey(Convert.ToInt32(buffpacket[1] + buffpacket[2] << 8)))
                                    {
                                        forward = false;
                                    }

                                    if (this.gamedata.scriptthread.ClientPacketsEXContainsKey(Convert.ToInt32(buffpacket[1] + buffpacket[2] << 8)))
                                    {
                                        ByteBuffer bb = new ByteBuffer(buffpacket);
                                        ScriptEvent sc_ev = new ScriptEvent();
                                        sc_ev.Type = EventType.ClientPacketEX;
                                        sc_ev.Type2 = Convert.ToInt32(buffpacket[1] + buffpacket[2] << 8);
                                        sc_ev.Variables.Add(new ScriptVariable(bb, "PACKET", Var_Types.BYTEBUFFER, Var_State.PUBLIC));
                                        sc_ev.Variables.Add(new ScriptVariable(DateTime.Now.Ticks, "TIMESTAMP", Var_Types.INT, Var_State.PUBLIC));
                                        this.gamedata.scriptthread.SendToEventQueue(sc_ev);
                                    }
                                }
                                else
                                {
                                    if (this.gamedata.scriptthread.Blocked_ClientPackets.ContainsKey(Convert.ToInt32(buffpacket[0])))
                                    {
                                        forward = false;
                                    }

                                    if (this.gamedata.scriptthread.ClientPacketsContainsKey(Convert.ToInt32(buffpacket[0])))
                                    {
                                        ByteBuffer bb = new ByteBuffer(buffpacket);

                                        ScriptEvent sc_ev = new ScriptEvent();
                                        sc_ev.Type = EventType.ClientPacket;
                                        sc_ev.Type2 = Convert.ToInt32(buffpacket[0]);
                                        sc_ev.Variables.Add(new ScriptVariable(bb, "PACKET", Var_Types.BYTEBUFFER, Var_State.PUBLIC));
                                        sc_ev.Variables.Add(new ScriptVariable(DateTime.Now.Ticks, "TIMESTAMP", Var_Types.INT, Var_State.PUBLIC));
                                        this.gamedata.scriptthread.SendToEventQueue(sc_ev);
                                    }
                                }
                            }

                            //Console.WriteLine("CMD ID FROM CLIENT: {0:X}", buffpacket[0]);

                            //this is where we would want to handle packets sent by the client
                            switch ((PClient)buffpacket[0])
                            {
                                case PClient.AuthLogin://protocol version
                                    int prot = BitConverter.ToInt16(buffpacket, 1);
                                    //Globals.l2net_home.Add_Debug("protocol version: " + prot.ToString(), Globals.Red, TextType.BOT);

                                    if (prot == -2)
                                    {
                                        //Globals.l2net_home.Add_Debug("-2 protocol... just a ping... we've been had XD...", Globals.Red, TextType.BOT);
                                    }
                                    else
                                    {
                                        //valid protocol...
                                        Console.WriteLine("valid protocol: {0}", prot);
                                        //Shall start a new game server node for a new client connection
                                        //There is probem for gab data if this program runs after a L2 client
                                        this.gamedata.gamethread = new ServerThread(this.gamedata);
                                        this.gamedata.gamethread.readthread.Start();
                                        this.gamedata.gamethread.sendthread.Start();
                                        this.gamedata.gameprocessdatathread = new GameServer(this.gamedata);
                                        this.gamedata.gameprocessdatathread.processthread.Start();

                                        this.gamedata.scriptthread = new ScriptEngine(this.gamedata);
                                    }
                                    break;
                                case PClient.NetPingReply:
                                    //Console.WriteLine("++Hold Client for NetPingReply");
                                    forward = true;
                                    break;
                            }

                            if (forward)
                            {
                                //send packets from the client right to the server
                                bbuffer0 = new ByteBuffer(buffpacketin);
                                this.gamedata.SendToGameServerNF(bbuffer0);
                            }
                        }

                        if (cnt > 2)
                        {
                            size = BitConverter.ToUInt16(buffread, 0);
                        }
                    }//end of while loop
                }//end of while running
            }
            catch (Exception e)
            {
                //Globals.l2net_home.Add_Error("crash: ClientReadThread : " + e.Message);
                network_exception();
                return;
            }
        }//end of read data
    }//end of clientsendthread
}
