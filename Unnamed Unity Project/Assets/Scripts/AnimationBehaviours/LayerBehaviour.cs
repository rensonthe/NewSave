using UnityEngine;
using System.Collections;

public class LayerBehaviour : StateMachineBehaviour {

    private PlayerController player;

    private Vector2 jumpSize = new Vector2(0.5f, 0.2f);
    private Vector2 jumpeOffSet = new Vector2(-0.04f, -0.8f);

    private Vector2 size;
    private Vector2 offset;

    private BoxCollider2D boxCollider;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = PlayerController.Instance.GetComponent<PlayerController>();
        if (boxCollider == null)
        {
            boxCollider = PlayerController.Instance.GetComponent<BoxCollider2D>();
            size = boxCollider.size;
            offset = boxCollider.offset;
        }
    }

    //OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetLayerWeight(1) == 0 && animator.gameObject.layer != 10 && !player.IsFalling)
        {
            animator.gameObject.layer = 15;
        }
        if(animator.gameObject.layer == 11 || player.IsJumping)
        {
            if(boxCollider.size != jumpSize && boxCollider.offset != jumpeOffSet)
            {
                boxCollider.size = jumpSize;
                boxCollider.offset = jumpeOffSet;
            }
        }
        if (animator.gameObject.layer == 15|| animator.gameObject.layer == 10)
        {
            boxCollider.size = size;
            boxCollider.offset = offset;
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMachineEnter is called when entering a statemachine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
    //
    //}

    // OnStateMachineExit is called when exiting a statemachine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    //
    //}
}
