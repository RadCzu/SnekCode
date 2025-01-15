using System.Collections.Generic;
public static class CrossMapFactory
{
    public static Map Build(int width, int height, int plusSize)
    {
        var map = new List<List<Tile>>();
        TileBuilder tileBuilder = new TileBuilder();

        for (int x = 0; x < width + 2; x++)
        {
            var row = new List<Tile>();
            for (int y = 0; y < height + 2; y++)
            {
                Tile tile = new(x, y, null);
                if (x == 0 || y == 0 || x == width + 1 || y == height + 1)
                {
                    tileBuilder.BuildWall(tile);
                }
                else if ((x < plusSize + 1 || x > width - plusSize) && (y < plusSize + 1 || y > height - plusSize))
                {
                    tileBuilder.BuildWall(tile);
                }
                else
                {
                    tileBuilder.BuildEmpty(tile);
                }
                row.Add(tile);
            }
            map.Add(row);
        }
        return new Map(map);
    }
}