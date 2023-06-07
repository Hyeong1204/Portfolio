using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
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
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
    // 이 상태머신에 들어왔을 때 (Entry 했을 때) 실행
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Gamemanager.Inst.Player.ShowWeaponAndSheild(true);
        Gamemanager.Inst.Player.WeaponEffectSwitch(true);           // 무기 이팩트 켜기
    }

    //onstatemachineexit is called when exiting a state machine via its exit node
    // 이 상태머신이 Exit로 갈 때 실행
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        Gamemanager.Inst.Player.WeaponEffectSwitch(false);          // 무기 이팩트 끄기
        animator.SetInteger("ComboState", 0);       // 콤보 상태 리셋
        animator.ResetTrigger("Attack");            // 엍택 트리거도 초기화
    }
}
