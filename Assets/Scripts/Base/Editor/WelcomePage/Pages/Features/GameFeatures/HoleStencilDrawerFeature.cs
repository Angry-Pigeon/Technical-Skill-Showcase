using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.GameFeatures
{
    public class HoleStencilDrawerFeature : ProjectFeatures
    {
        public HoleStencilDrawerFeature()
        {
            Title = "Hole Stencil Drawer";
            Description = "The Hole Stencil Drawer feature explores different approaches for implementing a dynamic hole system in the game.\n" +
                "Initially, I experimented with a collider-based method that involved cutting the ground mesh and collider into the shape of the hole; however, this approach was very resource-intensive.\n\n" +
                "To overcome these performance issues, I developed a stencil shader solution that is far more efficient and also allowed me to showcase my shader programming skills.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Testing/HoleSystem/Scripts/HoleCreation/RuntimeHoleSubtraction.cs"),
                new ReferenceToClasses("Assets/Shaders/GroundWithHole.shader"),
                new ReferenceToClasses("Assets/Shaders/HoleWriter.shader")
            };
        }
    }
}