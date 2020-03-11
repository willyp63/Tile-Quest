using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndicatorManager : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public GameObject baseIndicatorPrefab;
    List<GameObject> indicators = new List<GameObject>();

    public void AddIndicators(IList<Vector2Int> poss, Color color, bool isOnBase = false)
    {
        foreach (var pos in poss)
        {
            var worldPos = GridUtils.GetWorldPos(pos);

            worldPos.z += 0.05f;

            var newIndicator = Instantiate(isOnBase ? baseIndicatorPrefab : indicatorPrefab, worldPos, Quaternion.identity);

            newIndicator.GetComponentInChildren<SpriteRenderer>().color = color;
            newIndicator.transform.parent = transform;

            indicators.Add(newIndicator);
        }
    }

    public void ClearIndicators()
    {
        foreach (var indicator in indicators)
        {
            Destroy(indicator.gameObject);
        }
        
        indicators.Clear();
    }
}
