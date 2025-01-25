using UnityEngine;

public class PickupManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string powerType;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.tag == "Player"){//runs if the obstacle runs into the player
                if (powerType=="heal")
                {
                    other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(-1);
                }
                if (powerType=="atk")
                {
                    other.gameObject.GetComponent<InputScript>().AtkSpeedPickupTrigger();
                }
                if (powerType=="run")
                {
                    other.gameObject.GetComponent<InputScript>().RunSpeedPickupTrigger();
                }
                Destroy(gameObject);//destroys the power up
            }        
        }
}
