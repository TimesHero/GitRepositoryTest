using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject zones;
    public GameObject hpZone;
    public GameObject[] Portals;
    private int capturedZones = 0;
    private int portalsSpawned = 0;
    public GameObject wayPoint;
    public Transform player;
    public AudioSource audioSource; 
    public AudioSource battleMusic;
    public AudioSource ambientMusic;
    public Flowchart fungusFlowchart;
    private InputActionMap playerActionMap;
    public InputActionAsset inputActions;  
    public string portal1TutorialBlock; 
    public string portal2TutorialBlock; 
    public string zoneCappedBlock; 
    public GameObject contestIndicator; 
    public GameObject playerIndicator; 
    public GameObject enemyIndicator; 
    private bool playerPopUp = false;
    private bool enemyPopUp = false; 
    private bool contestPopUp = false; 
    public GameObject hpWayPoint;
    public GameObject hpWayPointArrow;

    void Start()
    {
        playerActionMap = inputActions.FindActionMap("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (zones.gameObject.GetComponent<ZoneController>().playerColliding == false)
    {
        Vector3 zoneViewportPosition = Camera.main.WorldToViewportPoint(zones.transform.position);
        float margin = 0.25f;
        if (zoneViewportPosition.x < -margin || zoneViewportPosition.x > 1 + margin || zoneViewportPosition.y < -margin || zoneViewportPosition.y > 1 + margin)
        {
            wayPoint.SetActive(true);
            Vector3 directionToZone = zones.transform.position - player.position;
            float angle = Mathf.Atan2(directionToZone.y, directionToZone.x) * Mathf.Rad2Deg;
            angle -= 90;
            wayPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            wayPoint.SetActive(false); 
        }
    }
    if (hpZone.gameObject.GetComponent<HealField>().playerColliding == false && player.GetComponent<PlayerHPManager>().HP <= player.GetComponent<PlayerHPManager>().HPMax / 3f)
        {
            hpWayPointArrow.SetActive(true);  
            Vector3 hpZoneViewportPosition = Camera.main.WorldToViewportPoint(hpZone.transform.position);
            float margin = -0.1f;
            if (hpZoneViewportPosition.x < -margin || hpZoneViewportPosition.x > 1 + margin || hpZoneViewportPosition.y < -margin || hpZoneViewportPosition.y > 1 + margin)
            {
                hpWayPoint.SetActive(true);
                Vector3 directionToHPZone = hpZone.transform.position - player.position;
                float hpAngle = Mathf.Atan2(directionToHPZone.y, directionToHPZone.x) * Mathf.Rad2Deg;
                hpAngle -= 90;
                hpWayPoint.transform.rotation = Quaternion.Euler(0, 0, hpAngle);
            }
            else
            {
                hpWayPoint.SetActive(false);
                hpWayPointArrow.SetActive(false);  
            }
        }
        else
            {
                hpWayPoint.SetActive(false);
                hpWayPointArrow.SetActive(false);  
            }
        
         //-------------------------------------------------------------
        if (zones.gameObject.GetComponent<ZoneController>().playerCapture==true && playerPopUp==false)
        {
            playerIndicator.GetComponent<Animator>().SetTrigger("popUpTriggerP");
            playerPopUp = true;
            enemyPopUp = false;
            contestPopUp = false;
        }
        if (zones.gameObject.GetComponent<ZoneController>().contested==true && contestPopUp==false)
        {
            contestIndicator.GetComponent<Animator>().SetTrigger("popUpTriggerC");
            playerPopUp = false;
            enemyPopUp = false;
            contestPopUp = true;
        }
        if (zones.gameObject.GetComponent<ZoneController>().enemyCapture==true && enemyPopUp==false )
        {
            enemyIndicator.GetComponent<Animator>().SetTrigger("popUpTriggerE");
            playerPopUp = false;
            enemyPopUp = true;
            contestPopUp = false;
        }
        //---------------------------------------
        if (zones.gameObject.GetComponent<ZoneController>().capturePercentage==1)
        {
            audioSource.Stop();
            battleMusic.Play();
        }
        if (zones.gameObject.GetComponent<ZoneController>().capturePercentage==10&&portalsSpawned!=1)
        {
            Portals[0].SetActive(true);
            portalsSpawned++;
            fungusFlowchart.ExecuteBlock(portal1TutorialBlock);
            playerActionMap.Disable();
           
        }
        if (zones.gameObject.GetComponent<ZoneController>().capturePercentage==35)
        {
            Portals[1].SetActive(true);
        }
        if (zones.gameObject.GetComponent<ZoneController>().capturePercentage==60&&portalsSpawned!=2)
        {
            Portals[2].SetActive(true);
            portalsSpawned++;
            fungusFlowchart.ExecuteBlock(portal2TutorialBlock);
            playerActionMap.Disable();
           
        }
        if (zones.gameObject.GetComponent<ZoneController>().Captured==true && capturedZones==0)
        {
            zones.SetActive(false);
            foreach (var portal in Portals)
            {
                Destroy(portal);
            }
            portalsSpawned=0;
            battleMusic.Stop();
            ambientMusic.Play();
            fungusFlowchart.ExecuteBlock(zoneCappedBlock);
            playerActionMap.Disable();
            capturedZones++;
        }

    }

}