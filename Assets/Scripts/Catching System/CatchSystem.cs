using UnityEngine;

public class CatchSystem : MonoBehaviour
{
    public static CatchSystem instance = null;

    public GameObject minigame;

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

    public void StartCatch()
    {
        minigame.SetActive(true);
        minigame.GetComponent<LassoingMinigame>().EnableMinigame();
    }

    public void EndCatch(bool victory)
    {
        minigame.GetComponent<LassoingMinigame>().DisableMinigame();
        minigame.SetActive(false);

        if (victory)
        {
            Debug.Log("Captured!");
        }
        else
        {
            Debug.Log("Fail.");
        }
    }
}
