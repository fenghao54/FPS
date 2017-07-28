using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RayImpact
{
    [Range(0f, 1000f)]
    [SerializeField]
    private float damageMax = 15f;

    [Range(0f, 1000f)]
    [SerializeField]
    private float impulseMax = 15f;

    [SerializeField]
    private AnimationCurve distanceCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.8f, 0.5f),
        new Keyframe(1f, 0f));


    public float GetDamageAtDistance(float distance, float maxDistance)
    {
        return ApplyCurveToValue(damageMax, distance, maxDistance);
    }

    public float GetImpulseAtDistance(float distance, float maxDistance)
    {
        return ApplyCurveToValue(impulseMax, distance, maxDistance);
    }

    private float ApplyCurveToValue(float value, float distance, float maxDistance)
    {
        float maxDistanceAbsolute = Mathf.Abs(maxDistance);
        float distanceClamped = Mathf.Clamp(distance, 0f, maxDistanceAbsolute);

        return value * distanceCurve.Evaluate(distanceClamped / maxDistanceAbsolute);
    }
}

public class WeaponBehaviour : MonoBehaviour
{

    private PlayerState player;
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

    public bool IsEquiped { get; private set; }

    public Message Equip = new Message();

    public Message Unequip = new Message();

    public Message Attack = new Message();

    public virtual bool AttackOnceHandle(Camera camera) { return false; }

    public virtual bool AttackContinuouslyHandle(Camera camera) { return false; }


    public virtual void OnEquip()
    {
        IsEquiped = true;
        Equip.Send();
    }

    public virtual void OnUnEquip()
    {
        IsEquiped = false;
        Unequip.Send();
    }
}
