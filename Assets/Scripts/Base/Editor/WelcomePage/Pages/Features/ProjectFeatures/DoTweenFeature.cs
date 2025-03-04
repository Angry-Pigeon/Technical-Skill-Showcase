using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class DoTweenFeature : ProjectFeatures
    {
        public DoTweenFeature()
        {
            Title = "DoTween";
            Description = "DoTween is a fast, powerful, and flexible animation system for Unity, providing an easy-to-use API for creating smooth animations.\n" +
                "It supports a wide range of animation types and is ideal for both runtime and editor applications.\n\n" +
                "In this project, I leveraged DoTween to animate UI elements in the HoleLevelAndExperienceUI script and other components,\n" +
                "enhancing the user interface with dynamic and responsive visual feedback.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/HoleLogic/HoleLevelAndExperienceUI.cs"),
            };
        }
    }
}