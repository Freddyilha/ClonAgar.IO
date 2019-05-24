using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tm;
    [HideInInspector] public static string nickName;
    [HideInInspector] public static int foodGrabed = 0;
    private float currentSpeed;
    [HideInInspector] public float speed;
    private static Dictionary<int, GameObject> playerDict = new Dictionary<int, GameObject>();
    public static float startingTime;
    public static float highestMass;
    public static float NPCsEaten;
    public static int topPosition;
    public static float timeOnLeaderboard;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tm = GetComponent<Transform>();
        speed = gameObject.CompareTag("PlayerClone") ? 60f : 20f;   /* Não achei o motivo quando o clone é criado ele esta começando com 0 de 'speed',
                                                                     porém, isso não estava acontecendo então eu passo o valor que deveria estar
                                                                     depois de multiplicar a velavidade na função 'burstOfSpeed'
                                                                    */
        //print(gameObject.GetInstanceID());
        playerDict.Add(gameObject.GetInstanceID(), gameObject);
    }

    private void OnEnable()
    {
        if (GameManagerBehaviour.instance.nameChosen)
        {
            foodGrabed = 0;
            timeOnLeaderboard = 0;
            topPosition = 100;
            NPCsEaten = 0;
            startingTime = Time.time;
            GameManagerBehaviour.instance.nameChosen = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (KeyValuePair<int, GameObject> player in playerDict)
            {
                if (player.Value.transform.localScale.x >= 16)
                {
                    divideItself(player.Value.transform);
                }
            }
        }

        currentSpeed = speed - (Mathf.Sqrt(tm.localScale.x));
    }

    void FixedUpdate()
    {
        rb.MovePosition(tm.position + (Vector3)GameManagerBehaviour.instance.getDirection(tm.position) * currentSpeed * Time.deltaTime);
    }

    public void divideItself(Transform individualTransform)
    {
        GameObject playerClone = GameManagerBehaviour.instance.createClone(individualTransform.position);
        playerClone.transform.localScale = individualTransform.localScale / 1.3f;
        playerClone.tag = "PlayerClone";
        playerClone.GetComponent<PlayerBehaviour>().speed = 15f;
        StartCoroutine(burstOfSpeed(playerClone));
        individualTransform.localScale = individualTransform.localScale / 1.3f;
    }

    public IEnumerator burstOfSpeed(GameObject clone)
    {
        clone.GetComponent<PlayerBehaviour>().speed *= 3;
        yield return new WaitForSecondsRealtime(.2f);
        // If para não dar erro quando o clone é arremessado no inimigo
        if (clone != null)
            clone.GetComponent<PlayerBehaviour>().speed /= 3;
    }

    public void divideIntoSix(Transform mainPlayer)
    {
        Vector3[] positionsToSpawnClones = { Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.up + Vector2.right, Vector2.down + Vector2.left };
        for (int i = 0; i < 6; i++)
        {
            GameObject playerClone = GameManagerBehaviour.instance.createClone(mainPlayer.position);
            playerClone.transform.localScale = mainPlayer.localScale / 6;
            playerClone.tag = "PlayerClone";
            StartCoroutine(burstOfSpeed(playerClone));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Food(Clone)")
        {
            collision.gameObject.SetActive(false);
            SpawnerBehaviour.instance.reactivateFood();
            foodGrabed += 1;
            tm.localScale += new Vector3(1f,1f);
            GameManagerBehaviour.instance.updateFoodCounterUI();
            GameManagerBehaviour.instance.increaseCameraSize(1);
            GameManagerBehaviour.instance.addToList(nickName);
            GameManagerBehaviour.instance.updateHighScoreTable();
        }

        if (collision.collider.name == "NPC(Clone)" || collision.collider.name == "NPC")
        {
            Transform npcTm = collision.collider.GetComponent<Transform>();
            if (tm.localScale.x > (npcTm.localScale.x)*1.2)
            {
                NPCsEaten += 1;
                tm.localScale += npcTm.localScale/3;
                collision.gameObject.SetActive(false);
                SpawnerBehaviour.instance.reactivateNPC();
                GameManagerBehaviour.instance.addToList(nickName, (int)tm.localScale.x/3);
                GameManagerBehaviour.instance.updateHighScoreTable();
                GameManagerBehaviour.instance.increaseCameraSize((int)(npcTm.localScale.x / 6));
            }
            else if (tm.localScale.x * 1.2 < (npcTm.localScale.x))
            {
                npcTm.localScale += tm.localScale / 3;

                if (playerDict.Count == 1)
                {
                    startingTime = (Time.time - startingTime);
                    
                    gameObject.SetActive(false);
                    GameManagerBehaviour.instance.changeCameraFollow(npcTm);
                    GameManagerBehaviour.instance.onGameOver();
                    GameManagerBehaviour.instance.resetIndividualScore(nickName);
                    GameManagerBehaviour.instance.updateHighScoreTable();
                }
                else
                {
                    playerDict.Remove(gameObject.GetInstanceID());
                    GameManagerBehaviour.instance.changeCameraFollow(playerDict.First().Value.transform);
                    if (gameObject.CompareTag("PlayerClone"))
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }
            }
        }
        highestMass = highestMass > tm.localScale.x ? highestMass : tm.localScale.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform colTransform = collision.GetComponent<Transform>();
        if (collision.name == "HidingSpot(Clone)")
        {
            // Caso o player seja muito maior que o 'HidingSpot' não acontece a divisão
            if (tm.localScale.x > (colTransform.localScale.x) * 1.6)
            {
                Destroy(colTransform.gameObject);
                tm.localScale += colTransform.localScale / 2;
            }
            else if (tm.localScale.x > (colTransform.localScale.x) * 1.1)
            {
                Destroy(colTransform.gameObject);
                divideIntoSix(tm);
                tm.localScale = tm.localScale / 6;
            }
        }
    }
}
