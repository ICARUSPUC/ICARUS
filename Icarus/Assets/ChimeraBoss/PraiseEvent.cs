using UnityEngine;

public class PraiseEvent : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<BossDragon>().SpawnSummonEnemies();
    }
}
