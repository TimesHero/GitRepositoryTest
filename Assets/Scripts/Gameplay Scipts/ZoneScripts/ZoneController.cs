using UnityEngine;
using TMPro;
public class ZoneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int capturePercentage = 90;
    public bool playerColliding = false;
    bool enemyColliding = false; 
    float currentTime = 0f;  
    float tickInterval = 1f; 
    public GameObject spawnPortal;
    public Transform[] portalSpawnPoints;
    public TextMeshProUGUI percentageText;
    public GameObject logicManager;
    public int zoneLossGameOverCount;
    public bool Captured;
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
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);
            }
            else
            {
                capturePercentage += 1;
                capturePercentage = Mathf.Clamp(capturePercentage, 0, 100); // Ensure it stays within 0-100
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
                
            }

            if (capturePercentage==100)
            {
                percentageText.text = "Move to next Zone";
                Captured=true;
                //gameObject.SetActive(false);
            }
        }

        if (enemyColliding && playerColliding==false)
        {
            capturePercentage -= 1;
            capturePercentage = Mathf.Clamp(capturePercentage, -20, 100); 
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        if (enemyColliding ==false && playerColliding==false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        }
        percentageText.text = "Capture Percentage: " + capturePercentage;

        if (capturePercentage==zoneLossGameOverCount)
        {
            logicManager.gameObject.GetComponent<GameHandler>().GameOver();
        }
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
