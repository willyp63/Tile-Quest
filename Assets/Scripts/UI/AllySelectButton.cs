using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllySelectButton : MonoBehaviour
{
    public Ally allyPrefab;

    void Start()
    {
        transform.Find("Name").GetComponent<Text>().text = allyPrefab.GetName();
        transform.Find("HP").GetComponent<Text>().text = "HP - " + allyPrefab.GetMaxHealth().ToString();
        transform.Find("Attack").GetComponent<Text>().text = "Attack - " + allyPrefab.GetAttackDamage().ToString();
        transform.Find("Range").GetComponent<Text>().text = "Range - " + allyPrefab.GetAttackRange().ToString();
        transform.Find("Move").GetComponent<Text>().text = "Move - " + allyPrefab.GetMoveRange().ToString();

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick() {
        FindObjectOfType<SceneController>().SelectAlly(allyPrefab.GetName());
        GetComponent<Button>().interactable = false;
    }
}
