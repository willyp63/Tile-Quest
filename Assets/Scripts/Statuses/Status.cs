using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : ScriptableObject
{
    public string title;
    public int duration = 3;
    public bool hasInfiniteDuration = false;
    public GameObject indicatorPrefab;

    public string GetTitle() { return title; }
    public int GetDuration() { return duration; }
    public bool HasInfiniteDuration() { return hasInfiniteDuration; }
    public GameObject GetIndicatorPrefab() { return indicatorPrefab; }

    public abstract void BeforeTurn(Unit unit);
    public abstract void AfterTurn(Unit unit);
}
