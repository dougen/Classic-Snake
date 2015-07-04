using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeScript : MonoBehaviour 
{ 
    public float moveSpeed = 3f;
    public int initlength = 9;
    public Vector2 direction = -Vector2.right;
    public GameObject nodePrefeb;

    private List<GameObject> nodes = new List<GameObject>();

	void Start () 
	{
       nodes.Add(gameObject);
       for (int i = 0; i < initlength; ++i)
       {
           AddNode();
       }
	}
	
	void Update () 
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

        //IsTurn();
        transform.Translate(direction * moveSpeed * Time.deltaTime);
	}

    public void EatFood()
    {
        AddNode();
    }

    // 只有位置达到整数才可转弯
    private void IsTurn()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        if (direction == Vector2.up || direction == -Vector2.up)
        {
            posX = Mathf.Floor(posX);
        }
        if (direction == Vector2.right || direction == -Vector2.right)
        {
            posY = Mathf.Floor(posY);
        }
        transform.position = new Vector3(posX, posY, 0);
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void AddNode()
    {
        // 新建一个Noded对象
        GameObject node = (GameObject)Instantiate(nodePrefeb, Vector3.zero, Quaternion.identity);
        // 先找到最后一个node
        GameObject lastNode = nodes[nodes.Count - 1];
        // 然后将新node附加到最后那个node上
        node.GetComponent<NodeScript>().AddPrevious(lastNode);
        // 将新加入的放到nodes List中
        nodes.Add(node);
        node.transform.parent = transform;
    }
}
