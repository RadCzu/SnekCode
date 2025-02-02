using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MPDataManager : MonoBehaviour
{
    public Counter widthCounter;
    public Counter heightCounter;
    public Counter plusCounter;
    public Counter playerCounter;
    
    public void Setup() {
        SetWidth(() => widthCounter.Get());
        SetHeight(() => heightCounter.Get());
        if (plusCounter != null) {
            SetPlus(() => plusCounter.Get());
        }
        for (int i = 0; i < playerCounter.Get(); i++) {
            MultiplayerData.AddPlayer();
        }
    }

    public void SetWidthCounter(Counter widthCounter) {
        this.widthCounter = widthCounter;
    }

    public void SetHeightCounter(Counter heightCounter) {
        this.heightCounter = heightCounter;
    }

    public void SetPlusCounter(Counter plusCounter) {
        this.plusCounter = plusCounter;
    }

    public void SetWidth(Func<int> widthProvider)
    {
        MultiplayerData.SetWidth(widthProvider());
    }

    public void SetHeight(Func<int> heightProvider)
    {
        MultiplayerData.SetHeight(heightProvider());
    }

    public void SetType(string type)
    {
        MultiplayerData.SetMapType(type);
    }

    public void SetPlus(Func<int> plusProvider)
    {
        MultiplayerData.SetPlusSize(plusProvider());
    }

    private bool IsCorrect() {
        if (MultiplayerData.mapType == "cross") {
            int plusSize = MultiplayerData.plusSize;
            int width = MultiplayerData.mapWidth;
            int height = MultiplayerData.mapHeight;

            if (plusSize > width / 2 - 1 || plusSize > height / 2 - 1) {
                return false;
            } else {
                return true;
            }
        } else {
            return true;
        }
    }

    public void Transition2Scene(string sceneName)
    {
        if (IsCorrect()) {
            GameOptions.singleplayer = false;
            SceneTransitioner.Transition2Scene(sceneName);
        } else {
            Debug.Log("Wrong input");
        }
    }
}
