using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Main
{
    private int gameWidth;
    private int gameHeight;
    private int? plusSize;
    private string mapType;
    private List<SnakeAgent> agents;
    private List<Vector2Int> agentPositions;
    private double foodDensity;
    public int gameLength;
    public int turn;
    public Game game;

    public Main((int, int, int?) mapParams, string mapType, List<(SnakeAgent, int, int)> agents, int gameLength = 0, double foodDensity = 0.1)
    {
        gameWidth = mapParams.Item1;
        gameHeight = mapParams.Item2;
        plusSize = mapParams.Item3;
        this.mapType = mapType;
        this.foodDensity = foodDensity;
        this.agents = new List<SnakeAgent>();
        agentPositions = new();
        this.gameLength = gameLength;
        turn = 0;

        foreach (var agentInfo in agents)
        {
            var agent = agentInfo.Item1;
            this.agents.Add(agent);
            agentPositions.Add(new Vector2Int(agentInfo.Item2, agentInfo.Item3));
        }
    }

    private Game SetUp()
    {
        turn = 0;
        Map theMap;

        if (mapType == "cross" && plusSize != null)
        {
            theMap = CrossMapFactory.Build(gameWidth, gameHeight, (int)plusSize);
        }
        else if (mapType == "grid")
        {
            theMap = GridMapFactory.Build(gameWidth, gameHeight);
        }
        else
        {
            theMap = BoxMapFactory.Build(gameWidth, gameHeight);
        }
        var game = new Game(theMap, agentPositions, foodDensity);

        game.Begin();

        for(int i = 0; i < game.snakes.Count; i++)
        {
            agents[i].SetSnake(game.snakes[i]);
            agents[i].OnInit(game);
        }

        return game;
    }

    public void Init() {
        game = SetUp();
    }

    public void Run()
    {
        game = SetUp();

        int living = agents.Count;

        while (!game.over && living > 0) {
            living = agents.Count;
            foreach (var agent in agents) {   
                agent.snake.moved = false;
            }

            if (gameLength != 0 && turn >= gameLength) {
                game.GameOver();
            }

            turn++;

            foreach (var agent in agents) {
                if (agent.snake.dead) {
                    living--;
                    continue;
                }
                agent.MakeDecision(game);
            }
            game.snakeEliminator.Notify();
            game.snakeEliminator = new();
        }

        foreach (var agent in agents) {
            agent.OnGameOver(game);
        }
    }

    public void Tick()
    {

        int living = agents.Count;

        foreach (var agent in agents) {   
            agent.snake.moved = false;
        }

        if (gameLength != 0 && turn >= gameLength) {
            game.GameOver();
        }

        turn++;

        foreach (var agent in agents) {
            if (agent.snake.dead) {
                living--;
                continue;
            }
            agent.MakeDecision(game);
            agent.ForceMove(game);
        }

        game.snakeEliminator.Notify();
        game.snakeEliminator = new();
    }

}
