using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    float currentInterval;
    float interval = 1.0f;
    public GameObject enemyBullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame 
    void Update()
    {
        if (Time.time>currentInterval)//Player Shooting
        {
        GameObject bullet = Instantiate(enemyBullet,  transform.position, transform.rotation);
        Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
        rigidbodyB.linearVelocity=10*gameObject.transform.right;
        currentInterval = Time.time + interval;
        }
    }
}
