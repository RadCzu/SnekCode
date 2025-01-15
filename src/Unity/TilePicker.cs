using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePicker : MonoBehaviour
{
    // Assignable fields for Unity Tiles
    [Header("Empty Tile")]
    public TileBase emptyTile;

    [Header("Food Tile")]
    public TileBase foodTile;

    [Header("Wall Tile")]
    public TileBase wallTile;

    [Header("Out Of Bounds Tile")]
    public TileBase outOfBounds;

    [Header("Snake Tiles - Blue Variant")]
    public TileBase blueBodyHorizontal;
    public TileBase blueBodyVertical;
    public TileBase blueBodyTurnTopLeft;
    public TileBase blueBodyTurnTopRight;
    public TileBase blueBodyTurnBottomLeft;
    public TileBase blueBodyTurnBottomRight;
    public TileBase blueHeadUp;
    public TileBase blueHeadDown;
    public TileBase blueHeadLeft;
    public TileBase blueHeadRight;
    public TileBase blueTailUp;
    public TileBase blueTailDown;
    public TileBase blueTailLeft;
    public TileBase blueTailRight;

    [Header("Snake Tiles - Red Variant")]
    public TileBase redBodyHorizontal;
    public TileBase redBodyVertical;
    public TileBase redBodyTurnTopLeft;
    public TileBase redBodyTurnTopRight;
    public TileBase redBodyTurnBottomLeft;
    public TileBase redBodyTurnBottomRight;
    public TileBase redHeadUp;
    public TileBase redHeadDown;
    public TileBase redHeadLeft;
    public TileBase redHeadRight;
    public TileBase redTailUp;
    public TileBase redTailDown;
    public TileBase redTailLeft;
    public TileBase redTailRight;

    /// <summary>
    /// Determines the correct Unity Tile based on a 3x3 map fragment and the SnakePart connections.
    /// </summary>
    /// <param name="mapFragment">A 3x3 map fragment</param>
    /// <param name="snake">The snake to determine tile color (blue for part of this snake, red otherwise)</param>
    /// <returns>The correct Unity Tile</returns>
    public TileBase PickTile(List<List<Tile>> mapFragment, Snake snake)
    {
        if (mapFragment == null || mapFragment.Count != 3 || mapFragment[0].Count != 3)
        {
            Debug.LogError("Invalid map fragment! Must be 3x3.");
            return emptyTile;
        }

        Tile centerTile = mapFragment[1][1];
        ITileable centerContent = centerTile.GetContent();

        SnakePart centerSnakePart = centerContent as SnakePart;

        if (centerSnakePart != null)
        {
            Tile predecessor = FindPredecessor(centerSnakePart);
            Tile successor = FindSuccessor(centerSnakePart);
            Vector2Int centerPosition = centerTile.GetPosition();

            // Determine orientations
            Vector2Int predPosition = predecessor?.GetPosition() ?? centerPosition;
            Vector2Int succPosition = successor?.GetPosition() ?? centerPosition;

            // Determine tile orientation based on predecessor and successor positions
            return PickSnakeTile(centerSnakePart, predPosition, centerPosition, succPosition, snake);
        }

        // Fallback to handling non-snake tiles
        if (centerContent is ConcreteTile concreteTile)
        {
            string tileName = concreteTile.GetName();
            switch (tileName)
            {
                case "Empty": return emptyTile;
                case "Food": return foodTile;
                case "Wall": return wallTile;
                case "OutOfBounds": return outOfBounds;
            }
        }

        return emptyTile;
    }

    private Tile FindPredecessor(SnakePart centerPart)
    {
        if (centerPart.previous != null && centerPart.previous.GetTile() != null)
        {
            return centerPart.previous.GetTile();
        }
        return null;
    }

    private Tile FindSuccessor(SnakePart centerPart)
    {
        if (centerPart.next != null && centerPart.next.GetTile() != null)
        {
            return centerPart.next.GetTile();
        }
        return null;
    }

    private TileBase PickSnakeTile(SnakePart center, Vector2Int first, Vector2Int middle, Vector2Int third, Snake snake)
    {
        bool isBlue = center.IsPartOfSnake(snake);

        Vector2Int dir1 = middle - first;
        Vector2Int dir2 = third - middle;

        // Head
        if (first == middle)
        {
            if (third == middle)
            {
                return isBlue ? blueHeadDown : redHeadDown;
            }
            Vector2Int dirToSuccessor = dir2;
            if (dirToSuccessor == Vector2Int.up) return isBlue ? blueHeadUp : redHeadUp;
            if (dirToSuccessor == Vector2Int.down) return isBlue ? blueHeadDown : redHeadDown;
            if (dirToSuccessor == Vector2Int.left) return isBlue ? blueHeadLeft : redHeadLeft;
            if (dirToSuccessor == Vector2Int.right) return isBlue ? blueHeadRight : redHeadRight;
        }

        // Tail
        if (third == middle && first != middle)
        {
            Vector2Int dirFromPredecessor = dir1;
            if (dirFromPredecessor == Vector2Int.up) return isBlue ? blueTailUp : redTailUp;
            if (dirFromPredecessor == Vector2Int.down) return isBlue ? blueTailDown : redTailDown;
            if (dirFromPredecessor == Vector2Int.left) return isBlue ? blueTailLeft : redTailLeft;
            if (dirFromPredecessor == Vector2Int.right) return isBlue ? blueTailRight : redTailRight;
        }

        // Determine orientation of the snake part
        if (dir1 == Vector2Int.up && dir2 == Vector2Int.up || dir1 == Vector2Int.down && dir2 == Vector2Int.down)
            return isBlue ? blueBodyVertical : redBodyVertical;

        if (dir1 == Vector2Int.left && dir2 == Vector2Int.left || dir1 == Vector2Int.right && dir2 == Vector2Int.right)
            return isBlue ? blueBodyHorizontal : redBodyHorizontal;

        // Turns    
        if ((dir1 == Vector2Int.up && dir2 == Vector2Int.right) || (dir1 == Vector2Int.left && dir2 == Vector2Int.down))
            return isBlue ? blueBodyTurnTopRight : redBodyTurnTopRight;

        if ((dir1 == Vector2Int.up && dir2 == Vector2Int.left) || (dir1 == Vector2Int.right && dir2 == Vector2Int.down))
            return isBlue ? blueBodyTurnTopLeft : redBodyTurnTopLeft;

        if ((dir1 == Vector2Int.down && dir2 == Vector2Int.right) || (dir1 == Vector2Int.left && dir2 == Vector2Int.up))
            return isBlue ? blueBodyTurnBottomRight : redBodyTurnBottomRight;

        if ((dir1 == Vector2Int.down && dir2 == Vector2Int.left) || (dir1 == Vector2Int.right && dir2 == Vector2Int.up))
            return isBlue ? blueBodyTurnBottomLeft : redBodyTurnBottomLeft;

        return blueBodyHorizontal;
    }

private List<Tile> GetNeighbors(List<List<Tile>> mapFragment, Tile centerTile)
{
    List<Tile> neighbors = new List<Tile>();

    int centerX = -1;
    int centerY = -1;

    for (int y = 0; y < mapFragment.Count; y++)
    {
        for (int x = 0; x < mapFragment[y].Count; x++)
        {
            if (mapFragment[y][x] == centerTile)
            {
                centerX = x;
                centerY = y;
                break;
            }
        }
        if (centerX != -1) break;
    }

    if (centerX == -1 || centerY == -1)
    {
        Debug.LogError("Center tile not found in map fragment.");
        return neighbors;
    }

    for (int dx = -1; dx <= 1; dx++)
    {
        for (int dy = -1; dy <= 1; dy++)
        {
            if (dx == 0 && dy == 0) continue;

            int nx = centerX + dx;
            int ny = centerY + dy;

            if (nx >= 0 && nx < mapFragment[0].Count && ny >= 0 && ny < mapFragment.Count)
            {
                neighbors.Add(mapFragment[ny][nx]);
            }
        }
    }

    return neighbors;
}

}
