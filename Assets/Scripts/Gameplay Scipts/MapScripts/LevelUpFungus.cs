using UnityEngine;

public class LevelUpFungus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject buttonPrompt;
    public GameObject flowChart;
    public bool activeFlowchart; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                buttonPrompt.SetActive(true);
                other.gameObject.GetComponent<InputScript>().interactables.Add(gameObject);
        }        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                buttonPrompt.SetActive(false);
                other.gameObject.GetComponent<InputScript>().interactables.Remove(gameObject);
        }        
    }

    void OnUse()
        {
            if (activeFlowchart==false)
            {
                flowChart.SetActive(true);
                activeFlowchart=true;
            }
        }
}
