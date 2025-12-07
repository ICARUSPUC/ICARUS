using UnityEngine;

public class BackflipEvent : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<BossDragon>().SpawnFireProjectiles();
    }
}
