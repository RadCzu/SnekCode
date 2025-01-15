using System.Reflection;

public class KillSnakeAndReplace : IInteractionStrategy
{
  public ConcreteTile replacement;

  public KillSnakeAndReplace(ConcreteTile replacement) {
    this.replacement = replacement;
  }

  public void Execute(Snake snake) {
    snake.onDeath.Notify();
    snake.head.GetTile().SetContent(replacement);
    return;
  }
}