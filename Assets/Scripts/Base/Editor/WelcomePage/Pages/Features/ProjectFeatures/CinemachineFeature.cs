using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features.Features
{
    public class CinemachineFeature : ProjectFeatures
    {
        public CinemachineFeature()
        {
            Title = "Cinemachine";
            Description = "Cinemachine is a powerful and versatile camera system for Unity that enables dynamic, cinematic camera behavior.\n" +
                "It simplifies the creation of smooth camera movements and complex tracking setups.\n\n" +
                "In this project, I used a Cinemachine Virtual Camera to follow the player. Additionally, I developed a custom script to dynamically update the camera's Body Transposer offset as the player's hole grows, ensuring optimal framing at all times.";
            Completed = true;
            
            ReferenceToClasses = new List<ReferenceToClasses>()
            {
                new ReferenceToClasses("Assets/Scripts/Game/Camera/ChangeCameraFollowWithLevel.cs")
            };
        }
    }
}