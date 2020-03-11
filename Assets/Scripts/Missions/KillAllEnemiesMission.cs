using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kill All Enemies Mission")]
public class KillAllEnemiesMission : Mission
{
    public override bool IsComplete(List<Ally> allies, List<Enemy> enemies) {
        foreach (var enemy in enemies) {
            if (enemy.IsAlive()) { return false; }
        }
        return true;
    }
}
