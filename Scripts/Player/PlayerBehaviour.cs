using UnityEngine;

public class PlayerBehaviour : ActorBehaviour
{
    public PlayerState Player
    {
        get
        {
            if (player == null)
                player = GetComponent<PlayerState>();
            if (!player)
                player = GetComponentInParent<PlayerState>();
            return player;
        }
    }

    private PlayerState player;
}