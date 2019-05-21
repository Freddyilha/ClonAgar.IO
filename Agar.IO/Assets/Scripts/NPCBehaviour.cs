using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tm;
    private Transform startMovement;
    private Vector3 endMovement;
    private Vector3[] directions = {Vector3.down, Vector3.up, Vector3.left, Vector3.right, Vector3.up+Vector3.left,
                                    Vector3.left+Vector3.down, Vector3.down+ Vector3.right, Vector3.right+ Vector3.up};
    [Range(0f, 20f)] public float currentSpeed = 5f;
    private float speed = 50f;
    private float timeForDecision;
    private float startTime;
    private float distance;

    //[SerializeField] private CinemachineVirtualCamera virtualCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tm = GetComponent<Transform>();
        startTime = Time.deltaTime;
        timeForDecision = Random.Range(1,5);
        endMovement = tm.position + (directions[Random.Range(0, 8)] * currentSpeed);
        //print(new Vector3(1f, 1f) * 7);
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(endMovement, tm.position);
        float distCovered = (Time.deltaTime - startTime) * speed;
        float fracJourney = distCovered / distance;


        if (timeForDecision > 0)
            timeForDecision -= Time.deltaTime;
        else
        {
            //rb.velocity = directions[Random.Range(0, 8)] * currentSpeed;
            transform.position = Vector3.Lerp(transform.position, endMovement, fracJourney);
        }

        if (tm.localScale.x >= 16)
        {
            //GameManagerBehaviour.instance.divideItself(tm.localScale);
            tm.localScale -= tm.localScale / 2;
        }

        // VERIFICAR A FORMULA PARA VELOCIDADE TA ESTRANHO A PROGRESSÃO 
        currentSpeed = speed / (tm.localScale.x / 2);

    }

    void FixedUpdate()
    {
        //rb.velocity = new Vector3(Random.Range(0f,1f), Random.Range(0f, 1f)) * currentSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.name);
        if (collision.name == "Food(Clone)")
        {
            Destroy(collision.gameObject);
            tm.localScale += new Vector3(1f, 1f);
        }
    }

    

    //public Vector3 getDirection()
    //{
    //    mousePos = virtualCam.GetComponentInParent<Camera>().ScreenToWorldPoint(Input.mousePosition);
    //    Vector3 heroPos = tm.position;

    //    Vector3 heading = mousePos - heroPos;
    //    float distance = heading.magnitude;
    //    Vector3 direction = heading / distance;

    //    return direction;
    //}
}
