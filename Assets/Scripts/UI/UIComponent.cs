using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponent : MonoBehaviour
{
    public void Hide()
    {
        GetComponent<CanvasGroup>().alpha = 0;
    }

    public void Show()
    {
        GetComponent<CanvasGroup>().alpha = 1;
    }
}
