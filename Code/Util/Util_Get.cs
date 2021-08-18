namespace L2Robot
{
    public static partial class Util
    {

        static public void GetLoc(GameData gamedata, uint id, ref int x, ref int y, ref int z)
        {
            //is self?
            if (gamedata.my_char.ID == id)
            {
                x = Float_Int32(gamedata.my_char.Current_Pos.X);
                y = Float_Int32(gamedata.my_char.Current_Pos.Y);
                z = Float_Int32(gamedata.my_char.Current_Pos.Z);
                return;
            }

            //is in the list of chars?
            Globals.PlayerLock.EnterReadLock();
            try
            {
                CharInfo player = GetChar(gamedata, id);

                if (player != null)
                {
                    x = Float_Int32(player.Current_Pos.X);
                    y = Float_Int32(player.Current_Pos.Y);
                    z = Float_Int32(player.Current_Pos.Z);
                    return;
                }
            }
            finally
            {
                Globals.PlayerLock.ExitReadLock();
            }
        }

        public static TargetType GetType(GameData gamedata, uint id)
        {
            if (id == 0)
            {
                return TargetType.NONE;
            }
            //Globals.MyselfLock.EnterReadLock();
            if (gamedata.my_char.ID == id)
            {
                return TargetType.SELF;
            }
            //Globals.MyselfLock.ExitReadLock();

            Globals.PlayerLock.EnterReadLock();
            try
            {
                if (isChar(gamedata, id))
                {
                    return TargetType.PLAYER;
                }
            }
            finally
            {
                Globals.PlayerLock.ExitReadLock();
            }
            return TargetType.ERROR;
        }

        public static bool isChar(GameData gamedata, uint id)
        {

            if (gamedata.nearby_chars.ContainsKey(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static CharInfo GetChar(GameData gamedata, uint id)
        {
            try
            {
                return (CharInfo)gamedata.nearby_chars[id];
            }
            catch
            {
            }
            return null;
        }

        public static CharInfo GetChar(GameData gamedata, string str)
        {
            str = str.ToUpperInvariant();

            foreach (CharInfo player in gamedata.nearby_chars.Values)
            {
                if (System.String.Equals(player.Name.ToUpperInvariant(), str))
                {
                    return player;
                }
            }

            return null;
        }
    }
}
