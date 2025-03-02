using Zenject;
namespace Game.GameLogic.Managers.GameEvents
{
    public static class Game
    {
        
        private static BootStrapper _bootStrapper;
        
        public static void Initialize(BootStrapper bootStrapper)
        {
            _bootStrapper = bootStrapper;
        }
        
        public static void StartGame()
        {
            _bootStrapper.StartGame();
        }
        
        public static void NextLevel()
        {
            _bootStrapper.NextLevel();
        }
        
        public static void RestartLevel()
        {
            _bootStrapper.RestartLevel();
        }
        
        public static void EndGame(bool win)
        {
            _bootStrapper.EndGame(win);
        }
        
        public static int GetPreviewLevel()
        {
            return _bootStrapper.GetPreviewLevel();
        }
        
        public static BootStrapperContext GetBootStrapperContext()
        {
            return _bootStrapper.GetBootStrapperContext();
        }
    }
}