using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;  
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    // public TMP_Text theText;

    // public Color onHover;
    // public Color defaultColor;
    // public Button button;
    // public bool changeText;

    //void OnGUI() { Debug.Log(GUI.skin.button.hover.textColor); } 

    // void Awake()
    // {
    //     theText = transform.GetComponentInChildren<TMP_Text>();
    //     button = GetComponent<Button>();
    //     if(changeText)
    //     {
    //         button.targetGraphic = theText;
    //     }
        
    //     //playerInput.actions["InteractWithAmbient"].GetBindingDisplayString(group: playerInput.currentControlScheme)

    //     //Debug.Log(theText.color);
    // }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     Debug.Log("C0221A");
    //     // theText.text.textColor = onHover; //Or however you do your color
    //     // Color = onHover;
    // }

    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     Debug.Log("204D25");
    //     theText.color = defaultColor; //Or however you do your color
    // }

    // public void OnMouseOver() {
    //     Debug.Log("rggwer");
    //     theText.color = onHover;
    // }

    // void OnMouseExit()
    // {
    //     Debug.Log("ggeisso");
    //     theText.color = defaultColor;
    // }


    public void ButtonFunction(string buttonName)
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.ButtonFunction(buttonName);
        }
    }
}
