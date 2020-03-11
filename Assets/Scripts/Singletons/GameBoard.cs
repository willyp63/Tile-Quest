using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public List<GameObject> tilePrefabs;
    public List<Ally> allyPrefabs;
    public Mission mission;

    int[,] dataArray;

	List<Ally> allies = new List<Ally>();
	List<Enemy> enemies = new List<Enemy>();
	List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

	Dictionary<Vector2Int, Unit> unitsByPos = new Dictionary<Vector2Int, Unit>();

    public int GetHeight() { return mission.height; }

    public int GetWidth() { return mission.width; }

    public List<Ally> GetAllies() { return allies; }

    public List<Enemy> GetEnemies() { return enemies; }

    public List<EnemySpawner> GetEnemySpawners() { return enemySpawners; }

    public bool IsTileAt(Vector2Int gridPos) { return GetTileAt(gridPos) > 0; }

    public int GetTileAt(Vector2Int gridPos)
    {
        if (gridPos.x >= 0 && gridPos.x < mission.width &&
            gridPos.y >= 0 && gridPos.y < mission.height)
        {
            return dataArray[mission.height - gridPos.y - 1, gridPos.x];
        }
        return 0;
    }

    public Unit GetUnitAt(Vector2Int pos)
    {
        if (!unitsByPos.ContainsKey(pos)) { return null; }
        return unitsByPos[pos];
    }

    public void AddEnemy(Enemy enemy) {
        enemies.Add(enemy);
        unitsByPos[enemy.position] = enemy;
    }

    public bool IsMissionComplete() {
        return mission.IsComplete(allies, enemies);
    }

    public bool HasMissionFailed() {
        return mission.HasFailed(allies, enemies);
    }

    public IEnumerator MoveUnit(Unit unit, MoveNode moveNode)
    {
        unitsByPos[unit.GetPosition()] = null;

        yield return StartCoroutine(unit.Move(moveNode));

        unitsByPos[unit.GetPosition()] = unit;
    }

    public void RemoveDeadUnits()
    {
        for (int i = 0; i < allies.Count; i++)
        {
            if (!allies[i].IsAlive())
            {
                unitsByPos[allies[i].GetPosition()] = null;
                allies.RemoveAt(i--);
            }
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].IsAlive())
            {
                unitsByPos[enemies[i].GetPosition()] = null;
                enemies.RemoveAt(i--);
            }
        }
    }

    public List<MoveNode> GetWalkablePossInRange(Vector2Int pos, int range)
    {
        return GetPossInRange(pos, range, false);
    }

    public List<MoveNode> GetPossWithUnitInRange(Vector2Int pos, int range)
    {
        var possWithUnitInRange = new List<MoveNode>();

        foreach (var posInRange in GetPossInRange(pos, range))
        {
            if (GetUnitAt(posInRange.pos) != null)
            {
                possWithUnitInRange.Add(posInRange);
            }
        }

        return possWithUnitInRange;
    }

    public List<MoveNode> GetPossWithEnemyInRange(Vector2Int pos, int range)
    {
        var possWithEnemyInRange = new List<MoveNode>();

        foreach (var posInRange in GetPossInRange(pos, range))
        {
            if (GetUnitAt(posInRange.pos) as Enemy != null)
            {
                possWithEnemyInRange.Add(posInRange);
            }
        }

        return possWithEnemyInRange;
    }

    public List<MoveNode> GetPossWithAllyInRange(Vector2Int pos, int range)
    {
        var possWithAllyInRange = new List<MoveNode>();

        foreach (var posInRange in GetPossInRange(pos, range))
        {
            if (GetUnitAt(posInRange.pos) as Ally != null)
            {
                possWithAllyInRange.Add(posInRange);
            }
        }

        return possWithAllyInRange;
    }

    public List<MoveNode> GetPossInRange(Vector2Int pos, int range, bool canMoveThroughUnits = true)
    {
        var possInRange = new List<MoveNode>();
        var existingPoss = new Dictionary<Vector2Int, bool>();

        var possToCheck = new List<MoveNode>();
        var newPoss = new List<MoveNode>();

        possToCheck.Add(new MoveNode(pos, null));
        existingPoss[pos] = true;

        for (int i = 0; i < range; i++)
        {
            foreach (var posToCheck in possToCheck)
            {
                foreach (var adjacentPos in GridUtils.GetAdjacentPoss(posToCheck.pos))
                {
                    if (!existingPoss.ContainsKey(adjacentPos) && IsTileAt(adjacentPos) && (canMoveThroughUnits || !GetUnitAt(adjacentPos)))
                    {
                        existingPoss[adjacentPos] = true;

                        var newNode = new MoveNode(adjacentPos, posToCheck);
                        possInRange.Add(newNode);
                        newPoss.Add(newNode);
                    }
                }
            }

            possToCheck = newPoss;
            newPoss = new List<MoveNode>();
        }

        return possInRange;
    }

    public void Start() {
        dataArray = mission.GetDataArray();

        InitTerrain();
        InitAllies();
        InitUnitRefs();
    }

    private void InitTerrain() {
        for (int i = 0; i < mission.height; i++)
        {
            for (int j = 0; j < mission.width; j++)
            {
                var tileType = Mathf.Abs(dataArray[i, j]);

                if (tileType == 0) { continue; }

                if (tileType > tilePrefabs.Count)
                {
                    Debug.LogError("No tilePrefab for tile type: " + tileType);
                    continue;
                }

                var tilePrefab = tilePrefabs[tileType - 1];
                var worlPos = GridUtils.GetWorldPos(new Vector2Int(j, mission.height - i - 1), false);

                GameObject newTile = Instantiate(tilePrefab, worlPos, Quaternion.identity);
                newTile.transform.parent = transform;
            }
        }
    }

    private void InitAllies() {
        var choosenAllyNames = FindObjectOfType<SceneController>().GetSelectedAllyNames();
        var choosenAllyPrefabs = new List<Ally>();

        // for testing levels w/o going through menu
        if (choosenAllyNames.Count == 0) {
            choosenAllyNames.Add("Chad");
            choosenAllyNames.Add("Garrett");
            choosenAllyNames.Add("Belgarath");
        }

        foreach (var name in choosenAllyNames) {
            foreach (var allyPrefab in allyPrefabs) {
                if (allyPrefab.GetName() == name) {
                    choosenAllyPrefabs.Add(allyPrefab);
                    break;
                }
            }
        }

        for (int i = 0; i < choosenAllyPrefabs.Count; i++) {
            var newAlly = Instantiate(choosenAllyPrefabs[i], transform.position, Quaternion.identity);
            newAlly.transform.position = GridUtils.GetWorldPos(mission.allyStartingPoss[i]);
            newAlly.position = mission.allyStartingPoss[i];
        }
    }

    private void InitUnitRefs() {
        foreach (var ally in FindObjectsOfType<Ally>())
		{
			allies.Add(ally);
			unitsByPos[ally.GetPosition()] = ally;
		}

		foreach (var enemy in FindObjectsOfType<Enemy>())
		{
			enemies.Add(enemy);
			unitsByPos[enemy.GetPosition()] = enemy;
		}

        // TODO: seperate from units
        foreach (var enemySpawner in FindObjectsOfType<EnemySpawner>())
		{
			enemySpawners.Add(enemySpawner);
		}
    }
}
