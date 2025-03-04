using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class OdinInspectorFeature : ProjectFeatures
    {
        public OdinInspectorFeature()
        {
            Title = "Odin Inspector";
            Description = "Odin Inspector is a powerful Unity plugin that transforms the editor into a highly customizable, user-friendly environment.\n" +
                "It offers a comprehensive suite of tools and attributes to create robust editor workflows without writing any custom editor code.\n\n" +
                "In this project, I leveraged Odin Inspector to streamline the development of custom editor windows and feature drawers, " +
                "enhancing workflow efficiency and maintainability.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Base/Editor/WelcomePage"),
                new ReferenceToClasses("Assets/Scripts/Base/Editor/WelcomePage/Pages/Features"),
                new ReferenceToClasses("Assets/Scripts/Base/Editor/WelcomePage/TechnicalShowcaseOdinWindow.cs"),
                new ReferenceToClasses("Assets/Scripts/Base/Editor/WelcomePage/Pages/FirstPageDrawer.cs"),
                new ReferenceToClasses("Assets/Scripts/Base/Editor/WelcomePage/Pages/Features/Drawers/GenericFeaturesDrawer.cs"),
            };
        }
    }
}