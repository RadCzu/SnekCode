using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerPlacement", menuName = "Placements/Player")]
public class SOPlayerPlacement : ScriptableObject
{
    [SerializeField] public Vector2 position;
    [SerializeField] public Color playerColor;
}