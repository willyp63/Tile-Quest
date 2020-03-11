using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DOT Status")]
public class DamageOverTimeStatus : Status
{
    public int damagePerTurn = 10;

    public override void BeforeTurn(Unit unit) { }

    public override void AfterTurn(Unit unit) {
        unit.Damage(damagePerTurn);
    }
}
