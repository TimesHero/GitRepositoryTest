using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageModifier(int damage)
    // If something needs to heal the player instead, use this function still but make the int variable passed a negative number
    {
        HP-= damage;//Deals damage to the player.
        if (HP==0)
        {
            Destroy(gameObject);
        }
    }
}
