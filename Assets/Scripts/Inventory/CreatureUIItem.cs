using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;

public class CreatureUIItem : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text breedText;
    public Button selectButton;

    private CreatureData assignedFeline; 
    public static event Action<CreatureData> OnCreatureSelectedForCrossing;

    void Awake()
    {
        selectButton.onClick.AddListener(OnSelectButtonClick);
    }

    public void Setup(CreatureData feline)
    {
        assignedFeline = feline;


        breedText.text = feline.race.ToString();

        selectButton.gameObject.SetActive(false); 
    }

    private void OnSelectButtonClick()
    {
        Debug.Log($"Criatura {assignedFeline.name} selecionada para cruzamento.");
        OnCreatureSelectedForCrossing?.Invoke(assignedFeline);
    }

    public void SetSelectionMode(bool enable)
    {
        selectButton.gameObject.SetActive(enable);
    }
}