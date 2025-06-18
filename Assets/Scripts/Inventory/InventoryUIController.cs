using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryScreen;

    public GameObject inventoryPanel; // Painel principal do inventário
    public GameObject breedingPanel;  // Painel de cruzamento
    public GameObject inventoryContentParent;  
    public GameObject breedingContentParent;
    public GameObject creatureUIItemPrefab; // Prefab do item de UI de criatura

    public Button openBreedingPanelButton;
    public Button closeInventoryButton;
    public Button closeBreedingPanelButton;

    private List<GameObject> spawnedCreatureUIItems = new List<GameObject>();

    void Start()
    {
        // inventoryScreen.SetActive(false);

        // inventoryPanel.SetActive(false);
        breedingPanel.SetActive(false);

        // Adiciona listeners para os botões
        openBreedingPanelButton.onClick.AddListener(OpenBreedingPanel);
        closeInventoryButton.onClick.AddListener(CloseInventory);
        closeBreedingPanelButton.onClick.AddListener(CloseBreedingPanel);

        // Assina o evento de mudança do inventário
        CreatureInventoryManager.instance.onInventoryChangedCallback += UpdateInventoryUI;

        // Assina o evento de seleção de criatura para cruzamento
        CreatureUIItem.OnCreatureSelectedForCrossing += SelectCreatureForBreeding;
    }

    void OnDestroy()
    {
        // Remove os listeners para evitar erros quando o objeto for destruído
        if (CreatureInventoryManager.instance != null)
        {
            CreatureInventoryManager.instance.onInventoryChangedCallback -= UpdateInventoryUI;
        }
        CreatureUIItem.OnCreatureSelectedForCrossing -= SelectCreatureForBreeding;
    }


    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
        breedingPanel.SetActive(false); // Garante que o painel de cruzamento esteja fechado
        UpdateInventoryUI(); // Atualiza a UI sempre que o inventário é aberto
    }

    public void CloseInventory()
    {
        GameObject.Find("Player").GetComponent<PlayerStateMachine>().StopInteracting();

        inventoryScreen.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public void OpenBreedingPanel()
    {
        inventoryPanel.SetActive(false); // Mantém o inventário aberto para seleção
        breedingPanel.SetActive(true);
        
        UpdateBreedingUI();
        SetupBreedingSelectionUI(); // Configura a UI para seleção
    }

    public void CloseBreedingPanel()
    {

        breedingPanel.SetActive(false);
        // Desabilita o modo de seleção em todos os itens de criatura
        foreach (GameObject itemGO in spawnedCreatureUIItems)
        {
            itemGO.GetComponent<CreatureUIItem>().SetSelectionMode(false);
        }

        OpenInventory();
    }

    // --- Atualização da UI ---
    private void UpdateInventoryUI()
    {
        // Limpa os itens antigos
        foreach (GameObject itemGO in spawnedCreatureUIItems)
        {
            Destroy(itemGO);
        }
        spawnedCreatureUIItems.Clear();

        // Obtém a lista atualizada de criaturas
        List<CreatureData> creatures = CreatureInventoryManager.instance.GetAllCreatures();

        // Instancia e configura novos itens para cada criatura
        foreach (CreatureData feline in creatures)
        {
            GameObject itemGO = Instantiate(creatureUIItemPrefab, inventoryContentParent.transform);
            CreatureUIItem uiItem = itemGO.GetComponent<CreatureUIItem>();
            if (uiItem != null)
            {
                uiItem.Setup(feline);
                spawnedCreatureUIItems.Add(itemGO);
            }
        }
    }

    private void UpdateBreedingUI()
    {
        // Limpa os itens antigos
        foreach (GameObject itemGO in spawnedCreatureUIItems)
        {
            Destroy(itemGO);
        }
        spawnedCreatureUIItems.Clear();

        // Obtém a lista atualizada de criaturas
        List<CreatureData> creatures = CreatureInventoryManager.instance.GetAllCreatures();

        // Instancia e configura novos itens para cada criatura
        foreach (CreatureData feline in creatures)
        {
            GameObject itemGO = Instantiate(creatureUIItemPrefab, breedingContentParent.transform);
            CreatureUIItem uiItem = itemGO.GetComponent<CreatureUIItem>();
            if (uiItem != null)
            {
                uiItem.Setup(feline);
                spawnedCreatureUIItems.Add(itemGO);
            }
        }
    }

    // --- Lógica de Seleção para Cruzamento ---
    private CreatureData selectedFeline1;
    private CreatureData selectedFeline2;

    public Image selectedFeline1Icon;
    public TextMeshProUGUI selectedFeline1Name;
    public Image selectedFeline2Icon;
    public TextMeshProUGUI selectedFeline2Name;
    public Button startBreedingButton; // Botão "Começar Cruzamento"

    void Awake()
    {
        // ... (resto do Awake)
        startBreedingButton.onClick.AddListener(StartBreedingProcess);
        ClearBreedingSelection(); // Limpa as seleções no início
    }

    private void SetupBreedingSelectionUI()
    {
        ClearBreedingSelection();
        // Habilita o botão de seleção em todos os itens de criatura no inventário
        foreach (GameObject itemGO in spawnedCreatureUIItems)
        {
            itemGO.GetComponent<CreatureUIItem>().SetSelectionMode(true);
        }
    }

    private void SelectCreatureForBreeding(CreatureData selectedFeline)
    {
        if (selectedFeline1 == null)
        {
            selectedFeline1 = selectedFeline;
            selectedFeline1Name.text = selectedFeline.name;
        }
        else if (selectedFeline2 == null && selectedFeline.id != selectedFeline1.id)
        {
            selectedFeline2 = selectedFeline;
            selectedFeline2Name.text = selectedFeline.name;
        }

        startBreedingButton.interactable = (selectedFeline1 != null && selectedFeline2 != null);
    }

    private void ClearBreedingSelection()
    {
        selectedFeline1 = null;
        selectedFeline2 = null;
        selectedFeline1Icon.sprite = null; // Ou sprite padrão
        selectedFeline1Name.text = "Selecione 1";
        selectedFeline2Icon.sprite = null; // Ou sprite padrão
        selectedFeline2Name.text = "Selecione 2";
        startBreedingButton.interactable = false;
    }

    private void StartBreedingProcess()
    {
        if (selectedFeline1 == null || selectedFeline2 == null)
        {
            return;
        }

        Breed newBreed = BreedManager.instance.GetCrossbreedResult(selectedFeline1, selectedFeline2);

        // Creature newCreature = new CreatureData();

        // CreatureInventoryManager.instance.AddCreature(newCreature);

        // Remove os pais do inventário (opcional, dependendo da sua mecânica)
        CreatureInventoryManager.instance.RemoveCreature(selectedFeline1);
        CreatureInventoryManager.instance.RemoveCreature(selectedFeline2);

        // Debug.Log($"Cruzamento bem-sucedido! Nova criatura: {newCreature.name} ({newCreature.race})");             <-------------------------

        CloseBreedingPanel();
        UpdateInventoryUI();
    }

}