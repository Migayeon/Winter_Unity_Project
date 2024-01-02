using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class handAnimationController : MonoBehaviour
{
    private Animator manaStoneGrapAnim;
    [SerializeField]
    private float targetX, targetY, originX, originY;
    private void Awake()
    {
        manaStoneGrapAnim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        float progress = (float) (Math.Log(manaStoneGambleManager.nowPower + 1) / Math.Log(manaStoneGambleManager.maxPower + 1));
        float horizontalPos = originX * (1 - progress) + targetX * progress;
        float verticallPos = originY * (1 - progress) + targetY * progress;
        transform.position = new Vector2(horizontalPos, verticallPos);
    }
}
