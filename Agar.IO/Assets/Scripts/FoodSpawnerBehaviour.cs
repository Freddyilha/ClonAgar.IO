using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnerBehaviour : MonoBehaviour
{

    public GameObject foodPrefab;
    public List<GameObject> foodPool;
    [HideInInspector] public int maxAvaiableFood;
    [HideInInspector] public int foodOnScreen;
    public GameObject foodHolder;

    public static FoodSpawnerBehaviour instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        maxAvaiableFood = 300;
        foodPool = new List<GameObject>();
        for (int i = 0; i < maxAvaiableFood; i++)
        {
            GameObject food = (GameObject)Instantiate(foodPrefab);
            food.SetActive(false);
            foodPool.Add(food);
            food.transform.SetParent(foodHolder.transform);
        }
        for (int i = 0; i < maxAvaiableFood; i++)
        {
            reactivateFood();
        }
    }

    public void reactivateFood()
    {
        Vector2 spawnPoint = new Vector2(Random.Range(-135f, 135f), Random.Range(-135f, 135f));
        if (!Physics2D.OverlapCircle(spawnPoint, 0.28f, LayerMask.GetMask("Game")))
        {
            GameObject food = getPooledFood();
            if (food != null)
            {
                food.transform.position = spawnPoint;
                food.SetActive(true);
            }
            food.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        }

        foodOnScreen += 1;
    }

    public GameObject getPooledFood()
    {
        for (int i = 0; i < foodPool.Count; i++)
        {
            if (!foodPool[i].activeSelf)
            {
                return foodPool[i];
            }
        }
        return null;
    }
}
