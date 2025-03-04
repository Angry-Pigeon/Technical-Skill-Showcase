using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.GameFeatures
{
    public class HoleEatableSystemFeature : ProjectFeatures
    {
        public HoleEatableSystemFeature()
        {
            Title = "Eatable Object System";
            
            Description = "Eatable Object System is a dynamic solution designed to simulate objects that can be 'consumed' by a growing hole. " +
                "It begins with basic cube colliders equipped with a rigidbody, and is enhanced by adding four additional rigidbodies at each corner, " +
                "which are then connected to the main rigidbody using fixed joints. This setup creates a realistic physics interaction.\n\n" +
                "In this implementation, when the hole comes into contact with an eatable object, the object falls into the hole in a realistic and satisfying manner. " +
                "A custom script was originally developed to automatically generate and connect the corner rigidbodies; however, it was removed from the final version due to build errors. " +
                "Earlier versions of this functionality are available in the project's git repository.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/EatableObjects"),
                new ReferenceToClasses("Assets/Scripts/Game/EatableObjects/EatableObject.cs"),
                new ReferenceToClasses("Assets/Prefabs/UsableModels/Buildings/Building_0X0.prefab")
            };
        }
    }
}