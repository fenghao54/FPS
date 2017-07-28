using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEventData
{
    public float Delta { get; set; }

    public ActorState Hurter { get; private set; }

    public Vector3 HitPoint { get; private set; }

    public Vector3 HitDirection { get; private set; }

    public float HitImpulse { get; private set; }


    public DamageEventData(float delta, ActorState damager = null, Vector3 hitPoint = default(Vector3), Vector3 hitDirection = default(Vector3), float hitImpulse = 0f)
    {
        Delta = delta;
        Hurter = damager;
        HitPoint = hitPoint;
        HitDirection = hitDirection;
        HitImpulse = hitImpulse;
    }
}

public interface IDamageable
{
    void TakeDamage(DamageEventData damageData);
}