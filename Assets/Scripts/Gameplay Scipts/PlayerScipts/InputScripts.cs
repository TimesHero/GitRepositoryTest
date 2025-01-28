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
    float dashSpeed = 5;
    private float dashTime = 0f;   
    public float dashDuration = 0.2f;
    public float mana = 100;
    float reloadTimer;
    public Slider MPBar;
    int bulletType = 0;
    public Projectile regularProjectile;
    public Projectile tripleProjectile;
    public Projectile pierceProjectile;
    Projectile currentProjectile;
    void Start()
    {
        myPI = GetComponent<PlayerInput>();
        myRB = GetComponent<Rigidbody2D>();
        currentInterval = Time.time;
        currentProjectile=regularProjectile;
        ChangeBulletType();
    }

    void Update()
    {
        if (dashing && Time.time > dashTime)
        {
            dashing = false;
            gameObject.GetComponent<PlayerHPManager>().invincible = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); 

        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            myRB.AddForce (new Vector2(moveDirection.x*baseSpeed, moveDirection.y*baseSpeed)); 
        }
        if (isFiring==true && Time.time>currentInterval && mana>0)//Player Shooting
        {
            reloadTimer=0;
            shoot();
        }

        if (isFiring==false)
        {
            reloadTimer+=1;
            if (reloadTimer>=120&&mana<100)
            {
                mana+=0.3f;
                MPBar.value=mana;
            }
        }

        if (dashing==true)
        {
            myRB.AddForce(moveDirection.normalized * dashSpeed, ForceMode2D.Impulse);
            gameObject.GetComponent<PlayerHPManager>().invincible = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.5f);

        }

    }
    public void shoot()
    {
        mana-=currentProjectile.manaCost;
        MPBar.value=mana;
        if (currentProjectile.triple == true) {
            float spreadAngle = 25f; 
            for (int i = -1; i <= 1; i++) 
            {
                GameObject bullet = Instantiate(normalBullet, aimReticle.position, aimReticle.rotation);
                Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
                rigidbodyB.linearVelocity = currentProjectile.velocity * 
                (Quaternion.Euler(0, 0, i * spreadAngle) * aimReticle.transform.up).normalized;
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
        }
        if (bulletType==1)
        {
            currentProjectile=tripleProjectile;
        }
        if (bulletType==2)
        {
            currentProjectile=pierceProjectile;
        }
        interval=currentProjectile.timeBetweenShots;
        
    }

    public void AtkSpeedPickupTrigger()
    {
        StartCoroutine(atkSpeedTimer());
    }
    private IEnumerator atkSpeedTimer()
    {
        atkSpeedPickup=true;
        interval=currentProjectile.atkSpeedUpTimeBetweenShots;
        ChangeBulletType();
        yield return new WaitForSeconds(8f);
        atkSpeedPickup=false;
        interval=currentProjectile.timeBetweenShots;
        ChangeBulletType();
        
        
    }
    public void RunSpeedPickupTrigger()
    {
        StartCoroutine(runTimer());
    }

    private IEnumerator runTimer()
    {
        baseSpeed=150;
        dashSpeed=7;
        yield return new WaitForSeconds(8f);
        baseSpeed=50;
        dashSpeed=5;
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
    }
    public void OnLeftBumper(InputValue inputValue)
    {
        bulletType-=1;
        if (bulletType==-1)
        {
            bulletType=2;
        }
        ChangeBulletType();
    }
    public void OnRightBumper(InputValue inputValue)
    {
        bulletType+=1;
        if (bulletType==3)
        {
            bulletType=0;
        }
        ChangeBulletType();
    }
    public void OnDash(InputValue inputValue)
    {
         if (inputValue.isPressed && !dashing) 
        {
            dashing = true;
            dashTime = Time.time + dashDuration; // Set when to stop dashing
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
}
