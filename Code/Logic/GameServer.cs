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
            //set up all the arraylists for data
            //flush and clear all the shortcuts
            /*
            for (int i = 0; i < Globals.Skills_Pages * Globals.Skills_PerPage; i++)
            {
                Globals.gamedata.ShortCuts.Add(new ShortCut());
            }
            */

            //Globals.gamedata.botoptions = new BotOptions();

            //Util.Setup_Threads();

            //process command line crap
        }

        public void ProcessDataThread()
        {
            //this.gamedata.my_char.lastVerifyTime = DateTime.Now;
            DateTime last_animate = DateTime.Now;
            DateTime last_alert = DateTime.Now;
            DateTime last_clean = DateTime.Now;
            //DateTime last_draw = DateTime.Now;

            //Globals.l2net_home.timer_mybuffs.Start();

            //Console.WriteLine("ProcessDataThread start");

            try
            {
                while (true)
                {
                    //Util.PopUp_Check();

                    HandlePackets();

                    System.Threading.Thread.Sleep(Globals.SLEEP_ProcessDataThread);//sleep for 10ms; when we get new data it should wake us up

#if false
                    if ((DateTime.Now - last_animate).Ticks > Globals.SLEEP_Animate)
                    {
                        last_animate = DateTime.Now;
                    }

                    if ((DateTime.Now - last_alert).Ticks > Globals.SLEEP_Sound_Alerts)
                    {
                        last_alert = DateTime.Now;
                    }

                    if ((DateTime.Now - last_clean).Ticks > Globals.CLEAN_TIMER)
                    {
                        //CleanUp();
                        last_clean = DateTime.Now;
                    }
#endif
                }//end of while running
            }
            catch (Exception e)
            {
                //Globals.l2net_home.Add_Error("crash: ProcessDataThread : hardcore crash... every bot function is DOA now : " + e.Message);
            }
        }
    }//end of class
}
