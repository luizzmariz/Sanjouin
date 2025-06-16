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
