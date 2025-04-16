using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class SerializedResourcedScriptableObjectFeature : ProjectFeatures
    {
        public SerializedResourcedScriptableObjectFeature()
        {
            Title = "Serialized Resourced Scriptable Object";
            Description = "Serialized Resourced Scriptable Object is an evolution of Unity's ScriptableSingleton concept.\n" +
                "Originally designed for editor-only use, this system has been reimagined to function seamlessly in both runtime and editor contexts,\n" +
                "automatically adjusting its behavior based on the current environment.\n\n" +
                "I developed this system years ago and have since integrated it into nearly every project, ensuring efficient data management\n" +
                "and consistency across diverse workflows.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Base/Runtime/SerializedResourcedScriptableObject/SerializedResourcedScriptableObject.cs"),
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/Managers/LevelLoading/LevelSettingsDatabase.cs"),
                new ReferenceToClasses("Assets/Resources/SingletonDataSO/LevelSettingsDatabase.asset"),
            };
        }
    }
}