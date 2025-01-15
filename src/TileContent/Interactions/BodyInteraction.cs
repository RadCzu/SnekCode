using System;

public class BodyInteraction : IInteractionStrategy
{
    private Snake mySnake;
    public ConcreteTile self;

    public BodyInteraction(Snake mySnake, ConcreteTile self)
    {
        this.mySnake = mySnake;
        this.self = self;
    }

    public BodyInteraction(Snake mySnake)
    {
        this.mySnake = mySnake;
    }

    public void Execute(Snake snake)
    {
        // If mySnake hasn't moved and has a body (not just head)
        if (!mySnake.moved && mySnake.head.next != null)
        {
            
            snake.onDeath.Notify(); // The other snake dies on collision
        }

        // Check if this body part is not part of the other snake
        if (!mySnake.head.IsPartOfSnake(snake))
        {
            snake.head.GetTile().SetContent(self);
            snake.head.SetTile(null); // Clear the tile
        }
        else
        {
           // It is
            snake.onDeath.Notify(); // The other snake dies on collision
        }
    }
}
