using UnityEngine;

public class HeadInteraction : IInteractionStrategy
{
    private Snake mySnake;

    public HeadInteraction(Snake mySnake)
    {
        this.mySnake = mySnake;
    }

    public void Execute(Snake snake)
    {
        // If mySnake hasn't moved
        if (!mySnake.moved)
        {
            if (mySnake.head.next != null)
            {
                // If mySnake has a next part, the other snake dies on collision
                snake.onDeath.Notify();
                snake.head.GetTile().SetContent(mySnake.head);
                snake.head.SetTile(null);
            }
            else
            {
                Vector2Int dirDiff = snake.direction - mySnake.direction;
                if (dirDiff == Vector2Int.zero) // Both snakes are stationary and overlapping
                {
                    snake.onDeath.Notify();
                    mySnake.onDeath.Notify();
                    mySnake.head.GetTile().SetContent(null);
                    snake.head.GetTile().SetContent(null);
                }
                return;
            }
        }
        else
        {
            // Handle regular collisions for moving snakes
            mySnake.onDeath.Notify();
            mySnake.head.GetTile().SetContent(null);
            snake.onDeath.Notify();
            snake.head.GetTile().SetContent(null);
        }
    }
}
