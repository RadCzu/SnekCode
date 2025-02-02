using System.Collections.Generic;
using UnityEngine;

public class TileBuilder
{
    private readonly ScriptableObjectBuilder<SOTileContent> tileContentBuilder;

    public TileBuilder()
    {
        tileContentBuilder = new ScriptableObjectBuilder<SOTileContent>("Objects/Tiles");
    }

    private ConcreteTile BuildTile(string faceName, IInteractionStrategy interaction, Tile tile = null)
    {
        var face = tileContentBuilder.GetObject(faceName);
        return tile == null ? new ConcreteTile(face, interaction) : new ConcreteTile(face, tile, interaction);
    }

    private SnakePart BuildSnakePart(string faceName, IInteractionStrategy interaction, Tile tile = null)
    {
        var face = tileContentBuilder.GetObject(faceName);
        return tile == null ? new SnakePart(face, interaction) : new SnakePart(face, tile, interaction);
    }

    public ConcreteTile BuildEmpty(Tile tile = null) =>
        BuildTile("Empty", new Nothing(), tile);

    public ConcreteTile BuildWall(Tile tile = null)
    {
        var wall = BuildTile("Wall", null, tile);
        wall.interactionMethod = new KillSnakeAndReplace(wall);
        return wall;
    }

    public ConcreteTile BuildFood(Tile tile = null) =>
        BuildTile("Food", new EatFood(new Observer()), tile);

    public SnakePart BuildSnakeHead(Snake snake, Tile tile = null) =>
        BuildSnakePart("SnakeHead", new HeadInteraction(snake), tile);

    public SnakePart BuildSnakeBody(Snake snake, Tile tile = null)
    {
        var interaction = new BodyInteraction(snake);
        var part = BuildSnakePart("SnakeBody", interaction, tile);
        interaction.self = part;
        return part;
    }

    public ConcreteTile BuildOutOfBounds(Tile tile = null) =>
        BuildTile("OutOfBounds", new KillSnake(), tile);
}
