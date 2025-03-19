using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    private PlayerInput myPI;
    private Rigidbody2D myRB;
    private SpriteRenderer myRenderer;
    private Animator myAnim;
    private PlayerHPManager playerHPManager;

    public float baseSpeed;
    private Vector2 moveDirection;
    private Vector2 lookDirection;

    public GameObject normalBullet;
    public GameObject meleeWeapon;
    public GameObject logicManager;
    public GameObject rotaryMenu;
    public GameObject reticle;
    public Transform lookTransform;
    public Transform aimReticle;

    private bool moving = false;
    private bool isFiring = false;
    private bool dashing = false;
    private bool atkSpeedPickup = false;
    private bool meleeAttack = false;

    private float currentInterval;
    private float interval;
    public float dashSpeed = 1;
    private float dashTime = 0f;
    public float dashDuration = 0.2f;
    private float currentDashInterval;
    public float meleeInterval;
    private float currentMeleeInterval;
    private float reloadTimer;
    public Slider MPBar;
    private int bulletType = 0;

    public Projectile regularProjectile;
    public Projectile tripleProjectile;
    public Projectile pierceProjectile;
    private Projectile currentProjectile;

    public AudioSource failedCast;
    public GameObject lvlUpScreen;
    public GameObject lvlUpScript;

    public List<GameObject> interactables;

    private bool facingRight = true;
    public GameObject dashTrail;
    public float smoothingSpeed;
    private Vector2 targetLookDirection;
    public float sensitivity; 
 
    

    private Vector2 currentLookDirection = Vector2.zero;

    void Start()
    {
        myPI = GetComponent<PlayerInput>();
        myRB = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        playerHPManager = GetComponent<PlayerHPManager>();

        currentInterval = Time.time;
        currentDashInterval = Time.time;
        currentMeleeInterval = Time.time;
        currentProjectile = regularProjectile;
        ChangeBulletType();
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            myRB.AddForce(moveDirection * baseSpeed);
        }

        if (isFiring && Time.time > currentInterval)
        {
            if (playerHPManager.mana > 0)
            {
                shoot();
                reloadTimer = 0;
            }
            else
            {
                failedCast.Play();
            }
        }

        reloadTimer += 1;
        if (reloadTimer >= 80 && playerHPManager.mana < playerHPManager.manaMax)
        {
            playerHPManager.UseMana(-0.8f);
        }
        //AIMING------------------------------------------------
        if (targetLookDirection.magnitude > 0)
        {
            currentLookDirection = Vector2.Lerp(currentLookDirection, targetLookDirection, smoothingSpeed * Time.deltaTime);
        }
        else
        {
            currentLookDirection = Vector2.Lerp(currentLookDirection, Vector2.zero, smoothingSpeed * Time.deltaTime);
        }

        if (currentLookDirection.magnitude > 0.01f)
        {
            lookTransform.up = currentLookDirection;
        }

        Flip();
        myAnim.SetBool("isMoving", moving);
    }

    private void Flip()
    {
        float angle = Vector2.SignedAngle(Vector2.right, lookTransform.up);

        if ((angle > 90 || angle < -90) && facingRight)
        {
            facingRight = false;
            myRenderer.flipX = false;
            meleeWeapon.GetComponent<MeleeSwing>().myRenderer.flipX = false;
        }
        else if (angle <= 90 && angle >= -90 && !facingRight)
        {
            facingRight = true;
            myRenderer.flipX = true;
            meleeWeapon.GetComponent<MeleeSwing>().myRenderer.flipX = true;
        }
    }

    private IEnumerator Dash()
    {
        dashing = true;
        dashTrail.SetActive(true);

        myRB.AddForce(moveDirection.normalized * dashSpeed, ForceMode2D.Impulse);
        playerHPManager.invincible = true;
        myRenderer.color = new Color(0f, 0f, 0f, 0.5f);

        StartCoroutine(EnablePlayerCollider());
        yield return new WaitForSeconds(dashDuration);

        playerHPManager.invincible = false;
        myRenderer.color = Color.white;
        dashTrail.SetActive(false);
        dashing = false;
    }

    private IEnumerator meleeAttackTrigger()
    {
        meleeWeapon.GetComponent<MeleeSwing>().Attack();
        myAnim.SetTrigger("meleeAttack");
        currentMeleeInterval = Time.time + interval;
        meleeAttack = true;
        yield return new WaitForSeconds(0.5f);
        meleeAttack = false;
    }

    private IEnumerator EnablePlayerCollider()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectile"), true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectile"), false);
    }

    public void shoot()
    {
        float manaCost = atkSpeedPickup ? currentProjectile.manaCost / 2 : currentProjectile.manaCost;
        playerHPManager.UseMana(manaCost);

        myAnim.SetTrigger("shoot");

        if (currentProjectile.triple)
        {
            float spreadAngle = 25f;
            for (int i = -1; i <= 1; i++)
            {
                GameObject bullet = Instantiate(normalBullet, aimReticle.position, aimReticle.rotation);
                Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
                rigidbodyB.linearVelocity = currentProjectile.velocity * (Quaternion.Euler(0, 0, i * spreadAngle) * aimReticle.transform.up).normalized;
                bullet.GetComponent<BulletBase>().PeramPass(currentProjectile);
            }
        }
        else
        {
            GameObject bullet = Instantiate(normalBullet, aimReticle.position, aimReticle.rotation);
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            rigidbodyB.linearVelocity = currentProjectile.velocity * aimReticle.transform.up;
            bullet.GetComponent<BulletBase>().PeramPass(currentProjectile);
        }
        Camera.main.GetComponent<CameraFollow>().TriggerShake(0.5f, 1f, 5f);
        currentInterval = Time.time + interval;
    }

    public void ChangeBulletType()
    {
        switch (bulletType)
        {
            case 0:
                currentProjectile = regularProjectile;
                reticle.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
                break;
            case 1:
                currentProjectile = tripleProjectile;
                reticle.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 0f);
                break;
            case 2:
                currentProjectile = pierceProjectile;
                reticle.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
                break;
        }

        if (!atkSpeedPickup)
        {
            interval = currentProjectile.timeBetweenShots;
        }
    }

    public void AtkSpeedPickupTrigger()
    {
        StartCoroutine(atkSpeedTimer());
    }

    private IEnumerator atkSpeedTimer()
    {
        atkSpeedPickup = true;
        interval /= 2;
        ChangeBulletType();
        yield return new WaitForSeconds(8f);
        atkSpeedPickup = false;
        interval *= 2;
        ChangeBulletType();
    }

    public void RunSpeedPickupTrigger()
    {
        StopCoroutine(runTimer());
        StartCoroutine(runTimer());
    }

    private IEnumerator runTimer()
    {
        baseSpeed = 200;
        yield return new WaitForSeconds(8f);
        baseSpeed = 100;
    }

    // PLAYER CONTROLS --------------------------------------------------------------------------------------
    public void OnMove(InputValue moveValue)
    {
        moveDirection = moveValue.Get<Vector2>().normalized;
        moving = moveDirection.magnitude > 0;
    }

   // This method is called by the input system when the Look action is triggered
    public void OnLook(InputValue lookValue)
    {
        targetLookDirection = lookValue.Get<Vector2>().normalized;
        targetLookDirection *= sensitivity;
    }


    public void OnFire(InputValue inputValue)
    {
        isFiring = inputValue.isPressed;
    }

    public void OnMeleeWeapon(InputValue inputValue)
    {
        if (!meleeAttack)
        {
            StartCoroutine(meleeAttackTrigger());
        }
    }

    public void OnLeftBumper(InputValue inputValue)
    {
        bulletType = (bulletType == 0) ? 2 : bulletType - 1;
        ChangeBulletType();
        rotaryMenu.gameObject.GetComponent<RotaryMenu>().right=false;
        rotaryMenu.gameObject.GetComponent<RotaryMenu>().left=true;
        rotaryMenu.gameObject.GetComponent<RotaryMenu>().bulletType=bulletType;
    }

    public void OnDash(InputValue inputValue)
    {
        if (inputValue.isPressed && !dashing)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnPause(InputValue inputValue)
    {
        if (Time.timeScale == 0)
        {
            logicManager.GetComponent<GameHandler>().Unpause();
        }
        else
        {
            logicManager.GetComponent<GameHandler>().PauseGame();
        }
    }

    public void OnInteract(InputValue inputValue)
    {
        foreach (GameObject obj in interactables)
        {
            obj.SendMessage("OnUse");
        }
    }
}
