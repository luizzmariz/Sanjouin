using System;
using System.Collections.Generic;
using UnityEngine;

public class JournalInformationManager : MonoBehaviour
{
    public static JournalInformationManager instance;

    private HashSet<Breed> discoveredBreeds = new HashSet<Breed>();

    [Serializable]
    public struct BreedJournalEntry
    {
        public Breed breed;
        public string defaultDescription;
        public string hiddenDescription;
    }

    public List<BreedJournalEntry> allBreedJournalEntries = new List<BreedJournalEntry>();
    private Dictionary<Breed, BreedJournalEntry> breedInfoDictionary = new Dictionary<Breed, BreedJournalEntry>();

    public delegate void OnBreedDiscovered(Breed breed);
    public event OnBreedDiscovered onBreedDiscoveredCallback;

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

        LoadDictionary();
    }

    void LoadDictionary()
    {
        foreach (BreedJournalEntry feline in allBreedJournalEntries)
        {
            if (!breedInfoDictionary.ContainsKey(feline.breed))
            {
                breedInfoDictionary.Add(feline.breed, feline);
            }
        }
    }

    public void CheckBreedInJournal(Breed breed)
    {
        if (!IsBreedDiscovered(breed))
        {
            discoveredBreeds.Add(breed);
            onBreedDiscoveredCallback?.Invoke(breed);
        }
    }
    
    public bool IsBreedDiscovered(Breed breed)
    {
        return discoveredBreeds.Contains(breed);
    }

    public BreedJournalEntry GetBreedInfo(Breed _breed)
    {
        if (breedInfoDictionary.ContainsKey(_breed))
        {
            Debug.Log("truth");
            return breedInfoDictionary[_breed];
        }

        return new BreedJournalEntry { breed = _breed, defaultDescription = _breed.ToString(), hiddenDescription = "????" };
    }
}
