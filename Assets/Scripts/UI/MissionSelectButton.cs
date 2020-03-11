using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSelectButton : MonoBehaviour
{
    public Mission mission;

    void Start()
    {
        transform.Find("Title").GetComponent<Text>().text = mission.title;
        transform.Find("Description").GetComponent<Text>().text = mission.description;

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick() {
        FindObjectOfType<SceneController>().SelectLevel(mission.title);
    }
}
