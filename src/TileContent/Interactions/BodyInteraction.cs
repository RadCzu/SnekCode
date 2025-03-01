using System;

public class BodyInteraction : IInteractionStrategy
{
    private Snake mySnake;
    public SnakePart self;

    public BodyInteraction(Snake mySnake, SnakePart self)
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
        if (snake == mySnake)
        {
            if (mySnake.length <= 2 || self.GetSecondToLast() == self)
            {
                snake.onDeath.Notify();
                snake.head.GetTile().SetContent(self);
                return;
            }
        }

        if (mySnake.moved || self.next != null)
        {
            snake.onDeath.Notify();
            snake.head.GetTile().SetContent(self);
        }
    }
}
    
