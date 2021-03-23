namespace L2Robot
{
    public partial class GameServer
    {
        public static void CleanUp(GameData gamedata)
        {
            if (gamedata.running)
            {
                AddInfo.CleanUp_Char(gamedata);
                //AddInfo.CleanUp_NPC();
                //AddInfo.CleanUp_Item();
            }
        }
    }
}
