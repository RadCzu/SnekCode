
using System;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;

public class Map
{
    private List<List<Tile>> state = new();

    public Map(List<List<Tile>> state)
    {
        this.state = state;
    }

    public Map Clone()
    {
        List<List<Tile>> cloneState = new();

        for (int i = 0; i < state.Count; i++)
        {
            cloneState.Add(new List<Tile>());
            for (int j = 0; j < state[i].Count; j++)
            {
                ITileable contentClone = state[i][j].GetContent().Clone();
                Tile clonedTile = new Tile(i, j, contentClone);
                cloneState[i].Add(clonedTile);
            }
        }

        Map cloneMap = new Map(cloneState);

        return cloneMap;
    }

    public List<List<Tile>> GetMapState() {
        return state;
    }

    public Tile GetTile(int x, int y) {
        try {
            Tile result = state[x][y];
            return result;
        }  catch (Exception) {
            TileBuilder tileBuilder = new();
            Tile empty = new Tile(x, y, tileBuilder.BuildOutOfBounds());
            empty.GetContent().SetTile(empty);
            return empty;
        }
    }

    bool SetTile(int x, int y, ConcreteTile tileContent) {
        try {
            state[x][y].SetContent(tileContent);
            return true;
        } catch {
            return false;
        }
    }

    public Vector2Int GetMapSize() {
        return new Vector2Int(state.Count, state[0].Count);
    }

    public void MoveTileByCoordinates(int x1, int y1, int x2, int y2)
    {
        Tile tile1 = GetTile(x1, y1);
        Tile tile2 = GetTile(x2, y2);

        tile2.SetContent(tile1.GetContent());

        TileBuilder builder = new TileBuilder();
        tile1.SetContent(builder.BuildEmpty(tile1));
    }

    public List<List<Tile>> GetMapFragment(int startX, int startY, int endX, int endY)
    {
        int requestedWidth = endX + 1 - startX;
        int requestedHeight = endY + 1 - startY;

        List<List<Tile>> tempMap = new();
        for (int i = 0; i < requestedHeight; i++)
        {
            tempMap.Add(new List<Tile>(new Tile[requestedWidth]));
        }

        for (int col = startX; col <= endX; col++)
        {
            for (int row = startY; row <= endY; row++)
            {
                try
                {
                    tempMap[row - startY][col - startX] = GetTile(col, row);
                }
                catch (IndexOutOfRangeException)
                {
                    Tile tile = GetTile(col, row);
                    tempMap[row - startY][col - startX] = tile;
                }
            }
        }

        return tempMap;
    }

    public static List<List<float[]>> NormalizeFragment(List<List<Tile>> fragment) {
        int numRows = fragment.Count;
        int numCols = numRows > 0 ? fragment[0].Count : 0;

        List<List<float[]>> normMap = new();
        for (int row = 0; row < numRows; row++) {
            List<float[]> normRow = new();
            for (int col = 0; col < numCols; col++) {
                normRow.Add(fragment[row][col].GetContent().ToNumbers());
            }
            normMap.Add(normRow);
        }
        return normMap;
    }

    public int GetPlayableWidth() {
        return state.Count - 2;
    }

    public int GetPlayableHeight() {
        return state[0].Count - 2;
    }

    public static List<List<float[]>> ResizeFragment(List<List<float[]>> normalizedFragment, int width, int height)
    {
        int originalWidth = normalizedFragment[0].Count;
        int originalHeight = normalizedFragment.Count;

        Mat mat = new Mat(originalHeight, originalWidth, MatType.CV_32FC3);

        for (int y = 0; y < originalHeight; y++)
        {
            for (int x = 0; x < originalWidth; x++)
            {
                var color = normalizedFragment[y][x];
                mat.Set(y, x, new Vec3f(color[0], color[1], color[2]));
            }
        }

        Mat resizedMat = new Mat();
        Cv2.Resize(mat, resizedMat, new OpenCvSharp.Size(width, height), 0, 0, InterpolationFlags.Linear);

        List<List<float[]>> resizedFragment = new List<List<float[]>>();

        for (int y = 0; y < height; y++)
        {
            List<float[]> row = new List<float[]>();
            for (int x = 0; x < width; x++)
            {
                Vec3f pixel = resizedMat.At<Vec3f>(y, x);
                row.Add(new float[] { pixel.Item0, pixel.Item1, pixel.Item2 });
            }
            resizedFragment.Add(row);
        }

        return resizedFragment;
    }
}
