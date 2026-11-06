using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyCollider1 : MonoBehaviour
{
    [SerializeField] private float _length;
    [SerializeField] private float _width;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.RegisterEnemy(this);
        }
    }
    
    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.UnregisterEnemy(this);
        }
    }


    private void Update()
    {
        var posX = this.transform.position.x + _width * .5f;
        var posY = this.transform.position.y + _length * .5f;
        
        var negX = this.transform.position.x - _width * .5f;
        var negY = this.transform.position.y - _length * .5f;

        max = new Vector2(posX, posY);
        min = new Vector2(negX, negY);
        
        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), Color.red);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), Color.red);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), Color.red);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), Color.red);
    }
}