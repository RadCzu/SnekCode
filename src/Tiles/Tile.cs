using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{
    private readonly int x;
    private readonly int y;

    private ITileable content;
    public Tile(int x, int y, ITileable tileable) {
        this.x = x;
        this.y = y;
        content = tileable;
    }

    public void SetContent(ITileable newContent) {
        content = newContent;
        newContent.SetTile(this);
    }

    public ITileable GetContent() {
        return this.content;
    }
        
    public Vector2Int GetPosition() {
        return new Vector2Int(x, y);
    }

}
