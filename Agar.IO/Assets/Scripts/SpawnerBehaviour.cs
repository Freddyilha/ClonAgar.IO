using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehaviour : MonoBehaviour
{
    [Header("Food spawner components")]
    public GameObject foodPrefab;
    public List<GameObject> foodPool;
    [HideInInspector] public int maxAvaiableFood;
    [HideInInspector] public int foodOnScreen;
    public GameObject foodHolder;

    [Header("NPC spawner components")]
    public GameObject NPCPrefab;
    public List<GameObject> NPCPool;
    [HideInInspector] public int maxAvaiableNPCs;
    [HideInInspector] public int NPCOnScreen;
    public GameObject NPCHolder;

    public static SpawnerBehaviour instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        maxAvaiableNPCs = 50;
        NPCPool = new List<GameObject>();
        for (int i = 0; i < maxAvaiableNPCs; i++)
        {
            GameObject npc = (GameObject)Instantiate(NPCPrefab);
            npc.SetActive(false);
            npc.GetComponent<NPCBehaviour>().NPCNickName = ("NPC - " + i);
            NPCPool.Add(npc);
            npc.transform.SetParent(NPCHolder.transform);
        }
        for (int i = 0; i < maxAvaiableNPCs; i++)
        {
            reactivateNPC();
        }

        maxAvaiableFood = 500;
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

    public void reactivateNPC()
    {
        GameObject npc = getPooledNPC();
        if (npc != null)
        {
            npc.transform.position = GameManagerBehaviour.instance.findAnEmptySpot();
            npc.SetActive(true);
        }
        npc.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);

        NPCOnScreen += 1;
    }

    public GameObject getPooledNPC()
    {
        for (int i = 0; i < NPCPool.Count; i++)
        {
            if (!NPCPool[i].activeSelf)
            {
                return NPCPool[i];
            }
        }
        return null;
    }

    public void reactivateFood()
    {
        GameObject food = getPooledFood();
        if (food != null)
        {
            food.transform.position = GameManagerBehaviour.instance.findAnEmptySpot();
            food.SetActive(true);
        }
        food.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);

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
