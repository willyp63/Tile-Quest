using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public IEnumerator TakeTurn(GameBoard board)
    {
        var hasMoves = GetNumMovesLeft() > 0;
        var hasAttacks = GetNumAttacksLeft() > 0;

        List<MoveNode> possibleMoves;
        if (hasMoves) {
            possibleMoves = board.GetWalkablePossInRange(position, moveRange);
            possibleMoves.Add(new MoveNode(position, null));
        } else {
            possibleMoves = new List<MoveNode>();
            possibleMoves.Add(new MoveNode(position, null));
        }

        Ally bestTarget = null;
        MoveNode bestTargetMove = null;
        int bestScore = 1000000;
        bool canAttack = false;

        foreach (var ally in board.GetAllies())
        {
            int score = 0;

            MoveNode bestMove = null;
            int bestDistToAlly = 1000;

            int distanceScore = GridUtils.GetGridDist(position, ally.position) * 10000;
            foreach (var move in possibleMoves) {
                var distToAlly = GridUtils.GetGridDist(move.pos, ally.position);

                if (distToAlly <= attackRange) {
                    distanceScore = 0;
                    bestMove = move;
                    canAttack = true;

                    break;
                }

                if (distToAlly <= bestDistToAlly) {
                    bestDistToAlly = distToAlly;
                    bestMove = move;
                }
            }
            score += distanceScore;

            score += ally.GetHealth();

            if (score <= bestScore) {
                bestTarget = ally;
                bestScore = score;
                bestTargetMove = bestMove;
            }
        }

        if (hasMoves) {
            yield return StartCoroutine(board.MoveUnit(this, bestTargetMove));
        }

        if (hasAttacks && canAttack) {
            yield return StartCoroutine(Attack(bestTarget));
        }

        board.RemoveDeadUnits();
    }
}
