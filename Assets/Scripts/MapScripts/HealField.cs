using UnityEngine;

public class HealField : MonoBehaviour
{
    float currentTime = 0f;  
    float tickInterval = 1f; 
    private GameObject player;
    public bool playerColliding;
    public AudioClip Tick;

    private float maxHeal = 50f;
    private float totalHealed = 0f;

    private SpriteRenderer spriteRenderer;
    private float fadeAmount = 0.05f;

    void Start()
    {
        currentTime = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= tickInterval)
        {
            if (playerColliding && totalHealed < maxHeal)
            {
                float healingAmount = -5f;
                player.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(healingAmount);
                totalHealed += Mathf.Abs(healingAmount);
                AudioManager.Instance.PlaySound(Tick);
                FadeSprite();
            }

            if (totalHealed >= maxHeal)
            {
                Destroy(gameObject);
            }

            currentTime = 0f;
        }
    }

    private void FadeSprite()
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a -= fadeAmount;

        if (currentColor.a < 0f) currentColor.a = 0f;

        spriteRenderer.color = currentColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerColliding = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerColliding = false; 
        }
    }
}
