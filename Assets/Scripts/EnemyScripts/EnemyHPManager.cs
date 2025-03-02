using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float HP;
    public float scoreValue;
    public AudioClip[] hurtSound; 
    public AudioClip dieSound; 
    public GameObject[] pickups;
    public GameObject deathParticles;
    GameObject player;
    private bool knockback;
    private float knockbackTime = 0f;   
    private float knockbackDuration = 0.2f;
    private Vector2 knockbackDirection;
    private float knockbackAmount;
    public AudioClip knockBackSound; 
    Rigidbody2D myRB;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
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
     public void TakeDamage(float damage)
    {
        HP-= damage;
        int hurtSoundType = Random.Range(0, hurtSound.Length);
        AudioManager.Instance.PlaySound(hurtSound[hurtSoundType]);
        StartCoroutine(DmgFlash());
        if (HP<=0)
        {
            player.gameObject.GetComponent<PlayerHPManager>().ComboTrigger(scoreValue);
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
        gameObject.GetComponent<AIPath>().canMove =false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);
        yield return new WaitForSeconds(0.5f); // Time invincible
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        gameObject.GetComponent<AIPath>().canMove =true;
    }

}
