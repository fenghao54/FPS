using UnityEngine;

public class ActorBehaviour : MonoBehaviour
{
    public ActorState Actor
    {
        get
        {
            if (actor == null)
                actor = GetComponent<ActorState>();
            if (!actor)
                actor = GetComponentInParent<ActorState>();
            return actor;
        }
    }

    private ActorState actor;
}