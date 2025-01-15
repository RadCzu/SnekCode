using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioManager manager;
    public string musicTitle;

    void Start() {
        manager.PlayMusic(musicTitle);
    }
}
