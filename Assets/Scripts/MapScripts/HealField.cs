using UnityEngine;

public class HealField : MonoBehaviour
{
    // Healing related variables
    float currentTime = 0f;  
    float tickInterval = 1f; 
    private GameObject player;
    public bool playerColliding;
    public AudioClip Tick;

    void Start()
    {
        currentTime = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        currentTime += Time.deltaTime;
        if (currentTime >= tickInterval)
        {
            if (playerColliding)
            {
                player.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(-5); 
                AudioManager.Instance.PlaySound(Tick); 
            }
            currentTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerColliding = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerColliding = false; 
        }
    }
}
