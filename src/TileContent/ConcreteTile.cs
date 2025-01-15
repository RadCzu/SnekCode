using UnityEngine;

public class ConcreteTile : ITileable
{
    private SOTileContent self = null;
    private ConcreteTile content = null;
    private Tile tile = null;
    public IInteractionStrategy interactionMethod = null;

    public ConcreteTile(SOTileContent self, ConcreteTile content, Tile tile, IInteractionStrategy interaction) {
        this.self = self;
        this.content = content;
        this.tile = tile;
        interactionMethod = interaction;
        tile.SetContent(this);
    }

    public ConcreteTile(SOTileContent self, Tile tile, IInteractionStrategy interaction) {
        this.self = self;
        this.tile = tile;
        interactionMethod = interaction;
        tile.SetContent(this);
    }

    public ConcreteTile(SOTileContent self, ConcreteTile content, IInteractionStrategy interaction) {
        this.self = self;
        this.content = content;
        interactionMethod = interaction;
    }

    public ConcreteTile(SOTileContent self, Tile tile) {
        this.self = self;
        this.tile = tile;
        tile.SetContent(this);
    }

    public ConcreteTile(SOTileContent self) {
        this.self = self;
    }

    public ConcreteTile(SOTileContent self, IInteractionStrategy interaction) {
        this.self = self;
        interactionMethod = interaction;
    }
  
    public string GetName()
    {
        return self.tileName;
    }

    public ITileable GetContent()
    {
        return content;
    }

    public Tile GetTile()
    {
        return tile;
    }

    public void SetTile(Tile tile)
    {
        this.tile = tile;
    }


    public void Interact(Snake snake)
    {
        interactionMethod.Execute(snake);
    }

    public override string ToString()
    {
        return self.representation;
    }

    public int ToInt()
    {
        return self.tileValue;
    }

    public float[] ToNumbers()
    {
        return self.numbers;
    }

    public bool IsDeadly()
    {
        return self.isDeadly;
    }

    public ITileable Clone()
    {
        return new ConcreteTile(self, interactionMethod);
    }
}

