using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance => instance;
    private static CollisionManager instance;

    public List<Player> colliders = new List<Player>();
    private List<EnemyCollider1> enemies = new List<EnemyCollider1>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void RegisterPlayer(Player collider)
    {
        if (colliders.Contains(collider)) return;
        colliders.Add(collider);
    }
    
    public void UnregisterPlayer(Player collider)
    {
        if (!colliders.Contains(collider)) return;
        colliders.Remove(collider);
    }

    public void RegisterEnemy(EnemyCollider1 e){if (enemies.Contains(e)) return; 
    enemies.Add(e);
    }
    
    public void UnregisterEnemy(EnemyCollider1 e){if (!enemies.Contains(e)) 
    return; enemies.Remove(e);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < colliders.Count; i++)
        {
            var colliderA = colliders[i];
            for (int j = 0; j < colliders.Count; j++)
            {
                if (i == j) continue;
                var colliderB = colliders[j];
                if (Overlaps(colliderA, colliderB))
                {
                    colliderA.OnCollision(colliderB);
                    colliderB.OnCollision(colliderA);
                }
            }
        }
    }
    
    bool Overlaps(Player a, Player b)
    {
        return a.min.x <= b.max.x && b.min.x <= a.max.x &&
               a.min.y <= b.max.y && b.min.y <= a.max.y;
    }

}