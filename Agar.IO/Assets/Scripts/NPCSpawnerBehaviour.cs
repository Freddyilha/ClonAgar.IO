using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnerBehaviour : MonoBehaviour
{
    public GameObject NPCPrefab;
    public List<GameObject> NPCPool;
    [HideInInspector] public int maxAvaiableNPCs;
    [HideInInspector] public int NPCOnScreen;
    public GameObject NPCHolder;

    public static NPCSpawnerBehaviour instance;

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
    }

    public void reactivateNPC()
    {
        Vector2 spawnPoint = new Vector2(Random.Range(-135f, 135f), Random.Range(-135f, 135f));
        if (!Physics2D.OverlapCircle(spawnPoint, 0.75f, LayerMask.GetMask("Game")))
        {
            GameObject npc = getPooledNPC();
            if (npc != null)
            {
                npc.transform.position = spawnPoint;
                npc.SetActive(true);
            }
            npc.GetComponent<SpriteRenderer>().color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);
        }

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
}
