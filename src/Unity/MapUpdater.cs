using UnityEngine;
using UnityEngine.Tilemaps;

public class MapUpdater : MonoBehaviour
{
    public Tilemap tilemap;
    public TilePicker tilePicker;

    /// <summary>
    /// Fills a region of the Tilemap with tiles from the provided Map.
    /// </summary>
    /// <param name="start">The starting coordinate in the Tilemap to begin placing tiles.</param>
    /// <param name="size">The width and height of the region to fill.</param>
    /// <param name="map">The Map object to retrieve tiles from.</param>
    /// <param name="startTile">The starting tile coordinate in the Map.</param>
    public void Fill(Vector2Int start, Vector2Int size, Map map, Vector2Int startTile, Snake perspective)
    {
        if (tilemap == null || tilePicker == null)
        {
            Debug.LogError("Tilemap or TilePicker not assigned.");
            return;
        }

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int mapX = startTile.x + x;
                int mapY = startTile.y + y;

                var fragment = map.GetMapFragment(mapX - 1, mapY - 1, mapX + 1, mapY + 1);

                var unityTile = tilePicker.PickTile(fragment, perspective);

                Vector3Int tilemapPos = new Vector3Int(start.x + x, start.y - y, 0);
                tilemap.SetTile(tilemapPos, unityTile);
            }
        }
    }
}
