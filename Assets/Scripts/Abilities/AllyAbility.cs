using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AllyAbility : ScriptableObject
{
    public string title;
    public string description;
    public int cost = 3;

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetCost()
    {
        return cost;
    }

    public abstract void OnAnimStateActivate(Ally ally, int animState);
}
