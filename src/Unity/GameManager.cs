using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private Main main;
    public MapUpdater mapUpdater;
    public Canvas mainCanvas;
    public float timer;
    public float tickTime;
    public float tickTimeLimit;
    public float tickTimeDecay;
    public ArrowKeyController arrowKeyController;
    public AudioManager audioManager;
    public TextMeshProUGUI scoreText;
    public ScoreManager scoreManager;
    private Snake spectatedSnake;
    public UIFadeAnimator gameOverScreen;
    public void Start()
    {
        if (GameOptions.singleplayer) {
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
            snake.onEatFood.Subscribe(() => {if (tickTime > tickTimeLimit ) tickTime -= tickTimeDecay;});
            snake.onDeath.Subscribe(() => {
                audioManager.Play("Die");
            });

            ScriptableObjectBuilder<SOPlayerPlacement> placements = new("Objects/PlayerPlacements");
            scoreManager.playerScores.Add(new(snake.score, "you", placements.GetObject("P1").playerColor));
            var playerScore = scoreManager.playerScores[0];
            snake.onScoreIncrease.Subscribe(() => {playerScore.setScore(snake.score);});

            SetCameraTo(main.game.snakes.Count - 1);
            
        } else {
            int width = MultiplayerData.mapWidth;
            int height = MultiplayerData.mapHeight;
            int? plusSize = MultiplayerData.plusSize;
            string mapType = MultiplayerData.mapType;
            List<Vector2> snakePos = MultiplayerData.positions;
            List<Color> snakeColors = MultiplayerData.colors;
            bool anyHumans = MultiplayerData.anyHumans;

            SnakeBrainAdapter brain = new SnakeBrainAdapter("NeuralNetworks/snake_brain_retrained_2");

            List<(SnakeAgent, int, int)> agents = new();

            for (int i = 0; i < snakePos.Count - 1; i++) {
                agents.Add((new AIAgent(brain), (int)((width+2)  * snakePos[i].x), (int)((height+2)  * snakePos[i].y)));
            } 

            if (anyHumans) {
                agents.Add((new HumanAgent(arrowKeyController), (int)((width+2) * snakePos[^1].x), (int)((height+2) * snakePos[^1].y)));
            } else {
                agents.Add((new AIAgent(brain, 20, new PickBest()), (int)((width+2) * snakePos[^1].x), (int)((height+2) * snakePos[^1].y)));
            }

            main = new Main((width, height, plusSize), mapType, agents);
            main.Init();
            Debug.Log($"Game initiation complete");
            for (int i = 0; i < main.game.snakes.Count; i++) {
                Snake snake = main.game.snakes[i];
                snake.direction = new(0, 1);
                Vector2Int tailPos = snake.head.GetTile().GetPosition() - new Vector2Int(0, 1);
                snake.GrowOnTile(main.game.map.GetTile(tailPos.x, tailPos.y));
                snake.onEatFood.Subscribe(() => {if (tickTime > tickTimeLimit ) tickTime -= tickTimeDecay;});
                snake.onDeath.Subscribe(() => {
                    audioManager.Play("Die");
                });
                scoreManager.playerScores.Add(new(snake.score, 
                    i==main.game.snakes.Count - 1 ? "you" : $"AI-{i+1}", 
                    snakeColors[i]));
                var playerScore = scoreManager.playerScores[^1];
                snake.onScoreIncrease.Subscribe(() => {playerScore.setScore(snake.score);});
            }
            foreach (var agentInfo in agents)
            {   
                SnakeAgent agent = agentInfo.Item1;
                agent.MakeDecision(main.game);
            }

            spectatedSnake = main.game.snakes[ main.game.snakes.Count - 1];
            
            SetCameraTo(main.game.snakes.Count - 1);
            
            MultiplayerData.Reset();
        }

        scoreManager.CreateScoreboard(new(0,1), new(10, -10), mainCanvas.transform);

    }

    private Action snakeDeathCallback;
    private Action snakeEatFoodCallback;
    private Action cameraChangeCallback;

    public void SetCameraTo(int snakeId)
    {
        Snake snake = main.game.snakes[snakeId];
        
        if (spectatedSnake != null)
        {
            spectatedSnake.onDeath.Unsubscribe(snakeDeathCallback);
            spectatedSnake.onEatFood.Unsubscribe(snakeEatFoodCallback);
        }
        snakeDeathCallback = () => {
            bool foundAliveSnake = false;
            for (int i = main.game.snakes.Count - 1; i >= 0; i--)
            {
                if (!main.game.snakes[i].dead)
                {
                    int newSnakeId = i;
                    cameraChangeCallback = () => {
                        SetCameraTo(newSnakeId);
                    };
                    main.game.snakes[newSnakeId].onMove.Subscribe(cameraChangeCallback);
                    foundAliveSnake = true;
                    break;
                }
            }

            if (!foundAliveSnake)
            {
                gameOverScreen.FadeInUI(3f, 1f, 0.7f);
                scoreText.text = $"Score: {main.game.snakes[main.game.snakes.Count - 1].score}";
            }
        };
        
        snakeEatFoodCallback = () => audioManager.Play("Eat");
        
        snake.onDeath.Subscribe(snakeDeathCallback);
        snake.onEatFood.Subscribe(snakeEatFoodCallback);

        spectatedSnake = snake;
        UpdateScreen(spectatedSnake);
    }


    public void TurnTick() {

        main.Tick();
        UpdateScreen(spectatedSnake);
        scoreManager.UpdateAllScoreboards();
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
            TurnTick();
        }
    }

}
