using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    public GameObject logicManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageOrHeal(int damage)
    // If something needs to heal the player instead, use this function still but make the int variable passed a negative number
    {
        HP-= damage;
        if (HP<=0)
        {
            Destroy(gameObject);
            logicManager.gameObject.GetComponent<GameHandler>().GameOver();
        }
    }
}
