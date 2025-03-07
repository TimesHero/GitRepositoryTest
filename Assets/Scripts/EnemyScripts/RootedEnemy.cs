using System.Collections;
using UnityEngine;
public class RootedEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    private float Steptimer = 0f;
    private bool stepping = false; 
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        zone = GameObject.FindGameObjectWithTag("Zone");
        myAnim = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
        currentInterval = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lookTransform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer >= burrowDistance)
        {
            Steptimer += Time.deltaTime;

            if (Steptimer >= 1 && stepping == false)
            {
                 StartCoroutine(Burrow());
            }
        }
        //Attack
        if (Time.time>currentInterval  && distanceToPlayer <= attackRange)
        {
            StartCoroutine(Shoot());
        }
        myAnim.SetBool("isAttacking", attacking);
        Debug.Log(attacking);
        Flip(angle);
    }
    private IEnumerator Burrow()
    {
        stepping = true;
        myAnim.SetBool("isBurrowing",true);
        yield return new WaitForSeconds(1f);
        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector2 targetPosition = (Vector2)player.transform.position - directionToPlayer * 6f; // last number is distance from the player
        if (!IsValidPosition(targetPosition))   // Check if the calculated target position is valid
        {
            Vector2 adjustedPosition = FindClosestValidPosition(targetPosition);
            transform.position = adjustedPosition;
        }
        else
        {
            transform.position = targetPosition;
        }
        Vector3 currentPosition = transform.position;
        myAnim.SetBool("isBurrowing", false);
        transform.position = new Vector3(currentPosition.x, currentPosition.y);
        Steptimer = 0f;
        stepping = false; 
    }
    private Vector2 FindClosestValidPosition(Vector2 targetPosition)
    {
        float searchRadius = 30f; 
        Vector2 closestValidPosition = targetPosition;

        for (float xOffset = -searchRadius; xOffset <= searchRadius; xOffset += 0.5f)
        {
            for (float yOffset = -searchRadius; yOffset <= searchRadius; yOffset += 0.5f)
            {
                Vector2 checkPosition = targetPosition + new Vector2(xOffset, yOffset);

                if (IsValidPosition(checkPosition))
                {
                    closestValidPosition = checkPosition;
                    break;
                }
            }
            if (closestValidPosition != targetPosition)
                break;
        }

        return closestValidPosition;
    }
    bool IsValidPosition(Vector2 position)
    {
        LayerMask obstacleLayer = LayerMask.GetMask("Obstacle"); 
        return !Physics2D.OverlapCircle(position, 0.5f, obstacleLayer); // Check if there's any collider in the area
    }


    private IEnumerator Shoot()
    {
    // Loop to shoot in 8 directions (Up, Down, Left, Right, and 4 diagonals)
        {
            attacking=true; 
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
            attacking=false;
        }
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
