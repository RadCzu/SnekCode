using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    public void Transition2Scene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
