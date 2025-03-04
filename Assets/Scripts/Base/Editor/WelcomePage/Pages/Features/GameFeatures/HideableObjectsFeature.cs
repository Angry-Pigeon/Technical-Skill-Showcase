using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.GameFeatures
{
    public class HideableObjectsFeature : ProjectFeatures
    {
        public HideableObjectsFeature()
        {
            Title = "Hideable Objects";
            Description = "The Hideable Objects system dynamically manages the visibility of game objects at runtime.\n" +
                "Initially, I attempted to achieve this effect by making objects transparent; however, the visual result was unsatisfactory.\n\n" +
                "To resolve this, I developed a custom system that modifies material properties in real time. Although this approach initially caused issues in the built version due to shader stripping (which I addressed by disabling shader stripping), it effectively delivers the desired effect. With further refinement, a more optimized solution could be implemented.";
            Completed = true;
            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/Camera/CameraObscuringObjectDetector.cs")
            };
        }
    }
}