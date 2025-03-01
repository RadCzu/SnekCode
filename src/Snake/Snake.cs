
using System;
using UnityEngine;

public class Snake {
    public int score;
    public bool dead = false;
    public int length;
    public bool moved = false;
    public Vector2Int direction = new Vector2Int(1, 0);
    public Vector2Int previousDirection = new Vector2Int(0, 0);
    public Tile backTile = null;
    public Observer onScoreIncrease = new();
    public Observer onMove = new();
    public Observer onDeath = new();
    public Observer onEatFood = new();
    public SnakePart head;
    public Map map;

    public Snake(int x, int y, Map map){
        TileBuilder tileBuilder = new();
        head = tileBuilder.BuildSnakeHead(this, map.GetTile(x, y));
        map.GetTile(x, y).SetContent(head);
        this.map = map;
        length = 1;
        score = 0;
        
        onDeath.Subscribe(() => {this.dead = true;} );
    }

    public void ValidateSnake()
    {
        SnakePart current = head;
        string snakePositions = "Snake positions: [";
        bool isMalformed = false;

        while (current != null)
        {
            Vector2Int currentPos = current.GetTile().GetPosition();
            snakePositions += $"{currentPos}, ";

            if (current.next != null)
            {
                Vector2Int nextPos = current.next.GetTile().GetPosition();

                // Calculate the Manhattan distance
                int distance = Mathf.Abs(currentPos.x - nextPos.x) + Mathf.Abs(currentPos.y - nextPos.y);

                if (distance != 1)
                {
                    Debug.LogError($"Snake is malformed: Distance between {currentPos} and {nextPos} is {distance}");
                    isMalformed = true;
                }
            }

            current = current.next;
        }

        // Remove trailing comma and space, and close the brackets
        if (snakePositions.EndsWith(", "))
        {
            snakePositions = snakePositions.Substring(0, snakePositions.Length - 2);
        }
        snakePositions += "]";

        Debug.Log(snakePositions);

        if (!isMalformed)
        {
            Debug.Log("SNAKE IS CORRECT");
        }
    }

    public void Move()
    {
        //ValidateSnake();
        onMove.Notify();
        Tile headTile = head.GetTile();
        Vector2Int position = headTile.GetPosition();
        Tile nextTile = map.GetTile(position.x + direction.x, position.y + direction.y);
        if (nextTile == null){
            return;
        }
        ITileable content = nextTile.GetContent();
        SnakePart lastPart = head.GetLast();
        backTile = lastPart.GetTile();
        previousDirection = direction;
        map.MoveTileByCoordinates(position.x, position.y, position.x + direction.x, position.y + direction.y);
        
        if (lastPart != head){
            Vector2Int lastPartPosition = lastPart.GetTile().GetPosition();
            map.MoveTileByCoordinates(lastPartPosition.x, lastPartPosition.y, position.x, position.y);
            SnakePart second2Last = head.GetSecondToLast();

            if (second2Last == head) {
                content.Interact(this);
                return;
            }

            lastPart.next = head.next;
            lastPart.previous = head;
            head.next.previous = lastPart;
            head.next = lastPart;
            second2Last.next = null;
            
            // Debug.Log($"======================================");
            // Debug.Log($"New part positions for length {length}");
            // Debug.Log($"Head: {head.GetTile().GetPosition()} Head's next: {head.next?.GetTile().GetPosition() ?? null}");
            // Debug.Log($"Second: {lastPart.GetTile().GetPosition()} Seconds's next: {lastPart.next?.GetTile().GetPosition() ?? null}");
            // Debug.Log($"Last: {second2Last.GetTile().GetPosition()} Last's next: {second2Last.next?.GetTile()?.GetPosition() ?? null}");

        }

        content.Interact(this);
        moved = true;
    }

    public void GrowOnTile(Tile tile)
    {
        SnakePart lastPart = head.GetLast();
        TileBuilder tileBuilder = new();
        SnakePart newBody = tileBuilder.BuildSnakeBody(this, tile);
        tile.SetContent(newBody);
        lastPart.next = newBody;
        newBody.previous = lastPart;
        onEatFood.Notify();
        length += 1;
    }

    public void Destroy() {
        dead = true;
        TileBuilder tileBuilder = new();
        SnakePart c = head;
        while (c != null) {
            ConcreteTile empty = tileBuilder.BuildEmpty();
            try {
                Tile tile = c.GetTile();
                ITileable content = tile.GetContent();
                if (content == c) { 
                    tile.SetContent(empty);
                }
            } catch (Exception e) {
                Debug.LogError("No Tile: " + e.Message);
            }
            c = c.next;
        }
    }

    public (int, int, int, int) GetDistanceFromDeadly()
    {
        return (
            GetDeadlyLeft(),
            GetDeadlyRight(),
            GetDeadlyUp(),
            GetDeadlyDown()
        );
    }

    private int GetDeadlyRight()
    {
        Vector2Int position = head.GetTile().GetPosition();
        int distance = 0;

        while (true)
        {
            Tile tile = map.GetTile(position.x, position.y + distance + 1);
            if (!tile.GetContent().IsDeadly())
            {
                distance++;
            }
            else
            {
                return distance;
            }
        }
    }

    private int GetDeadlyLeft()
    {
        Vector2Int position = head.GetTile().GetPosition();
        int distance = 0;

        while (true)
        {
            Tile tile = map.GetTile(position.x, position.y - distance - 1);
            if (!tile.GetContent().IsDeadly())
            {
                distance++;
            }
            else
            {
                return distance;
            }
        }
    }

    private int GetDeadlyUp()
    {
        Vector2Int position = head.GetTile().GetPosition();
        int distance = 0;

        while (true)
        {
            Tile tile = map.GetTile(position.x - distance - 1, position.y);
            if (!tile.GetContent().IsDeadly())
            {
                distance++;
            }
            else
            {
                return distance;
            }
        }
    }

    private int GetDeadlyDown()
    {
        Vector2Int position = head.GetTile().GetPosition();
        int distance = 0;

        while (true)
        {
            Tile tile = map.GetTile(position.x + distance + 1, position.y);
            if (!tile.GetContent().IsDeadly())
            {
                distance++;
            }
            else
            {
                return distance;
            }
        }
    }

}