using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitscanWeapon : WeaponBehaviour
{
    public enum FireMode
    {
        //半自动
        SemiAuto,
        //全自动
        FullAuto
    }

    [SerializeField]
    private RayImpact rayImpact;

    [SerializeField]
    private FireMode fireMode;

    private float timeBetweenShotsMin;
    private float nextTimeCanFire;

    [SerializeField]
    private SoundsPlayer fireAudio;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [Range(0f, 30f)]
    [SerializeField]
    private float spreadNormal = 0.8f;

    [SerializeField]
    [Range(0f, 30f)]
    private float spreadAim = 0.95f;

    [Range(1, 20)]
    [SerializeField]
    private int rayCount = 1;

    [SerializeField]
    private float distanceMax = 150f;


    [SerializeField]
    private LayerMask damageMax;

    [SerializeField]
    private GameObject tracer;

    [SerializeField]
    private int shotsPerMinute = 450;

    [SerializeField]
    private float shotDuration = 0.22f;

    public override bool AttackOnceHandle(Camera camera)
    {
        if (Time.time < nextTimeCanFire || !IsEquiped)
            return false;

        nextTimeCanFire = Time.time + timeBetweenShotsMin;

        Shoot(camera);

        return true;
    }

    public override bool AttackContinuouslyHandle(Camera camera)
    {
        if (fireMode == FireMode.SemiAuto)
            return false;

        return AttackOnceHandle(camera);
    }


    protected void Shoot(Camera camera)
    {
        fireAudio.Play(SoundsPlayer.Selection.Randomly, audioSource, 1f);

        if (muzzleFlash)
            muzzleFlash.Play(true);

        for (int i = 0; i < rayCount; i++)
            DoHitscan(camera);

        Attack.Send();

    }

    protected void DoHitscan(Camera camera)
    {
        float spread = Player.aim.Active ? spreadAim : spreadNormal;
        RaycastHit hitInfo;

        Ray ray = camera.ViewportPointToRay(Vector2.one * 0.5f);
        Vector3 spreadVector = camera.transform.TransformVector(new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0f));
        ray.direction = Quaternion.Euler(spreadVector) * ray.direction;

        if (Physics.Raycast(ray, out hitInfo, distanceMax, damageMax, QueryTriggerInteraction.Ignore))
        {
            float impulse = rayImpact.GetImpulseAtDistance(hitInfo.distance, distanceMax);

            //伤害
            float damage = rayImpact.GetDamageAtDistance(hitInfo.distance, distanceMax);
            var damageable = hitInfo.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                var damageData = new DamageEventData(-damage, Player, hitInfo.point, ray.direction, impulse);
                damageable.TakeDamage(damageData);
            }
            else if (hitInfo.rigidbody)
            {
                hitInfo.rigidbody.AddForceAtPosition(ray.direction * impulse, hitInfo.point, ForceMode.Impulse);
            }

            //音效
            if (GameplayStatics.SurfaceDatabase)
            {
                var data = GameplayStatics.SurfaceDatabase.GetSurface(hitInfo);

                data.PlaySound(SoundsPlayer.Selection.Randomly, Surface.ResponseType.BulletImpact, 1f, hitInfo.point);
            }
        }

        if (tracer)
            Instantiate(tracer, transform.position, Quaternion.LookRotation(ray.direction));

    }

    private void Start()
    {

        if (fireMode == FireMode.SemiAuto)
            timeBetweenShotsMin = shotDuration;
        else
            timeBetweenShotsMin = 60f / shotsPerMinute;
    }

}
