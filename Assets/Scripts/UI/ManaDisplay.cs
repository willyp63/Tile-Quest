using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaDisplay : UIComponent
{
    Text textObj;

    private void Awake()
    {
		  textObj = transform.Find("Text").GetComponent<Text>();
    }

    public void SetMana(int mana)
	{
        textObj.text = "x" + mana.ToString();
	}
}
