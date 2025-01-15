using System.Reflection;

public class Nothing : IInteractionStrategy
{
  public void Execute(Snake snake) {
    return;
  }
}