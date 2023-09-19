using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLU.SteeringBehaviours
{
    public class Distraction : MonoBehaviour
    {
        [SerializeField]private CatBehaviour catBehaviour;
        private void Start()
        {
            catBehaviour.GetDestracted(gameObject);
        }

        private void Update()
        {

        }
    }
}
