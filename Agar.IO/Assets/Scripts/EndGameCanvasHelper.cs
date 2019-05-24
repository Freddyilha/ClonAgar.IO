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

    public void onRestartButtonPressed()
    {
        StartCoroutine(GameManagerBehaviour.instance.waitForLogin());
    }

    public void onQuitButtonPressed()
    {
        Application.Quit();
    }
}
