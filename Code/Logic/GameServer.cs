using System;
using System.IO;
using System.Threading;

namespace L2Robot
{
    public partial class GameServer
    {
        public Thread processthread;
        GameData gamedata;

        public GameServer(GameData gamedata)
        {
            this.gamedata = gamedata;
            Init();
        }

        public void Init()
        {

            Globals.PATH = Environment.CurrentDirectory;
            processthread = new Thread(new ThreadStart(ProcessDataThread));
            processthread.IsBackground = true;
        }

        public void ProcessDataThread()
        {
            DateTime last_animate = DateTime.Now;
            DateTime last_alert = DateTime.Now;
            DateTime last_clean = DateTime.Now;

            try
            {
                while (true)
                {
                    HandlePackets();

                    System.Threading.Thread.Sleep(Globals.SLEEP_ProcessDataThread);//sleep for 10ms; when we get new data it should wake us up

                    if ((DateTime.Now - last_clean).Ticks > Globals.CLEAN_TIMER)
                    {
                        CleanUp(this.gamedata);
                        last_clean = DateTime.Now;
                    }

                }//end of while running
            }
            catch (Exception e)
            {
                //Globals.l2net_home.Add_Error("crash: ProcessDataThread : hardcore crash... every bot function is DOA now : " + e.Message);
            }
        }
    }//end of class
}
