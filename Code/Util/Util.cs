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