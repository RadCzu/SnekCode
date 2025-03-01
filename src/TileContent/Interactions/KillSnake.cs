using System.Reflection;

public class KillSnake : IInteractionStrategy
{
  public void Execute(Snake snake) {
    snake.onDeath.Notify();
  }
}