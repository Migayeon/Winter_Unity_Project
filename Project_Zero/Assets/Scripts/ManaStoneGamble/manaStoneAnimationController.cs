using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Security.Cryptography;
using static UnityEngine.UI.Image;

public class manaStoneAnimationController : MonoBehaviour
{
    public Animator manaStoneAnimator;
    public SpriteRenderer manaStoneRenderer;
    [SerializeField]
    private float originX, originY;
    private Vector2 originPos;

    private void destroy()
    {
        manaStoneAnimator.SetBool("start", false);
        manaStoneAnimator.SetBool("die", false);
        manaStoneAnimator.SetBool("none", true);
        manaStoneRenderer.enabled = false;
    }

    private void Awake()
    {
        manaStoneRenderer = GetComponent<SpriteRenderer>();
        manaStoneAnimator = GetComponent<Animator>();
        originPos = new Vector2(originX, originY);
    }

    private void Update()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.None)
        {
            manaStoneAnimator.SetBool("die", false);
            manaStoneAnimator.SetBool("none", true);
            manaStoneRenderer.enabled = true;
        }
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
        {
            manaStoneAnimator.SetBool("none", false);
            manaStoneAnimator.SetBool("start", true);
        }
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Die)
        {
            manaStoneAnimator.SetBool("none", false);
            manaStoneAnimator.SetBool("start", false);
            manaStoneAnimator.SetBool("die", true);
        }
    }

    public void FixedUpdate()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing || manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Selected)
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
            transform.position = originPos;
        }
    }
}
