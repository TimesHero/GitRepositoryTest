using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class InputScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerInput myPI;
    public float baseSpeed;
    Vector2 moveDirection;
    Vector2 lookDirection;
    public GameObject normalBullet;
    public GameObject meleeWeapon;
    public GameObject logicManager;
    public GameObject rotaryMenu;
    public GameObject reticle;
    public Transform lookTransform;
    public Transform aimReticle;
    Rigidbody2D myRB;
    bool moving=false;
    bool isFiring=false;
    bool dashing = false;
    bool runSpeedPickup = false;
    bool atkSpeedPickup = false;
    bool reloading;
    float currentInterval;
    float interval;
    public float dashSpeed = 1;
    private float dashTime = 0f;   
    public float dashDuration = 0.2f;
    float dashInterval = 0.8f;
    float currentDashInterval; 
    float reloadTimer;
    public Slider MPBar;
    int bulletType = 0;
    public Projectile regularProjectile;
    public Projectile tripleProjectile;
    public Projectile pierceProjectile;
    bool ableToDash = true;
    Projectile currentProjectile;
    public AudioSource failedCast;
    public GameObject lvlUpScreen;
    public GameObject lvlUpScript;
    public List<GameObject> interactables;
    public bool facingRight = true;
    SpriteRenderer myRenderer;
    Animator myAnim;
    public GameObject dashTrail;

    void Start()
    {
        myPI = GetComponent<PlayerInput>();
        myRB = GetComponent<Rigidbody2D>();
        currentInterval = Time.time;
        currentDashInterval = Time.time;
        currentProjectile=regularProjectile;
        myRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        ChangeBulletType();
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            myRB.AddForce (new Vector2(moveDirection.x*baseSpeed, moveDirection.y*baseSpeed)); 
        }
        if (isFiring == true && Time.time > currentInterval) // Player Shooting
        {
            
            if (gameObject.GetComponent<PlayerHPManager>().mana > 0) // Check if the player has mana
            {
                shoot();
                reloadTimer = 0;
            }
            else
            {
                failedCast.Play();
                
            }
        }

        {
            reloadTimer+=1;
            if (reloadTimer>=80 && gameObject.GetComponent<PlayerHPManager>().mana < gameObject.GetComponent<PlayerHPManager>().manaMax)
            {
                gameObject.GetComponent<PlayerHPManager>().UseMana(-0.8f);
            }
        }
        Flip();
        myAnim.SetBool("isMoving",moving);

    }
   void Flip()
{
    // Calculate the angle between the right-facing direction (Vector2.right) and the direction the player is aiming (lookTransform.up)
    float angle = Vector2.SignedAngle(Vector2.right, lookTransform.up);

    // If the angle is greater than 90 or less than -90, flip the player to face left
    if (angle > 90 || angle < -90)
    {
        if (facingRight) 
        {
            facingRight = false;
            myRenderer.flipX = false; // Flip the sprite to face left (flipX = true for left-facing)
        }
    }
    else // If the angle is between -90 and 90, the player is facing right
    {
        if (!facingRight)
        {
            facingRight = true;
            myRenderer.flipX = true; // Reset sprite flip to face right (flipX = false for right-facing)
        }
    }
}




     private IEnumerator Dash()
    {
        dashing = true;
        dashTrail.SetActive(true);
        myRB.AddForce(moveDirection.normalized * dashSpeed, ForceMode2D.Impulse);
        gameObject.GetComponent<PlayerHPManager>().invincible = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);

        StartCoroutine(EnablePlayerCollider());
        yield return new WaitForSeconds(dashDuration);
        gameObject.GetComponent<PlayerHPManager>().invincible = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        StopCoroutine(EnablePlayerCollider()); 
        dashTrail.SetActive(false);
        dashing = false;
    }
    private IEnumerator EnablePlayerCollider()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectile"), true);
        ableToDash=false;
        yield return new WaitForSeconds(0.5f); // Time invincible
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Projectile"), false);
        ableToDash=true;
        dashing = false;
    }
    public void shoot()
    {
        if (atkSpeedPickup) 
        {
            gameObject.GetComponent<PlayerHPManager>().UseMana(currentProjectile.manaCost/2);
        }
        else
        {
            gameObject.GetComponent<PlayerHPManager>().UseMana(currentProjectile.manaCost);
        }
        myAnim.SetTrigger("shoot");
        if (currentProjectile.triple == true) {
            float spreadAngle = 25f; 
            for (int i = -1; i <= 1; i++) 
            {
                GameObject bullet = Instantiate(normalBullet, aimReticle.position, aimReticle.rotation);
                Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
                rigidbodyB.linearVelocity = currentProjectile.velocity * (Quaternion.Euler(0, 0, i * spreadAngle) * aimReticle.transform.up).normalized;
                bullet.gameObject.GetComponent<BulletBase>().PeramPass(currentProjectile);
            }
        } 
        else 
        {
            GameObject bullet = Instantiate(normalBullet, aimReticle.position, aimReticle.rotation);
            Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
            rigidbodyB.linearVelocity=currentProjectile.velocity*aimReticle.transform.up;
            bullet.gameObject.GetComponent<BulletBase>().PeramPass(currentProjectile);
        }
        currentInterval = Time.time + interval;
    }

    public void ChangeBulletType()
    {
        if (bulletType==0)
        {
            currentProjectile=regularProjectile;
            reticle.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
        }
        if (bulletType==1)
        {
            currentProjectile=tripleProjectile;
            reticle.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0.5f, 0f);
        }
        if (bulletType==2)
        {
            currentProjectile=pierceProjectile;
            reticle.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
        }
        if (atkSpeedPickup!=true)
        {
        interval=currentProjectile.timeBetweenShots;
        }
        
    }

    public void AtkSpeedPickupTrigger()
    {
        StartCoroutine(atkSpeedTimer());
    }
    private IEnumerator atkSpeedTimer()
    {
        atkSpeedPickup=true;
        Debug.Log("START ATTACK");
        interval=interval/2;
        Debug.Log(interval);
        ChangeBulletType();
        yield return new WaitForSeconds(8f);
        atkSpeedPickup=false;
        interval=interval*2;
        atkSpeedPickup=false;
        ChangeBulletType();
        
        
    }
    public void RunSpeedPickupTrigger()
    {
        StartCoroutine(runTimer());
    }

    private IEnumerator runTimer()
    {
        baseSpeed=200;
        yield return new WaitForSeconds(8f);
        baseSpeed=100;
    }

    //PLAYER CONTROLS--------------------------------------------------------------------------------------
    public void OnMove(InputValue moveValue)
    {
        moveDirection = moveValue.Get<Vector2>().normalized;
        if (Mathf.Abs(moveDirection.magnitude)>0)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    public void OnLook(InputValue lookValue)
    {
        lookDirection = lookValue.Get<Vector2>().normalized;
        if (Mathf.Abs(lookDirection.magnitude)>0)
        {
            lookTransform.up = lookDirection;
        }
    }

    public void OnFire(InputValue inputValue)
    {   
        isFiring = inputValue.isPressed;
    }

    public void OnMeleeWeapon(InputValue inputValue)
    {
        meleeWeapon.gameObject.GetComponent<MeleeSwing>().Attack();
        myAnim.SetTrigger("meleeAttack");
    }
    public void OnLeftBumper(InputValue inputValue)
    {
        bulletType-=1;
        if (bulletType==-1)
        {
            bulletType=2;
        }
        ChangeBulletType();
        rotaryMenu.gameObject.GetComponent<RotaryMenu>().right=false;
        rotaryMenu.gameObject.GetComponent<RotaryMenu>().left=true;
        rotaryMenu.gameObject.GetComponent<RotaryMenu>().bulletType=bulletType;

    }
    public void OnDash(InputValue inputValue)
    {
         if (inputValue.isPressed && !dashing &&ableToDash==true) 
        {
            StartCoroutine(Dash());
        }
    }

    public void OnPause(InputValue inputValue)
    {
        if (Time.timeScale == 0)
        {
            logicManager.gameObject.GetComponent<GameHandler>().Unpause();
        }
        else 
        {
            logicManager.gameObject.GetComponent<GameHandler>().PauseGame();
        }
        
    }
    public void OnDebug(InputValue inputValue)
    {
        bool isActive = !lvlUpScreen.activeSelf;
        lvlUpScreen.SetActive(isActive);
        Debug.Log(isActive);

        if (!isActive)
        {
            Time.timeScale = 1; 
        }
        else
        {
            Time.timeScale = 0; 
        }
        lvlUpScript.GetComponent<LevelUpButtons>().SetInput();
    }
    public void OnInteract(InputValue inputValue)
    {
        foreach (GameObject obj in interactables)
                {
                    obj.SendMessage("OnUse");
                }
    }
}
