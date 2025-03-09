using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] zones;
    public GameObject[] zone1Portals;
    public GameObject[] zone2Portals;
    public GameObject[] zone3Portals;
    private int capturedZones = 2;
    private int portalsSpawned = 0;
    public GameObject wayPoint;
    public Transform player;
    public AudioClip zone1music;
    public AudioClip ambience;
    public AudioSource audioSource; 
    public AudioSource battleMusic;
    public AudioSource ambientMusic;
    public TextMeshProUGUI zoneNumberText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zones[capturedZones].gameObject.GetComponent<ZoneController>().playerColliding == false)
    {
        Vector3 zoneViewportPosition = Camera.main.WorldToViewportPoint(zones[capturedZones].transform.position);
        float margin = 0.25f;
        if (zoneViewportPosition.x < -margin || zoneViewportPosition.x > 1 + margin || zoneViewportPosition.y < -margin || zoneViewportPosition.y > 1 + margin)
        {
            wayPoint.SetActive(true);
            Vector3 directionToZone = zones[capturedZones].transform.position - player.position;
            float angle = Mathf.Atan2(directionToZone.y, directionToZone.x) * Mathf.Rad2Deg;
            angle -= 90;
            wayPoint.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            wayPoint.SetActive(false); 
        }
    }


        //Test level 1------------------------------------------------------------------------------------------------
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==1&&portalsSpawned==0)
        {
            audioSource.Stop();
            battleMusic.Play();
            zone1Portals[0].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==15)
        {
            zone1Portals[1].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==25)
        {
            zone1Portals[2].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==35)
        {
            zone1Portals[3].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==40)
        {
            //zone1Portals[4].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==50)
        {
            zone1Portals[5].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==60)
        {
            zone1Portals[6].SetActive(true);
            //zone1Portals[7].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==70)
        {
            zone1Portals[8].SetActive(true);
            zone1Portals[10].SetActive(true);
        }
         if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==85)
        {
            //zone1Portals[9].SetActive(true);
            zone1Portals[11].SetActive(true);
            zone1Portals[12].SetActive(true);
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==85)
        {
            //zone1Portals[13].SetActive(true);
            //zone1Portals[14].SetActive(true);
        }
        
        if (zones[0].gameObject.GetComponent<ZoneController>().Captured==true && capturedZones==0)
        {
            zones[0].SetActive(false);
            foreach (var portal in zone1Portals)
            {
                Destroy(portal);
            }
            capturedZones++;
            zones[capturedZones].SetActive(true);
            portalsSpawned=0;
            battleMusic.Stop();
            ambientMusic.Play();
            zoneNumberText.text = "Zone 2";
        }
        //Test Level 2--------------------------------------------------------------------------------------------------------
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==1&&portalsSpawned==0)
        {
           
            ambientMusic.Stop();
            battleMusic.Play();
            zone2Portals[0].SetActive(true);
            portalsSpawned++;
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==10)
        {
            zone2Portals[1].SetActive(true);
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==25)
        {
            zone2Portals[2].SetActive(true);
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==30)
        {
            zone2Portals[3].SetActive(true);
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==45)
        {
            zone2Portals[4].SetActive(true);
            zone2Portals[5].SetActive(true);
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==65)
        {
            zone2Portals[6].SetActive(true);
            zone2Portals[7].SetActive(true);
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==85)
        {
            zone2Portals[8].SetActive(true);
            zone2Portals[9].SetActive(true);
        }
         if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==90)
        {
            //zone2Portals[10].SetActive(true);
            //zone2Portals[11].SetActive(true);
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().Captured==true)
        {
            zones[1].SetActive(false);
            foreach (var portal in zone2Portals)
            {
                Destroy(portal);
            }
            capturedZones++;
            zones[capturedZones].SetActive(true);
            portalsSpawned=0;
            battleMusic.Stop();
            ambientMusic.Play();
            zoneNumberText.text = "Zone 3";
        }
        //Test Level 3--------------------------------------------------------------------------------------------------------
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==1&&portalsSpawned==0)
        {
            portalsSpawned++;
            ambientMusic.Stop();
            battleMusic.Play();
            zone3Portals[0].SetActive(true);
        }
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==10)
        {
            zone3Portals[1].SetActive(true);
        }
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==20)
        {
            zone3Portals[4].SetActive(true);
        }
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==35)
        {
            zone3Portals[2].SetActive(true);
            zone3Portals[3].SetActive(true);
        }
         if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==45)
        {
            zone3Portals[5].SetActive(true);
            zone3Portals[6].SetActive(true);
        }
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==55)
        {
            zone3Portals[7].SetActive(true);
            zone3Portals[8].SetActive(true);
        }
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==70)
        {
            zone3Portals[9].SetActive(true);
            zone3Portals[10].SetActive(true);
        }
         if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==85)
        {
            zone3Portals[11].SetActive(true);
        }
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==100)
        {
            gameObject.GetComponent<GameHandler>().GameOver(true);
            zones[2].SetActive(false);
            foreach (var portal in zone3Portals)
            {
                Destroy(portal);
            }
        }
        }
    }

