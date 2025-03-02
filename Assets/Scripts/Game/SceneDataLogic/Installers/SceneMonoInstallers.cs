using UnityEngine.Serialization;
using Zenject;
namespace Game.SceneDataLogic.Installers
{
    public class SceneMonoInstallers : MonoInstaller
    {
        [FormerlySerializedAs("SceneData")] public SceneDataContext SceneDataContext;

        public override void InstallBindings()
        {
            
        }
    }
}