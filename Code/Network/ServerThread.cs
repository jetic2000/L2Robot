using System.Collections.Generic;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace L2Robot
{
    /// <summary>
    /// Sends packets between the bot and the server
    /// </summary>


    public class ServerThread
    {
        public Thread readthread;
        public Thread sendthread;
        GameData gamedata;

        public ServerThread(GameData gamedata)
        {
            this.gamedata = gamedata;
            Init();
        }

        public void Init()
        {
            OpenGameServerConnection();
            sendthread = new Thread(new ThreadStart(GameSendThread));
            readthread = new Thread(new ThreadStart(GameReadThread));
            sendthread.IsBackground = true;
            readthread.IsBackground = true;
            sendthread.Priority = ThreadPriority.Highest;
            readthread.Priority = ThreadPriority.Highest;
        }

        public void OpenGameServerConnection()
        {
            this.gamedata.Game_IP = this.gamedata.Override_Game_IP;
            this.gamedata.Game_Port = this.gamedata.Override_Game_Port;
            this.gamedata.Game_GameSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.gamedata.Game_GameSocket.Connect(this.gamedata.Game_IP, this.gamedata.Game_Port);

            this.gamedata.Game_GameSocket.NoDelay = true;
            this.gamedata.Game_GameSocket.ReceiveTimeout = Globals.TimeOut;
            this.gamedata.Game_GameSocket.SendTimeout = Globals.TimeOut;
            this.gamedata.Game_GameSocket.SendBufferSize = Globals.BUFFER_NETWORK;
            this.gamedata.Game_GameSocket.ReceiveBufferSize = Globals.BUFFER_NETWORK;
            Console.WriteLine("bot -> gameserver : connected");
        }

        private void GameSendThread()
        {
            byte[] b2 = new byte[2];
            byte[] buff;
            byte[] buffe;

            ByteBuffer bbuffer0;
            try
            {
                while (true)
                {
                    while (this.gamedata.GetCount_DataToServer() > 0)
                    {
                        bbuffer0 = null;

                        Globals.GameSendQueueLock.EnterWriteLock();
                        try
                        {
                            bbuffer0 = (ByteBuffer)this.gamedata.gamesendqueue.Dequeue();
                        }
                        finally
                        {
                            Globals.GameSendQueueLock.ExitWriteLock();
                        }

                        buff = bbuffer0.Get_ByteArray();
                        
                        //Console.WriteLine("[TO S]:" + BitConverter.ToString(buff, 0).Replace("-", string.Empty).ToLower());
                        /*if (L2NET.Mixer != null)
                        {
                            L2NET.Mixer.Encrypt0(buff);
                        }*/

                        this.gamedata.crypt_out.encrypt(buff);

                        buffe = new byte[2 + buff.Length];

                        b2 = BitConverter.GetBytes(buffe.Length);
                        buffe[0] = b2[0];
                        buffe[1] = b2[1];

                        buff.CopyTo(buffe, 2);

                        this.gamedata.Game_GameSocket.Send(buffe, 0, buffe.Length, SocketFlags.None);
                    }

                    Thread.Sleep(Globals.SLEEP_GameSendThread);//sleep for 100 mili seconds; this is okay because a new send should wake the thread up
                }//end of while running
            }
            catch (Exception e)
            {
                //Globals.l2net_home.Add_Error("crash: GameSendThread : " + e.Message);
            }
        }

        private void GameReadThread()
        {
            byte[] buffread = new byte[Globals.BUFFER_MAX];
            byte[] buffpacket;

            int cnt = 0;
            int size = 0;
            bool handle = true;
            bool forward = true;

            ByteBuffer bbuffer0;
            ByteBuffer bbuffer1;
            ByteBuffer bbtmp1;

            try
            {
                while (true)
                {
                    cnt += this.gamedata.Game_GameSocket.Receive(buffread, cnt, Globals.BUFFER_PACKET - cnt, SocketFlags.None);
                    size = BitConverter.ToUInt16(buffread, 0);


                    while (cnt >= size && cnt > 2)
                    {
                        //if we got partial shit we cant use, read some more until it is full
                        while (size > cnt)
                        {
                            cnt += this.gamedata.Game_GameSocket.Receive(buffread, cnt, Globals.BUFFER_PACKET - cnt, SocketFlags.None);
                        }

                        while (size < 2)
                        {
                            this.gamedata.Game_ClientSocket.Send(buffread, cnt, SocketFlags.None);
                            cnt = this.gamedata.Game_GameSocket.Receive(buffread, 0, Globals.BUFFER_PACKET, SocketFlags.None);
                            size = BitConverter.ToUInt16(buffread, 0);
                        }

                        buffpacket = new byte[size - 2];
                        Array.Copy(buffread, 2, buffpacket, 0, size - 2);
                        this.gamedata.crypt_in.decrypt(buffpacket);

                        //shift the data over by size
                        for (uint i = 0; i < cnt - size; i++)
                        {
                            buffread[i] = buffread[size + i];
                        }

                        cnt -= size;

                        if (buffpacket.Length > 0)
                        {
                            handle = true;
                            forward = true;
                            bbuffer0 = new ByteBuffer(buffpacket);
                            bbuffer1 = new ByteBuffer(buffpacket);
                            
                            //TODO: Must ensure script running after IG login
                            if (this.gamedata.CurrentScriptState == ScriptState.Running)
                            {
                                if ((PServer)buffpacket[0] == PServer.EXPacket)
                                {
                                    if (this.gamedata.scriptthread.Blocked_ServerPacketsEX.ContainsKey(Convert.ToInt32(buffpacket[1] + buffpacket[2] << 8)))
                                    {
                                        forward = false;
                                    }
                                }
                                else
                                {
                                    if (this.gamedata.scriptthread.Blocked_ServerPackets.ContainsKey(Convert.ToInt32(buffpacket[0])))
                                    {
                                        forward = false;
                                    }
                                }
                            }

                            switch ((PServer)buffpacket[0])
                            {
                                case PServer.VersionCheck:
                                    //Console.WriteLine("[S]:" + BitConverter.ToString(buffpacket, 0).Replace("-", string.Empty).ToLower());
                                    handle = false;
                                    forward = false;
                                    this.gamedata.game_key[0] = buffpacket[2];
                                    this.gamedata.game_key[1] = buffpacket[3];
                                    this.gamedata.game_key[2] = buffpacket[4];
                                    this.gamedata.game_key[3] = buffpacket[5];
                                    this.gamedata.game_key[4] = buffpacket[6];//0xa1;
                                    this.gamedata.game_key[5] = buffpacket[7];//0x6c;
                                    this.gamedata.game_key[6] = buffpacket[8];//0x54;
                                    this.gamedata.game_key[7] = buffpacket[9];//0x87
                                    this.gamedata.game_key[8] = 0xc8;
                                    this.gamedata.game_key[9] = 0x27;
                                    this.gamedata.game_key[10] = 0x93;
                                    this.gamedata.game_key[11] = 0x01;
                                    this.gamedata.game_key[12] = 0xa1;
                                    this.gamedata.game_key[13] = 0x6c;
                                    this.gamedata.game_key[14] = 0x31;
                                    this.gamedata.game_key[15] = 0x97;
                                    // ugly
                                    bbtmp1 = new ByteBuffer(buffpacket);

                                    bbtmp1.ReadByte();
                                    bbtmp1.ReadByte();
                                    bbtmp1.ReadInt32();
                                    bbtmp1.ReadInt32();
                                    bbtmp1.ReadInt32();
                                    this.gamedata.Server_ID = bbtmp1.ReadInt32();
                                    bbtmp1.ReadByte();
                                    this.gamedata.Obfuscation_Key = bbtmp1.ReadInt32();

                                    Console.WriteLine("Obfuscation_Key: {0:X}", this.gamedata.Obfuscation_Key);

                                    try
                                    {
                                        //this.gamedata.Mixer = new MixedPackets(this.gamedata, BitConverter.ToInt32(buffpacket, 19));
                                        this.gamedata.Mixer = new MixedPackets(this.gamedata, this.gamedata.Obfuscation_Key);
                                    }
                                    catch
                                    {
                                        //Globals.l2net_home.Add_Debug("gameserver - Negative client ID encryption key... going to continue", Globals.Red, TextType.BOT);
                                        Console.WriteLine("gameserver - Negative client ID encryption key");
                                    }
                                    finally
                                    {
                                        //Globals.l2net_home.Add_Debug("gameserver - got client ID encryption key", Globals.Red, TextType.BOT);
                                        Console.WriteLine("gameserver - got client ID encryption key");
                                    }


                                    //setup the key
                                    this.gamedata.crypt_in.setKey(this.gamedata.game_key);
                                    this.gamedata.crypt_out.setKey(this.gamedata.game_key);
                                    this.gamedata.crypt_clientin.setKey(this.gamedata.game_key);
                                    this.gamedata.crypt_clientout.setKey(this.gamedata.game_key);
                                    this.gamedata.logged_in = true;

                                    {
                                        //need to send this here... so it dones't get encrypted
                                        byte[] key_send = new byte[2 + buffpacket.Length];
                                        byte[] b2 = BitConverter.GetBytes((short)key_send.Length);
                                        key_send[0] = b2[0];
                                        key_send[1] = b2[1];

                                        buffpacket.CopyTo(key_send, 2);
                                        this.gamedata.Game_ClientSocket.Send(key_send);
                                        Console.WriteLine("gameserver:{0} - keys forwarded to client", this.gamedata.Server_ID);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (forward)
                            {
                                Console.WriteLine("[S]:" + BitConverter.ToString(buffpacket, 0).Replace("-", string.Empty).ToLower());
                                //bbuffer0 = new ByteBuffer(buffpacket);
                                this.gamedata.SendToClient(bbuffer0);
                            }
                            if (handle)
                            {
                                //bbuffer1 = new ByteBuffer(buffpacket);
                                this.gamedata.SendToBotRead(bbuffer1);
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
                //Globals.l2net_home.Add_Error("crash: GameReadThread : " + e.Message);
            }
        }//end of read data
    }//end of class
}
