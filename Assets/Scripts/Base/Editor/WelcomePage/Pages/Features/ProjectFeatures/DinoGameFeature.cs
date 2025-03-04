using System;
using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class DinoGameFeature : ProjectFeatures
    {
        public DinoGameFeature()
        {
            Title = "Dino Game";
            Description = "Dino Game is a custom editor-based recreation of the classic Chrome Dino Game.\n" +
                "It demonstrates advanced custom editor design and interactive UI capabilities within the Unity Editor.\n\n" +
                "In this project, I implemented a simplified version featuring dynamic gameplay mechanics such as seed selection, score tracking, highscore management, progressive game speed, and variable jump height based on input duration.\n" +
                "Although it is a basic clone, this implementation effectively showcases the power and flexibility of custom editor tools.";
            Completed = true;

            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Testing/DinoRunner/Editor/DinoGameWindow.cs"),
            };
            
            Actions = new List<FeatureActions>()
            {
                new FeatureActions("Open Dino Game", OpenDinoGameWindow)
            };

        }
        
        private void OpenDinoGameWindow()
        {
            DinoGameWindow.ShowWindow();
        }
    }
}