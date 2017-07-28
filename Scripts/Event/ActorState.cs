using UnityEngine;

public class ActorState : MonoBehaviour
{
    public Value<float> health = new Value<float>(100f);

    public Value<bool> isGrounded = new Value<bool>(true);

    public Value<Vector3> velocity = new Value<Vector3>(Vector3.zero);

    public Message<float> land = new Message<float>();

    public Message death = new Message();
}
