using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnerBehaviour : MonoBehaviour
{
    public GameObject foodPrefab;
    private int maxAvaiableFood = 100;
    public int maxFoodOnScreen;

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("generateFood", 2, 0.5f);
    }
    


    // Update is called once per frame
    void Update()
    {
        if (maxFoodOnScreen < maxAvaiableFood)
            generateFood();
    }

    public void generateFood()
    {
        Vector2 spawnPoint = new Vector2(Random.Range(-30f, 30f), Random.Range(-30f, 30f));
        if (!Physics2D.OverlapCircle(spawnPoint, 0.28f))
        {
            GameObject food = Instantiate<GameObject>(foodPrefab, spawnPoint, new Quaternion());
            food.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        }
    }
}
