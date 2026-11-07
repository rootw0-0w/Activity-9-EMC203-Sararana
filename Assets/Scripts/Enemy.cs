using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector2 targetLanePos; 
    
    [Header("Perspective")]
    public float horizonY = 4f; 
    public float lifetime = 3f;
    
    private float t = 0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    public void Setup(Vector2 targetPos)
    {
        targetLanePos = targetPos;
        t = 0f;
        
        float currentX = targetLanePos.x * 0f;
        float currentY = Mathf.Lerp(horizonY, targetLanePos.y, 0f);
        transform.position = new Vector3(currentX, currentY, 0f);
        
        transform.localScale = Vector3.one * 0.1f;
        if (spriteRenderer != null) spriteRenderer.sortingOrder = 0;
    }

    private void Update()
    {
        t += Time.deltaTime / lifetime;
        float visualT = Mathf.Clamp01(t);

        float currentScale = Mathf.Lerp(0.1f, 2.0f, visualT);
        transform.localScale = Vector3.one * currentScale;

        float currentX = targetLanePos.x * visualT;
        float currentY = Mathf.Lerp(horizonY, targetLanePos.y, visualT);

        transform.position = new Vector3(currentX, currentY, 0f);

        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = Mathf.RoundToInt(visualT * 1000f);

        if (t > 1.2f) Destroy(gameObject);
    }

    public float GetProgress() { return t; }
}