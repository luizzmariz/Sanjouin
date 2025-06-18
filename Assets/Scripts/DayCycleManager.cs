using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    public Light2D sunLight;
    public Gradient ambientColor;

    [Header("CycleRotation")]
    public bool timeIsPassing;
    public bool isDay;
    public DayPhase dayPhase;

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

        newDayTime = dayTime;
    }

    void FixedUpdate()
    {
        if (timeIsPassing)
        {
            newDayTime += Time.deltaTime;

            CheckDayPhase();
            sunLight.color = ambientColor.Evaluate(newDayTime / 24);
        }
    }

    void CheckDayPhase()
    {
        if (newDayTime >= 7 && newDayTime < 12)
        {
            if (dayPhase != DayPhase.MORNING)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.MORNING;
        }

        if (newDayTime >= 12 && newDayTime < 16)
        {
            if (dayPhase != DayPhase.AFTERNOON)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.AFTERNOON;
        }

        if (newDayTime >= 16 && newDayTime < 20)
        {
            if (dayPhase != DayPhase.EVENING)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.EVENING;
        }

        if (newDayTime >= 20 && newDayTime < 24)
        {
            if (dayPhase != DayPhase.NIGHT)
            {
                CreatureSpawner.instance.SpawnEnemies();
            }
            dayPhase = DayPhase.NIGHT;
        }

        if (newDayTime >= 24)
        {
            newDayTime = newDayTime % 24 + dayTime;
        }
    }
}