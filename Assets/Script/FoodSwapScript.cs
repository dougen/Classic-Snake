using UnityEngine;
using System.Collections.Generic;

public class FoodSwapScript : MonoBehaviour 
{
    public GameObject food;
    public static Vector2 foodPos;

    private List<Vector2> swapList = new List<Vector2>();

	void Update () 
	{
        SwapFood();
	}

    public void SwapFood()
    {
        if (GameObject.FindGameObjectsWithTag("food").Length <=0)
        {
            swapList.Clear();
            for (int i = 0; i < GameManagerScript.w; ++i)
            {
                for (int j = 0; j < GameManagerScript.h; ++j)
                {
                    swapList.Add(new Vector2(i, j));
                    foreach (GameManagerScript.SnakeNode node in GameManagerScript.snakeNodes)
                    {
                        if (i == node.pos.x && j == node.pos.y)
                        {
                            swapList.RemoveAt(swapList.Count - 1);
                        }
                    }
                }
            }
            int index = Random.Range(0, swapList.Count);
            foodPos = swapList[index];
            Instantiate(food, new Vector3(swapList[index].x, swapList[index].y, 0f), Quaternion.identity);
        }
    }
}
