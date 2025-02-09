using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    Rigidbody2D myRB;
    public float rotationTotal = 120;
    public bool attacking =false;
    float currentInterval;
    float interval=0.5f;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        currentInterval = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking==true && Time.time>currentInterval)
        {
            rotationTotal-=6;
            if (rotationTotal>0)
            {
                transform.Rotate(transform.rotation.x,transform.rotation.y,-6f );
            }
            if (rotationTotal<0)
            {
                attacking=false;
                rotationTotal=120;
                gameObject.SetActive(false);
                transform.Rotate(transform.rotation.x,transform.rotation.y,114f );
                currentInterval = Time.time + interval;
            }
        }
        else
        {
            attacking=false;
            gameObject.SetActive(false);
        }
    }
    public void Attack()
    {
        attacking=true;
        gameObject.SetActive(true);
    }
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
                //other.gameObject.GetComponent<Enemy>().TakeDamage(1);
                Vector2 knockbackDirection = transform.position - other.transform.position;
                knockbackDirection.Normalize();
                other.gameObject.GetComponent<Enemy>().ApplyKnockback(knockbackDirection,35f);
        }        
    }
}
