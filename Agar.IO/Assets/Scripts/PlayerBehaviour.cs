using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tm;
    private int foodGrabed;
    [Range(0f, 20f)] public float currentSpeed = 5f;
    private float speed = 100f;
    //[SerializeField] private CinemachineVirtualCamera virtualCam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tm = GetComponent<Transform>();
        foodGrabed = 1;     /*começa em 1 para não dar problema com a divisão*/

        //print(new Vector2(1f, 1f) * 7);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (gameObject.CompareTag("Player"))
            {
                if (tm.localScale.x >= 16)
                {
                    GameManagerBehaviour.instance.divideItself(tm.localScale);
                    tm.localScale -= tm.localScale / 2;
                }
            }
        }

        // VERIFICAR A FORMULA PARA VELOCIDADE TA ESTRANHO A PROGRESSÃO 
        currentSpeed = speed / (tm.localScale.x/2);

    }

    void FixedUpdate()
    {
        rb.velocity = GameManagerBehaviour.instance.getDirection(gameObject) * currentSpeed;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print(collision.name);
        if (collision.name == "Food(Clone)")
        {
            Destroy(collision.gameObject);
            foodGrabed += 1;
            tm.localScale += new Vector3(1f,1f);
        }
    }

    //public Vector2 getDirection()
    //{
    //    mousePos = virtualCam.GetComponentInParent<Camera>().ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 heroPos = tm.position;

    //    Vector2 heading = mousePos - heroPos;
    //    float distance = heading.magnitude;
    //    Vector2 direction = heading / distance;

    //    return direction;
    //}
}
