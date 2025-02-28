// Base class for scene actions

using UnityEngine;
public abstract class BaseSceneAction 
{
    public abstract void Execute(string scenePath, System.Action onComplete);
}