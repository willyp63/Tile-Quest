using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Heal Ability")]
public class HealAbility : TargetableAllyAbility
{
    public int healAmount = 20;

    public override void OnAnimStateActivate(Ally ally, Unit target, int animState) {
        if (animState == 2) {
            target.Heal(healAmount);
        }
    }
}
