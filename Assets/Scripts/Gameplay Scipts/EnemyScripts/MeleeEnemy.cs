using System.Collections;
using Pathfinding;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
public class MeleeEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float attackRange;
    public float aggroRange; 
    public float interval;
    private GameObject player;
    private GameObject zone;
    private float angle;
    public GameObject enemyBullet;
    Rigidbody2D myRB;
    private string target;
    private bool knockback;
    private float knockbackTime = 0f;   
    private float knockbackDuration = 0.2f;
    private Vector2 knockbackDirection;
    private float knockbackAmount;
    float currentInterval;
    public Transform lookTransform;
    private float distanceToPlayer;
    private Transform targetTransform;
    public AudioClip knockBackSound; 
    public Projectile bulletType;
    public bool attacking = false; 
    public bool facingRight = true;
    SpriteRenderer myRenderer;
    Animator myAnim;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        zone = GameObject.FindGameObjectWithTag("Zone");
        myAnim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        currentInterval = Time.time;
        target = "zone";
        //TESTING PURPOSE
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lookTransform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //Attack
        if (Time.time>currentInterval  && distanceToPlayer <= attackRange)
        {
            StartCoroutine(Shoot());
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
        myAnim.SetBool("isAttacking", attacking);
        Flip(angle);

        if (distanceToPlayer <= aggroRange)
        {
            gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
            target = "Player";
        }
        else
        {
            gameObject.GetComponent<AIDestinationSetter>().target = GameObject.FindGameObjectWithTag("Zone").transform;
            target = "Zone";
        }
    }
    private IEnumerator Shoot()
    {
    // Loop to shoot in 8 directions (Up, Down, Left, Right, and 4 diagonals)
        attacking=true; 
        currentInterval = Time.time + interval;
        gameObject.GetComponent<AIPath>().canMove =false;
        yield return new WaitForSeconds(0.2f);
        GameObject bullet = Instantiate(enemyBullet, lookTransform.position, lookTransform.rotation * Quaternion.Euler(0, 0, -90));
        Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
        rigidbodyB.linearVelocity=bulletType.velocity*lookTransform.transform.right;
        bullet.gameObject.GetComponent<BulletBase>().PeramPass(bulletType);
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<AIPath>().canMove =true;
        attacking=false;
    }
    
    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        AudioManager.Instance.PlaySound(knockBackSound);
        knockback = true; 
        knockbackDirection = direction.normalized;  
        knockbackAmount = knockbackForce;
        knockbackTime = Time.time + knockbackDuration;
        StartCoroutine(ColliderDisable());

    }
    private IEnumerator ColliderDisable()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(0.5f); // Time invincible
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    }
    void Flip(float angle)
    {
        if (angle > 90 || angle < -90) // Looking left
        {
            if (facingRight)
            {
                facingRight = false;
                myRenderer.flipX = true;
            }
        }
        else
        {
            if (!facingRight)
            {
                facingRight = true;
                myRenderer.flipX = false;
            }
        }
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
