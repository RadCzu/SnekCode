using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class SnakePart : ConcreteTile
{
  public SnakePart next = null;
  public SnakePart previous = null;
  public SnakePart(SOTileContent self, IInteractionStrategy interaction) : base(self, interaction)
  {
  }

  public SnakePart(SOTileContent self, Tile tile, IInteractionStrategy interaction) : base(self, tile, interaction)
  {
  }

  public SnakePart(SOTileContent self, ConcreteTile content, IInteractionStrategy interaction) : base(self, content, interaction)
  {
  }

  public SnakePart(SOTileContent self, Tile tile) : base(self, tile)
  {
  }

  public SnakePart(SOTileContent self) : base(self)
  {
  }


  public SnakePart(SOTileContent self, ConcreteTile content, Tile tile, IInteractionStrategy interaction) : base(self, content, tile, interaction)
  {
  }

  public SnakePart GetLast() {
      if (next == null){
          return this;
      } else {
          return next.GetLast();
      }
  }

  public SnakePart GetSecondToLast() {
      if (next == null){
        return this;
      } else {
        return next.next == null ? this : next.GetSecondToLast();
      }
  }

    /// <summary>
    /// Determine if this SnakePart is part of the given snake.
    /// </summary>
    /// <param name="snake">The snake object whose parts we want to check.</param>
    /// <returns>True if this part belongs to the snake, False otherwise.</returns>
    public bool IsPartOfSnake(Snake snake)
    {
        var current = snake.head;
        while (current != null)
        {
            if (current == this)
            {
                return true;
            }
            current = current.next;
        }
        return false;
    }
}


