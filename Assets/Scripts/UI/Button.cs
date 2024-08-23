using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    public TMP_Text theText;

    public Color onHover;
    public Color onExit;
    public Button button;

    //void OnGUI() { Debug.Log(GUI.skin.button.hover.textColor); } 

    void Awake()
    {
        theText = transform.GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("C0221A");
        // theText.text.textColor = onHover; //Or however you do your color
        // Color = onHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("204D25");
        theText.color = onExit; //Or however you do your color
    }

    public void OnMouseOver() {
        theText.color = onHover;
    }

    void OnMouseExit()
    {
        theText.color = onExit;
    }


    public void ButtonFunction(string buttonName)
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.ButtonFunction(buttonName);
        }
    }
}
