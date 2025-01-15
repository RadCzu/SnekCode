using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSwitch : MonoBehaviour
{
    [System.Serializable]
    public class ButtonGameObjectPair
    {
        public Button button;         
        public GameObject target;     
    }

    public List<ButtonGameObjectPair> buttonGameObjectPairs = new List<ButtonGameObjectPair>();

    void Start()
    {
        foreach (var pair in buttonGameObjectPairs)
        {
            if (pair.button != null && pair.target != null)
            {
                pair.button.onClick.AddListener(() => ShowGameObject(pair.target));
            }
        }
    }

    private void ShowGameObject(GameObject targetToShow)
    {
        foreach (var pair in buttonGameObjectPairs)
        {
            if (pair.target != null)
            {
                pair.target.SetActive(false);
            }
        }

        if (targetToShow != null)
        {
            targetToShow.SetActive(true);
        }
    }
}
