using UnityEngine;

[CreateAssetMenu(fileName = "NewTile", menuName = "Tiles/TileType")]
public class SOTileContent : ScriptableObject
{
    [SerializeField] public string tileName = "Default";
    [SerializeField] public string representation = "□";
    [SerializeField] public int tileValue = 0;
    [SerializeField] public bool isDeadly = false;
    [SerializeField] public float[] numbers = { 1.0f, 1.0f, 1.0f };
}