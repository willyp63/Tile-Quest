using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetableAllyAbility : AllyAbility
{
    public int range = 3;
    public bool canTargetEnemies = true;
    public bool canTargetAllies = true;
    public bool canTargetSelf = true;

    public int GetRange() {
        return range;
    }

	public bool CanTargetEnemies()
	{
		return canTargetEnemies;
	}

	public bool CanTargetAllies()
	{
		return canTargetAllies;
	}

    public bool CanTargetSelf()
	{
		return canTargetSelf;
	}

    public override void OnAnimStateActivate(Ally ally, int animState)
    {
        throw new System.NotImplementedException();
    }

    public abstract void OnAnimStateActivate(Ally ally, Unit target, int animState);
}
