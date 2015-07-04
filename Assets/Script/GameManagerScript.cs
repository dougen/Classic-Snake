using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{
    public static int w = 20;
    public static int h = 11;
    public List<Vector2> snake = new List<Vector2>();
    public Vector2 direction = -Vector2.right;
    public GameObject[,] grid = new GameObject[w, h];
    public GameObject node;

    private float time;

    void Start()
    {
        for (int i = 0; i < 16; ++i)
        {
            EatFood(new Vector2(4 + i, 2));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != -Vector2.up)
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up)
        {
            direction = -Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right)
        {
            direction = -Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != -Vector2.right)
        {
            direction = Vector2.right;
        }

        if (time > 0.1f)
        {
            SnakeMove(direction);
            time = 0;
        }
        time += Time.deltaTime;
    }

    void UpdateSnake()
    {
        foreach (Vector2 vec in snake)
        {
            int vecw = Mathf.RoundToInt(vec.x);
            int vech = Mathf.RoundToInt(vec.y);
            if (grid[vecw, vech] == null)
                grid[vecw, vech] = (GameObject)Instantiate(node, new Vector3(vec.x, vec.y, 0f), Quaternion.identity);
        }
    }

    private void EatFood(Vector2 vec)
    {
        snake.Add(vec);
        grid[Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y)] = (GameObject)Instantiate(node, new Vector3(vec.x, vec.y, 0f), Quaternion.identity);
    }

    private void SnakeMove(Vector2 dir)
    {
        Vector2 first = snake[0];
        snake.Insert(0, new Vector2(first.x+dir.x, first.y+dir.y));
        if (CheckCollision())
        {
            GameOver();
        }
        grid[Mathf.RoundToInt(snake[0].x), Mathf.RoundToInt(snake[0].y)] = (GameObject)Instantiate(node, new Vector3(snake[0].x, snake[0].y, 0f), Quaternion.identity);
        Destroy(grid[Mathf.RoundToInt(snake[snake.Count - 1].x), Mathf.RoundToInt(snake[snake.Count - 1].y)]);
        snake.RemoveAt(snake.Count - 1);
    }

    private bool CheckCollision()
    {
        Vector2 first = snake[0];
        if (Mathf.RoundToInt(first.x) < 20 && Mathf.RoundToInt(first.x)>=0 && Mathf.RoundToInt(first.y)<10 && Mathf.RoundToInt(first.y) >=0)
        {
            return false;
        }
        return true;
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
    }
}
