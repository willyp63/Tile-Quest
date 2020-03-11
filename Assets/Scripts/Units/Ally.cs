using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Unit
{
    public AllyAbility[] abilities;

    int abilityAnimState = 0;

    public AllyAbility[] GetAbilities()
    {
        return abilities;
    }

    public void SetAbilityAnimState(int abilityAnimState) {
        this.abilityAnimState = abilityAnimState;
    }

    public IEnumerator UseAbility(AllyAbility ability, Unit target)
    {
        // start animation
        anim.SetTrigger("onAbility(" + ability.GetTitle() + ")");
        abilityAnimState = 1;

        // changing [abilityAnimState] back to 0 ends the ability
        while (abilityAnimState > 0) {
            // let ability do its thang
            var targetableAbility = ability as TargetableAllyAbility;
            if (targetableAbility != null) {
                targetableAbility.OnAnimStateActivate(this, target, abilityAnimState);
            } else {
                ability.OnAnimStateActivate(this, abilityAnimState);
            }

            var currAnimState = abilityAnimState;

            // wait for anim state to change
            yield return new WaitUntil(() => abilityAnimState != currAnimState);
        }
    }
}
