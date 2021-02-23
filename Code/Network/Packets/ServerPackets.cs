using System;
using System.Threading;

namespace L2Robot
{
    /// <summary>
    /// Summary description for GamePackets.
    /// </summary>
    static public class ServerPackets
    {
        public static void StatusUpdate(GameData gamedata, ByteBuffer DataBuffer)
        {
            uint EntityID = DataBuffer.ReadUInt32();

        }

        public static void EXUserInfo(GameData gamedata, ByteBuffer buffe)
        {
            int loc = buffe.GetIndex();
            uint ID = buffe.ReadUInt32();//A0 B9 B0 49
            Console.WriteLine("User ID: 0x{0:x}", ID);
            buffe.ReadInt32();
            buffe.ReadInt32();
            buffe.ReadInt32();
            buffe.ReadInt32();
            buffe.ReadByte();
            string name = ""; //= buffe.ReadString();

            buffe.SetIndex(loc);

            //this packet only gets sent once
            if (name == gamedata.my_char.Name)
            {

            }
            else
            {

            }
        }

        public static void NetPing(GameData gamedata, ByteBuffer buffe)
        {

            byte cID_0 = buffe.ReadByte();
            byte cID_1 = buffe.ReadByte();
            byte cID_2 = buffe.ReadByte();
            byte cID_3 = buffe.ReadByte();

            ByteBuffer breply = new ByteBuffer(13);

            breply.WriteByte((byte)PClient.NetPingReply);

            breply.WriteByte(cID_0);
            breply.WriteByte(cID_1);
            breply.WriteByte(cID_2);
            breply.WriteByte(cID_3);

            breply.WriteInt32(Globals.Rando.Next(5, 15));
            breply.WriteInt32(0x660E);
            gamedata.SendToGameServer(breply);
        }

    }//end of class
}
