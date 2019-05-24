using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{
    public static GameManagerBehaviour instance;
    public GameObject playerPrefab;
    private GameObject player;
    [SerializeField] private Camera mainCamPrefab;
    private CinemachineVirtualCamera virtualCam;
    private Vector2 mousePos;
    private Vector2 mouseDistanceFromCenter;
    private Vector2 randomPos;
    public GameObject npcPrefab;
    private List<GameObject> npcList;
    public GameObject hidingSpotPrefab;
    private GameObject hidingSpot;
    public Canvas loginCanvasPrefab;
    public Canvas gameCanvasPrefab;
    public Canvas endGameCanvasPrefab;
    private Canvas activeCanvas;
    private Camera mainCamera;
    [HideInInspector] public Dictionary<string, int> highScoreList;
    private GameObject playerHighScore;
    public GameObject highScoreCellPrefab;
    private bool updatingHighScore;
    public bool nameChosen;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        updatingHighScore = true; // Começa em true para impedir de entrar antes do player ter entrado
    }

    void Start()
    {
        highScoreList = new Dictionary<string, int>();
        mainCamera = Instantiate<Camera>(mainCamPrefab);
        virtualCam = mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        player = Instantiate<GameObject>(playerPrefab);
        player.SetActive(false);
        //activeCanvas = Instantiate<Canvas>(loginCanvasPrefab);
        StartCoroutine(waitForLogin());
    }

    void Update()
    {
        //mouseDistanceFromCenter = (Vector2)virtualCam.GetComponentInParent<Camera>().ScreenToViewportPoint(Input.mousePosition) - new Vector2(0.5f, 0.5f);
    }

    public void updateHighScoreTable()
    {
        int lineCounter = 0;
        if (highScoreList.Count == 0)    //Caso o player clicar rapido no botao de login
            updatingHighScore = false;
        foreach (KeyValuePair<string, int> element in highScoreList.OrderByDescending(value => value.Value))    
        {
            if (element.Key == PlayerBehaviour.nickName)
            {
                PlayerBehaviour.timeOnLeaderboard += Time.deltaTime;
                PlayerBehaviour.topPosition = PlayerBehaviour.topPosition < lineCounter + 1 ? PlayerBehaviour.topPosition : lineCounter + 1;
            }
            if (activeCanvas.name == "GameCanvas(Clone)")
                activeCanvas.GetComponent<GameCanvasHelper>().playerName[lineCounter].text = element.Key;
            lineCounter++;
            if (lineCounter == 10 || highScoreList.Count < 10)
            {
                lineCounter = 0;
                break;
            }
        }
    }

    public Vector2 getDirection(Vector2 playerPosition)
    {
        mousePos = getMousePosition();
        mouseDistanceFromCenter = (Vector2)virtualCam.GetComponentInParent<Camera>().ScreenToViewportPoint(Input.mousePosition) - new Vector2(0.5f, 0.5f);
        Vector2 heading = mousePos - playerPosition;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        return direction * Mathf.Clamp(mouseDistanceFromCenter.sqrMagnitude, .2f, 0.5f);
    }

    public Vector2 getMousePosition()
    {
        mousePos = virtualCam.GetComponentInParent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        return mousePos;
    }

    public GameObject createClone(Vector2 ObjPosition)
    {
        return Instantiate<GameObject>(playerPrefab, ObjPosition + getDirection(ObjPosition), new Quaternion());
    }

    public Vector2 getPlayerPos()
    {
        return player.transform.position;
    }

    public void updateFoodCounterUI()
    {
        activeCanvas.GetComponent<GameCanvasHelper>().scoreValue.text = PlayerBehaviour.foodGrabed.ToString();
    }

    public void resetIndividualScore(string playerName)
    {
        highScoreList[playerName] = 0;
    }

    public void onGameOver()
    {
        updatingHighScore = true;
        Destroy(hidingSpot.gameObject);
        Destroy(activeCanvas.gameObject);
        activeCanvas = activeCanvas = Instantiate<Canvas>(endGameCanvasPrefab);
        activeCanvas.GetComponent<EndGameCanvasHelper>().foodEaten.text = PlayerBehaviour.foodGrabed.ToString();
        activeCanvas.GetComponent<EndGameCanvasHelper>().timeAlive.text = PlayerBehaviour.startingTime.ToString("F2");
        activeCanvas.GetComponent<EndGameCanvasHelper>().highestMass.text = PlayerBehaviour.highestMass.ToString("F2");
        activeCanvas.GetComponent<EndGameCanvasHelper>().cellsEaten.text = PlayerBehaviour.NPCsEaten.ToString();
        activeCanvas.GetComponent<EndGameCanvasHelper>().leaderBoardTime.text = PlayerBehaviour.timeOnLeaderboard.ToString("F2");
        activeCanvas.GetComponent<EndGameCanvasHelper>().topPosition.text = PlayerBehaviour.topPosition.ToString("");
    }

    public void addToList(string playerName, int point = 1)
    {
        try
        {
            highScoreList[playerName] += point;
        }
        catch (KeyNotFoundException)
        {
            highScoreList.Add(playerName, point);
        }
    }

    public void increaseCameraSize()
    {
        virtualCam.m_Lens.OrthographicSize += .05f;
    }

    public void decreaseCameraSize()
    {
        virtualCam.m_Lens.OrthographicSize -= .05f;
    }

    public void changeCameraFollow(Transform playerToFollow)
    {
        virtualCam.Follow = playerToFollow;
    }

    private void OnApplicationQuit()
    {
        Destroy(gameObject);
    }

    public IEnumerator waitForLogin()
    {
        if (activeCanvas == null)   //Primeira vez que o jogo é aberto
            activeCanvas = Instantiate<Canvas>(loginCanvasPrefab);
        else if (activeCanvas.name != "LoginCanvas(Clone)")
        {
            Destroy(activeCanvas.gameObject);
            activeCanvas = Instantiate<Canvas>(loginCanvasPrefab);
            nameChosen = false;
        }    
        activeCanvas.worldCamera = mainCamera;
        activeCanvas.sortingLayerName = "Game";
        while (!nameChosen)
        {
            yield return null;
        }

        updatingHighScore = false;
        for (int i = 0; i < 6; i++)
        {
            randomPos = new Vector2(Random.Range(-135f, 135f), Random.Range(-135f, 135f));
            hidingSpot = Instantiate<GameObject>(hidingSpotPrefab, randomPos, new Quaternion());
        }

        player.transform.position = findAnEmptySpot();
        player.gameObject.SetActive(true);

        virtualCam.Follow = player.transform;

        Destroy(activeCanvas.gameObject);
        activeCanvas = Instantiate<Canvas>(gameCanvasPrefab);
        activeCanvas.worldCamera = mainCamera;
        activeCanvas.sortingLayerName = "Game";

    }

    public Vector2 findAnEmptySpot()
    {
        Vector2 spawnPoint;
        do
        {
            spawnPoint = new Vector2(Random.Range(-125f, 125f), Random.Range(-125f, 125f));
        } while (Physics2D.OverlapCircle(spawnPoint, 0.75f, LayerMask.GetMask("Game")));
        return spawnPoint;
    }
}
