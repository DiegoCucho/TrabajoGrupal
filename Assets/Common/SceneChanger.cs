using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
public void ChangeScene(string sceneName) { 
    SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Single); 
    } 

public void Exit() { 
    Application.Quit (); 
    }
}
