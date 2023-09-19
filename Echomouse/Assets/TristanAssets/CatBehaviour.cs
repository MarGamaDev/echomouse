using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GLU.SteeringBehaviours
{
    [RequireComponent(typeof(GLU.SteeringBehaviours.Steering))]
    public class CatBehaviour : MonoBehaviour
    {
        [SerializeField] CatStats catStats;

        [SerializeField] protected CatFSM state;
        private NavMeshAgent agent;
        private Rigidbody rb;

        [SerializeField] Transform[] targets; //array of targets for like sounds or the player
        private Transform currentTarget;
        [SerializeField]private GameObject player;

        private List<IBehavior> wanderBehaviour;
        private List<IBehavior> chaseBehaviour;
        private List<IBehavior> investigateBehaviour;
        private List<IBehavior> attackBehaviour;
        private Steering steering;
        protected enum CatFSM
        {
            Wander,
            Investigate,
            Chase,
            Attack
        }

        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            state = CatFSM.Wander;

            // referentie naar het Steering component die wordt bewaard in een variabele
            steering = GetComponent<Steering>();

            wanderBehaviour = new List<IBehavior>();
            chaseBehaviour = new List<IBehavior>();
            investigateBehaviour = new List<IBehavior>();
            attackBehaviour = new List<IBehavior>();

            wanderBehaviour.Add(new Wander(gameObject.transform));
            wanderBehaviour.Add(new AvoidWall());
            chaseBehaviour.Add(new Pursue(player));
            chaseBehaviour.Add(new AvoidWall());
            investigateBehaviour.Add(new Pursue(player));//wordt later vervangen met distraction
            investigateBehaviour.Add(new AvoidWall());
        }

        private void FixedUpdate()
        {
            UpdateCatState();
        }

        private void UpdateCatState()
        {
            float distanceToTarget = Mathf.Infinity;

            foreach (Transform target in targets)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance < distanceToTarget)
                {
                    distanceToTarget = distance;
                    currentTarget = target;
                }
            }

            switch (state)
            {
                case CatFSM.Wander:
                    if (distanceToTarget <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                        WhatToDoInState();
                    break;
                case CatFSM.Chase:
                    if (distanceToTarget <= catStats.CatAttackRange)
                        state = CatFSM.Attack;
                    else if (distanceToTarget > catStats.CatSenseRange)
                        state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
                case CatFSM.Investigate:
                    if (distanceToTarget <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                    else
                        state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
            }
        }

        private void WhatToDoInState()
        {
            switch (state)
            {
                case CatFSM.Wander:
                        steering.SetBehaviors(wanderBehaviour);
                    break;
                case CatFSM.Chase:
                    steering.SetBehaviors(chaseBehaviour);
                    break;
                case CatFSM.Investigate:
                    steering.SetBehaviors(investigateBehaviour);
                    break;
                case CatFSM.Attack:
                    steering.SetBehaviors(attackBehaviour);
                    break;
            }
        }
    }
}
