using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reach Destination Mission")]
public class ReachDestinationMission : Mission
{
    public Vector2Int[] destinationPoss;
    public int numAlliesRequiredAtDestination = 3;

    public override bool IsComplete(List<Ally> allies, List<Enemy> enemies) {
        var numAlliesAtDestination = 0;
        foreach (var ally in allies) {
            foreach (var destPos in destinationPoss) {
                if (ally.position == destPos) {
                    numAlliesAtDestination++;
                }
            }
        }
        return numAlliesAtDestination >= numAlliesRequiredAtDestination;
    }
}
