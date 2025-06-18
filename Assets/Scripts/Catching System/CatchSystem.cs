using System;
using UnityEngine;

public class CatchSystem : MonoBehaviour
{
    public static CatchSystem instance = null;

    public GameObject minigame;

    GameObject creatureToCacth;
    [SerializeField] Transform Hands; 

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        minigame.SetActive(false);
    }

    public void StartCatch(GameObject creatureGameObject)
    {
        creatureToCacth = creatureGameObject;
        minigame.SetActive(true);

        foreach (GameObject creature in CreatureSpawner.instance.GetAllCreaturesAlive())
        {
            creature.GetComponent<SimpleCreatureStateMachine>().ChangeToCatchState();
        }

        float minigameDifficulty = creatureGameObject.GetComponent<Creature>().speed / 2.5f;
        minigameDifficulty =  Mathf.Clamp(minigameDifficulty, 1.5f, 5f);
        Debug.Log(minigameDifficulty);
        minigame.GetComponent<LassoingMinigame>().creatureSpeedMultiplicator = minigameDifficulty;

        minigame.GetComponent<LassoingMinigame>().EnableMinigame();
    }

    public void EndCatch(bool victory)
    {
        minigame.SetActive(false);

        foreach (GameObject creature in CreatureSpawner.instance.GetAllCreaturesAlive())
        {
            creature.GetComponent<SimpleCreatureStateMachine>().LeaveCatchState();
        }

        if (victory)
        {
            creatureToCacth.GetComponent<SimpleCreatureStateMachine>().ChangeToCapturedState();
        }

        Hands.GetChild(0).GetComponent<LassoController>().EndLassoing();
    }
}
