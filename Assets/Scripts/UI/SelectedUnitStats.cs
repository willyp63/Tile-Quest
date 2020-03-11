using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUnitStats : UIComponent
{
    Text nameObj;
	Text hpObj;
	Text attackObj;
	Text rangeObj;
	Text moveObj;
	Text statusesObj;

    private void Start()
    {
		nameObj = transform.Find("Name").GetComponent<Text>();
		hpObj = transform.Find("HP").GetComponent<Text>();
		attackObj = transform.Find("Attack").GetComponent<Text>();
		rangeObj = transform.Find("Range").GetComponent<Text>();
		moveObj = transform.Find("Move").GetComponent<Text>();
		statusesObj = transform.Find("Statuses").GetComponent<Text>();
    }

    public void SetUnit(Unit unit)
    {
        // name
        nameObj.text = unit.GetName();

        // HP
        hpObj.text = unit.GetHealth().ToString() + "/" + unit.GetMaxHealth().ToString();

        // Stats
        attackObj.text = "Attack - " + unit.GetAttackDamage().ToString();
        rangeObj.text = "Range - " + unit.GetAttackRange().ToString();
        moveObj.text = "Move - " + unit.GetMoveRange().ToString();

        // Statuses
        var statusText = "";
        var statuses = unit.GetStatuses();
        var remainingDurations = unit.GetStatusRemainingDurations();
        for (int i = 0; i < statuses.Count; i++)
        {
            statusText += statuses[i].GetTitle() + (statuses[i].HasInfiniteDuration() ? "" : "(" + remainingDurations[i] + ")");
        }
		statusesObj.text = statusText;
    }
}
