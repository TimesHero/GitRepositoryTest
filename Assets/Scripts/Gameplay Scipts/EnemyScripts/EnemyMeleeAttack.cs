using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float attackRate;
    float nextAttack;
    public int myDamage;
    public bool canAttack = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack && Time.time > nextAttack)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag =="Player")
        {
            canAttack=true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag =="Player")
        {
            canAttack=false;
        }
    }
}
