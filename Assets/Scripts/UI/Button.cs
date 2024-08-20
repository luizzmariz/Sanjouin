using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void ButtonFunction(string buttonName)
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.ButtonFunction(buttonName);
        }
    }
}
