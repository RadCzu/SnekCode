using UnityEngine;

public class HumanAgent: SnakeAgent
{

    private ArrowKeyController arrowKeyController;
    [SerializeField]

    public HumanAgent(ArrowKeyController controller) {
      arrowKeyController = controller;

      arrowKeyController.OnUpArrowPress = MoveUp;
      arrowKeyController.OnDownArrowPress = MoveDown;
      arrowKeyController.OnLeftArrowPress = MoveLeft;
      arrowKeyController.OnRightArrowPress = MoveRight;
    }

    void MoveUp()
    {
        snake.direction = Vector2Int.down;
    }

    void MoveDown()
    {
        snake.direction = Vector2Int.up;
    }

    void MoveLeft()
    {
        snake.direction = Vector2Int.left;
    }

    void MoveRight()
    {
        snake.direction = Vector2Int.right;
    }

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