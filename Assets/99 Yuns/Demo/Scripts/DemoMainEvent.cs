using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMainEvent : MonoBehaviour
{
    public string[] SceneName;
    public void OnClick_DemoButton_0() {
        SceneManager.LoadScene(SceneName[0]);
    }
    public void OnClick_DemoButton_1()
    {
        SceneManager.LoadScene(SceneName[1]);
    }
    public void OnClick_DemoButton_2()
    {
        SceneManager.LoadScene(SceneName[2]);
    }
    public void OnClick_DemoButton_3()
    {
        SceneManager.LoadScene(SceneName[3]);
    }
}
