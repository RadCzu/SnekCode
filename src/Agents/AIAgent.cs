using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; // Assuming UnityEngine for Vector2 and similar utilities
using Unity.Barracuda; // Barracuda namespaces for Tensor handling

public class AIAgent : SnakeAgent
{
    private SnakeBrainAdapter brainNetwork;
    private int moveMemory;
    private List<(float, float, float, float, int, bool)> moveHistory = new();
    private IMovePickingStrategy movePickingStrategy;

    public AIAgent(SnakeBrainAdapter brainNetwork, int moveMemory = 20, IMovePickingStrategy movePickingStrategy = null)
    {
        this.brainNetwork = brainNetwork;
        this.moveMemory = moveMemory;
        this.movePickingStrategy = movePickingStrategy ?? new PickBest();
        ClearHistory();
    }

    public override void OnInit(Game game)
    {
        snake.onDeath.Subscribe(() => ClearHistory());
        snake.onDeath.Subscribe(() => {brainNetwork.Dispose();});
    }

    public override void MakeDecision(Game game)
    {
        var possibleMoves = new List<float[]>
        {
            new[] { 1.0f, 0, 0, 0 },
            new[] { 0, 1.0f, 0, 0 },
            new[] { 0, 0, 0, 1.0f },
            new[] { 0, 0, 1.0f, 0 },
        };

        var (map64Tensor, fragmentTensor, deathTensor, closestFoodTensor, historyTensor) = GetGameData(game);

        var qValues = new List<float>();

        foreach (var moveDir in possibleMoves)
        {
            // Directly creating a Barracuda Tensor from the array
            var moveTensor =  new Tensor(new int[] { 1, 4, }, moveDir);

            float qValue = brainNetwork.Run(
                map64Tensor,
                fragmentTensor,
                deathTensor,
                closestFoodTensor,
                historyTensor,
                moveTensor
            );

            qValues.Add(qValue);
        }

        // string concat = "";
        // foreach (var qValue in qValues)
        // {
        //     concat = concat + qValue + ", ";
        // }
        // Debug.Log($"Q_Values: {concat}");
        
        int bestMoveIndex = movePickingStrategy.PickMove(qValues.ToArray());
        var bestMove = possibleMoves[bestMoveIndex];

                map64Tensor.Dispose();
                fragmentTensor.Dispose();
                deathTensor.Dispose();
                closestFoodTensor.Dispose();
                historyTensor.Dispose();

        AddToHistory(bestMove);
        game.Input(bestMoveIndex, snake);
    }

    public override void OnGameOver(Game game)
    {
        // Do nothing for now
    }

    public override void ForceMove(Game game)
    {
        snake.Move();
    }

    private void AddToHistory(float[] move)
    {
        var previousMove = moveHistory[0];
        var previousMoveVector = (previousMove.Item1, previousMove.Item2, previousMove.Item3, previousMove.Item4);
        bool previousMoveReal = previousMove.Item6;

        bool areMoveDirsTheSame =
            previousMoveVector.Item1 == move[0] &&
            previousMoveVector.Item2 == move[1] &&
            previousMoveVector.Item3 == move[2] &&
            previousMoveVector.Item4 == move[3];

        if (areMoveDirsTheSame && previousMoveReal)
        {
            var updatedMove = (
                previousMove.Item1,
                previousMove.Item2,
                previousMove.Item3,
                previousMove.Item4,
                previousMove.Item5 + 1,
                previousMove.Item6
            );
            moveHistory[0] = updatedMove;
        }
        else
        {
            if (moveHistory.Count >= moveMemory)
                moveHistory.RemoveAt(moveHistory.Count - 1);

            moveHistory.Insert(0, (move[0], move[1], move[2], move[3], 1, true));
        }
    }

    private (Tensor, Tensor, Tensor, Tensor, Tensor) GetGameData(Game game)
    {
        // Resized Map (64x64, 3 channels)
        var norm_state = Map.NormalizeFragment(game.map.GetMapState());
        var resizedMap = Map.ResizeFragment(norm_state, 64, 64);
        var map64Data = resizedMap.SelectMany(row => row.SelectMany(cell => cell)).ToArray();
        var map64Tensor = new Tensor(new int[] { 1, 64, 64, 3 }, map64Data);

        // Map Size
        var mapSize = game.map.GetMapSize();

        // Snake Position (2 elements)
        var snakePosition = snake.head.GetTile().GetPosition();
        var snakeXFloat = (float)snakePosition.x / mapSize.x;
        var snakeYFloat = (float)snakePosition.y / mapSize.y;
        var snakePositionData = new float[] { snakeXFloat, snakeYFloat };

        // Map Fragment Normalization (15x15, 3 channels)
        var fragment = game.map.GetMapFragment(
            snakePosition.x - 7, snakePosition.y - 7,
            snakePosition.x + 7, snakePosition.y + 7
        );
        var normalizedFragment = Map.NormalizeFragment(fragment);
        var fragmentData = normalizedFragment.SelectMany(row => row.SelectMany(cell => cell)).ToArray();
        var fragmentTensor = new Tensor(new int[] { 1, 15, 15, 3 }, fragmentData);
        
        var (leftDeath, rightDeath, upDeath, downDeath) = snake.GetDistanceFromDeadly();
        var deathData = new float[]
        {
            (float)leftDeath / mapSize.x,
            (float)rightDeath / mapSize.x,
            (float)upDeath / mapSize.y,
            (float)downDeath / mapSize.y
        };
        var deathTensor = new Tensor(new int[] { 1, 4 }, deathData);

        var closestFood = game.GetNClosestFoodItems(5, snakePosition.x, snakePosition.y);
        var closestFoodPositions = closestFood.Select(food =>
        {
            var foodPosition = food.GetTile().GetPosition();
            return new float[]
            {
                (foodPosition.x - snakePosition.x) / (float)mapSize.x,
                (foodPosition.y - snakePosition.y) / (float)mapSize.y,
                1
            };
        }).ToList();

        while (closestFoodPositions.Count < 5)
            closestFoodPositions.Add(new float[] { 0f, 0f, 0f });

        var closestFoodData = closestFoodPositions.SelectMany(x => x).ToArray();
        var closestFoodTensor = new Tensor(new int[] { 1, 15 }, closestFoodData);

        var historyData = moveHistory.SelectMany(h => new float[] { h.Item1, h.Item2, h.Item3, h.Item4, h.Item5, h.Item6 ? 1.0f : 0 }).ToArray();
        var historyTensor = new Tensor(new int[] { 1, moveHistory.Count * 6 }, historyData);

        return (map64Tensor, fragmentTensor, deathTensor, closestFoodTensor, historyTensor);
    }

    private void ClearHistory()
    {
        moveHistory.Clear();
        for (int i = 0; i < moveMemory; i++)
        {
            moveHistory.Add((0, 0, 0, 0, 0, false));
        }
    }
}
