namespace Events
{
    public static class GameEvents
    {
        public delegate void LoadLevelHandler(int level);
        public static LoadLevelHandler OnLoadLevel;

        public delegate void EndGameHandler();
        public static EndGameHandler OnGameFinish;

        public delegate void GameOverHandler();
        public static GameOverHandler OnGameOver;

        public delegate void NextGameHandler();
        public static NextGameHandler OnNextGame;
    }

    public static class CollectorEvents
    {
        public delegate void SlotClickHandler(Item item);
        public static SlotClickHandler OnSlotCollect;
    }
}
