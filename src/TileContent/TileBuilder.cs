using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore;

public class TileBuilder
{
    public readonly List<SOTileContent> tileTypes;

    public TileBuilder()
    {
        tileTypes = LoadTiles("Objects/Tiles");
    }

    private SOTileContent GetFace(string faceName)
    {
        SOTileContent foundTile = tileTypes.Find((SOTileContent face) => face.tileName == faceName);
        if (foundTile != null)
        {
            return foundTile;
        }
        else
        {
            Debug.LogError($"Tile: {faceName} does not exist.");
            return null;
        }
    }

    private List<SOTileContent> LoadTiles(string path)
    {
        return new List<SOTileContent>(Resources.LoadAll<SOTileContent>(path));
    }

    public ConcreteTile BuildEmpty()
    {
        SOTileContent face = GetFace("Empty");
        IInteractionStrategy interaction = new Nothing();
        return new ConcreteTile(face, interaction);
    }

    public ConcreteTile BuildEmpty(Tile tile)
    {
        SOTileContent face = GetFace("Empty");
        IInteractionStrategy interaction = new Nothing();
        return new ConcreteTile(face, tile, interaction);
    }

    public ConcreteTile BuildWall()
    {
        SOTileContent face = GetFace("Wall");
        ConcreteTile wall = new(face);
        IInteractionStrategy interaction = new KillSnakeAndReplace(wall);
        wall.interactionMethod = interaction;
        return wall;
    }

    public ConcreteTile BuildWall(Tile tile)
    {
        SOTileContent face = GetFace("Wall");
        ConcreteTile wall = new(face, tile);
        IInteractionStrategy interaction = new KillSnakeAndReplace(wall);
        wall.interactionMethod = interaction;
        return wall;
    }

    public ConcreteTile BuildFood()
    {
        SOTileContent face = GetFace("Food");
        IInteractionStrategy interaction = new EatFood(new Observer());
        return new ConcreteTile(face, interaction);
    }

    public ConcreteTile BuildFood(Tile tile)
    {
        SOTileContent face = GetFace("Food");
        IInteractionStrategy interaction = new EatFood(new Observer());
        return new ConcreteTile(face, tile, interaction);
    }

    public SnakePart BuildSnakeHead(Snake snake)
    {
        SOTileContent face = GetFace("SnakeHead");
        IInteractionStrategy interaction = new HeadInteraction(snake);
        return new SnakePart(face, interaction);
    }

    public SnakePart BuildSnakeHead(Tile tile, Snake snake)
    {
        SOTileContent face = GetFace("SnakeHead");
        IInteractionStrategy interaction = new HeadInteraction(snake);
        return new SnakePart(face, tile, interaction);
    }

    public SnakePart BuildSnakeBody(Snake snake)
    {
        SOTileContent face = GetFace("SnakeBody");
        BodyInteraction interaction = new BodyInteraction(snake);
        SnakePart part = new SnakePart(face, interaction);
        interaction.self = part;
        return part;

    }

    public SnakePart BuildSnakeBody(Tile tile, Snake snake)
    {
        SOTileContent face = GetFace("SnakeBody");
        BodyInteraction interaction = new BodyInteraction(snake);
        SnakePart part = new SnakePart(face, tile, interaction);
        interaction.self = part;
        return part;
    }

    public ConcreteTile BuildOutOfBounds()
    {
        SOTileContent face = GetFace("OutOfBounds");
        IInteractionStrategy interaction = new KillSnake();
        return new ConcreteTile(face, interaction);
    }

    public ConcreteTile BuildOutOfBounds(Tile tile)
    {
        SOTileContent face = GetFace("OutOfBounds");
        IInteractionStrategy interaction = new KillSnake();
        return new ConcreteTile(face, tile, interaction);
    }
}
