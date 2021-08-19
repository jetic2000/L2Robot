using System.Collections;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace L2Robot
{
    /// <summary>
    /// Summary description for AddInfo.
    /// </summary>
    public class FishFlyThread
    {
        public GameData gamedata;
        public Thread FishFlyProc;
        public FishFlyPlayer player;
        public List<string> BlackList;
        private readonly object bGetDataLock = new object();
        private readonly object bRunningLock = new object();
        private bool _bGetData = false;
        private bool _bRunning = true;
        public bool bRunning
        {
            get
            {
                bool tmp;
                lock (bRunningLock)
                {
                    tmp = this._bRunning;
                }
                return tmp;
            }
            set
            {
                lock (bRunningLock)
                {
                    _bRunning = value;
                }
            }
        }
        public bool bGetData
        {
            get
            {
                bool tmp;
                lock (bGetDataLock)
                {
                    tmp = this._bGetData;
                }
                return tmp;
            }
            set
            {
                lock (bGetDataLock)
                {
                    _bGetData = value;
                }
            }
        }

        public FishFlyThread(GameData gamedata)
        {
            this.gamedata = gamedata;
            Init();
        }

        public void Init()
        {
            //FishFlyProc = new Thread(new ThreadStart(FishFlyHandler));
            //gamedata. = new ClientThread(gamedata);
            //FishFlyPlayer
            FishFlyProc = new Thread(new ThreadStart(FishFlyHandler));
            FishFlyProc.IsBackground = true;
            FishFlyProc.Priority = ThreadPriority.Highest;
        }

        private void FishFlyHandler()
        {
            //bRunning = true;
            int count = 0;
            try
            {
                while (this.gamedata.running)
                {
                    if(bRunning)
                    {
                        //Console.WriteLine("+++++++ Do FishFly Check");
                        BlackList = gamedata.blacklist_chars.GetBlackList();
                        count = gamedata.GetCount_DataToFlyFish();
                        if (count > 0)
                        {
                            try
                            {
                                Globals.FishFlyPlayerQueueLock.EnterWriteLock();
                                for (int i = 0; i < count; i++)
                                {
                                    player = (FishFlyPlayer)gamedata.fishflyplayerqueue.Dequeue();
                                    if ((bGetData == true) && (gamedata.my_char.isSafeZone == false))
                                    {
                                        //Console.WriteLine("+++++++ Do FishFly Check");
                                        if (BlackList.Contains(player.PlayerName))
                                        {
                                            Console.WriteLine("Found Player in BlackList {0}", player.PlayerName);
                                            {
                                                PlayerMsg msg = new PlayerMsg();
                                                msg.PlayerName = gamedata.my_char.Name;
                                                msg.PlayerID = gamedata.my_char.ID;
                                                msg.isForRel = true;
                                                msg.msg = String.Format("[警报 {0}] 发现黑名单玩家:{1})", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"), player.PlayerName);
                                                //Console.WriteLine(h);
                                                Globals.l2net_home.UpdateLog(msg);
                                            }

                                            //bGetData = false;
                                            BackgroundKey key = new BackgroundKey(gamedata, gamedata.processPid, 0);
                                            key.PressKey();
                                            Thread.Sleep(100);
                                            //key.PressKey();
                                            //Handle Bad Player
                                            //Clear Queue
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                            finally
                            {
                                Globals.FishFlyPlayerQueueLock.ExitWriteLock();
                            }
                        }
                    }
                    Thread.Sleep(Globals.SLEEP_FishFLyThread);
                }
            }
            catch
            {

            }
        }
    }

    public class BackgroundKey
    {
        private int hwnd;
        private int key;
        public GameData gamedata;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, uint wParam, uint lParam);


        public BackgroundKey(GameData gamedata, int hwnd, int key)
        {
            this.gamedata = gamedata;
            this.hwnd = hwnd;
            this.key = key;
        }

        public BackgroundKey(int hwnd, int key)
        {
            this.hwnd = hwnd;
            this.key = key;
        }

/*
        public BackgroundKey(int hwnd, int key)
        {
            //this.gamedata = gamedata;
            this.hwnd = hwnd;
            this.key = key;
        }
*/        
        public void PressKey()
        {
            const int VK_F12 = 0x7B;
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int VK_NUMPAD1 = 0x61;
            const int VK_NUMPAD2 = 0x62;
            const int VK_NUMPAD3 = 0x63;
            const int VK_NUMPAD4 = 0x64;



            //for (int i = 0; i < 3; i++)
            {
                if (hwnd != 0)
                {
                    /*
                    SendMessage(hwnd, WM_KEYDOWN, VK_NUMPAD1, 0);
                    SendMessage(hwnd, WM_KEYUP, VK_NUMPAD1, 0);

                    SendMessage(hwnd, WM_KEYDOWN, VK_NUMPAD2, 0);
                    SendMessage(hwnd, WM_KEYUP, VK_NUMPAD2, 0);

                    SendMessage(hwnd, WM_KEYDOWN, VK_NUMPAD3, 0);
                    SendMessage(hwnd, WM_KEYUP, VK_NUMPAD3, 0);

                    SendMessage(hwnd, WM_KEYDOWN, VK_NUMPAD4, 0);
                    SendMessage(hwnd, WM_KEYUP, VK_NUMPAD4, 0);
                    */

                    SendMessage(hwnd, WM_KEYDOWN, VK_F12, 0);
                    SendMessage(hwnd, WM_KEYUP, VK_F12, 0);

                    Console.WriteLine("Key Pressed");
                }
            }
        }
    }

}//end of namespace