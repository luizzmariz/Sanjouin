using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsPage;
    [SerializeField] GameObject journalPage;

    [SerializeField] Button settingsButton;
    [SerializeField] Button journalButton;

    public GameObject JournalEntryPrefab;
    public Transform JournalContentParent;    // O pai (Content) do Scroll View onde os itens serão instanciados

    private Dictionary<Breed, JournalUIItem> activeJournalItems = new Dictionary<Breed, JournalUIItem>();

    void Awake()
    {
        if (JournalInformationManager.instance != null)
        {
            JournalInformationManager.instance.onBreedDiscoveredCallback += OnBreedDiscovered;
        }
    }

    public void Start()
    {
        ChangePage("settings");
    }

    void OnDestroy()
    {
        if (JournalInformationManager.instance != null)
        {
            JournalInformationManager.instance.onBreedDiscoveredCallback -= OnBreedDiscovered;
        }
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

                PopulateJournal();
                break;

            default:
                break;
        }
    }

    private void PopulateJournal()
    {
        foreach (var item in activeJournalItems.Values)
        {
            Destroy(item.gameObject);
        }
        activeJournalItems.Clear();

        foreach (Breed breed in System.Enum.GetValues(typeof(Breed)))
        {
            if (breed == Breed.none)
            {
                continue;
            }
            GameObject entryGO = Instantiate(JournalEntryPrefab, JournalContentParent);
            JournalUIItem JournalItem = entryGO.GetComponent<JournalUIItem>();

            JournalInformationManager.BreedJournalEntry entryInfo = JournalInformationManager.instance.GetBreedInfo(breed);
            bool isDiscovered = JournalInformationManager.instance.IsBreedDiscovered(breed);

            JournalItem.Setup(breed, entryInfo, isDiscovered);
            activeJournalItems.Add(breed, JournalItem);
        }
    }

    private void OnBreedDiscovered(Breed breed)
    {
        if (activeJournalItems.ContainsKey(breed))
        {
            if (journalPage.activeInHierarchy)
            {
                CreatureData helper = new CreatureData(breed);
                JournalInformationManager.BreedJournalEntry entryInfo = JournalInformationManager.instance.GetBreedInfo(breed);
                activeJournalItems[breed].RevealInfo(entryInfo.defaultDescription, helper.GetColor(breed));
            }
        }
    }
}
