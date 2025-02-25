using UnityEngine;
using System.Collections;

public class EnemyHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float HP;
    public AudioClip[] hurtSound; 
    public AudioClip dieSound; 
    public GameObject[] pickups;
    public GameObject deathParticles;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
