using System.Collections;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame(float delay)
    {
        StartCoroutine(ExitWithDelay(delay));
    }

    private IEnumerator ExitWithDelay(float delay)
    {
        Debug.Log($"Exiting the game in {delay} seconds...");
        yield return new WaitForSeconds(delay);
        Application.Quit();
        Debug.Log($"Exited the app");
    }
}
