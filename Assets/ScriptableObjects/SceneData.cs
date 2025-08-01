using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scriptable Objects/SceneData")]
public class SceneData : ScriptableObject
{
    public int currentScene = 0;
    public int maxScene = 10;
    public List<int> bossScene = new List<int>();
    public bool StageCheck()
    {
#if UNITY_EDITOR
        currentScene = 0;
        return true;
#else
        currentScene++;
        return (currentScene <= maxScene);
#endif
    }
}
