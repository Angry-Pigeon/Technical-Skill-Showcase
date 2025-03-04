using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class UniRXFeature : ProjectFeatures
    {
        public UniRXFeature()
        {
            Title = "UniRX";
            Description = "UniRX is a reimplementation of the .NET Reactive Extensions library for Unity.\n" +
                "It offers a powerful approach to composing asynchronous and event-based programs using observable sequences and LINQ-style query operators.\n\n" +
                "In this project, I integrated UniRX into the collision detection system for holes, demonstrating how reactive programming can simplify event handling and improve responsiveness.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>();
            ReferenceToClasses.Add(new ReferenceToClasses("Assets/Scripts/Game/HoleLogic/HoleFallAreaCollisionDetection.cs"));
        }
    }
}