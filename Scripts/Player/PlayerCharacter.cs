using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : PlayerBehaviour
{
    [Header("General")]

    [SerializeField]
    private Camera worldCamera;

    [SerializeField]
    [Tooltip("控制玩家的移动加速度")]
    private float acceleration = 5f;

    [SerializeField]
    [Tooltip("控制玩家停止移动时产生的阻尼")]
    private float damping = 8f;

    [SerializeField]
    private float forwardSpeed = 4f;

    [SerializeField]
    private float sidewaysSpeed = 3.5f;

    [SerializeField]
    private float backwardSpeed = 3f;

    [SerializeField]
    private float gravity = 20f;

    [SerializeField]
    private float jumpHeight = 0.8f;

    [SerializeField]
    [Range(0f, 1f)]
    private float airControl = 0.15f;

    private CharacterController cc;
    private float desiredSpeed;
    private Vector3 velocityCurrent;
    private Vector3 velocityDesired;
    private CollisionFlags collisionFlagsLast;
    private bool previouslyGrounded;

    public WeaponBehaviour equippedWeapon;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Player.jump.AddStartHandle(StartJumpHandle);


        Player.attackOnce.SetHandle(() => OnAttack(false));

        Player.attackContinuously.SetHandle(() => OnAttack(true));

        EquipWeapon(equippedWeapon);

    }

    bool StartJumpHandle()
    {
        bool canJump = Player.isGrounded.Get();

        if(canJump)
        {
            velocityCurrent.y = 0f;
            Debug.Log(CalculateJumpSpeed(jumpHeight));
            velocityCurrent += Vector3.up * CalculateJumpSpeed(jumpHeight);

            return true;
        }

        return false;
    }

    private float CalculateJumpSpeed(float heightToReach)
    {
        return Mathf.Sqrt(2f * gravity * heightToReach);
    }

    private void Update()
    {
        collisionFlagsLast = cc.Move(velocityCurrent * Time.deltaTime);

        if ((collisionFlagsLast & CollisionFlags.Below) == CollisionFlags.Below && !previouslyGrounded)
        {
            if (Player.jump.Active)
            {
                Player.jump.DirectStop();
            }

            Player.land.Send(Mathf.Abs(Player.velocity.Get().y));
        }

        Player.isGrounded.Set(cc.isGrounded);
        Player.velocity.Set(cc.velocity);

        if (!cc.isGrounded)
        {
            UpdateFalling();
        }
        else if(!Player.jump.Active)
        {
            UpdateMovement();
        }



        previouslyGrounded = cc.isGrounded;
    }

    private void UpdateMovement()
    {
        CalculateDesiredVelocity();

        //玩家移动时使用加速度，反之使用阻尼
        float targetAccel = velocityDesired.sqrMagnitude > 0f ? acceleration : damping;

        velocityCurrent = Vector3.Lerp(velocityCurrent, velocityDesired, targetAccel * Time.deltaTime);

    }

    private void UpdateFalling()
    {
        if (previouslyGrounded && !Player.jump.Active)
            velocityCurrent.y = 0f;

        velocityCurrent += velocityDesired * acceleration * airControl * Time.deltaTime;

        velocityCurrent.y -= gravity * Time.deltaTime;
    }

    private void CalculateDesiredVelocity()
    {
        // 限制输入向量的长度为1，如果不这么做，对角线移动时，movementInputClamped将高于1.4
        Vector2 movementInputClamped = Vector2.ClampMagnitude(Player.movementInput.Get(), 1f);

        // 当前是否有移动输入
        bool wantsToMove = movementInputClamped.sqrMagnitude > 0f;

        var targetDirection = (wantsToMove ? transform.TransformDirection(new Vector3(movementInputClamped.x, 0f, movementInputClamped.y)) : cc.velocity.normalized);
        desiredSpeed = 0f;

        if (wantsToMove)
        {
            //向前移动的速度
            desiredSpeed = forwardSpeed * 1;

            //向两边移动的速度
            if (Mathf.Abs(movementInputClamped.x) > 0f)
                desiredSpeed = sidewaysSpeed;

            //向后移动的速度
            if (movementInputClamped.y < 0f)
                desiredSpeed = backwardSpeed;

        }

        velocityDesired = targetDirection * desiredSpeed;
    }


    private bool OnAttack(bool continuously)
    {
        if (equippedWeapon == null)
            return false;

        bool attackWasSuccessful;

        if (continuously)
            attackWasSuccessful = equippedWeapon.AttackContinuouslyHandle(worldCamera);
        else
            attackWasSuccessful = equippedWeapon.AttackOnceHandle(worldCamera);


        if(attackWasSuccessful)
        {
            GetComponentInChildren<Animator>().SetTrigger("Fire");
        }

        return attackWasSuccessful;
    }


    void EquipWeapon(WeaponBehaviour Weapon)
    {
        if (Weapon)
        {
            SetCurrentWeapon(Weapon, equippedWeapon);
        }
    }

    void UnEquipWeapon(WeaponBehaviour Weapon)
    {
        if (Weapon && Weapon == equippedWeapon)
        {
            SetCurrentWeapon(null, Weapon);
        }
    }

    void SetCurrentWeapon(WeaponBehaviour NewWeapon, WeaponBehaviour LastWeapon /*= NULL*/)
    {
        WeaponBehaviour LocalLastWeapon = null;

        if (LastWeapon != null)
        {
            LocalLastWeapon = LastWeapon;
        }
        else if (NewWeapon != equippedWeapon)
        {
            LocalLastWeapon = equippedWeapon;
        }

        if (LocalLastWeapon)
        {
            LocalLastWeapon.OnUnEquip();
        }

        equippedWeapon = NewWeapon;

        if (NewWeapon)
        {

            NewWeapon.OnEquip();
        }
    }
}