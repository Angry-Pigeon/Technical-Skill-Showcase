using Zenject;
namespace Game.GameLogic.Managers
{
    public class BootStrapperInstaller : MonoInstaller<BootStrapperInstaller>
    {
        public BootStrapper BootStrapper;
        public SaveSystemManager SaveSystemManager;
        public LevelLoadingManager LevelLoadingManager;
        
        public override void InstallBindings()
        {
            Container.Bind<BootStrapper>().FromInstance(BootStrapper).AsSingle();
            Container.Bind<SaveSystemManager>().FromInstance(SaveSystemManager).AsSingle();
            Container.Bind<LevelLoadingManager>().FromInstance(LevelLoadingManager).AsSingle();
        }
    }
}