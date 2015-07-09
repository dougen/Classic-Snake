using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManagerScript : MonoBehaviour 
{
    public Text score;
    public GameObject rankPanel;
    public Text firstscore;
    public Text secondscore;
    public Text thirdscore;

    public GameObject startPanel;

    public GameObject gameoverPanel;
    public Text finalscore;

	void Start () 
	{
	
	}
	
	void Update () 
	{
        score.text = GameManagerScript.score.ToString();
	}

    public void ShowStartPanel(bool active)
    {
        startPanel.SetActive(active);
    }

    public void ShowRankPanel(bool active)
    {
        rankPanel.SetActive(active);
    }

    public void ShowGameOverPanel(bool active)
    {
        if (active == true)
        {
            finalscore.text = GameManagerScript.score.ToString();
        }
        gameoverPanel.SetActive(active);
    }
}
