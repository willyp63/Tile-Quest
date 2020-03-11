using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mission : ScriptableObject
{
    public string title;
    public string description;
    public int height = 6;
    public int width = 9;
    [TextArea] public string data;
    public Vector2Int[] allyStartingPoss;

    public int[,] GetDataArray()
    {
        var dataArray = new int[height, width];
        var lines = data.Split('\n');
        for (int i = 0; i < lines.Length; i++) {
            var words = lines[i].Split(' ');
            for (int j = 0; j < words.Length; j++) {
                dataArray[i, j] = Int32.Parse(words[j].Trim());
            }
        }
        return dataArray;
    }

    public abstract bool IsComplete(List<Ally> allies, List<Enemy> enemies);
    
    public virtual bool HasFailed(List<Ally> allies, List<Enemy> enemies) {
        foreach (var ally in allies) {
            if (ally.IsAlive()) { return false; }
        }
        return true;
    }
}
