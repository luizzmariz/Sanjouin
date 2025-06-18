using UnityEngine;
using System.Collections.Generic;
public class CreatureInventoryManager : MonoBehaviour
{
    public static CreatureInventoryManager instance;

    [SerializeField] private List<CreatureData> capturedCreatures = new List<CreatureData>();

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged onInventoryChangedCallback;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCreature(CreatureData newCreature)
    {
        capturedCreatures.Add(newCreature);
        Debug.Log($"Criatura {newCreature.race} adicionada ao inventário.");

        onInventoryChangedCallback?.Invoke();
    }

    public void RemoveCreature(CreatureData creatureToRemove)
    {
        capturedCreatures.Remove(creatureToRemove);
        Debug.Log($"Criatura {creatureToRemove.race} removida do inventário.");
        onInventoryChangedCallback?.Invoke();
    }

    public List<CreatureData> GetAllCreatures()
    {
        return new List<CreatureData>(capturedCreatures);
    }
}