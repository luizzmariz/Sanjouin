using UnityEngine;

public class ScenarioInteractable : Interactable
{
    [SerializeField] GameObject inventory;

    protected override void Interact()
    {
        inventory.SetActive(true);
        inventory.GetComponent<InventoryUIController>().OpenInventory();
    }

    public override string GetPromptMessage()
    {
        return "interact with " + promptMessage;
    }
}