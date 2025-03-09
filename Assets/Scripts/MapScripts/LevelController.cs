using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Public variables
    public GameObject[] zones;
    public GameObject[] zone1Portals;
    public GameObject[] zone2Portals;
    public GameObject[] zone3Portals;
    private int capturedZones = 0;
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

    void Update()
    {
        // Zone 1 logic
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 1 && portalsSpawned == 0)
        {
            audioSource.Stop();
            battleMusic.Play();
            zone1Portals[0].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 15 && portalsSpawned == 1)
        {
            zone1Portals[1].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 25 && portalsSpawned == 2)
        {
            zone1Portals[2].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 35 && portalsSpawned == 3)
        {
            zone1Portals[3].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 50 && portalsSpawned == 4)
        {
            zone1Portals[5].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 60 && portalsSpawned == 5)
        {
            zone1Portals[6].SetActive(true);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 70 && portalsSpawned == 6)
        {
            zone1Portals[8].SetActive(true);
            zone1Portals[10].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage == 85 && portalsSpawned == 8)
        {
            zone1Portals[11].SetActive(true);
            zone1Portals[12].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().Captured == true && capturedZones == 0)
        {
            zones[0].SetActive(false);
            foreach (var portal in zone1Portals)
            {
                Destroy(portal);
            }
            capturedZones++;
            zones[capturedZones].SetActive(true);
            portalsSpawned = 0;
            battleMusic.Stop();
            ambientMusic.Play();
            zoneNumberText.text = "Zone 2";
        }

        // Zone 2 logic
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 1 && portalsSpawned == 0)
        {
            ambientMusic.Stop();
            battleMusic.Play();
            zone2Portals[0].SetActive(true);
            portalsSpawned++;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 10 && portalsSpawned == 1)
        {
            zone2Portals[1].SetActive(true);
            portalsSpawned++;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 25 && portalsSpawned == 2)
        {
            zone2Portals[2].SetActive(true);
            portalsSpawned++;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 30 && portalsSpawned == 3)
        {
            zone2Portals[3].SetActive(true);
            portalsSpawned++;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 45 && portalsSpawned == 4)
        {
            zone2Portals[4].SetActive(true);
            zone2Portals[5].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 65 && portalsSpawned == 6)
        {
            zone2Portals[6].SetActive(true);
            zone2Portals[7].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage == 85 && portalsSpawned == 8)
        {
            zone2Portals[8].SetActive(true);
            zone2Portals[9].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[1].gameObject.GetComponent<ZoneController>().Captured == true)
        {
            zones[1].SetActive(false);
            foreach (var portal in zone2Portals)
            {
                Destroy(portal);
            }
            capturedZones++;
            portalsSpawned = 0;
            zones[capturedZones].SetActive(true);
            battleMusic.Stop();
            ambientMusic.Play();
            zoneNumberText.text = "Zone 3";
        }

        // Zone 3 logic
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 1 && portalsSpawned == 0)
        {
            ambientMusic.Stop();
            battleMusic.Play();
            zone3Portals[0].SetActive(true);
            portalsSpawned++;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 10 && portalsSpawned == 1)
        {
            zone3Portals[1].SetActive(true);
            portalsSpawned++;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 20 && portalsSpawned == 2)
        {
            zone3Portals[4].SetActive(true);
            portalsSpawned++;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 35 && portalsSpawned == 3)
        {
            zone3Portals[2].SetActive(true);
            zone3Portals[3].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 45 && portalsSpawned == 5)
        {
            zone3Portals[5].SetActive(true);
            zone3Portals[6].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 55 && portalsSpawned == 7)
        {
            zone3Portals[7].SetActive(true);
            zone3Portals[8].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 70 && portalsSpawned == 9)
        {
            zone3Portals[9].SetActive(true);
            zone3Portals[10].SetActive(true);
            portalsSpawned += 2;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 85 && portalsSpawned == 11)
        {
            zone3Portals[11].SetActive(true);
            portalsSpawned++;
        }

        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage == 100)
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
