using System.Collections.Generic;
using UnityEngine;

public class CameraKiller : MonoBehaviour
{
    // Assign the cameras you want to keep in this list via the inspector
    public List<Camera> whitelist = new List<Camera>();

    void Awake()
    {
        Camera[] allCameras = FindObjectsOfType<Camera>();

        foreach (Camera camera in allCameras)
        {
            // If the camera is not in the whitelist, destroy it
            if (!whitelist.Contains(camera))
            {
                Destroy(camera.gameObject);
            }
        }

        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(this.gameObject);
    }
}
