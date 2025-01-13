using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    public GameObject player;
    public float MovementSpeed;
    Rigidbody2D myRB;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = MovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        float angle = Mathf.Atan2(player.transform.position.y, player.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    public void TakeDamage(int damage)
    {
        HP-= damage;
        if (HP==0)
        {
            Destroy(gameObject);
        }
    }
     private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                Destroy(gameObject);//destroys itself
                other.gameObject.GetComponent<PlayerHPManager>().DamageModifier(1);//goes into the player perams and runs the take dmg function. 

        }        
    }
}
