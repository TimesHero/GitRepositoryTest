using System.Text.RegularExpressions;
using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject[] zones;
    public GameObject[] hpZones;
    public GameObject[] zone1Portals;
    public GameObject[] zone2Portals;
    public GameObject[] zone3Portals;
    private int capturedZones = 0;
    private int portalsSpawned = 0;
    public GameObject wayPoint;
    public GameObject hpWayPoint;
    public GameObject hpWayPointArrow;
    public Transform player;
    public AudioClip zone1music;
    public AudioClip ambience;
    public AudioSource audioSource; 
    public AudioSource battleMusic;
    public AudioSource ambientMusic;
    public TextMeshProUGUI zoneNumberText;
    public Flowchart fungusFlowchart;
    public Flowchart bossFlowchart;
    public string zone2Block; 
    public string zone3Block; 
    public string finalBossSpawnBlock; 
    public string finalBossKillBlock; 
    private InputActionMap playerActionMap;
    public InputActionAsset inputActions;  
    public GameObject zoneUI;
    public GameObject BossUI; 
    public GameObject Boss; 
    private bool zone3Complete=false; 
    private bool finalCutsceneCalled=false;
    public GameObject contestIndicator; 
    public GameObject playerIndicator; 
    public GameObject enemyIndicator; 
    private bool playerPopUp = false;
    private bool enemyPopUp = false; 
    private bool contestPopUp = false; 
    public GameObject captureFrame; 

    void Start()
    {
        playerActionMap = inputActions.FindActionMap("Player");
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
        else
        {
            wayPoint.SetActive(false); 
        }
        //--------------HP WAYPOINT-------------------------------------
        if (hpZones[capturedZones].gameObject.GetComponent<HealField>().playerColliding == false && player.GetComponent<PlayerHPManager>().HP <= player.GetComponent<PlayerHPManager>().HPMax / 3f)
        {
            hpWayPointArrow.SetActive(true);  
            Vector3 hpZoneViewportPosition = Camera.main.WorldToViewportPoint(hpZones[capturedZones].transform.position);
            float margin = -0.1f;
            if (hpZoneViewportPosition.x < -margin || hpZoneViewportPosition.x > 1 + margin || hpZoneViewportPosition.y < -margin || hpZoneViewportPosition.y > 1 + margin)
            {
                hpWayPoint.SetActive(true);
                Vector3 directionToHPZone = hpZones[capturedZones].transform.position - player.position;
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
        if (zones[capturedZones].gameObject.GetComponent<ZoneController>().playerCapture==true && playerPopUp==false)
        {
            playerIndicator.GetComponent<Animator>().SetTrigger("popUpTriggerP");
            playerPopUp = true;
            enemyPopUp = false;
            contestPopUp = false;
            captureFrame.GetComponent<Animator>().SetBool("Player",true);
            captureFrame.GetComponent<Animator>().SetBool("Losing",false);
        }
        if (zones[capturedZones].gameObject.GetComponent<ZoneController>().contested==true && contestPopUp==false)
        {
            contestIndicator.GetComponent<Animator>().SetTrigger("popUpTriggerC");
            playerPopUp = false;
            enemyPopUp = false;
            contestPopUp = true;
            captureFrame.GetComponent<Animator>().SetBool("Player",false);
            captureFrame.GetComponent<Animator>().SetBool("Losing",false);
        }
        if (zones[capturedZones].gameObject.GetComponent<ZoneController>().enemyCapture==true && enemyPopUp==false )
        {
            enemyIndicator.GetComponent<Animator>().SetTrigger("popUpTriggerE");
            playerPopUp = false;
            enemyPopUp = true;
            contestPopUp = false;
            captureFrame.GetComponent<Animator>().SetBool("Player",false);
            captureFrame.GetComponent<Animator>().SetBool("Losing",true);
        }
        if (zones[capturedZones].gameObject.GetComponent<ZoneController>().enemyCapture==false && zones[capturedZones].gameObject.GetComponent<ZoneController>().playerCapture==false)
        {
            captureFrame.GetComponent<Animator>().SetBool("Player",false);
            captureFrame.GetComponent<Animator>().SetBool("Losing",false);
            playerPopUp = false;
            enemyPopUp = false;
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
            fungusFlowchart.ExecuteBlock(zone2Block);
            playerActionMap.Disable();
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
        if (zones[1].gameObject.GetComponent<ZoneController>().Captured==true&& capturedZones==1)
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
            fungusFlowchart.ExecuteBlock(zone3Block);
            playerActionMap.Disable();
        }
        //Test Level 3--------------------------------------------------------------------------------------------------------
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==1&&portalsSpawned==0)
        {
            portalsSpawned++;
            Debug.Log("ZONE3");
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
        if (zones[2].gameObject.GetComponent<ZoneController>().capturePercentage==100&&zone3Complete==false)
        {
            zones[2].SetActive(false);
            foreach (var portal in zone3Portals)
            {
                Destroy(portal);
            }
            Boss.SetActive(true);
            BossUI.SetActive(true);
            zoneUI.SetActive(false);
            zone3Complete=true;
            playerActionMap.Disable();
            bossFlowchart.ExecuteBlock(finalBossSpawnBlock);
        }
        if (Boss.GetComponent<EnemyHPManager>().bossDead==true&&finalCutsceneCalled==false&&Boss!=null)
        {
            playerActionMap.Disable();
            bossFlowchart.ExecuteBlock(finalBossKillBlock);
            finalCutsceneCalled=true;
        }
        }
    }

