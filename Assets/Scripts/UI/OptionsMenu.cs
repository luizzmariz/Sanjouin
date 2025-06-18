using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsPage;
    [SerializeField] GameObject journalPage;

    [SerializeField] Button settingsButton;
    [SerializeField] Button journalButton;

    void Start()
    {
        settingsPage.SetActive(true);
        journalPage.SetActive(false);
    }

    public void ChangePage(string page)
    {
        switch (page)
        {
            case "settings":
                settingsButton.interactable = false;
                journalButton.interactable = true;

                journalPage.SetActive(false);
                settingsPage.SetActive(true);
                break;

            case "journal":
                settingsButton.interactable = true;
                journalButton.interactable = false;

                settingsPage.SetActive(false);
                journalPage.SetActive(true);
                break;

            default:
                break;
        }
    }
}
