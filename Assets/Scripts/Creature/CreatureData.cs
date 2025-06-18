using UnityEngine;
using System;

public enum Breed
{
    none,

    yellow,

    red,
    green,
    blue,

    lime,
    purple,
    orange,
    brown,

    black,
    white,

    dark_red,
    dark_green,
    dark_blue,

    golden,
    grey,

    legendary1,
    legendary2,
    legendary3,

    hybrid1
}

[Serializable]
public class CreatureData
{
    public Breed race;

    public string id;
    public string name;

    public float speed;

    public CreatureData(Creature creature)
    {
        this.id = creature.id;
        this.race = creature.race;
        this.name = creature.name;
        this.speed = creature.speed;
    }

    public CreatureData(Breed race)
    {
        this.id = Guid.NewGuid().ToString();
        this.race = race;
        this.name = race.ToString();
        SetStatus();
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

    public Color32 GetColor(Breed race)
    {
        Color32 creatureColor;

        switch (race)
        {
            case Breed.yellow:
                return creatureColor = new Color32(245, 255, 0, 255);

            case Breed.blue:
                return creatureColor = new Color32(0, 152, 255, 255);

            case Breed.red:
                return creatureColor = new Color32(255, 93, 85, 255);

            case Breed.green:
                return creatureColor = new Color32(105, 255, 85, 255);

            case Breed.lime:
                return creatureColor = new Color32(150, 255, 0, 255);

            case Breed.purple:
                return creatureColor = new Color32(127, 0, 255, 255);

            case Breed.black:
                return creatureColor = new Color32(25, 25, 25, 255);

            case Breed.dark_blue:
                return creatureColor = new Color32(00, 7, 94, 255);

            case Breed.dark_red:
                return creatureColor = new Color32(65, 3, 0, 255);

            case Breed.dark_green:
                return creatureColor = new Color32(4, 65, 0, 255);

            case Breed.orange:
                return creatureColor = new Color32(255, 119, 0, 255);

            case Breed.brown:
                return creatureColor = new Color32(56, 23, 9, 255);

            case Breed.white:
                return creatureColor = new Color32(255, 255, 255, 255);

            case Breed.golden:
                return creatureColor = new Color32(255, 184, 0, 255);

            case Breed.grey:
                return creatureColor = new Color32(115, 115, 115, 255);

            case Breed.legendary1:
                return creatureColor = new Color32(255, 0, 76, 255);

            case Breed.legendary2:
                return creatureColor = new Color32(0, 255, 211, 255);

            case Breed.legendary3:
                return creatureColor = new Color32(70, 0, 89, 255);

            case Breed.hybrid1:
                return creatureColor = new Color32(255, 93, 146, 255);

            default:
                return creatureColor = new Color32(255, 255, 255, 0);
        }
    }
}
