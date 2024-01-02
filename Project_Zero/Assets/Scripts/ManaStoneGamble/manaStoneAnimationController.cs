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

    public void FixedUpdate()
    {
        if (manaStoneGambleManager.isPlaying)
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
        else
        {
            manaStoneAnimator.SetFloat("speed", 0);
            transform.position = new Vector2(originX, originY);
        }
    }
}
