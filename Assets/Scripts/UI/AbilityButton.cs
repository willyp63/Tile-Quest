using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : UIButton
{
	Text titleObj;
	Text descriptionObj;
	Text manaCostObj;
	Text rangeObj;

    AllyAbility ability;

    private void Start()
    {
		titleObj = transform.Find("Title").GetComponent<Text>();
		descriptionObj = transform.Find("Description").GetComponent<Text>();
        manaCostObj = transform.Find("Mana Cost").GetComponent<Text>();
        rangeObj = transform.Find("Range").GetComponent<Text>();

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void SetAbility(AllyAbility ability)
	{
        this.ability = ability;

        titleObj.text = ability.GetTitle();
        descriptionObj.text = ability.GetDescription();
        manaCostObj.text = "x" + ability.GetCost();

        var targetableAbility = ability as TargetableAllyAbility;
        if (targetableAbility != null)
        {
            rangeObj.text = "Range: " + targetableAbility.GetRange();
        } else
        {
            rangeObj.text = "";
        }
    }

    private void OnClick()
    {
        FindObjectOfType<GameController>().OnAbility(ability);
    }
}
