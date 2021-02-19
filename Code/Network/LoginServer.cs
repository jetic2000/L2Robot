using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;


#if false
Globals.ig_loginthread = new Thread(new ThreadStart(LoginServer.IG_Login));
Globals.ig_listener = new Thread(new ThreadStart(LoginServer.IG_Listener));
Globals.ig_Gamelistener = new Thread(new ThreadStart(LoginServer.IG_StartGameLinks));
Globals.loginsendthread = new Thread(new ThreadStart(LoginServer.LoginSendThread));
Globals.loginreadthread = new Thread(new ThreadStart(LoginServer.LoginReadThread));


Globals.gameprocessdatathread = new Thread(new ThreadStart(GameServer.ProcessDataThread));

Globals.gamedrawthread = new Thread(new ThreadStart(MapThread.DrawGameThread));

Globals.gamethread = new ServerThread();
Globals.clientthread = new ClientThread();
#endif



namespace L2Robot
{
    class LoginServer
    {
        public static void IG_Init()
        {
            //Init all the parameters
            //Globals.gamedata = new GameData();

            //Only for Aden Server, Lazy
            //Globals.gamedata.Override_Game_IP = "121.51.218.57";
            //Globals.gamedata.Override_Game_Port = 7777;
            //Globals.gamedata.IG_Local_IP = "127.0.0.1";
            //Globals.gamedata.IG_Local_Game_Port = 3000;

            //Create the first client listeners, after one client is connected
            //will need to start another listener to enable multi instance
            //Globals.ig_listener.Start(); //IG_Listener()

            //In loop for new instance
            Thread ig_listener = new Thread(new ThreadStart(IG_Listener));
            ig_listener.Start();

        }

        public static void IG_Listener()
        {
            TcpListener Game_ClientLink;
            GameData g = new GameData();
            string IG_Local_IP = "127.0.0.1";
            int IG_Local_Game_Port = 3000;

            System.Net.IPAddress ipAd = System.Net.IPAddress.Parse(IG_Local_IP);//textBox_ig_local.Text);
            Game_ClientLink = new TcpListener(ipAd, IG_Local_Game_Port);
            Game_ClientLink.Start();

            while(true)
            {
                Console.WriteLine("Waiting a client");
                GameData gamedata = IG_Start_Listen(Game_ClientLink);
                Console.WriteLine("Found a client");
                IG_ProcData(gamedata);
            }
        }

        public static GameData IG_Start_Listen(TcpListener listener)
        {
            string ip = "";
            int port = -1;
            GameData gamedata = new GameData();
            gamedata.Override_Game_IP = "121.51.218.57";
            gamedata.Override_Game_Port = 7777;

            try
            {
                //Only need one instance as all data is from the some port from client by sock5
                bool got_connection = false;
                while (!got_connection)
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

                        Console.WriteLine("IP:{0}, PORT:{1}", ip, port);

                        if (Globals.Games.ContainsKey(port))
                        {
                            Globals.Games[port] = gamedata;
                        }
                        else
                        {
                            Globals.Games.Add(port, gamedata);
                        }

                        got_connection = true;
                    }
                    catch
                    {
                        //TODO: Handle fail things
                    }
                }
            }
            catch
            {
                //TODO: Handle fail things
            }
            return gamedata;
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
                Globals.proxy_serv = true;

                if (Globals.proxy_serv)
                {
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
                }
                
                //Now start read/send client data
                gamedata.clientthread = new ClientThread(gamedata);
                gamedata.clientthread.readthread.Start();
                gamedata.clientthread.sendthread.Start();
                
            }
            catch
            {
                //Globals.l2net_home.Add_Error("crash: ingame listener manager");
            }
        }
    }
}
