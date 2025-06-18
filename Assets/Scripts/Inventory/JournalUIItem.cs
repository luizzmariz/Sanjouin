using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalUIItem : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI breedNameText;
    public TextMeshProUGUI descriptionText;

    private Breed assignedBreed;

    public void Setup(Breed breed, JournalInformationManager.BreedJournalEntry entryInfo, bool isDiscovered)
    {
        assignedBreed = breed;
        breedNameText.text = breed.ToString(); // Exibe o nome da raça sempre

        if (isDiscovered)
        {
            CreatureData helper = new CreatureData(breed);
            RevealInfo(entryInfo.defaultDescription, helper.GetColor(breed));
        }
        else
        {
            HideInfo(entryInfo.hiddenDescription);
        }
    }

    public void RevealInfo(string description, Color32 _color)
    {
        descriptionText.text = description;
        iconImage.color = _color;
    }

    public void HideInfo(string hiddenDescription)
    {
        descriptionText.text = hiddenDescription;
        iconImage.color = Color.black;
    }
}