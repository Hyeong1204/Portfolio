using UnityEngine;

public class RandomIdleSelector : StateMachineBehaviour
{
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(0))     // 0번째 레이어가 트랜지션 중인지 아닌지 확인해서 아닐경우 아래코드 실행
        {
            animator.SetInteger("IdleSelect", RandomSelect());
        }
    }

    int RandomSelect()
    {
        float num = Random.Range(0.0f, 1.0f);

        //if (select < 0.04f)             // 4%
        //{
        //    return 2;
        //}
        //else if (select < 0.07f)        // 3%
        //{
        //    return 4;
        //}
        //else if (select < 0.1f)         // 3%
        //{
        //    return 3;
        //}
        //else if (select < 0.3f)         // 20%
        //{
        //    return 1;
        //}
        //else                            // 70%
        //{
        //    return 0;
        //}

        if (num < 0.7f)
        {
            // 70%
            return 0;
        }
        else if (num < 0.9f)
        {
            // 20%
            return 1;
        }
        else if (num < 0.94f)
        {
            // 4%
            return 2;
        }
        else if (num < 0.97f)
        {
            // 3%
            return 3;
        }
        else
        {
            // 3%
            return 4;
        }
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
