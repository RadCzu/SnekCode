using System.Reflection;
using UnityEditor;

public class EatFood : IInteractionStrategy
{
  public Observer onEaten;

  public EatFood(Observer foodObserver) {
    onEaten = foodObserver;
  }

  public void Execute(Snake snake) {
      snake.GrowOnTile(snake.backTile);
      snake.score = snake.score + GameOptions.food_score;
      snake.onScoreIncrease.Notify();
      onEaten.Notify();
  }
}