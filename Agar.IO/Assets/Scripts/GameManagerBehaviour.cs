using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
    

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Camera mainCamera = Instantiate<Camera>(mainCamPrefab);
        virtualCam = mainCamera.GetComponentInChildren<CinemachineVirtualCamera>();
        player = Instantiate<GameObject>(playerPrefab);
        virtualCam.Follow = player.transform;
        //enemy = Instantiate<GameObject>(enemyPrefab);
        //activeCanvas = Instantiate<Canvas>(areaZeroCanvasPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        //print(virtualCam.GetComponentInParent<Camera>().ScreenToViewportPoint(Input.mousePosition));
        mouseDistanceFromCenter = (Vector2)virtualCam.GetComponentInParent<Camera>().ScreenToViewportPoint(Input.mousePosition) - new Vector2 (0.5f, 0.5f);

        
            
    }

    public Vector2 getDirection(GameObject playerObject)
    {
        mousePos = getMousePosition();
        Vector2 heroPos = playerObject.transform.position;

        Vector2 heading = mousePos - heroPos;
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        return direction * mouseDistanceFromCenter.sqrMagnitude;
    }

    public Vector2 getMousePosition()
    {
        mousePos = virtualCam.GetComponentInParent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        return mousePos;
    }

    public void divideItself(Vector3 mainPlayerScale)
    {
        GameObject playerClone = Instantiate<GameObject>(playerPrefab,player.transform.position,new Quaternion());
        playerClone.transform.localScale = mainPlayerScale / 2;
        playerClone.tag = "PlayerClone";
        playerClone.GetComponent<Rigidbody2D>().AddForce(getMousePosition() * 100); // VERIFICAR ESSA PARTE O CLONE NÃO TA SENDO EMPURRADO
    }

    public Vector2 getPlayerPos()
    {
        return player.transform.position;
    }

}
