using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Move or Attack Changing Status")]
public class MoveAttackChangeStatus : Status
{
    public int numAttacks = -1;
    public int numMoves = -1;

    public override void BeforeTurn(Unit unit) {
        if (numAttacks >= 0) {
            unit.SetNumAttacksLeft(numAttacks);
        }
        if (numMoves >= 0) {
            unit.SetNumMovesLeft(numMoves);
        }
    }

    public override void AfterTurn(Unit unit) { }
}
