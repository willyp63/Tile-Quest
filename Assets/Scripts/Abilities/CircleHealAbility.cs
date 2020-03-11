using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Circle Heal Ability")]
public class CircleHealAbility : AllyAbility
{
    public int healAmount = 20;
    public int radius = 1;

    public override void OnAnimStateActivate(Ally ally, int animState)
    {
        if (animState == 2) {
            Heal(ally);
        }
    }

    private void Heal(Ally ally) {
        var board = FindObjectOfType<GameBoard>();
        var allyPossInRange = board.GetPossWithAllyInRange(ally.GetPosition(), radius);
        foreach (var node in allyPossInRange)
        {
            board.GetUnitAt(node.pos).Heal(healAmount);
        }
    }
}
