using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Security.Cryptography;

public class manaStoneAnimationController : MonoBehaviour
{
    public Animator manaStoneAnimator;
    [SerializeField]
    private float originX, originY;

    private void Awake()
    {
        manaStoneAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
            manaStoneAnimator.SetBool("start", true);
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Die)
            manaStoneAnimator.SetBool("die", true);
    }

    public void FixedUpdate()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
        {
            double logPower = Math.Log(manaStoneGambleManager.nowPower + 1) / Math.Log(manaStoneGambleManager.maxPower + 1);
            manaStoneAnimator.SetFloat("speed", (float) logPower);
            System.Random randomGenerator = new System.Random();
            double randomHorizontalShake = (randomGenerator.NextDouble() - 0.5) * logPower;
            double randomVerticalShake = (randomGenerator.NextDouble() - 0.5) * logPower;
            float purposeX = originX + (float)randomHorizontalShake;
            float purposeY = originY + (float)randomVerticalShake;
            transform.position = new Vector2(purposeX, purposeY);
        }
        else if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Die)
        {
            manaStoneAnimator.SetFloat("speed", 1);
            transform.position = new Vector2(originX, originY);
        }
    }
}
