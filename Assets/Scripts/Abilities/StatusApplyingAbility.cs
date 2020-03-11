using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Applying Ability")]
public class StatusApplyingAbility : TargetableAllyAbility
{
    public Status[] statuses;

    public override void OnAnimStateActivate(Ally ally, Unit target, int animState)
    {
        if (animState == 2) {
            foreach (var status in statuses)
            {
                target.AddStatus(status);
            }
        }
    }
}
