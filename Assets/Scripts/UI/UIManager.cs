using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public SelectedUnitStats selectedUnitStats;
    public ManaDisplay manaDisplay;
    public AbilityButton[] abilityButtons;

    public void SetMana(int mana)
    {
        manaDisplay.SetMana(mana);
    }

    public void Deselect()
    {
        selectedUnitStats.Hide();

        foreach (var abilityButton in abilityButtons)
        {
            abilityButton.Hide();
        }
    }

    public void SelectedUnit(Unit unit, int mana)
	{
        // reset UI
        Deselect();

        // unit info panel
        selectedUnitStats.Show();
        selectedUnitStats.SetUnit(unit);

        // ability buttons
        var ally = unit as Ally;
        if (ally != null)
        {
            var abilities = ally.GetAbilities();

            for (int i = 0; i < abilities.Length && i < abilityButtons.Length; i++)
            {
                abilityButtons[i].Show();
                abilityButtons[i].SetAbility(abilities[i]);

                if (mana >= abilities[i].GetCost())
                {
                    abilityButtons[i].Enable();
                }
                else
                {
                    abilityButtons[i].Disable();
                }
            }
        }
	}
}
