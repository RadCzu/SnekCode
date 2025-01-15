
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Map
{
    public List<List<Tile>> state = new();

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

    public int GetPlayableWidth() {
        return state.Count - 2;
    }

    public int GetPlayableHeight() {
        return state[0].Count - 2;
    }
    
}
