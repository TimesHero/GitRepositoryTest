using System.Collections;
using Pathfinding;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
public class ExplodingEnemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float attackRange;
    private GameObject player;
    private GameObject zone;
    private float angle;
    Rigidbody2D myRB;
    private string target;
    float currentInterval;
    public Transform lookTransform;
    private float distanceToPlayer;
    private Transform targetTransform;
    public AudioSource fuseSound;
    public AudioClip startFuse;
    public AudioClip stopFuse;
    public AudioClip explosionSound; 
    private bool startSound = false;
    private bool startSound2 = false;
    public bool attacking = false; 
    public bool facingRight = true;
    private Vector3 originalScale;
    float explodeCooldown = 0;
    public GameObject explosionParticles; 
    SpriteRenderer myRenderer;
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
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lookTransform.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        //Attack
        Explode();
        
        myAnim.SetBool("isAttacking", attacking);
        Flip(angle);
    }
    public void Explode()
    {
        if (distanceToPlayer <= attackRange)
        {
            startSound2=false;
            attacking=true;
            if (startSound==false)
            {
            fuseSound.clip = startFuse;
            fuseSound.Play();
            }
            startSound=true;
            gameObject.GetComponent<AIPath>().canMove = false;
            transform.localScale = new Vector3(transform.localScale.x + 0.001f,transform.localScale.y + 0.001f,transform.localScale.z);
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f+1f, 1f, 1f, 1f);
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
            if (transform.localScale.x > originalScale.x && player.GetComponent<Collider2D>().enabled == true)
            {
                startSound=false;
                if (startSound2==false)
                {
                fuseSound.clip = stopFuse;
                fuseSound.Play();
                }
                startSound2=true;
                transform.localScale = new Vector3(transform.localScale.x - 0.001f,transform.localScale.y -  0.001f,transform.localScale.z);
                 gameObject.GetComponent<SpriteRenderer>().color = new Color(1f-1f, 1f, 1f, 1f);
                explodeCooldown --;
                if (transform.localScale.x == originalScale.x)
                    {
                    gameObject.GetComponent<AIPath>().canMove = true;
                    attacking=false; 
                    }
            }
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
