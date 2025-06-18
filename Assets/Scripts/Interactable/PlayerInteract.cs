using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact with objects in space")]
    [SerializeField] float checkSphereRadius = 3f;
    private Collider2D closestCollider;
    public TMP_Text interactionText;

    void Start()
    {
        interactionText.text = "";
    }

    void Update()
    {
        closestCollider = null;
        CheckClosestCollider();
    }

    public void OnInteractWithAmbient(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (closestCollider != null)
            {
                closestCollider.GetComponent<Interactable>().BaseInteract();
                GetComponent<PlayerStateMachine>().StartInteracting();
            }
        }
    }

    public void OnEnter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    void CheckClosestCollider()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, checkSphereRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.GetComponent<Interactable>() != null)
            {
                if (closestCollider == null)
                {
                    closestCollider = hitCollider;
                }
                else
                {
                    if (Vector3.Distance(transform.position, closestCollider.transform.position) < Vector3.Distance(transform.position, hitCollider.transform.position))
                    {
                        closestCollider = hitCollider;
                    }
                }
            }
        }

        if (closestCollider != null)
        {
            PlayerInput playerInput = GetComponent<PlayerInput>();
            interactionText.text = "Press " + playerInput.actions["InteractWithAmbient"].GetBindingDisplayString(group: playerInput.currentControlScheme)
            + " to " + closestCollider.GetComponent<Interactable>().GetPromptMessage();
        }
        else
        {
            interactionText.text = "";
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, checkSphereRadius);
    }
}