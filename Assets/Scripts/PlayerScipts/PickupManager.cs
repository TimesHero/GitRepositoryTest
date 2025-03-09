using UnityEngine;
using UnityEngine.UIElements;

public class PickupManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string powerType;
    GameObject player;
    public float floatHeight = 0.2f;  // The maximum height it will float to
    public float floatSpeed = 1f;   // Speed of the floating motion
    private Vector3 startPosition;  // The initial position of the object
    public AudioClip pickupSound;
    public GameObject pickupParticle;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight + startPosition.y;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
    private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.tag == "Player"){//runs if the obstacle runs into the player
                if (powerType=="heal")
                {
                    other.gameObject.GetComponent<PlayerHPManager>().DamageOrHeal(-10);
                }
                if (powerType=="atk")
                {
                    other.gameObject.GetComponent<InputScript>().AtkSpeedPickupTrigger();
                }
                if (powerType=="run")
                {
                    other.gameObject.GetComponent<InputScript>().RunSpeedPickupTrigger();
                }
                if (powerType=="mana")
                {
                    other.gameObject.GetComponent<PlayerHPManager>().UseMana(-20);
                }
                AudioManager.Instance.PlaySound(pickupSound);
                Instantiate(pickupParticle, transform.position, transform.rotation);
                Destroy(gameObject);//destroys the power up

            }        
        }
}
