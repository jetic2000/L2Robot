using System.Collections;
using System.IO;
using System.Net.Sockets;

namespace L2Robot
{
    /// <summary>
    /// Summary description for Util.
    /// </summary>
    public static partial class Util
    {
        static public string Sanitize(string inp)
        {
            inp = inp.Replace("\\", "");
            inp = inp.Replace("\"", "");
            inp = inp.Replace("'", "");
            inp = inp.Replace(".", "");
            inp = inp.Replace("/", "");
            inp = inp.Replace("\n", "");
            inp = inp.Replace(":", "");

            return inp;
        }

        static public uint HexToUInt(string str)
        {
            uint val = 0;
            try
            {
                val = uint.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            catch
            {
                //oops
            }

            return val;
        }


        static public int Distance(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            double xlim = System.Convert.ToDouble(x1 - x2);
            double ylim = System.Convert.ToDouble(y1 - y2);
            double zlim = System.Convert.ToDouble(z1 - z2);

            double dist = System.Math.Sqrt(System.Math.Pow(xlim, 2) + System.Math.Pow(ylim, 2) + System.Math.Pow(zlim, 2));

            return Double_Int32(dist);
        }

        static public int Distance(uint id)
        {
            //this function will lock based on what we are checking distance to
            TargetType type = GetType(id);

            switch (type)
            {
                case TargetType.ERROR:
                case TargetType.NONE:
                    return System.Int32.MaxValue;
                case TargetType.SELF:
                    return 0;
                case TargetType.MYPET:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.MYPET1:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet1.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet1.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet1.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.MYPET2:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet2.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet2.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet2.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.MYPET3:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet3.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet3.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet3.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.PLAYER:
                    Globals.PlayerLock.EnterReadLock();
                    try
                    {
                        CharInfo player = GetChar(id);

                        if (player != null)
                        {
                            return (int)System.Math.Sqrt(
                                System.Math.Pow(player.X - Globals.gamedata.my_char.X, 2) +
                                System.Math.Pow(player.Y - Globals.gamedata.my_char.Y, 2) +
                                System.Math.Pow(player.Z - Globals.gamedata.my_char.Z, 2));
                        }
                    }//unlock
                    finally
                    {
                        Globals.PlayerLock.ExitReadLock();
                    }
                    break;
                case TargetType.NPC:
                    Globals.NPCLock.EnterReadLock();
                    try
                    {
                        NPCInfo npc = GetNPC(id);

                        if (npc != null)
                        {
                            return (int)System.Math.Sqrt(
                                System.Math.Pow(npc.X - Globals.gamedata.my_char.X, 2) +
                                System.Math.Pow(npc.Y - Globals.gamedata.my_char.Y, 2) +
                                System.Math.Pow(npc.Z - Globals.gamedata.my_char.Z, 2));
                        }
                    }//unlock
                    finally
                    {
                        Globals.NPCLock.ExitReadLock();
                    }
                    break;
                case TargetType.ITEM:
                    Globals.ItemLock.EnterReadLock();
                    try
                    {
                        ItemInfo item = GetItem(id);

                        if (item != null)
                        {
                            return (int)System.Math.Sqrt(
                                System.Math.Pow(item.X - Globals.gamedata.my_char.X, 2) +
                                System.Math.Pow(item.Y - Globals.gamedata.my_char.Y, 2) +
                                System.Math.Pow(item.Z - Globals.gamedata.my_char.Z, 2));
                        }
                    }//unlock
                    finally
                    {
                        Globals.ItemLock.ExitReadLock();
                    }
                    break;
            }

            return System.Int32.MaxValue;
        }


        static public int Distance(uint id, TargetType type)
        {
            //no locks needed... since the calling function has the locks in it
            switch (type)
            {
                case TargetType.ERROR:
                case TargetType.NONE:
                    return System.Int32.MaxValue;
                case TargetType.SELF:
                    return 0;
                case TargetType.MYPET:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.MYPET1:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet1.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet1.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet1.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.MYPET2:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet2.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet2.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet2.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.MYPET3:
                    return (int)System.Math.Sqrt(
                        System.Math.Pow(Globals.gamedata.my_pet3.X - Globals.gamedata.my_char.X, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet3.Y - Globals.gamedata.my_char.Y, 2) +
                        System.Math.Pow(Globals.gamedata.my_pet3.Z - Globals.gamedata.my_char.Z, 2));
                case TargetType.PLAYER:
                    CharInfo player = GetChar(id);

                    if (player != null)
                    {
                        return (int)System.Math.Sqrt(
                            System.Math.Pow(player.X - Globals.gamedata.my_char.X, 2) +
                            System.Math.Pow(player.Y - Globals.gamedata.my_char.Y, 2) +
                            System.Math.Pow(player.Z - Globals.gamedata.my_char.Z, 2));
                    }
                    break;
                case TargetType.NPC:
                    NPCInfo npc = GetNPC(id);

                    if (npc != null)
                    {
                        return (int)System.Math.Sqrt(
                            System.Math.Pow(npc.X - Globals.gamedata.my_char.X, 2) +
                            System.Math.Pow(npc.Y - Globals.gamedata.my_char.Y, 2) +
                            System.Math.Pow(npc.Z - Globals.gamedata.my_char.Z, 2));
                    }
                    break;
                case TargetType.ITEM:
                    ItemInfo item = GetItem(id);

                    if (item != null)
                    {
                        return (int)System.Math.Sqrt(
                            System.Math.Pow(item.X - Globals.gamedata.my_char.X, 2) +
                            System.Math.Pow(item.Y - Globals.gamedata.my_char.Y, 2) +
                            System.Math.Pow(item.Z - Globals.gamedata.my_char.Z, 2));
                    }
                    break;
            }

            return System.Int32.MaxValue;
        }
        public static void Read_String(ref string source, ref string outs)
        {
            int pipe = source.IndexOf("\r\n");
            if (pipe == -1)
            {
                outs = source;
                source = "";
            }
            else
            {
                outs = source.Substring(0, pipe);
                source = source.Remove(0, pipe + 2);
            }
        }

        public static ArrayList GetArray(string inp)
        {
            ArrayList val = new ArrayList();

            int pipe;

            while (inp.Length > 0)
            {
                pipe = inp.IndexOf(';');
                if (pipe == -1)
                {
                    val.Add(inp);
                    inp = "";
                }
                else
                {
                    val.Add(inp.Substring(0, pipe));
                }
                inp = inp.Remove(0, pipe + 1);
            }

            return val;
        }

        public static int MAX(int a, int b)
        {
            if (a > b)
            {
                return a;
            }

            return b;
        }

        public static double MAX(double a, double b)
        {
            if (a > b)
            {
                return a;
            }

            return b;
        }

        public static float MAX(float a, float b)
        {
            if (a > b)
            {
                return a;
            }

            return b;
        }

        public static int MIN(int a, int b)
        {
            if (a < b)
            {
                return a;
            }

            return b;
        }

        public static double MIN(double a, double b)
        {
            if (a < b)
            {
                return a;
            }

            return b;
        }

        public static float MIN(float a, float b)
        {
            if (a < b)
            {
                return a;
            }

            return b;
        }

        public static string MD5(string input)
        {
            // step 1, calculate MD5 hash from input
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static bool Between(this int value, int left, int right)
        {
            return value > left && value < right;
        }

        public static int RandomNumber(int min, int max)
        {
            System.Random random = new System.Random();
            return random.Next(min, max);
        }
    }
}