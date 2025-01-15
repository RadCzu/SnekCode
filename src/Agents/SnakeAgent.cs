public abstract class SnakeAgent
{
    public Snake snake;

    public void SetSnake(Snake snake)
    {
        this.snake = snake;
    }

    public abstract int GetCookies();
    public abstract void MakeDecision(Game game);
    public abstract void Validate(Game game);
    public abstract void OnGameOver(Game game);
    public abstract void OnInit(Game game);
    public abstract void ForceMove(Game game);
}