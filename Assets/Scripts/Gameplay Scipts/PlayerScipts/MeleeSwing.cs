using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    Rigidbody2D myRB;
    public float rotationSpeed = 360f; // Full rotation in degrees per second
    public bool attacking = false;
    float currentInterval;
    float interval = 0.5f;
    public AudioSource swingSound;

    private float totalRotation = 0f; // Track the total rotation

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        currentInterval = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking && Time.time > currentInterval)
        {
            // Increment the total rotation amount
            float rotationThisFrame = rotationSpeed * Time.deltaTime;
            totalRotation += rotationThisFrame;

            // Rotate the object
            transform.Rotate(0f, 0f, rotationThisFrame);

            // If we've completed one full rotation, stop the attack
            if (totalRotation >= 360f)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f); // Reset the rotation to 0 degrees
                attacking = false; // Stop attacking
                gameObject.SetActive(false); // Deactivate the object
                currentInterval = Time.time + interval; // Wait for the next interval before another attack

                // Reset the rotation tracker
                totalRotation = 0f;
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
            other.gameObject.GetComponent<Enemy>().ApplyKnockback(knockbackDirection, 40f);
            other.gameObject.GetComponent<Enemy>().TakeDamage(3);
        }
    }
}
