using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

public class InputScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    PlayerInput myPI;
    public float baseSpeed;
    Vector2 moveDirection;
    Vector2 lookDirection;
    public GameObject normalBullet;
    public GameObject tripleBullet;
    public GameObject pierceBullet;
    public GameObject meleeWeapon;
    public GameObject logicManager;
    public Transform lookTransform;
    public Transform aimReticle;
    public Transform aimReticleL;
    public Transform aimReticleR;
    Rigidbody2D myRB;
    bool moving=false;
    bool isFiring=false;
    bool dashing = false;
    float currentInterval;
    float interval = 0.3f;
    float dashSpeed = 5;
    private float dashTime = 0f;   
    public float dashDuration = 0.2f;
    public bool runSpeedPickup = false;
    public bool atkSpeedPickup = false;

    public GameObject pausePanel;

    public int bulletType = 0;
    void Start()
    {
        myPI = GetComponent<PlayerInput>();
        myRB = GetComponent<Rigidbody2D>();
        currentInterval = Time.time;
    }

    void Update()
    {
        if (dashing && Time.time > dashTime)
        {
            dashing = false;
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            myRB.AddForce (new Vector2(moveDirection.x*baseSpeed, moveDirection.y*baseSpeed)); 
        }
        if (isFiring==true && Time.time>currentInterval)//Player Shooting
        {
            if (bulletType==0) shoot();
            if (bulletType==1) shootTriple();
            if (bulletType==2) shootPierce();
        }

        if (dashing==true)
        {
            myRB.AddForce(moveDirection.normalized * dashSpeed, ForceMode2D.Impulse);
        }

    }

    public void shoot()
    {
        GameObject bullet = Instantiate(normalBullet, aimReticle.position, aimReticle.rotation);
        Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
        rigidbodyB.linearVelocity=30*aimReticle.transform.up;
        currentInterval = Time.time + interval;
    }

    public void shootTriple()
    {
        GameObject bullet = Instantiate(tripleBullet, aimReticle.position, aimReticle.rotation);
        GameObject bulletL = Instantiate(tripleBullet, aimReticleL.position, aimReticleL.rotation);
        GameObject bulletR = Instantiate(tripleBullet, aimReticleR.position, aimReticleR.rotation);
        Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidbodyBL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidbodyBR = bulletR.GetComponent<Rigidbody2D>();
        rigidbodyB.linearVelocity=20*aimReticle.transform.up;
        rigidbodyBL.linearVelocity=20*aimReticleL.transform.up;
        rigidbodyBR.linearVelocity=20*aimReticleR.transform.up;
        currentInterval = Time.time + interval;
    }
    public void shootPierce()
    {
        GameObject bullet = Instantiate(pierceBullet, aimReticle.position, aimReticle.rotation);
        Rigidbody2D rigidbodyB = bullet.GetComponent<Rigidbody2D>();
        rigidbodyB.linearVelocity=40*aimReticle.transform.up;
        currentInterval = Time.time + interval;
    }

    public void ChangeShootCooldown()
    {
        if (bulletType==0)
        {
            if (atkSpeedPickup==true)
            {
                interval=0.2f;
                print("FAST");
            }
            else
            {
                interval=0.4f;
            }
        }
        if (bulletType==1)
        {
            if (atkSpeedPickup==true)
            {
                interval=0.15f;
            }
            else
            {
                interval=0.3f;
            }
        }
        if (bulletType==2)
        {
            if (atkSpeedPickup==true)
            {
                interval=0.5f;
            }
            else
            {
                interval=1f;
            }
        }
    }

    public void AtkSpeedPickupTrigger()
    {
        StartCoroutine(atkSpeedTimer());
    }
    private IEnumerator atkSpeedTimer()
    {
        atkSpeedPickup=true;
        ChangeShootCooldown();
        yield return new WaitForSeconds(8f);
        atkSpeedPickup=false;
        ChangeShootCooldown();
        
        
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
        ChangeShootCooldown();
    }
    public void OnRightBumper(InputValue inputValue)
    {
        bulletType+=1;
        if (bulletType==3)
        {
            bulletType=0;
        }
        ChangeShootCooldown();
    }
    public void OnDash(InputValue inputValue)
    {
         if (inputValue.isPressed && !dashing) // Dash only if the button is pressed and we're not already dashing
        {
            dashing = true;
            dashTime = Time.time + dashDuration; // Set when to stop dashing
        }
    }

    public void OnPause(InputValue inputValue)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        else 
        {
        logicManager.gameObject.GetComponent<GameHandler>().PauseGame();
        Time.timeScale = 0;
        print("paused");
        }
        
    }
}
