using System.IO;
using System;
using System.Collections;
using System.Drawing;
using System.Threading;

namespace L2Robot
{
    /// <summary>
    /// Summary description for ClientPackets.
    /// </summary>
    public class ClientPackets
    {
        public ClientPackets()
        {
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

        public static void Send_CancelTarget(GameData gamedata)
        {
            ByteBuffer bbuff = new ByteBuffer(3);

            bbuff.WriteByte((byte)PClient.RequestTargetCanceled);
            bbuff.WriteByte(0x00);
            bbuff.WriteByte(0x00);

            gamedata.SendToGameServer(bbuff);
        }

        public static void MoveToPacket(GameData gamedata, int dx, int dy)
        {
            MoveToPacket(gamedata, dx, dy, Util.Float_Int32(gamedata.my_char.Z));
        }

        public static void MoveToPacket(GameData gamedata, int dx, int dy, int dz)
        {

            //Globals.l2net_home.Add_Text("I'm not attacking something (moving)", Globals.Green, TextType.BOT);
            gamedata.my_char.isAttacking = false; //char is not attacking if moving somewhere

            ByteBuffer bbuff = new ByteBuffer(29);

            bbuff.WriteByte((byte)PClient.MoveBackwardToLocation);
            bbuff.WriteInt32(dx);
            bbuff.WriteInt32(dy);
            bbuff.WriteInt32(dz);
            bbuff.WriteInt32(Util.Float_Int32(gamedata.my_char.X));
            bbuff.WriteInt32(Util.Float_Int32(gamedata.my_char.Y));
            bbuff.WriteInt32(Util.Float_Int32(gamedata.my_char.Z));
            bbuff.WriteInt32(1);//1 - mouse | 0 - keyboard

            gamedata.SendToGameServer(bbuff);
        }

        public static void Send_Restart(GameData gamedata)
        {
            ByteBuffer bbuff = new ByteBuffer(1);
            bbuff.WriteByte((byte)PClient.RequestRestart);

            gamedata.SendToGameServer(bbuff);
        }


        public static void TeleportPacket(GameData gamedata, string dest)
        {
            Console.WriteLine("dest = {0}", dest);

            if (dest == "活动蚂蚁洞窟")
            {
                ByteBuffer bbuff = new ByteBuffer(71);
                bbuff.WriteByte(0x23);
                string cmd = "menu_select?ask=-202006101&reply=1";
                byte[] byteArray = System.Text.Encoding.Unicode.GetBytes(cmd);

                for (int i = 0; i < byteArray.Length; i++)
                {
                    bbuff.WriteByte(byteArray[i]);
                }
                
                bbuff.WriteUInt16(0x0);
                Console.WriteLine("Send To Server CLIENT_PORT: {0}", gamedata.Client_Port);
                gamedata.SendToGameServer(bbuff);
            }
            else
            {
                ByteBuffer bbuff = new ByteBuffer(7);
                bbuff.WriteByte((byte)PClient.EXPacket);
                bbuff.WriteUInt16((UInt16)PClientEX.RequestTeleport);
                //bbuff.WriteByte(0x67);
                //bbuff.WriteByte(0x01);
                /*
                if (dest == "奇岩")
                    bbuff.WriteUInt32(0x19);
                else if (dest == "矿区西部")
                    bbuff.WriteUInt32(0x91);
                else if (dest == "纪念塔")
                    bbuff.WriteUInt32(0x13C);
                else if (dest == "傲慢之塔")
                    bbuff.WriteUInt32(0x62);
                else
                    bbuff.WriteUInt32(0x19);
                */
                bbuff.WriteUInt32(Convert.ToUInt32(dest, 16));
                
                Console.WriteLine("Send To Server CLIENT_PORT: {0}, dest:{1}", gamedata.Client_Port, dest);
                gamedata.SendToGameServer(bbuff);
            }

            //if (dest = )

            //bbuff.WriteByte((byte)PClient.Tel);

            //gamedata.SendToGameServer(bbuff);
        }
    }//end of class
}//end of namespace
