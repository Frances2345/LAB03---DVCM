using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Vector3 direction = Vector3.forward;
    public float force = 20f;
    public float fireRate = 2f;



    void Start()
    {
        InvokeRepeating("Shoot", 1f, fireRate);
    }

    public void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if(rb != null)
        {
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawRay(shootPoint.position, direction.normalized * (force * 0.2f));
        Gizmos.DrawSphere(shootPoint.position + direction.normalized * (force * 0.2f), 0.2f);
    }


}
