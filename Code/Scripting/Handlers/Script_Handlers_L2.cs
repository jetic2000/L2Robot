using System;
using System.Collections;

namespace L2Robot
{
    public partial class ScriptEngine
    {
        private void Script_GENERATE_POLY(string line)
        {
            double radius = Util.GetDouble(Get_String(ref line));
            int sides = Util.GetInt32(Get_String(ref line));
            double offset = Util.GetDouble(Get_String(ref line));

            if (sides < 3)
            {
                return;
            }

            this.gamedata.Paths.PointList.Clear();

            offset = offset - 180 / sides + 90;

            for (int i = 0; i < sides; i++)
            {
                double degrees = (360 / sides * i + offset) * Math.PI / 180;
                int x = (int)(radius * Math.Cos(degrees)) + Util.Float_Int32(this.gamedata.my_char.X);
                int y = (int)(radius * Math.Sin(degrees)) + Util.Float_Int32(this.gamedata.my_char.Y);

                Point p = new Point(x, y);
                this.gamedata.Paths.PointList.Add(p);
            }
        }

        private void Script_GET_ZONE(string line)
        {
            string svar = Get_String(ref line);

            ScriptVariable var = Get_Var(svar);

            var.Value = this.gamedata.cur_zone;
        }

        private void Script_BLOCK_CLIENT(string line)
        {
            int packet_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ClientPackets.ContainsKey(packet_id))
            {
            }
            else
            {
                Blocked_ClientPackets.Add(packet_id, 0);
            }
        }

        private void Script_BLOCKEX_CLIENT(string line)
        {
            int packetex_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ClientPacketsEX.ContainsKey(packetex_id))
            {
            }
            else
            {
                Blocked_ClientPacketsEX.Add(packetex_id, 0);
            }
        }

        private void Script_UNBLOCK_CLIENT(string line)
        {
            int packet_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ClientPackets.ContainsKey(packet_id))
            {
                Blocked_ClientPackets.Remove(packet_id);
            }
        }

        private void Script_UNBLOCKEX_CLIENT(string line)
        {
            int packetex_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ClientPacketsEX.ContainsKey(packetex_id))
            {
                Blocked_ClientPacketsEX.Remove(packetex_id);
            }
        }

        private void Script_CLEAR_BLOCK_CLIENT()
        {
            Blocked_ClientPackets.Clear();
        }

        private void Script_CLEAR_BLOCKEX_CLIENT()
        {
            Blocked_ClientPacketsEX.Clear();
        }

        private void Script_BLOCK_SELF(string line)
        {
            int packet_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_SelfPackets.ContainsKey(packet_id))
            {
            }
            else
            {
                Blocked_SelfPackets.Add(packet_id, 0);
            }
        }

        private void Script_BLOCK_SELF_ALL()
        {
            int opcode = 0;
            PClient[] values = (PClient[])Enum.GetValues(typeof(PClient));
            for (int i = 0; i < values.Length; i++)
            {
                opcode = (int)values[i];
                if (Blocked_SelfPackets.ContainsKey(opcode))
                {
                }
                else
                {
                    //Globals.l2net_home.Add_Debug(opcode + " added to blocklist", Globals.Green);
                    Blocked_SelfPackets.Add(opcode, 0);
                }
            }

        }

        private void Script_BLOCKEX_SELF(string line)
        {
            int packetex_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_SelfPacketsEX.ContainsKey(packetex_id))
            {
            }
            else
            {
                Blocked_SelfPacketsEX.Add(packetex_id, 0);
            }
        }

        private void Script_BLOCKEX_SELF_ALL()
        {
            int opcode = 0;
            PClient[] values = (PClient[])Enum.GetValues(typeof(PClient));
            for (int i = 0; i < values.Length; i++)
            {
                opcode = (int)values[i];
                if (Blocked_SelfPacketsEX.ContainsKey(opcode))
                {
                }
                else
                {
                    //Globals.l2net_home.Add_Debug(opcode + " added to blocklist", Globals.Green);
                    Blocked_SelfPacketsEX.Add(opcode, 0);
                }
            }

        }

        private void Script_UNBLOCK_SELF(string line)
        {
            int packet_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_SelfPackets.ContainsKey(packet_id))
            {
                Blocked_SelfPackets.Remove(packet_id);
            }
        }

        private void Script_UNBLOCKEX_SELF(string line)
        {
            int packetex_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_SelfPacketsEX.ContainsKey(packetex_id))
            {
                Blocked_SelfPacketsEX.Remove(packetex_id);
            }
        }

        private void Script_CLEAR_BLOCK_SELF()
        {
            Blocked_SelfPackets.Clear();
        }

        private void Script_CLEAR_BLOCKEX_SELF()
        {
            Blocked_SelfPacketsEX.Clear();
        }

        private void Script_BLOCK(string line)
        {
            int packet_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ServerPackets.ContainsKey(packet_id))
            {
            }
            else
            {
                Blocked_ServerPackets.Add(packet_id, 0);
            }
        }

        private void Script_BLOCKEX(string line)
        {
            int packetex_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ServerPacketsEX.ContainsKey(packetex_id))
            {
            }
            else
            {
                Blocked_ServerPacketsEX.Add(packetex_id, 0);
            }
        }

        private void Script_UNBLOCK(string line)
        {
            int packet_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ServerPackets.ContainsKey(packet_id))
            {
                Blocked_ServerPackets.Remove(packet_id);
            }
        }

        private void Script_UNBLOCKEX(string line)
        {
            int packetex_id = Util.GetInt32(Get_String(ref line));

            if (Blocked_ServerPacketsEX.ContainsKey(packetex_id))
            {
                Blocked_ServerPacketsEX.Remove(packetex_id);
            }
        }

        private void Script_CLEAR_BLOCK()
        {
            Blocked_ServerPackets.Clear();
        }

        private void Script_CLEAR_BLOCKEX()
        {
            Blocked_ServerPacketsEX.Clear();
        }

        private void Script_TARGET_SELF()
        {
            //ServerPackets.Target(this.gamedata.my_char.ID, (int)this.gamedata.my_char.X, (int)this.gamedata.my_char.Y, (int)this.gamedata.my_char.Z, true);
        }

        private void Script_CANCEL_TARGET()
        {
            //ServerPackets.Send_CancelTarget();
        }

        private void Script_SLEEP_HUMAN_READING(string inp)
        {
            string text = Get_String(ref inp);

            float words = text.Length / Globals.Average_Word_Length;

            float speed = Convert.ToSingle(Globals.Rando.NextDouble()) + 3.3F;

            float time = words / speed * 1000.0F;

            ((ScriptThread)Threads[CurrentThread]).Sleep_Until = DateTime.Now.AddMilliseconds(time);
        }

        private void Script_SLEEP_HUMAN_WRITING(string inp)
        {
            string text = Get_String(ref inp);

            //gotta find the relative difference between the letters

            //fast = 7 char per second
            //slow = 2.5 char per second

            //best difference = 0

            text = text.ToLower();

            float diff = 0;

            for (int i = 0; i < text.Length - 1; i++)
            {
                diff += Math.Abs(ELIZA.Get_Complexity(text[i].ToString()) - ELIZA.Get_Complexity(text[i + 1].ToString()));
            }

            float keys = diff / Globals.Difficulty_Balance;

            float speed = Convert.ToSingle(Globals.Rando.NextDouble()) + 3.0F;

            float time = keys / speed * 1000;

            ((ScriptThread)Threads[CurrentThread]).Sleep_Until = DateTime.Now.AddMilliseconds(time);
        }

        private void Script_GET_ELIZA(string inp)
        {
            string sdest = Get_String(ref inp);
            ScriptVariable dest = Get_Var(sdest);

            if (dest.Type != Var_Types.STRING)
            {
                Script_Error("INVLAID DESTINATION TYPE");
                return;
            }

            string query = Get_String(ref inp);
            string reply = ELIZA.GetReply(query);

            dest.Value = reply;
        }

        private void Script_TELEPORT(string inp)
        {
            
            string sdest = Get_String(ref inp);
            //ScriptVariable dest = Get_Var(sdest);
            Console.WriteLine("CMD: {0}, VAR: {1}", "Script_TELEPORT()", sdest);
            ClientPackets.TeleportPacket(this.gamedata, sdest);
        }

        private void Script_MOVE_TO(string line)
        {
            string sx = Get_String(ref line);
            string sy = Get_String(ref line);
            string sz = Get_String(ref line);

            //MOVE_TO 10 10 10
            int x = Util.GetInt32(sx);
            int y = Util.GetInt32(sy);
            int z = Util.GetInt32(sz);

            if (Globals.Script_Debugging)
            {
                //Globals.l2net_home.Add_Debug("MOVE_TO " + x.ToString() + "," + y.ToString() + "," + z.ToString());
            }

            ClientPackets.MoveToPacket(this.gamedata, x, y, z);
        }
        
        private void Script_RESTART()
        {
            ClientPackets.Send_Restart(this.gamedata);
        }

        private void Script_GET_NPCS(string inp)
        {
            string sdest = Get_String(ref inp);
            ScriptVariable dest = Get_Var(sdest);

            if (dest.Type == Var_Types.ARRAYLIST)
            {
                ((ArrayList)dest.Value).Clear();
            }
            else if (dest.Type == Var_Types.SORTEDLIST)
            {
                ((SortedList)dest.Value).Clear();
            }
            else
            {
                Script_Error("INVLAID DESTINATION TYPE");
                return;
            }

            Globals.NPCLock.EnterReadLock();
            try
            {
                foreach (NPCInfo npc in this.gamedata.nearby_npcs.Values)
                {
                    Script_ClassData cd = new Script_ClassData();
                    cd.Name = "NPC";
                    cd._Variables.Add("ID", new ScriptVariable((long)npc.ID, "ID", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("X", new ScriptVariable((long)npc.X, "X", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("Y", new ScriptVariable((long)npc.Y, "Y", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("Z", new ScriptVariable((long)npc.Z, "Z", Var_Types.INT, Var_State.PUBLIC));
                    //cd._Variables.Add("NAME", new ScriptVariable(Util.GetNPCName(npc.NPCID), "NAME", Var_Types.STRING, Var_State.PUBLIC));

                    cd._Variables.Add("NPC_ID", new ScriptVariable((long)npc.NPCID, "NPC_ID", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("TITLE", new ScriptVariable(npc.Title, "TITLE", Var_Types.STRING, Var_State.PUBLIC));
                    cd._Variables.Add("ATTACKABLE", new ScriptVariable((long)npc.isAttackable, "ATTACKABLE", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("LEVEL", new ScriptVariable((long)npc.Level, "LEVEL", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("HP", new ScriptVariable((long)npc.Cur_HP, "HP", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("MAX_HP", new ScriptVariable((long)npc.Max_HP, "MAX_HP", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("MP", new ScriptVariable((long)npc.Cur_MP, "MP", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("MAX_MP", new ScriptVariable((long)npc.Max_MP, "MAX_MP", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("CP", new ScriptVariable((long)npc.Cur_CP, "CP", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("MAX_CP", new ScriptVariable((long)npc.Max_CP, "MAX_CP", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("PER_HP", new ScriptVariable(Math.Round(npc.Cur_HP / (double)npc.Max_HP * 100, 2), "PER_HP", Var_Types.DOUBLE, Var_State.PUBLIC));
                    cd._Variables.Add("PER_MP", new ScriptVariable(Math.Round(npc.Cur_MP / (double)npc.Max_MP * 100, 2), "PER_MP", Var_Types.DOUBLE, Var_State.PUBLIC));
                    cd._Variables.Add("PER_CP", new ScriptVariable(Math.Round(npc.Cur_CP / (double)npc.Max_CP * 100, 2), "PER_CP", Var_Types.DOUBLE, Var_State.PUBLIC));

                    cd._Variables.Add("KARMA", new ScriptVariable((long)npc.Karma, "KARMA", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("ATTACK_SPEED", new ScriptVariable((long)(npc.PatkSpeed * npc.AttackSpeedMult), "ATTACK_SPEED", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("CAST_SPEED", new ScriptVariable((long)npc.PatkSpeed, "CAST_SPEED", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("RUN_SPEED", new ScriptVariable((long)(npc.RunSpeed * npc.MoveSpeedMult), "RUN_SPEED", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("TARGET_ID", new ScriptVariable((long)npc.TargetID, "TARGET_ID", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("FOLLOW_TARGET_ID", new ScriptVariable((long)npc.MoveTarget, "FOLLOW_TARGET_ID", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("DEST_X", new ScriptVariable((long)npc.Dest_X, "DEST_X", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("DEST_Y", new ScriptVariable((long)npc.Dest_Y, "DEST_Y", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("DEST_Z", new ScriptVariable((long)npc.Dest_Z, "DEST_Z", Var_Types.INT, Var_State.PUBLIC));

                    cd._Variables.Add("LOOKS_DEAD", new ScriptVariable((long)npc.isAlikeDead, "LOOKS_DEAD", Var_Types.INT, Var_State.PUBLIC));
                    cd._Variables.Add("IN_COMBAT", new ScriptVariable((long)npc.isInCombat, "IN_COMBAT", Var_Types.INT, Var_State.PUBLIC));

                    ScriptVariable sv = new ScriptVariable(cd, "NPC", Var_Types.CLASS, Var_State.PUBLIC);

                    if (dest.Type == Var_Types.ARRAYLIST)
                    {
                        ((ArrayList)dest.Value).Add(sv);
                    }
                    else if (dest.Type == Var_Types.SORTEDLIST)
                    {
                        ((SortedList)dest.Value).Add(npc.ID.ToString(), sv);
                    }
                }
            }
            finally
            {
                Globals.NPCLock.ExitReadLock();
            }
        }
 
    }
}
