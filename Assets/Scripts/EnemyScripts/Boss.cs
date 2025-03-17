using System.Collections;
using Fungus;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;
public class Boss : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float rangedAttackRange;
    public float aoeRange;
    public float eightWayRange;
    public float interval;
    public GameObject shockwave; 
    private GameObject player;
    private GameObject zone;
    private float angle;
    public GameObject enemyBullet;
    Rigidbody2D myRB;
    private string target;
    float currentInterval;
    public Transform lookTransform;
    private float distanceToPlayer;
    private Transform targetTransform;
    public Projectile normalBullet;
    public Projectile eightWayBullet;
    public bool attacking = false; 
    public bool facingRight = true;
    SpriteRenderer myRenderer;
    public GameObject shockwavePrefab; 
    Animator myAnim;
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.GetComponent<AIDestinationSetter>().target = player.transform;
        zone = GameObject.FindGameObjectWithTag("Zone");
        myAnim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        currentInterval = Time.time;
        target = "zone";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<EnemyHPManager>().bossDead==false)
        {
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            lookTransform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            //Attack
            if (Time.time>currentInterval  && distanceToPlayer <= aoeRange)
            {
                StartCoroutine(SpawnShockwave());
            }
            else if (Time.time>currentInterval  && distanceToPlayer <= eightWayRange)
            {
                StartCoroutine(ShootEight());
            }
            else if (Time.time>currentInterval  && distanceToPlayer <= rangedAttackRange)
            {
                StartCoroutine(Shoot());
            }
        }
        //myAnim.SetBool("isAttacking", attacking);
        Flip(angle);
    }

    private IEnumerator SpawnShockwave()
    {
        currentInterval = Time.time + interval;
        yield return new WaitForSeconds(0.4f);
        Instantiate(shockwave, gameObject.transform.position, gameObject.transform.rotation);
        Camera.main.GetComponent<CameraFollow>().TriggerShake(0.5f, 2f, 5f);
        yield return new WaitForSeconds(0.4f);
    }
    public void SpawnBulletNormal()
    {
        GameObject bullet = Instantiate(enemyBullet, lookTransform.position, lookTransform.rotation * Quaternion.Euler(0, 0, -90));
        Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
        rigidbodyB.linearVelocity=normalBullet.velocity*lookTransform.transform.right;
        bullet.gameObject.GetComponent<BulletBase>().PeramPass(normalBullet);
    }
    public void SpawnEightWayBullet(bool offset)
    {
        float spreadAngle = 45f;
        float offsetAngle = 22.5f; 
        for (int i = 0; i < 8; i++)
        {
            if (offset==true)
            {
                angle = i * spreadAngle + offsetAngle;
            }
            else
            {
                angle = i * spreadAngle;
            }
            GameObject bullet = Instantiate(enemyBullet, lookTransform.position, lookTransform.rotation * Quaternion.Euler(0, 0, -90));
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.transform.up;

            rigidbodyB.linearVelocity = eightWayBullet.velocity * direction.normalized;

            float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, bulletAngle - 90)); 

            bullet.gameObject.GetComponent<BulletBase>().PeramPass(eightWayBullet);
        }
    }
    private IEnumerator Shoot()
    {
    // Loop to shoot in 8 directions (Up, Down, Left, Right, and 4 diagonals)
        attacking=true; 
        currentInterval = Time.time + interval;
        yield return new WaitForSeconds(0.2f);
        SpawnBulletNormal();
        yield return new WaitForSeconds(0.2f);
        SpawnBulletNormal();
        yield return new WaitForSeconds(0.2f);
        SpawnBulletNormal();
        yield return new WaitForSeconds(0.2f);
        attacking=false;
    }

    private IEnumerator ShootEight()
    {
    // Loop to shoot in 8 directions (Up, Down, Left, Right, and 4 diagonals)
        attacking=true; 
        currentInterval = Time.time + interval;
        yield return new WaitForSeconds(0.2f);
        SpawnEightWayBullet(false);
        yield return new WaitForSeconds(0.4f);
        SpawnEightWayBullet(true);
        yield return new WaitForSeconds(0.4f);
        SpawnEightWayBullet(false);
        yield return new WaitForSeconds(0.4f);
        SpawnEightWayBullet(true);
        yield return new WaitForSeconds(0.4f);
        attacking=false;
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
