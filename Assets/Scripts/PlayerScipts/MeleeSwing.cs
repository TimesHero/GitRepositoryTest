using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    Rigidbody2D myRB;
    public float rotationTotal = 120;
    public bool attacking =false;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking==true)
        {
            rotationTotal-=12;
            if (rotationTotal>0)
            {
                transform.Rotate(transform.rotation.x,transform.rotation.y,-12f );
            }
            if (rotationTotal<0)
            {
                attacking=false;
                rotationTotal=120;
                gameObject.SetActive(false);
                transform.Rotate(transform.rotation.x,transform.rotation.y,108f );
            }
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
                Destroy(gameObject);
                other.gameObject.GetComponent<Enemy>().TakeDamage(1);

        }        
    }
}
