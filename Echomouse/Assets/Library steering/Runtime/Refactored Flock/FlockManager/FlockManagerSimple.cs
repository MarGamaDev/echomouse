using System.Collections.Generic;
using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public class FlockManagerSimple : FlockManager
    {
        public override void ProcessNeighbors(Vector3 position, float searchRadius, IFlockManager.ProcessNeighbor processNeighbor)
        {
            // process all neighbors
            foreach (KeyValuePair<GameObject, Flock> neighbor in m_flockingObjects)
                processNeighbor(neighbor.Key);
        }
    }
}
