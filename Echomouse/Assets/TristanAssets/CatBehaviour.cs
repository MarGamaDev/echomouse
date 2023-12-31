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
        private float distanceToDistraction;
        [SerializeField]private GameObject player;
        [SerializeField] private GameObject attackHitBox;
        private GameObject currentTarget;
        private Coroutine currentCoroutine;
        private Coroutine soundCoroutine;
        private AudioSource audioSource;
        [SerializeField] private AudioClip catAttackAudio;
        [SerializeField] private AudioClip catChaseAndInvestigateAudio;
        [SerializeField] private List<AudioClip> catWanderAudioList;
        private AudioClip catWanderAudio { get { return catWanderAudioList[Random.Range(0, catWanderAudioList.Count)]; } }
        [SerializeField] private float catSoundInterval = 8f, catSoundIntervalVariance = 1f;

        [SerializeField] private Vector3 attackBoxHalfSize = Vector3.one;
        [SerializeField] private float smackUpNumber, smackForce;

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

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {

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
        }

        private void FixedUpdate()
        {
            UpdateCatState();
        }

        private void UpdateCatState()
        {
            //check de afstand tussen de player en kat
            float distanceToPlayer = Mathf.Infinity;

            float distancePlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distancePlayer < distanceToPlayer)
            {
                distanceToPlayer = distancePlayer;
            }

            //check de afstand tussen de afleiding en kat wanneer er een afleiding is
            if (currentTarget != null)
            {
                distanceToDistraction = Mathf.Infinity;

                float distanceDistraction = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (distanceDistraction < distanceToDistraction)
                {
                    distanceToDistraction = distanceDistraction;
                }
            }

            //switch case voor om te bepalen wanneer de AI van behavior moet wisselen
            switch (state)
            {
                case CatFSM.Wander:
                    if (distanceToPlayer <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                        WhatToDoInState();
                    break;
                case CatFSM.Chase:
                    if (distanceToPlayer <= catStats.CatAttackRange)
                        state = CatFSM.Attack;
                    else if (distanceToPlayer > catStats.CatSenseRange)
                        state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
                case CatFSM.Investigate:
                    if (distanceToPlayer <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                    else if (distanceToDistraction <= catStats.CatSenseRange)
                        state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
                case CatFSM.Attack:
                    if (distanceToPlayer <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                    WhatToDoInState();
                    break;
            }
        }

        /// <summary>
        /// zorgt ervoor dat de kat naar het object gaat dat wordt meegegeven bij deze functie
        /// </summary>
        /// <param name="distraction">object waar de kat door wordt afgeleid</param>
        public void GetDestracted(GameObject distraction)
        {
            investigateBehaviour.Clear();

            currentTarget = distraction;
            
            investigateBehaviour.Add(new Pursue(currentTarget));
            investigateBehaviour.Add(new AvoidWall());
            state = CatFSM.Investigate;
        }

        private void WhatToDoInState()
        {
            //switch case om te bepalen wat er gedaan moet worden in elke state
            switch (state)
            {
                case CatFSM.Wander:
                    steering.SetBehaviors(wanderBehaviour);
                    if (soundCoroutine == null)
                        soundCoroutine = StartCoroutine(PlaySound(catWanderAudio));
                    break;
                case CatFSM.Chase:
                    steering.SetBehaviors(chaseBehaviour);
                    if (soundCoroutine == null)
                        soundCoroutine = StartCoroutine(PlaySound(catChaseAndInvestigateAudio));
                    break;
                case CatFSM.Investigate:
                    steering.SetBehaviors(investigateBehaviour);
                    if (soundCoroutine == null)
                        soundCoroutine = StartCoroutine(PlaySound(catChaseAndInvestigateAudio));
                    break;
                case CatFSM.Attack:
                    if (currentCoroutine == null)
                    {
                        currentCoroutine = StartCoroutine(Attack());
                    }
                    steering.SetBehaviors(attackBehaviour);
                    if (soundCoroutine == null)
                        soundCoroutine = StartCoroutine(PlaySound(catAttackAudio));
                    break;
            }
        }

        private IEnumerator Attack()
        {
            yield return new WaitForSeconds(1f);
            //attackHitBox.SetActive(true);

            Collider[] cols = Physics.OverlapBox(attackHitBox.transform.position, attackBoxHalfSize);
            foreach (Collider col in cols)
            {
                col.GetComponent<PlayerHealth>()?.GetAttacked(new Vector3(player.transform.position.x - transform.position.x, smackUpNumber, player.transform.position.z - transform.position.z).normalized * smackForce);
            }

            yield return new WaitForSeconds(1f);
            //attackHitBox.SetActive(false);

            currentCoroutine = null;
        }

        private IEnumerator PlaySound(AudioClip audioClip)
        {
            yield return null;

            audioSource.PlayOneShot(audioClip);
            EchoPointManager.instance.GetPulse(transform.position);
            yield return new WaitForSeconds(Random.Range(catSoundInterval - catSoundIntervalVariance, catSoundInterval + catSoundIntervalVariance));

            soundCoroutine = null;
        }
    }
}
