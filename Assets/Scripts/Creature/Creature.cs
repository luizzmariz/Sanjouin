using System;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public string id;
    public Breed race = Breed.none;

    public float speed;

    public float attenionRange;
    [SerializeField] Color32 color;

    public void LoadCreature(CreatureData creatureData)
    {
        this.id = creatureData.id;

        race = creatureData.race;

        this.speed = creatureData.speed;

        gameObject.name = creatureData.name;

        color = creatureData.GetColor(race);
        SetStatus();
    }

    public void SetStatus()
    {
        GetComponent<BaseCreatureStateMachine>().movementSpeed = speed;

        GetComponentInChildren<SpriteRenderer>().color = this.color;
    }
}