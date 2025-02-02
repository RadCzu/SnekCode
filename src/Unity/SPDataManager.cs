using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SPDataManager : MonoBehaviour
{
    public Counter widthCounter;
    public Counter heightCounter;
    public Counter plusCounter;
    
    public void Setup() {
        SetWidth(() => widthCounter.Get());
        SetHeight(() => heightCounter.Get());
        if (plusCounter != null) {
            SetPlus(() => plusCounter.Get());
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
        SingleplayerData.SetWidth(widthProvider());
    }

    public void SetHeight(Func<int> heightProvider)
    {
        SingleplayerData.SetHeight(heightProvider());
    }

    public void SetType(string type)
    {
        SingleplayerData.SetMapType(type);
    }

    public void SetPlus(Func<int> plusProvider)
    {
        SingleplayerData.SetPlusSize(plusProvider());
    }

    private bool IsCorrect() {
        if (SingleplayerData.mapType == "cross") {
            int plusSize = SingleplayerData.plusSize;
            int width = SingleplayerData.mapWidth;
            int height = SingleplayerData.mapHeight;

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
            GameOptions.singleplayer = true;
            SceneTransitioner.Transition2Scene(sceneName);
        } else {
            Debug.Log("Wrong input");
        }
    }
}
