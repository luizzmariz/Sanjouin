using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    public SpriteRenderer characterSpriteRenderer;
    public SpriteRenderer handsSpriteRenderer;
    public Sprite characterDown;
    public Sprite characterUp;
    public Sprite characterSide;

    void Awake()
    {
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
        handsSpriteRenderer =  transform.parent.GetChild(1).GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(float orientationAngle)
    {
        characterSpriteRenderer.flipX = false;

        if((orientationAngle > 315f && orientationAngle <= 360f) || (orientationAngle >= 0f && orientationAngle <= 45f))
        {
            // spriteOrientation = "back";
            characterSpriteRenderer.sprite = characterDown;
            // animator.SetInteger("orientationNumber", 3);
        }
        else if(orientationAngle > 45f && orientationAngle <= 135f)
        {
            // spriteOrientation = "right";
            characterSpriteRenderer.sprite = characterSide;
            // animator.SetInteger("orientationNumber", 0);
        }
        else if(orientationAngle > 135f && orientationAngle <= 225f)
        {
            // spriteOrientation = "forward";
            characterSpriteRenderer.sprite = characterUp;
            // animator.SetInteger("orientationNumber", 2);
        }
        else if(orientationAngle > 225f && orientationAngle <= 315f)
        {
            characterSpriteRenderer.sprite = characterSide;
            characterSpriteRenderer.flipX = true;
            // animator.SetInteger("orientationNumber", 1);
        }

        if(orientationAngle >= 90 && orientationAngle <= 270)
        {
            handsSpriteRenderer.sortingOrder = 0;
        }
        else
        {
            handsSpriteRenderer.sortingOrder = 2;
        }
    }
}
