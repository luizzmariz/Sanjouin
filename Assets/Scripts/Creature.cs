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

        legendary1,
        legendary2,
        legendary3,

        hybrid1
    }

    public Vector2Int genes;
    public Breed race = Breed.none;

    public float worldSpeed;
    public float inGameSpeed;

    public float attenionRange;

    public void SetBreed(Breed creatureRace)
    {
        race = creatureRace;
    }

    void Start()
    {

    }

    void Update()
    {

    }
}