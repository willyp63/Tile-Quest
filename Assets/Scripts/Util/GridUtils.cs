using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridUtils
{
    public static Vector3 GetWorldPos(Vector2Int gridPos, bool isOnTopOfTerrain = true)
    {
        float xPos = 1 + 0.75f * gridPos.x;
        float YOffset = (int)Mathf.Abs(gridPos.x) % 2 == 1 ? -0.25f : 0;
        float yPos = 1 + 0.5f * gridPos.y + YOffset;
        float zPos = yPos + (isOnTopOfTerrain ? -0.75f : 0);

        return new Vector3(xPos, yPos, zPos);
    }

    public static Vector2Int GetGridPos(Vector3 worldPos)
    {
        int xPos = Mathf.RoundToInt((worldPos.x - 1) / 0.75f);
        float YOffset = (int)Mathf.Abs(xPos) % 2 == 1 ? -0.25f : 0;
        int yPos = Mathf.RoundToInt((worldPos.y - 1 - YOffset) / 0.5f);

        return new Vector2Int(xPos, yPos);
    }

    public static int GetGridDist(Vector2Int pos1, Vector2Int pos2)
    {
        int deltaX = Mathf.Abs(pos1.x - pos2.x);
        int deltaY = Mathf.Abs(pos1.y - pos2.y);

        int diagonalLength = Mathf.Min(deltaX, deltaY * 2);

        int yMovesAlongDiagonal;
        if ((pos2.y - pos1.y) * (pos1.x % 2 == 0 ? 1 : -1) > 0)
        {
            yMovesAlongDiagonal = (int)Mathf.Ceil(diagonalLength / 2f);
        }
        else
        {
            yMovesAlongDiagonal = (int)Mathf.Floor(diagonalLength / 2f);
        }

        int remainingXMoves = deltaX - diagonalLength;
        int remainingYMoves = deltaY - yMovesAlongDiagonal;

        return diagonalLength + remainingXMoves + remainingYMoves;
    }

    public static Vector2Int[] GetAdjacentPoss(Vector2Int pos)
    {
        var leftRightY = pos.x % 2 == 0 ? 1 : -1;
        Vector2Int[] adjacentPoss =
        {
            pos + new Vector2Int(0, 1),
            pos + new Vector2Int(0, -1),
            pos + new Vector2Int(1, 0),
            pos + new Vector2Int(-1, 0),
            pos + new Vector2Int(1, leftRightY),
            pos + new Vector2Int(-1, leftRightY),
        };

        return adjacentPoss;
    }
}
