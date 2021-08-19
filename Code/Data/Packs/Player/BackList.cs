using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace L2Robot
{
    /// <summary>
    /// Summary description for Char_Info.
    /// </summary>
    public class BlackListPlayers : Object_Base
    {
        private List<String> BlackList = new List<String>();
        private ReaderWriterLockSlim BlackListLock = new ReaderWriterLockSlim();

        public List<String> UpdataBlackList(string cfg)
        {
            BlackListLock.EnterWriteLock();
            try
            {
                BlackList.Clear();
                //Console.WriteLine("Read Cfg File:{0}", cfg);
                using (StreamReader sr = new StreamReader(cfg))
                {
                    string line;
                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        BlackList.Add(line);
                    }
                }
            }
            catch
            {
                Console.WriteLine("没有配置文件");
            }
            finally
            {
                BlackListLock.ExitWriteLock();
            }

            return GetBlackList();
            
        }
        public List<String> GetBlackList()
        {
            BlackListLock.EnterWriteLock();
            List<String> tmp = new List<String>();
            foreach(string i in BlackList)
            {
                tmp.Add(i);
            }
            BlackListLock.ExitWriteLock();
            //Console.WriteLine(tmp.Count);
            return tmp;
        }

        public void ClearBlackList()
        {
            BlackListLock.EnterWriteLock();
            BlackList.Clear();
            BlackListLock.ExitWriteLock();
        }
    }//end of class
}
