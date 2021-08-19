using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;


namespace L2Robot
{
    class LoginServer
    {
        public enum TCP_TABLE_CLASS : int
        {
            TCP_TABLE_BASIC_LISTENER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTENER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTENER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL
        } 

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public uint state;
            public uint localAddr;
            public byte localPort1;
            public byte localPort2;
            public byte localPort3;
            public byte localPort4;
            public uint remoteAddr;
            public byte remotePort1;
            public byte remotePort2;
            public byte remotePort3;
            public byte remotePort4;
            public int owningPid;

            public ushort LocalPort
            {
                get
                {
                    return BitConverter.ToUInt16(
                        new byte[2] { localPort2, localPort1}, 0);
                }
            }

            public ushort RemotePort
            {
                get
                {
                    return BitConverter.ToUInt16(
                        new byte[2] { remotePort2, remotePort1}, 0);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPTABLE_OWNER_PID
        {
            public uint dwNumEntries;
            MIB_TCPROW_OWNER_PID table;
        }

        public struct ProcessInfo
        {
            public int pid;
            public uint remotePort;
            public uint localPort;
        }


        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion, TCP_TABLE_CLASS tblClass, uint reserved = 0);
        public static void IG_Init()
        {
            //In loop for new instance
            Console.WriteLine("IG_Init()");
            Globals.PATH = Environment.CurrentDirectory;
            //LoadData.LoadDataFiles();
            //Globals.l2net_home.UpdateLog("AAAA");

            //Begin to Monitor L2.bin in process list
            //SmartTimer timer_instances = new SmartTimer();
            //timer_instances.Interval = 3000;
            //timer_instances.OnTimerTick += Validate_Instances;
            //timer_instances.Start();
            //Get_L2_Ports();

            Globals.IGListener = new Thread(new ThreadStart(IG_Listener));
            Globals.running = true;
            Globals.IGListener.Start();
        }


        public static void Validate_Instances()
        {
            //Check if game process is running or not?
            Globals.InstancesLock.EnterWriteLock();

            Check_L2_Ports();

            Globals.InstancesLock.ExitWriteLock();
        }

        public static void Check_L2_Ports()
        {
            try
            {
                int AF_INET = 2;    // IP_v4
                int buffSize = 0;
                MIB_TCPROW_OWNER_PID[] tTable;
                List<ProcessInfo> ps = new List<ProcessInfo>();

                uint ret = GetExtendedTcpTable(IntPtr.Zero, ref buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
                IntPtr buffTable = Marshal.AllocHGlobal(buffSize);
                try
                {
                    ret = GetExtendedTcpTable(buffTable, ref buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
                    //Console.WriteLine(buffTable.ToString());
                    MIB_TCPTABLE_OWNER_PID tab = (MIB_TCPTABLE_OWNER_PID)Marshal.PtrToStructure(buffTable, typeof(MIB_TCPTABLE_OWNER_PID));
                    IntPtr rowPtr = (IntPtr)((long)buffTable + Marshal.SizeOf(tab.dwNumEntries));
                    tTable = new MIB_TCPROW_OWNER_PID[tab.dwNumEntries];
                    //Console.WriteLine("Get TCP Table");
                    for (int i = 0; i < tab.dwNumEntries; i++)
                    {
                        MIB_TCPROW_OWNER_PID tcpRow = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(rowPtr, typeof(MIB_TCPROW_OWNER_PID));
                        tTable[i] = tcpRow;
                        // next entry
                        rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(tcpRow));
                        if (tTable[i].RemotePort == 10091)
                        {
                            Console.WriteLine("Game PID:{0}, Local Port:{1}", tTable[i].owningPid, tTable[i].LocalPort);
                            ProcessInfo p = new ProcessInfo();
                            p.remotePort = tTable[i].RemotePort;
                            p.localPort = tTable[i].LocalPort;
                            p.pid = tTable[i].owningPid;
                            ps.Add(p);
                        }
                    }

                    //Check if all Games Instance stil alive
                    foreach (int index in Globals.Games.Keys)
                    {
                        bool result = false;
                        Console.WriteLine("Globals.Games.Keys = {0}", Globals.Games[index].Client_Port);
                        foreach (ProcessInfo i in ps)
                        {
                            if (Globals.Games[index].Client_Port == i.localPort)
                            {
                                result = true;
                                Globals.Games[index].processPid = i.pid;
                                break;
                            }
                        }

                        if (result == false)
                        {
                            //Not found, dead instance
                        }
                    }

                }
                finally
                {
                    Marshal.FreeHGlobal(buffTable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void IG_Listener()
        {
            TcpListener Game_ClientLink;
            string IG_Local_IP = "127.0.0.1";
            int IG_Local_Game_Port = 3000;

            System.Net.IPAddress ipAd = System.Net.IPAddress.Parse(IG_Local_IP);//textBox_ig_local.Text);
            Game_ClientLink = new TcpListener(ipAd, IG_Local_Game_Port);
            Game_ClientLink.Start();

            // GameData g1 = new GameData();
            // g1.Mixer = new MixedPackets(g1, 0x12121212);

            // GameData g2 = new GameData();
            // g1.Mixer = new MixedPackets(g2, 0x12121212);

            while(Globals.running)
            {
                GameData gamedata = new GameData();
                Globals.waitingGameData = gamedata;
                Console.WriteLine("Waiting a client");
                if (IG_Start_Listen(gamedata, Game_ClientLink))
                {
                    Console.WriteLine("Found a client");
                    IG_ProcData(gamedata);
                }
            }
        }

        public static bool IG_Start_Listen(GameData gamedata, TcpListener listener)
        {
            string ip = "";
            int port = -1;
            //GameData gamedata = new GameData();
            //gamedata.Game_ClientLink = listener;
            gamedata.Override_Game_IP = "180.102.96.21";
            gamedata.Override_Game_Port = 10091;
            gamedata.Chron = Chronicle.CT4_0;
            //Globals.Games.Add(1000, gamedata);
            //Globals.l2net_home.timer_instances.Start();

            try
            {
                //Only need one instance as all data is from the some port from client by sock5
                bool got_connection = false;
                while ((!got_connection))
                {
                    try
                    {
                        gamedata.Game_ClientSocket = listener.AcceptSocket();
                        gamedata.Game_ClientSocket.NoDelay = true;
                        gamedata.Game_ClientSocket.ReceiveTimeout = Globals.TimeOut;
                        gamedata.Game_ClientSocket.SendTimeout = Globals.TimeOut;
                        gamedata.Game_ClientSocket.SendBufferSize = Globals.BUFFER_NETWORK;
                        gamedata.Game_ClientSocket.ReceiveBufferSize = Globals.BUFFER_NETWORK;
                        
                        ip = gamedata.Game_ClientSocket.RemoteEndPoint.ToString().Split(':')[0];
                        port = int.Parse(gamedata.Game_ClientSocket.RemoteEndPoint.ToString().Split(':')[1]);
                        //
                        //port = 0;
                        gamedata.Client_Port = port;

                        Console.WriteLine("IP:{0}, PORT:{1}", ip, port);

                        Globals.InstancesLock.EnterWriteLock();
                        if (Globals.Games.ContainsKey(port))
                        {
                            Globals.Games[port] = gamedata;
                        }
                        else
                        {
                            Globals.Games.Add(port, gamedata);
                        }
                        Globals.InstancesLock.ExitWriteLock();

                        got_connection = true;
                        //Globals.l2net_home.timer_instances.Start();
                    }
                    catch
                    {
                        //TODO: Handle fail things
                        return false;
                    }
                }
            }
            catch
            {
                //TODO: Handle fail things
            }
            return true;
        }

        public static void IG_ProcData(object arg)
        {
            GameData gamedata = (GameData)arg;
            try
            {
                //Connect sock5
                byte[] buffread = new byte[Globals.BUFFER_MAX];
                int cnt = 0;
                int proxy_step = 0;


                //use proxy server by default
                bool temp_proxy_var = true;
                while (temp_proxy_var)
                {

                    cnt = gamedata.Game_ClientSocket.Receive(buffread, 0, Globals.BUFFER_PACKET, SocketFlags.None);
                    if (buffread.Length > 0)
                    {
                        if (proxy_step == 0) // first step == 1 packet
                        {
                            if (buffread[0] == 5)// checking proxy version
                            {
                                // packet struct for sock 5 
                                //based on http://www.faqs.org/rfcs/rfc1928.html
                                // buffread[0]  == 5  // proxy sock ver 5
                                // buffread[1]  ==(1-255) no. methods for log in on serv
                                // buffread[2-no methods] - methods for login (without login/pass , with login pass etc)

                                /*     methods "id"
                                    *           o  X'00' NO AUTHENTICATION REQUIRED
                                                o  X'01' GSSAPI
                                                o  X'02' USERNAME/PASSWORD
                                                o  X'03' to X'7F' IANA ASSIGNED
                                                o  X'80' to X'FE' RESERVED FOR PRIVATE METHODS
                                                o  X'FF' NO ACCEPTABLE METHODS
                                    * 
                                    * 
                                    */

                                for (int i = 2; i < buffread.Length; i++)//try to find method (0) aka without login/pass
                                {
                                    if (buffread[i] == 0)// found method 0 (without pass)
                                    {
                                        // seding answer to client...
                                        buffread[1] = 0;
                                        gamedata.Game_ClientSocket.Send(buffread, 2, SocketFlags.None);
                                        proxy_step = 1;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //Globals.l2net_home.Add_Debug("wrong proxy version", Globals.Red, TextType.BOT);
                            }
                        }
                        else if (proxy_step == 1)
                        {

                            // we get packet with info about dest conection
                            // atm without handler (lazy :P) ( upd  4.12.12 - some parsing :P)
                            Globals.proxy_serv_ip[0] = buffread[4];
                            Globals.proxy_serv_ip[1] = buffread[5];
                            Globals.proxy_serv_ip[2] = buffread[6];
                            Globals.proxy_serv_ip[3] = buffread[7];

                            Globals.proxy_serv_port[0] = buffread[9]; // litle / big endian port ...
                            Globals.proxy_serv_port[1] = buffread[8];

                            // sending answer
                            buffread[0] = 5; //ver sock 5
                            buffread[1] = 0;//sucefull
                            buffread[2] = 0;// reserved (must be 0)
                            buffread[3] = 1;//adres type (1) ipv4
                            buffread[4] = 127;
                            buffread[5] = 0;
                            buffread[6] = 0;
                            buffread[7] = 1;
                            buffread[8] = 0;
                            buffread[9] = 0;
                            //
                            gamedata.Game_ClientSocket.Send(buffread, 10, SocketFlags.None);
                            temp_proxy_var = false;
                        }
                    }
                }// after while (proxy ....) reci protocol packet
                
                //Now start read/send client data
                gamedata.clientthread = new ClientThread(gamedata);
                gamedata.fishflythread = new FishFlyThread(gamedata);
                gamedata.running = true;
                gamedata.clientthread.readthread.Start();
                gamedata.clientthread.sendthread.Start();
                gamedata.fishflythread.FishFlyProc.Start();
            }
            catch
            {
                //Globals.l2net_home.Add_Error("crash: ingame listener manager");
            }
        }
    }
}
