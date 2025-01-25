using UnityEngine;
using UnityEngine.UI;
public class PlayerHPManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int HP;
    public GameObject logicManager;
     public Slider HPBar;
    void Start()
    {
        HPBar.value = HP;
        HPBar.maxValue = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DamageOrHeal(int damage)
    // If something needs to heal the player instead, use this function still but make the int variable passed a negative number
    {
        HP-= damage;
        HPBar.value=HP;
        if (HP<=0)
        {
            Destroy(gameObject);
            logicManager.gameObject.GetComponent<GameHandler>().GameOver();
        }
    }
}
