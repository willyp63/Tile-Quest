using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector2Int position;
    public int turnsBetweenSpawns = 8;
    public int turnsBetweenSpawnsOffset = -6;
    public int maxNumSpawns = -1;
    public Enemy[] enemies;

    int turnTimer = 0;
    int enemyIndex = 0;
    int numSpawns = 0;

    GameBoard board;

    private void Start() {
        board = FindObjectOfType<GameBoard>();
        transform.position = GridUtils.GetWorldPos(position);

        turnTimer -= turnsBetweenSpawnsOffset;
    }

    public void AfterTurn() {
        if (maxNumSpawns > 0 && numSpawns >= maxNumSpawns) { return; }

        turnTimer++;

        if (turnTimer >= turnsBetweenSpawns) {
            SpawnEnemy();

            turnTimer = 0;

            enemyIndex++;
            if (enemyIndex >= enemies.Length) {
                enemyIndex = 0;
            }

            numSpawns++;
        }
    }

    private void SpawnEnemy() {
        var newEnemy = Instantiate(enemies[enemyIndex], transform.position, Quaternion.identity);
        newEnemy.position = position;
        newEnemy.transform.position = GridUtils.GetWorldPos(position);

        board.AddEnemy(newEnemy);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}
