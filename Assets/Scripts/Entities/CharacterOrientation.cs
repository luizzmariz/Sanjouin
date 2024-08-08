using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOrientation : MonoBehaviour
{
    // public Vector3 lastOrientation;
    public Transform handTransform;
    public Vector2 lastOrientation;
    //private string spriteOrientation;
    //public SpriteChanger sc;
    // public Animator animator;
    public SpriteRenderer spriteRenderer;

    // void Start()
    // {
    //     lastOrientation = new Vector3(0, 0, -1);
    //     // spriteOrientation = "forward";
    //     animator = transform.GetComponent<Animator>();
    //     spriteRenderer = transform.GetComponent<SpriteRenderer>();
    // }

    void Awake()
    {
        lastOrientation = Vector2.zero;
        // spriteOrientation = "forward";
        //animator = transform.GetComponent<Animator>();
        spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
    }


    public void ChangeOrientation(Vector3 targetPoint)
    {
        Vector3 relativePos = targetPoint - transform.position;

        //Debug.Log(relativePos);
        //Debug.DrawLine(targetPoint, transform.position, Color.red, 50);

        if(relativePos != Vector3.zero)
        {
            // AngleCheck(relativePos);

            handTransform.up = -relativePos.normalized;
            
            lastOrientation = relativePos;
            // CheckSpriteOrientation(relativePos); --- Old way of changing sprites
        }
        else
        {
            handTransform.up = -lastOrientation.normalized;
            // AngleCheck(lastOrientation);
        }
    }

    // void AngleCheck(Vector3 relativePos)
    // {
    //     // Quaternion LookAtRotation = Quaternion.LookRotation(relativePos, Vector3.forward);

    //     // if(LookAtRotation.eulerAngles.z == 0)
    //     // {
    //     //     handTransform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles.x, handTransform.rotation.eulerAngles.y, LookAtRotation.eulerAngles.y);
    //     //     ChangeSpriteOrientation(LookAtRotation.eulerAngles.y);
    //     // }
    //     // else
    //     // {
    //     //     float x = 0;
    //     //     if(Math.Abs(270 - LookAtRotation.eulerAngles.z) < 1)
    //     //     {
    //     //         if(LookAtRotation.eulerAngles.x <= 90)
    //     //         {
    //     //             x = LookAtRotation.eulerAngles.x - 90;
    //     //         }
    //     //         else
    //     //         {
    //     //             x = -(90 - (LookAtRotation.eulerAngles.x - 360));
    //     //         }
    //     //     }
    //     //     else if(Math.Abs(90 - LookAtRotation.eulerAngles.z) < 1)
    //     //     {
    //     //         if(LookAtRotation.eulerAngles.x <= 90)
    //     //         {
    //     //             x = 90 - LookAtRotation.eulerAngles.x;
    //     //         }
    //     //         else
    //     //         {
    //     //             x = 90 + (360 - LookAtRotation.eulerAngles.x);
    //     //         }
    //     //     }

    //     //     Debug.Log(LookAtRotation.eulerAngles.z + " - " + x);

    //     //     handTransform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles.x, handTransform.rotation.eulerAngles.y, x);

    //     //     if(x < 0)
    //     //     {
    //     //         x = 360 - x;
    //     //     }
            
    //     //     ChangeSpriteOrientation(x);
    //     // }

    //     // ChangeSpriteOrientation()
    //     handTransform.up = -relativePos.normalized;
    // }

    // void CheckSpriteOrientation(Vector3 orientationVector) --- Old way of changing sprites: the facing forward and facing back sprites had priority  
    // {
    //     if(orientationVector.x > 0f)
    //     {
    //         spriteOrientation = "right";
    //         Debug.Log(orientationVector + " means right");
    //     }
    //     else if(orientationVector.x < 0f)
    //     {
    //         spriteOrientation = "left";
    //         Debug.Log(orientationVector + " means left");
    //     }
    //     if(orientationVector.z > 0f)
    //     {
    //         spriteOrientation = "back";
    //         Debug.Log(orientationVector + " means back");
    //     }
    //     else if(orientationVector.z < 0f)
    //     {
    //         spriteOrientation = "forward";
    //         Debug.Log(orientationVector + " means forward");
    //     }
    //     sc.ChangeSprite(spriteOrientation);
    // }

    // void ChangeSpriteOrientation(float yAngle)
    // {
    //     if((yAngle > 315f && yAngle <= 360f) || (yAngle >= 0f && yAngle <= 45f))
    //     {
    //         // spriteOrientation = "back";
    //         spriteRenderer.flipX = false;
    //         // animator.SetInteger("orientationNumber", 3);
    //     }
    //     else if(yAngle > 46f && yAngle <= 135f)
    //     {
    //         // spriteOrientation = "right";
    //         spriteRenderer.flipX = false;
    //         // animator.SetInteger("orientationNumber", 0);
    //     }
    //     else if(yAngle > 135f && yAngle <= 225f)
    //     {
    //         // spriteOrientation = "forward";
    //         spriteRenderer.flipX = false;
    //         // animator.SetInteger("orientationNumber", 2);
    //     }
    //     else if(yAngle > 225f && yAngle <= 315f)
    //     {
    //         // spriteOrientation = "left";
    //         spriteRenderer.flipX = true;
    //         // animator.SetInteger("orientationNumber", 1);
    //     }
    //     //sc.ChangeSprite(spriteOrientation);
    // }
}