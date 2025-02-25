using System.Collections;
using Pathfinding;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float HP;
    GameObject player;
    public GameObject enemyBullet;
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
    private float distanceToPlayer;
    private Transform targetTransform;
    public GameObject explosionParticles; 
    public GameObject deathParticles;
    public AudioClip explosionSound; 
    public AudioClip knockBackSound; 
    public AudioClip[] hurtSound; 
    public AudioClip dieSound; 
    public AudioSource fuseSound;
    public AudioClip startFuse;
    public AudioClip stopFuse;
    private bool startSound = false;
    private bool startSound2 = false;
    public Animator animator;
    //TESTING
    public EnemyBase testMask; 
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentInterval = Time.time;
        //TESTING PURPOSE
        PeramPass(testMask);
    }
     public void PeramPass(EnemyBase enemytype)
    {
        targetTransform = GameObject.FindGameObjectWithTag(enemytype.target).transform;
        gameObject.GetComponent<AIDestinationSetter>().target = targetTransform;

        gameObject.GetComponent<AIPath>().slowdownDistance = enemytype.attackRange + 0.6f;
        gameObject.GetComponent<AIPath>().endReachedDistance = enemytype.attackRange-3;
        
        gameObject.GetComponent<AIPath>().maxSpeed = enemytype.moveSpeed;
        gameObject.GetComponent<AIPath>().canMove =!enemytype.stationary;

        HP = enemytype.HP;
        animator.runtimeAnimatorController = enemytype.animationController;

        gameObject.GetComponent<SpriteRenderer>().sprite = enemytype.enemySprite;
        interval = enemytype.attackInterval;
        stationary=enemytype.stationary;
        currentEnemy = enemytype;
        originalScale = transform.localScale;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lookTransform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

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
                Vector3 currentPosition = transform.position;
                transform.position = new Vector3(currentPosition.x, currentPosition.y);
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
        if (currentEnemy.explodingAttack==true)
        {
            Explode();
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
    public void Explode()
    {
        if (distanceToPlayer <= currentEnemy.attackRange)
        {
            startSound2=false;
            if (startSound==false)
            {
            fuseSound.clip = startFuse;
            fuseSound.Play();
            }
            startSound=true;
            stationary=true;
            gameObject.GetComponent<AIPath>().canMove = !stationary;
            transform.localScale = new Vector3(transform.localScale.x + 0.001f,transform.localScale.y + 0.001f,transform.localScale.z);
            explodeCooldown ++;
            if (explodeCooldown==240)
            {
                player.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(12);
                Instantiate(explosionParticles, transform.position, transform.rotation);
                AudioManager.Instance.PlaySound(explosionSound);
                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.localScale.x > originalScale.x)
            {
                startSound=false;
                if (startSound2==false)
                {
                fuseSound.clip = stopFuse;
                fuseSound.Play();
                }
                startSound2=true;
                transform.localScale = new Vector3(transform.localScale.x - 0.001f,transform.localScale.y -  0.001f,transform.localScale.z);
                explodeCooldown --;
                if (transform.localScale.x == originalScale.x)
                    {
                    stationary=false;
                    gameObject.GetComponent<AIPath>().canMove = !stationary;
                    }
            }
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
            GameObject bullet = Instantiate(enemyBullet, lookTransform.position, lookTransform.rotation * Quaternion.Euler(0, 0, -90));
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.transform.up;

            rigidbodyB.linearVelocity = currentEnemy.bulletType.velocity * direction.normalized;

             float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bulletAngle - 90)); 

            bullet.gameObject.GetComponent<BulletBase>().PeramPass(currentEnemy.bulletType);
        }
    }
    else
        {
            GameObject bullet = Instantiate(enemyBullet, lookTransform.position, lookTransform.rotation * Quaternion.Euler(0, 0, -90));
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            rigidbodyB.linearVelocity=currentEnemy.bulletType.velocity*lookTransform.transform.right;
            bullet.gameObject.GetComponent<BulletBase>().PeramPass(currentEnemy.bulletType);
        }
    currentInterval = Time.time + interval;
    }
  
    public void TakeDamage(float damage)
    {
        HP-= damage;
        int hurtSoundType = Random.Range(0, hurtSound.Length);
        AudioManager.Instance.PlaySound(hurtSound[hurtSoundType]);
        StartCoroutine(DmgFlash());
        if (HP<=0)
        {
            player.gameObject.GetComponent<PlayerHPManager>().ComboTrigger();
            int doDrop = Random.Range(0,9);
            if (doDrop==1)
            {
                int pickupType = Random.Range(0,4);
                Instantiate(pickups[pickupType],transform.position, Quaternion.identity);
            }
            Instantiate(deathParticles, transform.position, transform.rotation);
            AudioManager.Instance.PlaySound(dieSound);
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
