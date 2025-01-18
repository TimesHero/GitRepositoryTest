using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    public GameObject player;
    public float MovementSpeed;
    Rigidbody2D myRB;
    public bool stationary;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stationary==false)
        {
        float step = MovementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(1);//goes into the player perams and runs the take dmg function. 

        }        
    }
}
