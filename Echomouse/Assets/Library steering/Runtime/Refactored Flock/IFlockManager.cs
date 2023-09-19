using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public interface IFlockManager
    {
        // Delegate used by Flock objects to ask flock manager to process all the neighbors
        public delegate void ProcessNeighbor(GameObject neighbor);

        // called by Flock objects: register flock object
        public void RegisterFlockObject(GameObject flockingObject, Flock flock);

        // called by Flock objects: unregister flock object
        public void UnregisterFlockObject(GameObject flockingObject);

        // called by FlockManagerFactory: step the flock manager (update all registered flocking objects)
        public void Step(float dt);

        // called by Flock objects: process all neighbors
        public void ProcessNeighbors(Vector3 position, float searchRadius, ProcessNeighbor processNeighbor);
    }
}
