using UnityEngine.Serialization;
using Zenject;
namespace Game.SceneDataLogic.Installers
{
    public class SceneMonoInstallers : MonoInstaller
    {
        public SceneData SceneData;

        public override void InstallBindings()
        {
            Container.Bind<SceneData>().FromInstance(SceneData).AsSingle();
        }
    }
}