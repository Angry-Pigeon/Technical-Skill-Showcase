using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class ZenjectFeature : ProjectFeatures
    {
        public ZenjectFeature()
        {
            Title = "ZenjectFeature";
            Description = "Zenject is a lightweight dependency injection framework for Unity, originally inspired by the StrangeIoC framework.\n" +
                "It provides an efficient, modular approach to managing dependencies, supporting advanced binding and installation patterns.\n" +
                "Zenject helps decouple components and improves testability and scalability in Unity projects.\n\n" +
                "In this project, I have integrated Zenject for basic dependency injection within the BootStrapper module.\n" +
                "This implementation demonstrates a practical use of Zenject for cleaner, more maintainable code.\n" +
                "Further refinement would enable a more robust and scalable system.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/Managers/Installers/BootStrapperInstaller.cs"),
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/Managers/BootStrapperContext.cs"),
                new ReferenceToClasses("Assets/Scripts/Game/GameLogic/BootStrapper.cs"),
            };
        }
    }
}