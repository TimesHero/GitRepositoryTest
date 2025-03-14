using System.Collections;
using Pathfinding;
using UnityEngine;

public class RootedEnemy : MonoBehaviour
{
    public float attackRange;
    public float interval;
    private GameObject player;
    private GameObject zone;
    private float angle;
    public GameObject enemyBullet;
    Rigidbody2D myRB;
    public float burrowDistance;
    float currentInterval;
    public Transform lookTransform;
    private float distanceToPlayer;
    private Transform targetTransform;
    public Projectile bulletType;
    public bool attacking = false; 
    public bool facingRight = true;
    SpriteRenderer myRenderer;
    Animator myAnim;
    public bool burrowing = true; 
    public Collider2D myCollider;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
        zone = GameObject.FindGameObjectWithTag("Zone");
        myAnim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        currentInterval = Time.time;
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
       
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (Time.time > currentInterval && distanceToPlayer <= attackRange)
        {
            StartCoroutine(Shoot());
        }
        if ( distanceToPlayer > attackRange)
        {
            burrowing = true; 
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectile"), false);
        }
        else
        {
            burrowing=false;
            Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectile"), false);
        }

        myAnim.SetBool("isAttacking", attacking);
        myAnim.SetBool("isBurrowing", burrowing);
        Flip(angle);
    }

    private IEnumerator Shoot()
    {
        attacking = true; 
        currentInterval = Time.time + interval;
        yield return new WaitForSeconds(0.2f);

        float spreadAngle = 45f;
        for (int i = 0; i < 8; i++)
        {
            angle = i * spreadAngle;
            GameObject bullet = Instantiate(enemyBullet, lookTransform.position, lookTransform.rotation * Quaternion.Euler(0, 0, -90));
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.transform.up;

            rigidbodyB.linearVelocity = bulletType.velocity * direction.normalized;

            float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bulletAngle - 90)); 

            bullet.gameObject.GetComponent<BulletBase>().PeramPass(bulletType);
        }

        yield return new WaitForSeconds(0.2f);
        attacking = false;
    }

    void Flip(float angle)
    {
        if (angle > 90 || angle < -90)
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
            if (!other.gameObject.GetComponent<PlayerHPManager>().invincible)
                other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(3); 
            print("hurt");
        }        
    }
}
