using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;


public class GameCanvasHelper : MonoBehaviour
{

    //[HideInInspector] public List<string> highScoreNames;
    //[HideInInspector] public Dictionary<string, int> highScoreList;

    public TextMeshProUGUI[] playerName;
    public TextMeshProUGUI scoreValue;
    public GameObject highScorePanel;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        //highScoreList = new Dictionary<string, int>();
        //highScoreList.Add("bananan", 27);
        //highScoreList.Add("mamamo", 3);
        //highScoreList.Add("abacaxi", 15);
    }

    // Update is called once per frame
    void Update()
    {
        //    foreach (KeyValuePair<string, int> player in highScoreList.OrderBy(value => value.Value))
        //    {
        //        print(player);
        //    }
    }

    
}
