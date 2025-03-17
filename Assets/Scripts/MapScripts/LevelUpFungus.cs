using Fungus;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class LevelUpFungus : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject buttonPrompt;
    public GameObject flowChart;
    public Flowchart fungusFlowchart;
    public bool activeFlowchart; 
    public InputActionAsset inputActions;  // Reference to the InputActionAsset (assigned in the Inspector)
    private InputActionMap playerActionMap;
    public GameObject lvlUpScreen;
    public GameObject lvlUpScript;
    private EventSystem input;
    void Start()
    {
        playerActionMap = inputActions.FindActionMap("Player");
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
                //EventSystem.current.SetSelectedGameObject(null);
                fungusFlowchart.ExecuteBlock("Start");
                playerActionMap.Disable();
                
            }
        }

    public void ActivateControls()
    {
        playerActionMap.Enable();
        activeFlowchart=false;
    }
    public void SetScreenActive()
    {
        lvlUpScreen.SetActive(true);
        lvlUpScript.GetComponent<LevelUpButtons>().SetInput();
    }
}
