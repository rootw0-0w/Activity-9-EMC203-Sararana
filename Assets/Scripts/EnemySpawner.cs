using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public Transform parent;
    public float laneDistance = 3f;

    [Header("Timing")]
    public float startDelay = 1f;
    public float interval   = 0.6f;

    public List<Item> items = new List<Item>();

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), startDelay, interval);
    }

    private void BeginSpawn()
    {
        if (itemToSpawn == null) return;

        var spawned = Instantiate(itemToSpawn, parent);
        
        int lane = Random.Range(0, 3);
        float targetX = (lane - 1) * laneDistance;
        float targetY = -3f; // matches player
        
        spawned.Setup(new Vector2(targetX, targetY));
        items.Add(spawned);
    }

    private void Update()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i] == null) items.RemoveAt(i);
        }
    }
}