using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour
{
    private float time;
    private int initLength = 9;
    private UIManagerScript uiScript;

    public static int w = 20;
    public static int h = 11;
    public static STATE state = STATE.Start;

    public static List<SnakeNode> snakeNodes = new List<SnakeNode>();
    public Vector2 direction = -Vector2.right;
    public GameObject[,] grid = new GameObject[w, h];
    public GameObject node;
    public enum STATE { Start, Gaming, GameOver, Pause };
    public static int score = 0;
    public static int level = 1;

    public struct SnakeNode
    {
        public Vector2 pos;
        public Vector2 dir;
    }


    void Start()
    {
        uiScript = GetComponent<UIManagerScript>();

        for (int i = 0; i < initLength; ++i)
        {
            EatFood();
        }
    }

    void Update()
    {
        if (state == STATE.Gaming)
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = STATE.Pause;
            }

            if (time > 0.2f)
            {
                SnakeMove(direction);
                time = 0;
            }
            time += Time.deltaTime;
        }
        else if (state == STATE.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                state = STATE.Gaming;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameOver();
            }
        }
        else if (state == STATE.GameOver)
        {
            if (Input.anyKeyDown)
            {
                ReturnMenu();
            }
        }
    }

    // 增加贪吃蛇的长度
    private void EatFood()
    {
        SnakeNode lastNode;
        if (snakeNodes.Count <= 0)
        {
            lastNode.pos = new Vector2(4, 2);
            lastNode.dir = direction;
            snakeNodes.Add(lastNode);
        }
        else
        {
            lastNode.pos = snakeNodes[snakeNodes.Count - 1].pos - snakeNodes[snakeNodes.Count - 1].dir;
            lastNode.dir = snakeNodes[snakeNodes.Count - 1].dir;
            snakeNodes.Add(lastNode);
        }

        grid[Mathf.RoundToInt(lastNode.pos.x), Mathf.RoundToInt(lastNode.pos.y)] = (GameObject)Instantiate(node, new Vector3(lastNode.pos.x, lastNode.pos.y, 0f), Quaternion.identity);

        // 计分
        if (snakeNodes.Count > initLength)
        {
            score = (snakeNodes.Count - initLength) * level;

            if (snakeNodes.Count == w * h)
            {
                score += 100;
                GameOver();
            }
        }
    }

    // 贪吃蛇移动
    private void SnakeMove(Vector2 dir)
    {
        if (CheckCollision(new Vector2(snakeNodes[0].pos.x + dir.x, snakeNodes[0].pos.y + dir.y)))
        {
            GameOver();
            return;
        }
        CheckFood(new Vector2(snakeNodes[0].pos.x + dir.x, snakeNodes[0].pos.y + dir.y));

        SnakeNode snakeNode;
        snakeNode.pos = snakeNodes[0].pos + dir;
        snakeNode.dir = dir;
        snakeNodes.Insert(0, snakeNode);

        grid[Mathf.RoundToInt(snakeNode.pos.x), Mathf.RoundToInt(snakeNode.pos.y)] = (GameObject)Instantiate(node, new Vector3(snakeNode.pos.x, snakeNode.pos.y, 0f), Quaternion.identity);
        Destroy(grid[Mathf.RoundToInt(snakeNodes[snakeNodes.Count - 1].pos.x), Mathf.RoundToInt(snakeNodes[snakeNodes.Count - 1].pos.y)]);
        snakeNodes.RemoveAt(snakeNodes.Count - 1);
    }

    // 检测是否碰撞到了墙壁和自己的身体
    private bool CheckCollision(Vector2 vec)
    {
        if ((Mathf.RoundToInt(vec.x) >= w || Mathf.RoundToInt(vec.x) < 0) || (Mathf.RoundToInt(vec.y) >= h || Mathf.RoundToInt(vec.y) < 0))
        {
            Debug.Log("碰到墙壁了" + vec);
            return true;
        }

        foreach (SnakeNode node in snakeNodes)
        {
            if (vec == node.pos)
            {
                Debug.Log("碰到身体了" + node);
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
            EatFood();
            Destroy(GameObject.FindGameObjectWithTag("food"));
        }
    }

    // 游戏开始函数
    public void GameStart()
    {
        state = STATE.Gaming;
        uiScript.ShowStartPanel(false);

        // 重置所有数据
        score = 0;
        snakeNodes.Clear();
        for (int i = 0; i < w; ++i)
        {
            for (int j = 0; j < h; ++j)
            {
                if (grid[i, j] != null)
                {
                    Destroy(grid[i, j]);
                }
            }
        }

        direction = -Vector2.right;

        for (int i = 0; i < initLength; ++i)
        {
            EatFood();
        }
    }

    // 游戏结束函数
    public void GameOver()
    {
        state = STATE.GameOver;
        uiScript.ShowGameOverPanel(true);
    }

    // 回到主界面
    public void ReturnMenu()
    {
        state = STATE.Start;
        uiScript.ShowGameOverPanel(false);
        uiScript.ShowRankPanel(false);
        uiScript.ShowStartPanel(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
