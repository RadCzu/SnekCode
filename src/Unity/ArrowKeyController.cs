using UnityEngine;

public class ArrowKeyController : MonoBehaviour
{
    public delegate void OnArrowKeyPress();
    public OnArrowKeyPress OnUpArrowPress;
    public OnArrowKeyPress OnDownArrowPress;
    public OnArrowKeyPress OnLeftArrowPress;
    public OnArrowKeyPress OnRightArrowPress;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && OnUpArrowPress != null)
        {
            OnUpArrowPress();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && OnDownArrowPress != null)
        {
            OnDownArrowPress();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && OnLeftArrowPress != null)
        {
            OnLeftArrowPress();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && OnRightArrowPress != null)
        {
            OnRightArrowPress();
        }
    }
}
