using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIndicator : MonoBehaviour
{
	Vector2Int position;

	public void SetPosition(Vector2Int position)
	{
		this.position = position;
	}

	public Vector2Int GetPosition()
	{
		return position;
	}
}
