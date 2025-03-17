using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject zones;
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