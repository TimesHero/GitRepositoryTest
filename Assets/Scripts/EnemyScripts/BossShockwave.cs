using UnityEngine;

public class BossShockwave : MonoBehaviour
{
    private bool maxLimit;

    // Set a scaling speed so that it can be adjusted easily.
    public float scalingSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        // If the shockwave has reached its maximum size, start shrinking.
        if (transform.localScale.x > 15)
        {
            maxLimit = true;
        }

        // If we have not reached the max size, scale up.
        if (maxLimit == false)
        {
            transform.localScale = new Vector3(transform.localScale.x + scalingSpeed * Time.deltaTime, transform.localScale.y + scalingSpeed * Time.deltaTime, transform.localScale.z);
        }
        else
        {
            // If max size is reached, start scaling down.
            transform.localScale = new Vector3(transform.localScale.x - scalingSpeed * 2 * Time.deltaTime, transform.localScale.y - scalingSpeed * 2 * Time.deltaTime, transform.localScale.z);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<PlayerHPManager>().invincible == false)
            {
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(10);
            }
        }        
    }
}
