using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ZoneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int capturePercentage = 90;
    public bool playerColliding = false;
    bool enemyColliding = false; 
    public ParticleSystem outlineParticles; 
    float currentTime = 0f;  
    float tickInterval = 0.8f; 
    public TextMeshProUGUI percentageText;
    public GameObject logicManager;
    public GameObject uiColour; 
    public GameObject outline; 
    public int zoneLossGameOverCount;
    public bool Captured;
    public int enemieInZone = 0; 
    public AudioClip upTick;
    public AudioClip downTick;
    public AudioClip contestSound;
    public AudioClip cappedSound;

    
void Start()
{
    currentTime = 0f;
}

    [System.Obsolete]
    void Update()
{
    currentTime += Time.deltaTime; 

    if (currentTime >= tickInterval)
    {
        currentTime = 0f;

        if (playerColliding)
        {
            if (enemieInZone!=0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 0.25f);
                outlineParticles.startColor = new Color(1, 0.5f, 0.5f, 0.5f);
                uiColour.gameObject.GetComponent<Image>().color = new Color(1, 0.5f, 0.5f);
                outline.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f);
                AudioManager.Instance.PlaySound(contestSound); 
            }
            else
            {
                capturePercentage += 1;
                capturePercentage = Mathf.Clamp(capturePercentage, 0, 100); // Ensure it stays within 0-100
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 0.25f);
                outlineParticles.startColor = new Color(1, 1, 0f, 0.5f);
                uiColour.gameObject.GetComponent<Image>().color = new Color(1, 1, 0);
                outline.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0);
                AudioManager.Instance.PlaySound(upTick); 
                
            }

            if (capturePercentage==100)
            {
                percentageText.text = "Move to next Zone";
                AudioManager.Instance.PlaySound(cappedSound); 
                Captured=true;
            }
        }

        if (enemieInZone!=0 && playerColliding==false)
        {
            capturePercentage -= 1;
            capturePercentage = Mathf.Clamp(capturePercentage, -20, 100); 
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 0.25f);
            outlineParticles.startColor = new Color(1f, 0f, 1f, 0.5f);
            uiColour.gameObject.GetComponent<Image>().color = new Color(1, 0 , 1);
            outline.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0 , 1);
            AudioManager.Instance.PlaySound(downTick); 
        }
        if (enemieInZone==0 && playerColliding==false)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.25f);
            outlineParticles.startColor = new Color(1f, 1f, 1f, 0.5f);
            uiColour.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            outline.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        }
        percentageText.text = capturePercentage + "%";

        if (capturePercentage==-20)
        {
            logicManager.gameObject.GetComponent<GameHandler>().GameOver(false);
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
            
            enemieInZone++;
            
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
            
            enemieInZone--; 
            
        }   
    }
}
