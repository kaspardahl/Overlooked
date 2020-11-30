﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KD
{
    public class AddVelocity_ASB : StateMachineBehaviour
    {

        public float life = 0.4f;
        public float force = 6;
        public Vector3 direction;
        [Space]
        [Header("This will override the direction")]
        public bool useTransformForward;
        public bool additive; // adds the direction on top of the transform.forward
        public bool onEnter;
        public bool onExit;
        [Header("When Ending Applying velocity! Not anim state")]
        public bool onEndClampVelocity; //when you stop applying the force (set with the life variable) make the velocity 0
        [Header("Use this to tailor the force application")]
        public bool useForceCurve; 
        public AnimationCurve forceCurve;

        StateManager states;
        HandleMovement_Player ply;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onEnter)
            {
                if (useTransformForward && !additive)
                    direction = animator.transform.forward;

                if (useTransformForward && additive)
                    direction += animator.transform.forward;

                if (states == null)
                    states = animator.transform.GetComponent<StateManager>();

                if (!states.isPlayer)
                    return;

                if (ply == null)
                    ply = animator.transform.GetComponent<HandleMovement_Player>();

                ply.AddVelocity(direction, life, force, onEndClampVelocity, useForceCurve, forceCurve);
            }
        }


        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (onExit)
            {
                if (useTransformForward && !additive)
                    direction = animator.transform.forward;

                if (useTransformForward && additive)
                    direction += animator.transform.forward;

                if (states == null)
                    states = animator.transform.GetComponent<StateManager>();

                if (!states.isPlayer)
                    return;

                if (ply == null)
                    ply = animator.transform.GetComponent<HandleMovement_Player>();

                ply.AddVelocity(direction, life, force, onEndClampVelocity, useForceCurve, forceCurve);
            }
        }


    }
}
