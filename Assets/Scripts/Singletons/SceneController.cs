using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    public static SceneController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private static string selectedLevelSceneName;
    private static List<string> selectedAllyNames = new List<string>();

    public void SelectLevel(string sceneName) {
        selectedLevelSceneName = sceneName;
        SceneManager.LoadScene("Ally Select");
    }

    public void SelectAlly(string allyName) {
        selectedAllyNames.Add(allyName);

        if (selectedAllyNames.Count == 3) {
            SceneManager.LoadScene(selectedLevelSceneName);
        }
    }

    public List<string> GetSelectedAllyNames()
    {
        return selectedAllyNames;
    }

    public void LoadMissionSelectScene() {
        selectedLevelSceneName = null;
        selectedAllyNames.Clear();

        SceneManager.LoadScene("Mission Select");
    }

    public void LoadMissionFailedScene() {
        SceneManager.LoadScene("Mission Failed");
    }

    public void LoadMissionCompleteScene() {
        SceneManager.LoadScene("Mission Complete");
    }
}
