using UnityEngine;

public class TickStateBehavior : StateMachineBehaviour
{
    public AudioClip sfx; 
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AudioManager.Instance.PlaySound(sfx);
    }
    
}
