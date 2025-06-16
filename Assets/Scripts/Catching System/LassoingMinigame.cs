using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LassoingMinigame : MonoBehaviour
{
    [Header("Creature")]
    [SerializeField] Transform rightPivot;
    [SerializeField] Transform leftPivot;

    [SerializeField] Transform creature;

    float creaturePosition;
    float creatureDestination;

    float creatureTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float creatureSpeed;
    [SerializeField] float smoothMotion = 1f;

    [Header("Lasso")]
    [SerializeField] Transform lasso;
    float lassoPosition;
    [SerializeField] float lassoSize = 0.1f;
    [SerializeField] float lassoPower = 0.5f;

    [SerializeField] float lassoProgress;
    [SerializeField] float lassoPullVelocity;

    [SerializeField] float lassoPullPower = 0.01f;
    [SerializeField] float lassoGravityPower = 0.005f;
    [SerializeField] float lassoProgressDegradationPower = 0.1f;

    [Header("ProgressBar")]
    [SerializeField] Transform progressBar;
    [SerializeField] Transform teste1;
    [SerializeField] Transform teste2;

    [Header("Game")]
    bool pause = false;
    [SerializeField] float failTimer = 10f;
    bool minigameStarted = false;

    public void EnableMinigame()
    {
        minigameStarted = true;
        lassoProgress = 0;
        pause = false;
    }

    void Update()
    {
        if(minigameStarted)
        {
            if (pause) return;

            Creature();
            Lasso();
            ProgressCheck();
        }
    }

    void Creature()
    {
        creatureTimer -= Time.deltaTime;
        if (creatureTimer <= 0)
        {
            creatureTimer = UnityEngine.Random.value * timerMultiplicator;

            creatureDestination = UnityEngine.Random.value;
        }

        creaturePosition = Mathf.SmoothDamp(creaturePosition, creatureDestination, ref creatureSpeed, smoothMotion);
        creature.position = Vector3.Lerp(leftPivot.position, rightPivot.position, creaturePosition);
    }

    void Lasso()
    {
        if (Input.GetMouseButton(0))
        {
            lassoPullVelocity += lassoPullPower * Time.deltaTime;
        }
        lassoPullVelocity -= lassoGravityPower * Time.deltaTime;


        lassoPosition += lassoPullVelocity;
        lassoPosition = Mathf.Clamp(lassoPosition, 0, 1);

        if (lassoPosition == 0 || lassoPosition == 1)
        {
            lassoPullVelocity = 0;
        }

        lasso.position = Vector3.Lerp(leftPivot.position, rightPivot.position, lassoPosition);
    }

    void ProgressCheck()
    {
        Vector3 ls = progressBar.localScale;
        ls.x = lassoProgress;
        progressBar.localScale = ls;

        float min = lassoPosition - lassoSize / 2;
        float max = lassoPosition + lassoSize / 2;

        teste1.position = new Vector3(lasso.localPosition.x - min, 0, 0);
        teste2.position = new Vector3(lasso.localPosition.x + max, 0, 0);

        if (min < creaturePosition && creaturePosition < max)
        {
            creature.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            lassoProgress += lassoPower * Time.deltaTime;
        }
        else
        {
            creature.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            lassoProgress -= lassoProgressDegradationPower * Time.deltaTime;

            failTimer -= Time.deltaTime;
            if (failTimer <= 0)
            {
                Lose();
            }
        }

        if (lassoProgress >= 4.5f)
        {
            Win();
        }

        lassoProgress = Mathf.Clamp(lassoProgress, 0, 4.5f);
    }

    void Win()
    {
        pause = true;
        minigameStarted = false;
        CatchSystem.instance.EndCatch(true);
    }

    void Lose()
    {
        pause = true;
        minigameStarted = false;
        CatchSystem.instance.EndCatch(true);
    }
}
