using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;


public class Game
{
    public Map map;
    public List<Snake> snakes;
    private List<ConcreteTile> foodList;
    private bool active;
    public bool over;
    private double foodDensity;
    public Observer snakeEliminator = new();

    public Game(Map map, List<Vector2Int> snakePositions, double foodDensity = 0.1)
    {
        this.map = map;
        snakes = new List<Snake>();
        foodList = new List<ConcreteTile>();
        active = false;
        over = false;
        this.foodDensity = foodDensity;

        foreach (var snakePosition in snakePositions)
        {
            var snake = new Snake(snakePosition.x, snakePosition.y, this.map);
            snakes.Add(snake);
            snake.onEatFood.Subscribe(() => SpawnRandomFood(1));
            snake.onDeath.Subscribe(() => snakeEliminator.Subscribe(() => KillSnake(snake)));
        }
    }

    public void SpawnRandomFood(int amount)
    {
        TileBuilder tileBuilder = new();
        for (int i = 0; i < amount; i++)
        {
            var emptyTile = GetRandomEmptyTile();
            if (emptyTile != null)
            {
                ConcreteTile food = tileBuilder.BuildFood(emptyTile);
                foodList.Add(food);
                if (food.interactionMethod.GetType() == typeof(EatFood)) {
                  EatFood interaction = (EatFood)food.interactionMethod;
                  interaction.onEaten.Subscribe(() => RemoveFoodFromList(food));
                }

            }
        }
    }

public Tile GetRandomEmptyTile()
{
    var allTiles = map.GetMapState();
    List<Tile> emptys = new();
    
    foreach (List<Tile> row in allTiles) 
    {
        foreach (Tile tile in row)
        {
            if (tile.GetContent().GetName() == "Empty") 
            {
                emptys.Add(tile);
            }
        }
    }

    if (!emptys.Any())
    {
        return null;
    }

    var random = new System.Random();
    return emptys[random.Next(emptys.Count)];
}


    private void RemoveFoodFromList(ConcreteTile foodItem)
    {
        foodList.Remove(foodItem);
    }

    public List<ConcreteTile> GetNClosestFoodItems(int n, float x, float y)
    {
        return foodList
            .Select(food =>
            {
                var position = food.GetTile().GetPosition();
                var distance = Math.Sqrt(Math.Pow(position.x - x, 2) + Math.Pow(position.y - y, 2));
                return (distance, food);
            })
            .OrderBy(item => item.distance)
            .Take(n)
            .Select(item => item.food)
            .ToList();
    }

    public void Begin()
    {
        int foods = (int)(map.GetPlayableWidth() * map.GetPlayableHeight() * foodDensity);
        SpawnRandomFood(foods);
        Start();
    }

    public void Start()
    {
        active = true;
    }

    public void KillSnake(Snake snake) {
        snake.Destroy();
    }

    public void Stop()
    {
        active = false;
    }

    public void GameOver()
    {
        over = true;
        Stop();
    }

    public int GetScore()
    {
        return snakes.Sum(snake => snake.score);
    }

    public void Left(Snake snake)
    {
        if (active)
        {
            snake.direction = new UnityEngine.Vector2Int(-1, 0);
        }
    }

    public void Right(Snake snake)
    {
        if (active)
        {
            snake.direction = new UnityEngine.Vector2Int(1, 0);
        }
    }

    public void Up(Snake snake)
    {
        if (active)
        {
            snake.direction = new UnityEngine.Vector2Int(0, 1);
        }
    }

    public void Down(Snake snake)
    {
        if (active)
        {
            snake.direction = new UnityEngine.Vector2Int(0, -1);
        }
    }

    public void Update(Snake snake)
    {
        if (active)
        {
            snake.Move();
        }
    }

    public void Input(int number)
    {
        if (snakes.Count == 0) return;

        var snake = snakes[0]; // Assuming single-player for simplicity
        switch (number)
        {
            case 0: Left(snake); break;
            case 1: Right(snake); break;
            case 2: Up(snake); break;
            case 3: Down(snake); break;
            default:
                Stop();
                break;
        }
    }
    
}
