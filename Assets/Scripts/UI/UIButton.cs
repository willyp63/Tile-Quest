using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIComponent
{
    public void Disable()
    {
        GetComponent<Button>().interactable = false;
    }

    public void Enable()
    {
        GetComponent<Button>().interactable = true;
    }
}
