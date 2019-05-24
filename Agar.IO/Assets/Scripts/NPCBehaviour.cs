using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tm;
    [HideInInspector] public string NPCNickName;
    private Vector3 endMovement;
    private Vector3[] directions = {Vector3.down, Vector3.down + Vector3.left, Vector3.down + Vector3.right,
                                    Vector3.up, Vector3.up + Vector3.left, Vector3.up + Vector3.right,
                                    Vector3.left, Vector3.right};
    private Vector3 movingDirection;
    private float currentSpeed;
    private float speed = 10f;
    private float distance;
    private float distCovered;
    private float fracJourney;
    private bool goingForFood;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        tm = GetComponent<Transform>();
        goingForFood = false;
        InvokeRepeating("getNewDirection", 0f, 2f);
    }
   
    void Update()
    {
        currentSpeed = speed - (Mathf.Sqrt(tm.localScale.x));
    }

    void FixedUpdate()
    {
        rb.MovePosition(tm.position + movingDirection * currentSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform npcTm = collision.collider.GetComponent<Transform>();

        if (collision.collider.name == "Food(Clone)")
        {
            collision.gameObject.SetActive(false);
            SpawnerBehaviour.instance.reactivateFood();
            tm.localScale += new Vector3(1f, 1f);
            goingForFood = false;
            GameManagerBehaviour.instance.addToList(NPCNickName);
            GameManagerBehaviour.instance.updateHighScoreTable();
        }

        if (collision.collider.CompareTag("Wall"))
            movingDirection = -movingDirection;

        if (collision.collider.CompareTag("NPC"))
        {
            if (tm.localScale.x > (npcTm.localScale.x) * 1.3)
            {
                tm.localScale += npcTm.localScale / 3;
                collision.gameObject.SetActive(false);
                SpawnerBehaviour.instance.reactivateNPC();
                GameManagerBehaviour.instance.addToList(NPCNickName, (int)tm.localScale.x / 3);
                GameManagerBehaviour.instance.updateHighScoreTable();
            }
            else if (tm.localScale.x * 1.3 < (npcTm.localScale.x))
            {
                npcTm.localScale += tm.localScale / 3;
                GameManagerBehaviour.instance.resetIndividualScore(NPCNickName);
                gameObject.SetActive(false);
                SpawnerBehaviour.instance.reactivateNPC();
                GameManagerBehaviour.instance.addToList(collision.gameObject.GetComponent<NPCBehaviour>().NPCNickName, (int)tm.localScale.x / 3);
                GameManagerBehaviour.instance.updateHighScoreTable();
            }
        }
    }

    private void getNewDirection()
    {
        if (!goingForFood)
            movingDirection = directions[Random.Range(0, 8)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform colTransf = collision.GetComponent<Transform>();

        if (collision.name == "Food(Clone)" && !goingForFood)
        {
            // 20% de chance do NPC não ir em direção a comida
            if (Random.Range(0f, 1f) > .2)
            {
                movingDirection =  (colTransf.position - tm.position).normalized;
                goingForFood = true;
            }
        }

        if (collision.CompareTag("Player") || collision.CompareTag("PlayerClone") || collision.CompareTag("NPC"))
        {
            // 5% de chance de ser suicida
            if (colTransf.localScale.x * 1.2 > tm.localScale.x)
                movingDirection = Random.Range(0f, 1f) > .95 ? (colTransf.position - tm.position).normalized : -(colTransf.position - tm.position).normalized;
            else if (colTransf.localScale.x < tm.localScale.x * 1.2)
            {
                movingDirection = (colTransf.position - tm.position).normalized;
                //if (gameObject.CompareTag("NPC"))
                //{

                //    if (Random.Range(0f, 1f) > .80)
                //    {
                //        //GameManagerBehaviour.instance.divideItself
                //        //if (tm.localScale.x >= 16)
                //        //{
                //        GameManagerBehaviour.instance.divideNPC(colTransf);
                //        tm.localScale -= tm.localScale / 2;
                //        //}
                //    }

                //}
            }
        }
    }

    // PROBLEMA AQUI, NÃO CONSEGUI FAZER O NPC SE PROPULSAR NO INIMIGO
    //public void divideNPC(Transform enemy)
    //{
    //    Vector3 positionToSpawnClone = enemy.position - npc.transform.position;

    //    GameObject npcClone = Instantiate<GameObject>(npcPrefab, npc.transform.position + positionToSpawnClone, new Quaternion());
    //    npcClone.transform.localScale = npc.transform.localScale / 2;
    //    npcClone.tag = "NPCClone";

    //    StartCoroutine(burstOfSpeed(npcClone));
    //}

}