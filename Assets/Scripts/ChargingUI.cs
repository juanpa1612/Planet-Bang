﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingUI : MonoBehaviour
{
    [SerializeField] GameObject center;
    [SerializeField] GameObject chargingArrow;
    [SerializeField] float fireSpeed;

    float penaltyTime;
    float arrowDirectX;
    float arrowDirectY;
    float chargingTime;
    bool penalized;
    bool charging;
    bool fullyCharged;
    bool pressingJoystick;
    bool isFiring;
    bool backToPos;

    Vector3 joystickVector;
    Vector3 lastPos;
    PlayerMovement playerMove;
    PlayerAudio playerAudio;
    VFX vfxReference;

    public bool Charging
    {
        get
        {
            return charging;
        }
    }
    public float PenaltyTime
    {
        get
        {
            return penaltyTime;
        }
    }

    private void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        playerMove = GetComponent<PlayerMovement>();
        vfxReference = GetComponentInChildren<VFX>();
        chargingTime = 0;
        penalized = false;
        joystickVector = Vector3.zero;
        penaltyTime = 0;
        charging = false;
        fullyCharged = false;
    }

    public float GetChargingTime()
    {
        return Mathf.Round(chargingTime);
    }
    public void StartCharging ()
    {
        if (!charging && !penalized && !isFiring && !fullyCharged)
        {
            playerMove.enabled = false;
            charging = true;
            lastPos = transform.position;
            playerAudio.ChannelingSound();
            vfxReference.StartChargingParticle(true);
            vfxReference.StartShootingParticle(false);
        }
    }
    public void StopCharging ()
    {
        if (charging && !fullyCharged)
        {
            backToPos = true;
            charging = false;
            penaltyTime = 3f;
            penalized = true;
            chargingArrow.SetActive(false);
            playerAudio.StopSounds();
            vfxReference.StartChargingParticle(false);
        }
    }
    private void Update()
    {
        if (penalized)
        {
            penaltyTime -= Time.deltaTime;
            if (penaltyTime <= 0)
            {
                penalized = false;
            }
        }

        //Carga Incompleta
        if (backToPos)
        {
            if (transform.position != lastPos)
            {
                transform.LookAt(lastPos);
                transform.Translate(Vector3.forward * Time.deltaTime * fireSpeed);
            }
            if (Vector3.Distance(transform.position, lastPos) < 0.5f)
            {
                transform.position = lastPos;
                playerMove.enabled = true;
                backToPos = false;
            }
            playerAudio.FireSound();
        }
        if (charging)
        {
            transform.position = Vector3.MoveTowards(transform.position, center.transform.position, 1.5f);
        }
        //Carga Completa
        if (gameObject.transform.position == center.transform.position)
        {
            transform.eulerAngles = Vector3.zero;
            charging = false;
            fullyCharged = true;
            chargingArrow.SetActive(true);

            if (chargingTime < 10)
                chargingTime += Time.deltaTime;
        }
        if (fullyCharged && !isFiring)
        {
            //Apuntar
                if (arrowDirectX == 0 && arrowDirectY == 0)
                {
                    pressingJoystick = false;
                }
                if (arrowDirectX != 0 || arrowDirectY != 0)
                {
                    float angulo = Mathf.Atan2(arrowDirectX, arrowDirectY) * Mathf.Rad2Deg;
                    joystickVector.y = angulo;
                    gameObject.transform.eulerAngles = joystickVector;
                }
        }
        //Fire
        if (isFiring)
        {
            chargingArrow.SetActive(false);
            transform.Translate(-Vector3.forward * Time.deltaTime * fireSpeed);
            vfxReference.StartChargingParticle(false);
            vfxReference.StartShootingParticle(true);
            playerAudio.FireSound();
        }
    }
    public void Fire ()
    {
        if (fullyCharged)
            isFiring = true;
    }
    public void ArrowDirection (float x, float y)
    {
        arrowDirectX = x * -1;
        arrowDirectY = y;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (isFiring && collision.gameObject.CompareTag("LastRing"))
        {
            fullyCharged = false;
            isFiring = false;
            playerMove.enabled = true;
            playerMove.radius = 68;
            chargingTime = 0;
            playerMove.time = (Mathf.Atan2(transform.position.z, transform.position.x) / 2);
            penaltyTime = 3f;
            penalized = true;
            vfxReference.StartShootingParticle(false);
        }
    }
}
