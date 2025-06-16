using System.Collections;
using UnityEngine;

public enum DayPhase
{
    MORNING,
    AFTERNOON,
    EVENING,
    NIGHT,
}

public class DayCycleManager : MonoBehaviour
{
    [Header("DayCycleManager")]
    public static DayCycleManager instance = null;

    [Header("AmbientColor")]
    public Gradient ambientColor;

    [Header("CycleRotation")]
    public bool timeIsPassing;
    public bool isDay;
    public DayPhase dayPhase;
    // public GameObject sunLight;

    [Header("DayTime")]
    public float dayTime;
    public float newDayTime;
    public float secondsBetweenHours;

    [Header("ActionTokens")]
    public int currentActionTokensTaken;

    [Header("Player")]
    public GameObject player;
    public PlayerStateMachine playerStateMachine;

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

        dayPhase = DayPhase.MORNING;
        dayTime = 7f;


        newDayTime = dayTime;

        // timeIsPassing = true;
    }

    void FixedUpdate()
    {
        if (timeIsPassing)
        {
            dayTime += Time.deltaTime;

            CheckDayPhase();
            // RenderSettings.ambientLight = ambientColor.Evaluate(dayTime / 24);
        }
    }

    void CheckDayPhase()
    {
        if (dayTime >= 7 && dayTime < 12)
        {
            if (dayPhase != DayPhase.MORNING)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.MORNING;
        }

        if (dayTime >= 12 && dayTime < 16)
        {
            if (dayPhase != DayPhase.AFTERNOON)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.AFTERNOON;
        }

        if (dayTime >= 16 && dayTime < 20)
        {
            if (dayPhase != DayPhase.EVENING)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.EVENING;
        }

        if (dayTime >= 20 && dayTime < 24)
        {
            if (dayPhase != DayPhase.NIGHT)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.NIGHT;
        }

        if (dayTime >= 24)
        {
            dayTime = dayTime % 24 + 7;
        }
    }
}