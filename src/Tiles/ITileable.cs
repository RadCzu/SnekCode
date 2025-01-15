using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public interface ITileable
{
    string GetName();

    ITileable GetContent();

    Tile GetTile();

    void SetTile(Tile tile);

    void Interact(Snake snake);

    string ToString();

    int ToInt();

    bool IsDeadly();

    float[] ToNumbers();

    ITileable Clone();
}
