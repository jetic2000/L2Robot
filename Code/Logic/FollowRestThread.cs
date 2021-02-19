using System;
using System.Threading;

namespace L2Robot
{
    public class FollowRestThread
    {
        public Thread followrestthread;


        private static bool breaktotop;



        /*private float target_hp = 0;
        private float target_max_hp = 0;
        private uint NPCID = 0;*/

        public FollowRestThread()
        {
            followrestthread = new Thread(new ThreadStart(FollowRest));

            followrestthread.IsBackground = true;
        }

        private void FollowRest()
        {
        }
    }
}