using System;
using System.Collections.Generic;
using Base.Editor.WelcomePage;
using Base.Editor.WelcomePage.Pages.Features;
using Base.Editor.WelcomePage.Pages.Features.Features;
using Base.Editor.WelcomePage.Pages.Features.GameFeatures;
using Game.EatableObjects;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class FirstShowcasePage 
{
    public string title = "Technical Skill Showcase";

    public string explanation = 
        "Project Start Date: 25/02/2025\n" +
        "Project End Date: 04/03/2025\n" +
        "Total Time Spent: 7 Days\n\n" +
        "Hello and welcome! In this project, I aimed to showcase my technical skills in Unity by creating a partial clone of the Hole.IO game.\n" +
        "Below, you will find a list of features that I have implemented or planned for this project. While the core structure is complete, not all game mechanics were fully implemented due to time constraints.\n\n" +
        "This showcase includes widely adopted systems such as UniRX, Zenject, DOTween, and Odin Inspector, alongside custom solutions like my Encrypted Save System and Serialized Resourced Scriptable Objects.\n" +
        "I hope you enjoy exploring the showcase!";

    public List<ProjectFeatures> ProjectFeaturesList = new List<ProjectFeatures>()
    {
        new UniRXFeature(),
        new ZenjectFeature(),
        new OdinInspectorFeature(),
        new DoTweenFeature(),
        new CinemachineFeature(),
        new LevelLoadingSystemFeature(),
        new EnrycptedSaveSystemFeature(),
        new SerializedResourcedScriptableObjectFeature(),
        new DinoGameFeature(),
    };
    
    public List<ProjectFeatures> GameFeaturesList = new List<ProjectFeatures>()
    {
        new HoleStencilDrawerFeature(),
        new HoleEatableSystemFeature(),
        new HideableObjectsFeature(),
        new AISystemFeature(),
    };
}