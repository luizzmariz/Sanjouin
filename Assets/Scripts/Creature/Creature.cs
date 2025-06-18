using System;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public string id;
    public Breed race = Breed.none;

    public float speed;

    public float attenionRange;
    private Creature creature;

    public Creature(Creature creature)
    {
        this.creature = creature;
    }

    public void CreateCreature(Breed creatureRace)
    {
        id = Guid.NewGuid().ToString();

        race = creatureRace;

        SetStatus();
    }

    public void LoadCreature(CreatureData creatureData)
    {
        this.id = creatureData.id;

        race = creatureData.race;

        this.speed = creatureData.speed;

        gameObject.name = creatureData.name;
    }


    void SetStatus()
    {
        switch (race)
        {
            case Breed.yellow:
                speed = UnityEngine.Random.Range(1f, 1.5f);
                break;

            case Breed.blue:
                speed = UnityEngine.Random.Range(1f, 2f);
                break;

            case Breed.red:
                speed = UnityEngine.Random.Range(1f, 2f);
                break;

            case Breed.green:
                speed = UnityEngine.Random.Range(1f, 2f);
                break;

            case Breed.lime:
                speed = UnityEngine.Random.Range(3f, 4f);
                break;

            case Breed.purple:
                speed = UnityEngine.Random.Range(3f, 4f);
                break;

            case Breed.black:
                speed = 5f;
                break;

            case Breed.dark_blue:
                speed = UnityEngine.Random.Range(4f, 5.5f);
                break;

            case Breed.dark_red:
                speed = UnityEngine.Random.Range(4f, 5.5f);
                break;

            case Breed.dark_green:
                speed = UnityEngine.Random.Range(4f, 5.5f);
                break;

            case Breed.orange:
                speed = 5.5f;
                break;

            case Breed.brown:
                speed = 5.5f;
                break;

            case Breed.white:
                speed = 7f;
                break;

            case Breed.golden:
                speed = UnityEngine.Random.Range(7f, 8.5f);
                break;

            case Breed.grey:
                speed = UnityEngine.Random.Range(7f, 8.5f);
                break;

            case Breed.legendary1:
                speed = 10f;
                break;

            case Breed.legendary2:
                speed = 10f;
                break;

            case Breed.legendary3:
                speed = 10f;
                break;

            case Breed.hybrid1:
                speed = 10f;
                break;
        }
    }
}