using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _width = 1f;
    
    private float speed = 8f;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }

    private void Start()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.RegisterPlayer(this);
    }
    
    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.UnregisterPlayer(this);
    }

    private void Update()
    {
        var posX = transform.position.x + _width * .5f;
        var posY = transform.position.y + _length * .5f;
        var negX = transform.position.x - _width * .5f;
        var negY = transform.position.y - _length * .5f;

        max = new Vector2(posX, posY);
        min = new Vector2(negX, negY);

        Debug.DrawLine(new Vector3(min.x, max.y), new Vector3(max.x, max.y), Color.yellow);
        Debug.DrawLine(new Vector3(max.x, max.y), new Vector3(max.x, min.y), Color.yellow);
        Debug.DrawLine(new Vector3(max.x, min.y), new Vector3(min.x, min.y), Color.yellow);
        Debug.DrawLine(new Vector3(min.x, min.y), new Vector3(min.x, max.y), Color.yellow);

        PlayerMove();
        // PlayerHealth();
        // TakeDamage();
    }

    // public void OnGroundCollision(AABBCollider other) //stop gravity when touched
    // {
    //     if (other.max.y <= this.min.y + 0.2f) 
    //     {
    //         isGrounded = true;
    //         groundedTimer = 0.1f; 
    //         verticalVelocity = 0f;

    //         float playerHeight = this.max.y - this.min.y;
    //         float distanceToBottom = playerHeight / 2f;
    //         transform.position = new Vector2(transform.position.x, other.max.y + distanceToBottom);
    //     }
    // }

   
    void PlayerMove() 
    { 
        float horizontalInput = Input.GetAxis("Horizontal"); 
        transform.Translate(Vector2.right * Time.deltaTime * speed * horizontalInput);

    } 

    // void TakeDamage()
    // {
        
    // }

    // void PlayerHealth()
    // {
        
    // }

     public void OnCollision(Player other)
    {
        Debug.Log($"Hit {other.name}");
    }
}