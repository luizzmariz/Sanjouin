using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
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

    // public Vector2Int genes;
    public Breed race = Breed.none;

    public float speed;

    public float attenionRange;

    public void SetBreed(Breed creatureRace)
    {
        race = creatureRace;

        SetStatus();
    }

    void SetStatus()
    {
        switch (race)
        {
            case Breed.yellow:
                speed = Random.Range(1f, 1.5f);
                break;

            case Breed.blue:
                speed = Random.Range(1f, 2f);
                break;

            case Breed.red:
                speed = Random.Range(1f, 2f);
                break;

            case Breed.green:
                speed = Random.Range(1f, 2f);
                break;

            case Breed.lime:
                speed = Random.Range(3f, 4f);
                break;

            case Breed.purple:
                speed = Random.Range(3f, 4f);
                break;

            case Breed.black:
                speed = 5f;
                break;

            case Breed.dark_blue:
                speed = Random.Range(4f, 5.5f);
                break;

            case Breed.dark_red:
                speed = Random.Range(4f, 5.5f);
                break;

            case Breed.dark_green:
                speed = Random.Range(4f, 5.5f);
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
                speed = Random.Range(7f, 8.5f);
                break;

            case Breed.grey:
                speed = Random.Range(7f, 8.5f);
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