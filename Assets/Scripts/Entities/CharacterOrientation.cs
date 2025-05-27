using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOrientation : MonoBehaviour
{
    public Transform handTransform;
    public Vector2 lastOrientation;

    public SpriteHandler spriteHandler;

    void Awake()
    {
        lastOrientation = Vector2.zero;
        spriteHandler = transform.Find("Visual").GetComponent<SpriteHandler>();
    }


    public void ChangeOrientation(Vector3 targetPoint)
    {
        Vector3 relativePos = targetPoint - transform.position;

        if(relativePos != Vector3.zero)
        {

            handTransform.up = -relativePos.normalized;


            lastOrientation = relativePos;

            spriteHandler.ChangeSprite(handTransform.rotation.eulerAngles.z);
        }
        else
        {
            handTransform.up = -lastOrientation.normalized;
        }
    }
}