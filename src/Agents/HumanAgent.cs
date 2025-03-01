using UnityEngine;

public class HumanAgent: SnakeAgent
{

    private ArrowKeyController arrowKeyController;
    [SerializeField]

    public HumanAgent(ArrowKeyController controller) {
      arrowKeyController = controller;


    }

    void MoveUp(Game game)
    {
        game.Input(2, snake);
    }

    void MoveDown(Game game)
    {
        game.Input(3, snake);
    }

    void MoveLeft(Game game)
    {
        game.Input(0, snake);
    }

    void MoveRight(Game game)
    {
         game.Input(1, snake);
    }


    public override void MakeDecision(Game game)
    {

    }

    public override void OnGameOver(Game game)
    {

    }

    public override void OnInit(Game game)
    {
      arrowKeyController.OnUpArrowPress = () => MoveUp(game);
      arrowKeyController.OnDownArrowPress = () => MoveDown(game);
      arrowKeyController.OnLeftArrowPress = () =>  MoveLeft(game);
      arrowKeyController.OnRightArrowPress = () =>  MoveRight(game);
    }

    public override void ForceMove(Game game)
    {
        snake.Move();
    }
}