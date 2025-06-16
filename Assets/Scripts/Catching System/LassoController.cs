using UnityEngine;

public class LassoController : MonoBehaviour
{
    public float expandSpeed = 5f;          
    public float maxLassoDistace = 5f;        
    public float lassoLifetime = 3f;  
    public float lassoExpandtime = 3f;            
    public LayerMask captureLayer;          

    bool capturedOne = false;

    private bool isExpanding = false;
    private Vector3 initialScale;
    private CircleCollider2D lassoCollider;   
    private float currentLifetime = 0f;

    Transform hands;
    [SerializeField] LineRenderer rope;

    [SerializeField] PlayerStateMachine playerStateMachine;

    void Awake()
    {
        if (playerStateMachine == null)
        {
            playerStateMachine = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
        }

        if (lassoCollider == null)
        {
            lassoCollider = GetComponent<CircleCollider2D>();
        }

        // rope.enabled = false;
        lassoCollider.isTrigger = true; 
        initialScale = transform.localScale; // Guarda a escala inicial do laço
    }

    void OnEnable()
    {
        rope.enabled = true;
        transform.localScale = initialScale;
        lassoCollider.radius = initialScale.x / 2f; // Ajusta o raio inicial do colisor
        
        currentLifetime = 0f;
    }

    void Update()
    {
        rope.SetPosition(0, transform.position);
        rope.SetPosition(1, hands.position);

        if (Vector3.Distance(transform.position, hands.position) > maxLassoDistace)
        {
            EndLassoing();
        }

        // Debug.Log(Vector3.Distance(transform.position, hands.position));

        if (isExpanding)
        {
            ExpandLasso();
        }

        currentLifetime += Time.deltaTime;

        if (currentLifetime >= lassoExpandtime)
        {
            isExpanding = true;
        }

        if (currentLifetime >= lassoLifetime)
            {
                EndLassoing();
            }
    }

    void ExpandLasso()
    {
        Vector3 newScale = transform.localScale + Vector3.one * expandSpeed * Time.deltaTime;

        transform.localScale = newScale;

        lassoCollider.radius = newScale.x / 2f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Collision"))
        {
            EndLassoing();
        }

        if (((1 << other.gameObject.layer) & captureLayer) != 0 && !capturedOne)
        {
            capturedOne = true;

            CatchSystem.instance.StartCatch(other.gameObject);
                
            gameObject.SetActive(false);    
        }
    }

    public void LaunchLasso(Vector3 direction, float force)
    {
        hands = transform.parent;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void EndLassoing()
    {
        rope.enabled = false;
        gameObject.SetActive(false);
        playerStateMachine.StopLassoing();
        Destroy(gameObject);
    }
}