using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.GameFeatures
{
    public class LevelLoadingSystemFeature : ProjectFeatures
    {
        public LevelLoadingSystemFeature()
        {
            Title = "Level Loading System";
            Description = "This Level Loading System offers a unique approach compared to traditional scene-based loading methods.\n" +
                "By implementing levels as prefabs, it provides enhanced control over level management and reduces performance overhead.\n\n" +
                "In this project, the system maintains a persistent scene and dynamically loads level prefabs, thereby minimizing unnecessary operations.\n" +
                "A planned feature includes an in-editor level editing tool that would allow changes to be saved directly back to the prefab, although this functionality is slated for future updates.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/Managers/LevelLoading/LevelLoadingManager.cs"),
                new ReferenceToClasses("Assets/Resources/SingletonDataSO/LevelSettingsDatabase.asset"),
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/Managers/LevelLoading/LevelSettingsDatabase.cs"),
                new ReferenceToClasses("Assets/Prefabs/Levels/Level_Default.prefab")
            };
        }
    }
}