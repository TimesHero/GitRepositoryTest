using UnityEngine;
using System.Collections;
using Pathfinding;
using UnityEngine.UI;

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
    private GameObject logicManager; 
    Rigidbody2D myRB;
    private bool isKnockedBack = false;
    private Slider bossHPBar;
    public bool bossType; 
    public bool bossDead;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myRB = GetComponent<Rigidbody2D>();
        logicManager = GameObject.FindGameObjectWithTag("Manager");
        
        if (bossType==true)
        {
            GameObject HPBar = GameObject.FindGameObjectWithTag("BossBar");
            bossHPBar = HPBar.GetComponent<Slider>();
            bossHPBar.maxValue = HP;
            bossHPBar.value = HP;
        }
    }
    public void CanMove()
    {
        gameObject.GetComponent<AIPath>().canMove=true; 
    }

    void Update()
    {
        if (logicManager.GetComponent<GameHandler>().gameEnded==true)
        {
            Destroy(gameObject);
        }
    }

     public void TakeDamage(float damage)
    {
        HP-= damage;
        if (bossType==true)
        {
            bossHPBar.value = HP;
        }
        int hurtSoundType = Random.Range(0, hurtSound.Length);
        AudioManager.Instance.PlaySound(hurtSound[hurtSoundType]);
        StartCoroutine(DmgFlash());
        if (HP<=0&&bossType==false)
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
        else
        {
             AudioManager.Instance.PlaySound(dieSound);
             gameObject.GetComponent<AIPath>().canMove=true; 
             bossDead=true; 
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
    if (isKnockedBack) return;
    isKnockedBack = true;
    AudioManager.Instance.PlaySound(knockBackSound);
    Vector2 originalVelocity = myRB.linearVelocity;

    knockbackDirection = direction.normalized;  
    knockbackAmount = knockbackForce;
    myRB.AddForce(knockbackDirection * -knockbackAmount, ForceMode2D.Impulse);
    StartCoroutine(ColliderDisable(originalVelocity));
}

private IEnumerator ColliderDisable(Vector2 originalVelocity)
{
    gameObject.GetComponent<AIPath>().canMove = false;
    gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);

    yield return new WaitForSeconds(0.5f);

    gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
    gameObject.GetComponent<AIPath>().canMove = true;

    myRB.linearVelocity = originalVelocity;

    isKnockedBack = false;
}



}
