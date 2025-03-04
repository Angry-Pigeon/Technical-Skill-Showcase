using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.GameFeatures
{
    public class AISystemFeature : ProjectFeatures
    {
        public AISystemFeature()
        {
            Title = "AI System";
            Description = "The AI System is designed to provide a basic framework for incorporating artificial intelligence into the game.\n" +
                "It lays the foundation for decision-making, behavior logic, and reactive interactions within the game environment.\n\n" +
                "In this project, I initiated the development of the AI System; however, due to time constraints, only the core structure has been implemented.\n" +
                "The current version is non-functional and serves as a bare-bones prototype, intended to be expanded and refined in future updates.";
            
            Completed = false;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/AI/SimpleAI.cs"),
            };
        }
    }
}