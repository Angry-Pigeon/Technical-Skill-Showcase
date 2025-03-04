using System;
using System.Collections.Generic;
namespace Base.Editor.WelcomePage.Pages.Features
{
    [Serializable]
    public class ProjectFeatures 
    {
        public string Title;
        public string Description;
        public bool Completed;
        
        public List<ReferenceToClasses> ReferenceToClasses;
        
        public List<FeatureActions> Actions;
        
    }
    
    [Serializable]
    public class ReferenceToClasses
    {
        public string ClassAddress;
        
        public ReferenceToClasses(string classAddress)
        {
            ClassAddress = classAddress;
        }
    }
    
    public class FeatureActions
    {
        public string ActionName;
        public Action Action;
        
        public FeatureActions(string actionName, Action action)
        {
            ActionName = actionName;
            Action = action;
        }


    }
}