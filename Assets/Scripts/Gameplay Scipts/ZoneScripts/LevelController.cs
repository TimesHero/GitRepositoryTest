using UnityEngine;

public class LevelController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] zones;
    public GameObject[] spawnPortal;
    public Transform[] portalSpawnPoints1;
    public Transform[] portalSpawnPoints2;
    private int capturedZones = 0;
    private int portalsSpawned = 0;
    public GameObject wayPoint;
    public Transform player;

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
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==5 && portalsSpawned==0)
        {
            Instantiate(spawnPortal[0], portalSpawnPoints1[0].position, Quaternion.identity);
            portalsSpawned++;
        }

        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==35 && portalsSpawned==1)
        {
            Instantiate(spawnPortal[1], portalSpawnPoints1[1].position, Quaternion.identity);
            portalsSpawned++;
        }
        if (zones[0].gameObject.GetComponent<ZoneController>().capturePercentage==75 && portalsSpawned==2)
        {
            Instantiate(spawnPortal[1], portalSpawnPoints1[1].position, Quaternion.identity);
            portalsSpawned++;
        }
        
        if (zones[0].gameObject.GetComponent<ZoneController>().Captured==true && capturedZones==0)
        {
            zones[0].SetActive(false);
            capturedZones++;
            zones[capturedZones].SetActive(true);
            portalsSpawned=0;
        }
        //Test Level 2--------------------------------------------------------------------------------------------------------
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==5 && portalsSpawned==0)
        {
            Instantiate(spawnPortal[0], portalSpawnPoints2[2].position, Quaternion.identity);
            portalsSpawned++;
        }
        
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==25 && portalsSpawned==1)
        {
            Instantiate(spawnPortal[0], portalSpawnPoints2[0].position, Quaternion.identity);
            portalsSpawned++;
        }
        if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==50 && portalsSpawned==2)
        {
            Instantiate(spawnPortal[1], portalSpawnPoints2[1].position, Quaternion.identity);
            portalsSpawned++;
        }
         if (zones[1].gameObject.GetComponent<ZoneController>().capturePercentage==75 && portalsSpawned==3)
        {
            Instantiate(spawnPortal[1], portalSpawnPoints2[1].position, Quaternion.identity);
            Instantiate(spawnPortal[1], portalSpawnPoints2[2].position, Quaternion.identity);
            portalsSpawned++;
        }
    }
}
