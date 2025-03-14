using Fungus;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class CutsceneTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject flowChart;
    public Flowchart fungusFlowchart;
    public bool activeFlowchart; 
    public InputActionAsset inputActions;  
    private InputActionMap playerActionMap;
    private EventSystem input;
    void Start()
    {
        playerActionMap = inputActions.FindActionMap("Player");
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
                OnUse();
        }        
    }

    void OnUse()
        {
            if (activeFlowchart==false)
            {
                fungusFlowchart.ExecuteBlock("Start");
                playerActionMap.Disable();
                activeFlowchart=true;
            }
        }

    public void ActivateControls()
    {
        playerActionMap.Enable();
    }
}
   
