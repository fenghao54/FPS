using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class Surface
{
    public enum ResponseType
    {
        Footstep,
        BulletImpact,
        Chop,
        Hit,
        Jump,
        Land,
    }

    public string Name { get { return name; } }


    [SerializeField]
    private string name;

    [Header("Footsteps")]

    [SerializeField]
    private SoundsPlayer footstepSounds;

    [SerializeField]
    private SoundsPlayer jumpSounds;

    [SerializeField]
    private SoundsPlayer landSounds;

    [Header("Bullet Impact")]

    [SerializeField]
    private SoundsPlayer bulletImpactSounds;

    [Header("Chop & Hit")]

    [SerializeField]
    private SoundsPlayer chopSounds;

    [SerializeField]
    private SoundsPlayer hitSounds;

    public void PlaySound(SoundsPlayer.Selection selectionMethod, ResponseType soundType, float volumeFactor = 1f, AudioSource audioSource = null)
    {
        if (soundType == ResponseType.BulletImpact)
            bulletImpactSounds.Play(selectionMethod, audioSource, volumeFactor);
        else if (soundType == ResponseType.Footstep)
            footstepSounds.Play(selectionMethod, audioSource, volumeFactor);
        else if (soundType == ResponseType.Jump)
            jumpSounds.Play(selectionMethod, audioSource, volumeFactor);
        else if (soundType == ResponseType.Land)
            landSounds.Play(selectionMethod, audioSource, volumeFactor);
        else if (soundType == ResponseType.Chop)
            chopSounds.Play(selectionMethod, audioSource, volumeFactor);
        else if (soundType == ResponseType.Hit)
            hitSounds.Play(selectionMethod, audioSource, volumeFactor);
    }

    public void PlaySound(SoundsPlayer.Selection selectionMethod, ResponseType soundType, float volumeFactor = 1f, Vector3 position = default(Vector3))
    {
        if (soundType == ResponseType.BulletImpact)
            bulletImpactSounds.PlayAtPosition(selectionMethod, position, volumeFactor);
        else if (soundType == ResponseType.Footstep)
            footstepSounds.PlayAtPosition(selectionMethod, position, volumeFactor);
        else if (soundType == ResponseType.Jump)
            jumpSounds.PlayAtPosition(selectionMethod, position, volumeFactor);
        else if (soundType == ResponseType.Land)
            landSounds.PlayAtPosition(selectionMethod, position, volumeFactor);
        else if (soundType == ResponseType.Chop)
            chopSounds.PlayAtPosition(selectionMethod, position, volumeFactor);
        else if (soundType == ResponseType.Hit)
            hitSounds.PlayAtPosition(selectionMethod, position, volumeFactor);
    }

}
