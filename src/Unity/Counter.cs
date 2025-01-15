using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private int currentValue;
    private TextMeshProUGUI textMeshPro;

    public int upperLimit = int.MaxValue;
    public int lowerLimit = int.MinValue;

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (textMeshPro == null)
        {
            Debug.LogError("No TextMeshProUGUI component found on this GameObject.");
            return;
        }

        if (!int.TryParse(textMeshPro.text, out currentValue))
        {
            currentValue = 0;
        }

        currentValue = Mathf.Clamp(currentValue, lowerLimit, upperLimit);
        UpdateText();
    }

    public int Get()
    {
        return currentValue;
    }

    public void Increase(int amount = 1)
    {
        if (currentValue + amount <= upperLimit)
        {
            currentValue += amount;
            UpdateText();
        }
    }

    public void Decrease(int amount = 1)
    {
        if (currentValue - amount >= lowerLimit)
        {
            currentValue -= amount;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        textMeshPro.text = currentValue.ToString();
    }
}
