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
    //private Dictionary<int, GameObject> playerDict;
    [HideInInspector] private Dictionary<System.Int32, GameObject> NPCDict;
    private GameObject playerHighScore;
    public GameObject highScoreCellPrefab;
    private bool updatingHighScore;

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
        activeCanvas = Instantiate<Canvas>(loginCanvasPrefab);
        activeCanvas.worldCamera = mainCamera;
        activeCanvas.sortingLayerName = "Game";
        //updatingHighScore = false;
    }

    void Update()
    {
        mouseDistanceFromCenter = (Vector2)virtualCam.GetComponentInParent<Camera>().ScreenToViewportPoint(Input.mousePosition) - new Vector2(0.5f, 0.5f);
        if (!updatingHighScore)
        {
            updateHighScoreTable();
        }
    }

    public void startNewGame(string playerName)
    {
        updatingHighScore = false;
        for (int i = 0; i < 6; i++)
        {
            randomPos = new Vector2(Random.Range(-135f, 135f), Random.Range(-135f, 135f));
            hidingSpot = Instantiate<GameObject>(hidingSpotPrefab, randomPos, new Quaternion());
        }

        player = Instantiate<GameObject>(playerPrefab);
        player.GetComponent<PlayerBehaviour>().nickName = playerName;

        virtualCam.Follow = player.transform;

        Destroy(activeCanvas.gameObject);
        activeCanvas = Instantiate<Canvas>(gameCanvasPrefab);
        activeCanvas.worldCamera = mainCamera;
        activeCanvas.sortingLayerName = "Game";

    }

    private void updateHighScoreTable()
    {
        updatingHighScore = true;
        int lineCounter = 0;
        if (highScoreList.Count == 0)    //Caso o player clicar rapido no botao de login
            updatingHighScore = false;
        foreach (KeyValuePair<string, int> element in highScoreList.OrderByDescending(value => value.Value))
        {
            if (element.Key == player.GetComponent<PlayerBehaviour>().nickName)
                PlayerBehaviour.timeOnLeaderboard += Time.deltaTime;
            activeCanvas.GetComponent<GameCanvasHelper>().playerName[lineCounter].text = element.Key;
            lineCounter++;
            if (lineCounter == 10 || highScoreList.Count < 10)
            {
                lineCounter = 0;
                updatingHighScore = false;
                break;
            }
        }
    }

    public Vector2 getDirection(Vector2 playerPosition)
    {
        mousePos = getMousePosition();

        Vector2 heading = mousePos - playerPosition;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        return direction * Mathf.Clamp(mouseDistanceFromCenter.sqrMagnitude, .3f, 0.8f);
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

    public void onGameOver(GameObject npcObj)
    {
        updatingHighScore = true;
        Destroy(hidingSpot.gameObject);
        Destroy(activeCanvas.gameObject);
        activeCanvas = activeCanvas = Instantiate<Canvas>(endGameCanvasPrefab);
        activeCanvas.GetComponent<EndGameCanvasHelper>().foodEaten.text = PlayerBehaviour.foodGrabed.ToString();
        activeCanvas.GetComponent<EndGameCanvasHelper>().timeAlive.text = PlayerBehaviour.startingTime.ToString("F2");
        activeCanvas.GetComponent<EndGameCanvasHelper>().highestMass.text = PlayerBehaviour.highestMass.ToString();
        activeCanvas.GetComponent<EndGameCanvasHelper>().cellsEaten.text = PlayerBehaviour.NPCsEaten.ToString();
        activeCanvas.GetComponent<EndGameCanvasHelper>().leaderBoardTime.text = PlayerBehaviour.timeOnLeaderboard.ToString();
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
}
