using UnityEngine;
using TMPro;
public class ZoneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    int capturePercentage = 0;
    bool playerColliding = false;
    bool enemyColliding = false; 
    float currentTime = 0f;  
    float tickInterval = 1f; 
    public GameObject spawnPortal;
    public Transform[] portalSpawnPoints;
    public TextMeshProUGUI percentageText;
void Start()
{
    currentTime = 0f;
}

void Update()
{
    currentTime += Time.deltaTime; 

    if (currentTime >= tickInterval)
    {
        currentTime = 0f;

        if (playerColliding)
        {
            if (enemyColliding)
            {
                Debug.Log("contested");
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            else
            {
                capturePercentage += 1;
                capturePercentage = Mathf.Clamp(capturePercentage, 0, 100); // Ensure it stays within 0-100
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
                Debug.Log(capturePercentage);
                
            }

            if (capturePercentage==25)
            {
                int spawnPosition = Random.Range(0,3);
                GameObject portal = Instantiate(spawnPortal, portalSpawnPoints[spawnPosition].position, Quaternion.identity);
            }
        }

        if (enemyColliding && playerColliding==false)
        {
            capturePercentage -= 1;
            capturePercentage = Mathf.Clamp(capturePercentage, 0, 100); // Ensure it stays within 0-100
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            Debug.Log(capturePercentage);
        }
        if (enemyColliding ==false && playerColliding==false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        percentageText.text = "Capture Percentage: " + capturePercentage;
    }
}
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            
            playerColliding=true;
            
        }   
        if (other.tag == "Enemy")
        {
            
            enemyColliding=true;
            
        }       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            playerColliding=false;
        }
        if (other.tag == "Enemy")
        {
            
            enemyColliding=false;
            
        }   
    }
}
