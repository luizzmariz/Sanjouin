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
}
