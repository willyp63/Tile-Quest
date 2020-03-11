using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    Vector3 normalScale;

    private void Awake()
    {
        normalScale = transform.Find("health").localScale;
    }

    public void SetHealthPercent(float percent)
    {
        transform.Find("health").localScale = new Vector3(percent * normalScale.x, normalScale.y, normalScale.z);
    }
}
