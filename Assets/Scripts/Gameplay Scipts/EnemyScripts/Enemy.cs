using System.Collections;
using UnityEditor;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int HP;
    GameObject player;
    GameObject movementTarget;
    public GameObject enemyBullet;
    private float movementSpeed;
    Rigidbody2D myRB;
    public GameObject[] pickups;
    private string target;
    private bool knockback;
    public bool stationary;
    private float knockbackTime = 0f;   
    private float knockbackDuration = 0.2f;
    private Vector2 knockbackDirection;
    private float knockbackAmount;
    EnemyBase currentEnemy;
    float currentInterval;
    float interval;
    public Transform lookTransform;
    private Vector3 originalScale;
    float explodeCooldown = 0;
    private float Steptimer = 0f;
    AudioSource sound;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentInterval = Time.time;
        sound = gameObject.GetComponent<AudioSource>();
    }
     public void PeramPass(EnemyBase enemytype)
    {
        movementTarget =  GameObject.FindGameObjectWithTag(enemytype.target);
        HP = enemytype.HP;
        movementSpeed = enemytype.moveSpeed;
        gameObject.GetComponent<SpriteRenderer>().sprite = enemytype.enemySprite;
        interval = enemytype.attackInterval;
        stationary=enemytype.stationary;
        currentEnemy = enemytype;
        originalScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if (stationary==false)
        {
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, movementTarget.transform.position, step);
        }
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lookTransform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        //Stationary Enemy Movement
        if (stationary==true && currentEnemy.eightWayAttack==true && distanceToPlayer >= 10)
        {
            Steptimer += Time.deltaTime;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);

            if (Steptimer >= 1)
            {
                
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                //last number is distance from the player
                transform.position = (Vector2)player.transform.position - directionToPlayer * 6f;
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                Steptimer = 0f;
            }
        }
        //Attack
        if (Time.time>currentInterval && currentEnemy.bulletType!=null && distanceToPlayer <= currentEnemy.attackRange)
        {
            Shoot();
        }
        //exploding attack
        if (currentEnemy.explodingAttack==true &&distanceToPlayer <= currentEnemy.attackRange)
        {
            stationary=true;
            transform.localScale = new Vector3(transform.localScale.x + 0.001f,transform.localScale.y + 0.001f,transform.localScale.z);
            explodeCooldown ++;
            if (explodeCooldown==240)
            {
                player.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(12);
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.localScale.x > originalScale.x)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.001f,transform.localScale.y -  0.001f,transform.localScale.z);
                explodeCooldown --;
                if (transform.localScale.x == originalScale.x)
                    {
                    stationary=false;
                    }
            }
        }
        
        //Knockback
        if (knockback)
        {
            myRB.AddForce(knockbackDirection * -knockbackAmount, ForceMode2D.Impulse);
            knockback = false;
        }
        if (Time.time > knockbackTime)
        {
            knockback = false;  
            myRB.linearVelocity = Vector2.zero;  
        }
    }
    public void Shoot()
    {
    // Loop to shoot in 8 directions (Up, Down, Left, Right, and 4 diagonals)
    if (currentEnemy.eightWayAttack==true)
    {
        float spreadAngle = 45f;
        for (int i = 0; i < 8; i++)
        {
            float angle = i * spreadAngle;
            GameObject bullet = Instantiate(enemyBullet, transform.position, transform.rotation);
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.transform.up;

            rigidbodyB.linearVelocity = currentEnemy.bulletType.velocity * direction.normalized;
            bullet.gameObject.GetComponent<BulletBase>().PeramPass(currentEnemy.bulletType);
        }
    }
    else
        {
            GameObject bullet = Instantiate(enemyBullet,  transform.position, transform.rotation);
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            rigidbodyB.linearVelocity=currentEnemy.bulletType.velocity*lookTransform.transform.right;
            bullet.gameObject.GetComponent<BulletBase>().PeramPass(currentEnemy.bulletType);
        }
    currentInterval = Time.time + interval;
    }
  
    public void TakeDamage(int damage)
    {
        HP-= damage;
        StartCoroutine(DmgFlash());
        sound.Play();
        if (HP<=0)
        {
            player.gameObject.GetComponent<PlayerHPManager>().ComboTrigger();
            int doDrop = Random.Range(0,9);
            if (doDrop==1)
            {
                int pickupType = Random.Range(0,3);
                Instantiate(pickups[pickupType],transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
    private IEnumerator DmgFlash()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0f, 0f, 1f); 
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    
    
    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        knockback = true;  
        knockbackDirection = direction.normalized;  
        knockbackAmount = knockbackForce;
        knockbackTime = Time.time + knockbackDuration;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                if (other.gameObject.GetComponent<PlayerHPManager>().invincible == false)
                    //Destroy(gameObject);//destroys itself
                    other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(3);//goes into the player perams and runs the take dmg function. 
                    print("hurt");

        }        
    }
}
