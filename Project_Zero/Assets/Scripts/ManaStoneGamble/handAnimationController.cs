using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class handAnimationController : MonoBehaviour
{
    private Animator manaStoneGrapAnimator;
    [SerializeField]
    private float targetX, targetY, originX, originY, backX, backY;
    private Vector2 backPos;
    private void Awake()
    {
        manaStoneGrapAnimator = GetComponent<Animator>();
        backPos = new Vector2(backX, backY);
    }

    private void Update()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
            manaStoneGrapAnimator.SetBool("grap", true);
        else
            manaStoneGrapAnimator.SetBool("grap", false);
    }

    private void FixedUpdate()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
        {
            float progress = (float)(Math.Log(manaStoneGambleManager.nowPower + 1) / Math.Log(manaStoneGambleManager.maxPower + 1));
            float horizontalPos = originX * (1 - progress) + targetX * progress;
            float verticallPos = originY * (1 - progress) + targetY * progress;
            transform.position = new Vector2(horizontalPos, verticallPos);
        }
        else if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Die)
        {
            transform.position = Vector2.Lerp(transform.position, backPos, 0.3f);
        }
    }
}
