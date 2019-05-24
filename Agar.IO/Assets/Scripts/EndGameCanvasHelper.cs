using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameCanvasHelper : MonoBehaviour
{
    public TextMeshProUGUI foodEaten;
    public TextMeshProUGUI highestMass;
    public TextMeshProUGUI leaderBoardTime;
    public TextMeshProUGUI timeAlive;
    public TextMeshProUGUI cellsEaten;
    public TextMeshProUGUI topPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onRestartButtonPressed()
    {
        StartCoroutine(GameManagerBehaviour.instance.waitForLogin());
    }
}
