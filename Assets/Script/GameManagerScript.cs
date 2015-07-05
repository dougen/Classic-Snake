using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{
    public static int w = 20;
    public static int h = 11;
    public static STATE state = STATE.Start;
    public static List<Vector2> snake = new List<Vector2>();
    public Vector2 direction = -Vector2.right;
    public GameObject[,] grid = new GameObject[w, h];
    public GameObject node;
    public enum STATE { Start, Gaming, GameOver };
    public static int score = 0;

    private float time;
    

    void Start()
    {
        for (int i = 0; i < 9; ++i)
        {
            EatFood(new Vector2(4 + i, 2));
        }
    }

    void Update()
    {
        if (state == STATE.GameOver)
        {
            return;
        }
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

        if (time > 0.2f)
        {
            SnakeMove(direction);
            time = 0;
        }
        time += Time.deltaTime;
    }

    // 增加贪吃蛇的长度
    private void EatFood(Vector2 vec)
    {
        snake.Insert(0, vec);
        grid[Mathf.RoundToInt(snake[0].x), Mathf.RoundToInt(snake[0].y)] = (GameObject)Instantiate(node, new Vector3(vec.x, vec.y, 0f), Quaternion.identity);
    }

    // 贪吃蛇移动
    private void SnakeMove(Vector2 dir)
    {
        if (CheckCollision(new Vector2(snake[0].x + dir.x, snake[0].y + dir.y)))
        {
            GameOver();
            return;
        }
        CheckFood(new Vector2(snake[0].x + dir.x, snake[0].y + dir.y));
        snake.Insert(0, new Vector2(snake[0].x + dir.x, snake[0].y + dir.y));
        grid[Mathf.RoundToInt(snake[0].x), Mathf.RoundToInt(snake[0].y)] = (GameObject)Instantiate(node, new Vector3(snake[0].x, snake[0].y, 0f), Quaternion.identity);
        Destroy(grid[Mathf.RoundToInt(snake[snake.Count - 1].x), Mathf.RoundToInt(snake[snake.Count - 1].y)]);
        snake.RemoveAt(snake.Count - 1);
    }

    // 检测是否碰撞到了墙壁和自己的身体
    private bool CheckCollision(Vector2 vec)
    {
        if ((Mathf.RoundToInt(vec.x) >= w || Mathf.RoundToInt(vec.x) < 0) || (Mathf.RoundToInt(vec.y) >= h || Mathf.RoundToInt(vec.y) < 0))
        {
            Debug.Log("碰到墙壁了");
            return true;
        }
        
        foreach(Vector2 node in snake)
        {
            if (vec.x == node.x && vec.y == node.y)
            {
                Debug.Log("碰到身体了");
                return true;
            }
        }

        return false;
    }

    // 检查是否吃到了食物
    private void CheckFood(Vector2 vec)
    {
        if (vec == FoodSwapScript.foodPos)
        {
            EatFood(FoodSwapScript.foodPos);
            Destroy(GameObject.FindGameObjectWithTag("food"));
        }
    }

    // 游戏结束函数
    public void GameOver()
    {
        state = STATE.GameOver;
    }
}
