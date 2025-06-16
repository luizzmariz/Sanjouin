using UnityEngine;

public class PlayerLassoThrower : MonoBehaviour
{
    public GameObject lassoPrefab;          
    public float throwForce;    

    Vector3 throwDirection;

    public void ThrowLasso(Vector3 _throwDirection)
    {
        throwDirection = _throwDirection.normalized;

        GameObject newLasso = Instantiate(lassoPrefab, transform.position, Quaternion.identity, transform);

        LassoController lassoController = newLasso.GetComponent<LassoController>();

        lassoController.LaunchLasso(throwDirection, throwForce);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, throwDirection * 1f);
    }
}