using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManagerScript : MonoBehaviour 
{
    public Text score;

	void Start () 
	{
	
	}
	
	void Update () 
	{
        score.text = GameManagerScript.score.ToString();
	}
}
