using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BreedingCombination
{
    public Breed parent1;
    public Breed parent2;

    public Breed child;
}

public class BreedManager : MonoBehaviour
{
    public static BreedManager instance;

    public List<BreedingCombination> breedingCombinations = new List<BreedingCombination>();
    
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

    public Breed GetCrossbreedResult(CreatureData parent1, CreatureData parent2)
    {
        foreach (BreedingCombination combination in breedingCombinations)
        {
            if ((parent1.race == combination.parent1 && parent2.race == combination.parent2) ||
                (parent1.race == combination.parent2 && parent2.race == combination.parent1))
            {
                // Debug.Log($"Combinação encontrada! {parent1.race} + {parent2.race} = {combination.child}");
                return combination.child;
            }
        }

        // Debug.Log($"Nenhuma combinação específica para {parent1.race} + {parent2.race}. Retornando um pai aleatório.");
        if (UnityEngine.Random.value < 0.5f) return parent1.race;
        else return parent2.race;
    }
}
