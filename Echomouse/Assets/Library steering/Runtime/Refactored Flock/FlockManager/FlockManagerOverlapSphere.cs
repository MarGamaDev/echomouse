using System.Collections.Generic;
using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public class FlockManagerOverlapSphere : FlockManager
    {
        private int m_flockLayer;

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public FlockManagerOverlapSphere(SteeringSettings settings) : base()
        {
            // get layer mask using the flock layer name
            m_flockLayer = LayerMask.GetMask(settings.m_flockLayer);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public override void ProcessNeighbors(Vector3 position, float searchRadius, IFlockManager.ProcessNeighbor processNeighbor)
        {
            // find all neighbors
            Collider[] neighbors = Physics.OverlapSphere(position, searchRadius, m_flockLayer, QueryTriggerInteraction.Ignore);

            // process all neighbors
            foreach (Collider neighbor in neighbors)
                processNeighbor(neighbor.gameObject);
        }
    }
}
