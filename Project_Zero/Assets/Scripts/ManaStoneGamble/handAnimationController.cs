using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class handAnimationController : MonoBehaviour
{
    private Animator manaStoneGrapAnimator;
    [SerializeField]
    private float targetX, targetY, originX, originY, backX, backY;
    private Vector2 backPos, originPos;
    public bool handType;

    private float getVerticalPos(float x)
    {
        float sum = 0;
        int divide = 1;
        for (int n = 0; n <= 3; n++) {
            sum += Mathf.Sin((2 * n + 1) * 3 * Mathf.PI * x - Mathf.PI / 4) / divide + Mathf.Cos((2 * n + 1) * 3 * Mathf.PI * x - Mathf.PI / 4) / divide;
            divide *= 2;
        }
        if (handType)
        {
            sum = -sum;
        }
        return sum;
    }

    private void Awake()
    {
        manaStoneGrapAnimator = GetComponent<Animator>();
        backPos = new Vector2(backX, backY);
        originPos = new Vector2(originX, originY);
    }

    private void Update()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
            manaStoneGrapAnimator.SetBool("grap", true);
        else
            manaStoneGrapAnimator.SetBool("grap", false);
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.None)
            transform.position = originPos;
    }

    private void FixedUpdate()
    {
        if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Playing)
        {
            float progress = (float)(Math.Log(manaStoneGambleManager.nowPower + 1) / Math.Log(manaStoneGambleManager.maxPower + 1));
            float horizontalPos = originX * (1 - progress) + targetX * progress;
            float verticallPos = originY * (1 - progress) + targetY * progress + getVerticalPos(progress) / 5f;
            transform.position = new Vector2(horizontalPos, verticallPos);
        }
        else if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Die || manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.Selected)
        {
            transform.position = Vector2.Lerp(transform.position, backPos, 0.3f);
        }
        else if (manaStoneGambleManager.isPlaying == manaStoneGambleManager.State.None)
        {
            transform.position = originPos;
        }
    }
}
