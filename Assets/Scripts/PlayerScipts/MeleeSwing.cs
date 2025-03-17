using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    Rigidbody2D myRB;
    public float rotationSpeed = 360f; // Full rotation in degrees per second
    public bool attacking = false;
    public AudioSource swingSound;
    public SpriteRenderer myRenderer;

    private float totalRotation = 0f; // Track the total rotation

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            totalRotation += rotationThisFrame;
            if (myRenderer.flipX==true)
            {
                transform.Rotate(0f, 0f, rotationThisFrame);

                if (totalRotation >= 360f)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f); 
                    attacking = false; 
                    gameObject.SetActive(false); 
                    totalRotation = 0f;
                }
            }
            else
            {
                transform.Rotate(0f, 0f, -rotationThisFrame);

                if (totalRotation >= 360f)
                {
                    transform.eulerAngles = new Vector3(0f, 0f, 0f); 
                    attacking = false; 
                    gameObject.SetActive(false); 
                    totalRotation = 0f;
                }
            }
            
        }
        else if (!attacking)
        {
            gameObject.SetActive(false); // Ensure object is deactivated when not attacking
        }
    }

    public void Attack()
    {
        attacking = true;
        swingSound.Play();
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Apply knockback and damage to the enemy
            Vector2 knockbackDirection = transform.position - other.transform.position;
            knockbackDirection.Normalize();
            other.gameObject.GetComponent<EnemyHPManager>().ApplyKnockback(knockbackDirection, 20f);
            other.gameObject.GetComponent<EnemyHPManager>().TakeDamage(3);
        }
    }
}
