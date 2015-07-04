using UnityEngine;
using System.Collections;

public class NodeScript : MonoBehaviour 
{
    public NodeScript preNode = null;         // 上一个节点

    public int index;                           // 该节点所在的位置
    public Vector2 direction;                   // 该节点运动的方向

	void Start () 
	{
	    // 默认index = 0, 默认运动方向为SnakeScript运动方向
        index = 0;
        direction = FindObjectOfType<SnakeScript>().direction;
	}
	
	void Update () 
	{
        if (preNode != null)
        {
            direction = preNode.direction;
            transform.position = new Vector3(preNode.transform.position.x - direction.x, preNode.transform.position.y - direction.y, 0f);
        }
	}

    public void AddPrevious(GameObject pre)
    {
        preNode = pre.GetComponent<NodeScript>();
        direction = preNode.direction;
        index = preNode.index+1;
        transform.position = new Vector3(pre.transform.position.x - direction.x, pre.transform.position.y - direction.y, 0f);
    }
}
