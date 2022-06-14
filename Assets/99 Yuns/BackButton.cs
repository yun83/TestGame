using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public string SceneName = "DemoMain";

    public void OnClick_ThisButton()
    {
        SceneManager.LoadScene(SceneName);
    }
}
