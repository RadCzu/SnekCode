using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private Main main;
    public MapUpdater mapUpdater;
    public float timer;
    public float tickTime;
    public float tickTimeLimit;
    public float tickTimeDecay;
    public ArrowKeyController arrowKeyController;
    public AudioManager audioManager;
    public TextMeshProUGUI scoreText;

    public UIFadeAnimator gameOverScreen;
    public void Start()
    {
        int width = SingleplayerData.mapWidth;
        int height = SingleplayerData.mapHeight;
        int? plusSize = SingleplayerData.plusSize;
        string mapType = SingleplayerData.mapType;

        List<(SnakeAgent, int, int)> agents = new()
        {
            (new HumanAgent(arrowKeyController), width / 2, height / 2)
        };

        main = new Main((width, height, plusSize), mapType, agents);
        main.Init();
        Debug.Log($"Game initiation complete");

        Snake snake = main.game.snakes[0];
        snake.direction = new(0, 1);
        Vector2Int tailPos = snake.head.GetTile().GetPosition() - new Vector2Int(0, 1);
        snake.GrowOnTile(main.game.map.GetTile(tailPos.x, tailPos.y));
        snake.onEatFood.Subscribe(() => audioManager.Play("Eat"));

        snake.onEatFood.Subscribe(() => {if (tickTime > tickTimeLimit ) tickTime -= tickTimeDecay;});
        snake.onDeath.Subscribe(() => {
            gameOverScreen.FadeInUI(3f, 1f, 0.7f);
            audioManager.Play("Die");
            scoreText.text = $"Score: {snake.score}";
        });

        UpdateScreen(snake);
    }

    public void TurnTick(int snakeId) {
        main.Tick();
        UpdateScreen(main.game.snakes[snakeId]);
    }

    public void UpdateScreen(Snake snake) {
        Vector2Int snakeHeadPos = snake.head.GetTile().GetPosition();

        Vector2Int mapStart = new Vector2Int(snakeHeadPos.x - 9, snakeHeadPos.y - 7);
        Vector2Int tilemapStart = new Vector2Int(-9, 7);

        if (mapUpdater != null)
        {
            Vector2Int size = new Vector2Int(19, 13);
            mapUpdater.Fill(tilemapStart, size, main.game.map, mapStart, snake);
        }
        else
        {
            Debug.LogError("MapUpdater component not found!");
        }
    }

    void Update() {
        timer += Time.deltaTime;
        if (timer > tickTime) {
            timer = 0;
            TurnTick(0);
        }
    }

}
