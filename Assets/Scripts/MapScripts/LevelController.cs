using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] zones;
    public GameObject[] zone1Portals;
    public GameObject[] zone2Portals;
    private int capturedZones = 0;
    private int portalsSpawned = 0;
    public GameObject wayPoint;
    public Transform player;
    public AudioClip zone1music;
    public AudioClip ambience;
    public AudioSource audioSource; 
    public TextMeshProUGUI zoneNumberText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zones[capturedZones].gameObject.GetComponent<ZoneController>().playerColliding ==false)
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

        //Test level 1------------------------------------------------------------------------------------------------
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==1&&portalsSpawned<1)
        {
            audioSource.clip = zone1music;
            audioSource.volume = 0.2f;
            audioSource.Play();
            zone1Portals[0].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==1)
        {
            zone1Portals[1].SetActive(true);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==25)
        {
            zone1Portals[2].SetActive(true);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==25)
        {
            zone1Portals[3].SetActive(true);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==50)
        {
            zone1Portals[4].SetActive(true);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==60)
        {
            zone1Portals[5].SetActive(true);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==80)
        {
            zone1Portals[6].SetActive(true);
            zone1Portals[7].SetActive(true);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==90)
        {
            zone1Portals[8].SetActive(true);
            portalsSpawned++;
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
            audioSource.clip = ambience;
            audioSource.Play();
            zoneNumberText.text = "Zone 2";
        }
        //Test Level 2--------------------------------------------------------------------------------------------------------
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==1&&portalsSpawned<1)
        {
            audioSource.clip = zone1music;
            audioSource.volume = 0.2f;
            audioSource.Play();
            zone2Portals[0].SetActive(true);
            portalsSpawned++;
        }
    }
}
