public class NoAgent: SnakeAgent
{

    public override int GetCookies()
    {
        return 0;
    }

    public override void MakeDecision(Game game)
    {

    }

    public override void Validate(Game game)
    {

    }

    public override void OnGameOver(Game game)
    {

    }

    public override void OnInit(Game game)
    {
        
    }

    public override void ForceMove(Game game)
    {
        snake.Move();
    }
}