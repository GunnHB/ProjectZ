using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectZ.Core.FSM
{
    public class ReturnToNormalState : StateMachineBehaviour
    {
        private Characters.CharacterControls _controls;
        private Characters.PlayerControls _pControls;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_controls == null)
                _controls = animator.GetComponent<Characters.CharacterControls>();

            if (_controls != null && _pControls == null)
                _pControls = _controls as Characters.PlayerControls;

            if (_pControls != null)
            {
                _pControls.ThisAnimator.applyRootMotion = false;

                _pControls.CanMoveFlag = true;
                _pControls.PerformingActionFlag = false;

                _pControls.NormalStateAction?.Invoke();
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}