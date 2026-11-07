using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Sprites")]
    public Sprite middleSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public Sprite jumpSprite;
    public Sprite hitSprite;

    [Header("Bounds")]
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _width  = 1f;

    [Header("Lanes")]
    public float laneDistance = 3f;
    private int currentLane = 1; // 0 = left, 1 = middle, 2 = right
    public float moveSpeed = 10f;
    private float targetX;

    [Header("Jump")]
    public float groundY = -3f;
    public float jumpHeight = 5f;
    public float jumpDuration = 0.5f;
    private float jumpTimer = 0f;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public float regenRate = 1f;
    private float invincibilityTimer = 0f;

    public Vector2 max { get; private set; }
    public Vector2 min { get; private set; }
    
    private SpriteRenderer spriteRenderer;
    private float hitFlashTimer = 0f;
    private float laneFlashTimer = 0f;
    private int flashLane = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 0;
    }

    private void Start()
    {
        targetX = transform.position.x;
        if (CollisionManager.Instance != null) CollisionManager.Instance.RegisterPlayer(this);
    }

    private void OnDestroy()
    {
        if (CollisionManager.Instance != null) CollisionManager.Instance.UnregisterPlayer(this);
    }

    private void Update()
    {
        //health with lerp
        if (currentHealth < maxHealth)
        {
            currentHealth += regenRate * Time.deltaTime;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
        }
        if (invincibilityTimer > 0f) invincibilityTimer -= Time.deltaTime;
        if (hitFlashTimer > 0f) hitFlashTimer -= Time.deltaTime;
        if (laneFlashTimer > 0f) laneFlashTimer -= Time.deltaTime;

        //movement input
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane = Mathf.Max(0, currentLane - 1);
            laneFlashTimer = 0.2f; 
            flashLane = -1;        
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane = Mathf.Min(2, currentLane + 1);
            laneFlashTimer = 0.2f; 
            flashLane = 1;         
        }

        targetX = (currentLane - 1) * laneDistance;

        //Jump Input
        if (jumpTimer <= 0f && Input.GetKeyDown(KeyCode.Space))
        {
            jumpTimer = jumpDuration;
        }

        float x = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * moveSpeed);
        
        // x and y animation
        float y = groundY;
        if (jumpTimer > 0f)
        {
            jumpTimer -= Time.deltaTime;
            jumpTimer = Mathf.Max(0f, jumpTimer); // stops landing twitch
            
            float t = 1f - (jumpTimer / jumpDuration); // t goes from 0 to 1
            
            // math sin arc:
            y = groundY + Mathf.Sin(t * Mathf.PI) * jumpHeight;
        }

        transform.position = new Vector3(x, y, 0f);

        //Sprites
        if (hitFlashTimer > 0f)
        {
            spriteRenderer.sprite = hitSprite;
        }
        else if (jumpTimer > 0f) 
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else if (laneFlashTimer > 0f) 
        {
            spriteRenderer.sprite = (flashLane == -1) ? leftSprite : rightSprite;
        }
        else 
        {
            if (currentLane == 0) spriteRenderer.sprite = leftSprite;
            else if (currentLane == 2) spriteRenderer.sprite = rightSprite;
            else spriteRenderer.sprite = middleSprite;
        }

        //Bounds
        max = new Vector2(transform.position.x + _width * 0.5f, transform.position.y + _length * 0.5f);
        min = new Vector2(transform.position.x - _width * 0.5f, transform.position.y - _length * 0.5f);
        
        CheckEnemyHits();
    }

    private void CheckEnemyHits()
    {
        if (CollisionManager.Instance == null) return;

        var enemies = CollisionManager.Instance.enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            var e = enemies[i];
            if (e == null || e.enemy == null) continue;

            // checks collision when both are present
            if (e.enemy.GetProgress() < 0.8f) continue;

            bool xOverlap = min.x <= e.max.x && e.min.x <= max.x;
            bool yOverlap = min.y <= e.max.y && e.min.y <= max.y;
            bool jumpedOver = min.y > e.max.y;

            if (xOverlap && yOverlap && !jumpedOver)
            {
                TakeDamage(5f);
                Destroy(e.gameObject); // Destroy enemy 
                return;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (invincibilityTimer > 0f) return; //survivability because it will touch player
        
        currentHealth -= amount;
        invincibilityTimer = 2.5f;
        hitFlashTimer = 0.2f;

        if (currentHealth < 0f) currentHealth = 0f;
    }

    public void OnCollision(Player other) { } //collision manager link
}