using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move or Attack Changer Ability")]
public class MoveAttackChangerAbility : AllyAbility
{
    public int deltaMoves = 0;
    public int deltaAttacks = 0;

    public override void OnAnimStateActivate(Ally ally, int animState)
    {
        if (animState == 2) {
            ally.SetNumMovesLeft(ally.GetNumMovesLeft() + deltaMoves);
            ally.SetNumAttacksLeft(ally.GetNumAttacksLeft() + deltaAttacks);
        }
    }
}
