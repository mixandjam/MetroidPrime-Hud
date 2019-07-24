using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmCannon : MonoBehaviour
{

    private Vector3 cannonLocalPos;

    public Transform cannonModel;
    [Space]
    public ParticleSystem cannonParticleShooter;
    public ParticleSystem chargingParticle;
    public ParticleSystem chargedParticle;
    public ParticleSystem lineParticles;
    public ParticleSystem chargedCannonParticle;
    public ParticleSystem chargedEmission;
    public ParticleSystem muzzleFlash;

    public bool activateCharge;
    public bool charging;
    public bool charged;
    public float holdTime = 1;
    public float chargeTime = .5f;

    private float holdTimer;
    private float chargeTimer;

    [Space]

    public float punchStrenght = .2f;
    public int punchVibrato = 5;
    public float punchDuration = .3f;
    [Range(0, 1)]
    public float punchElasticity = .5f;

    [Space]
    [ColorUsageAttribute(true, true)]
    public Color normalEmissionColor;
    [ColorUsageAttribute(true, true)]
    public Color finalEmissionColor;


    void Start()
    {
        cannonLocalPos = cannonModel.localPosition;
    }

    void Update()
    {

        //SHOOT
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();

            holdTimer = Time.time;
            activateCharge = true;
        }

        //RELEASE
        if (Input.GetMouseButtonUp(0))
        {
            activateCharge = false;

            if (charging)
            {
                chargedCannonParticle.Play();
                charging = false;
                charged = false;
                chargedParticle.transform.DOScale(0, .05f).OnComplete(()=>chargedParticle.Clear());
                lineParticles.Stop();


                Sequence s = DOTween.Sequence();
                s.Append(cannonModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity));
                s.Join(cannonModel.GetComponentInChildren<Renderer>().material.DOColor(normalEmissionColor, "_EmissionColor", punchDuration));
                s.Join(cannonModel.DOLocalMove(cannonLocalPos, punchDuration).SetDelay(punchDuration));
            }
        }

        //HOLD CHARGE
        if (activateCharge && !charging)
        {
            if (Time.time - holdTimer > holdTime)
            {
                charging = true;
                chargingParticle.Play();
                lineParticles.Play();
                chargeTimer = Time.time;

                cannonModel.DOLocalMoveZ(cannonLocalPos.z - .22f, chargeTime);
                cannonModel.GetComponentInChildren<Renderer>().material.DOColor(finalEmissionColor, "_EmissionColor", chargeTime);
            }
        }

        //CHARGING
        if (charging && !charged)
        {
            if (Time.time - chargeTimer > chargeTime)
            {
                charged = true;
                chargedParticle.Play();
                chargedParticle.transform.localScale = Vector3.zero;
                chargedParticle.transform.DOScale(1, .4f).SetEase(Ease.OutBack);
                chargedEmission.Play();
            }
        }

    }


    void Shoot()
    {
        muzzleFlash.Play();

        cannonModel.DOComplete();
        cannonModel.DOPunchPosition(new Vector3(0, 0, -punchStrenght), punchDuration, punchVibrato, punchElasticity);

        cannonParticleShooter.Play();
    }

}
