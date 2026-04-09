using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject projectil;
    public Transform shootPoint;

    public float force = 20f;
    public float fireRate = 2f;



    void Start()
    {
        InvokeRepeating("Shoot", 1f, fireRate);
    }

    public void Shoot()
    {
        if (projectil == null || shootPoint == null) return;

        GameObject projectile = Instantiate(projectil, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if(rb != null)
        {
            rb.useGravity = false;

            rb.AddForce(transform.right * force, ForceMode.Impulse);
            Destroy(projectile, 5f);
        }
        else
        {
            Debug.LogWarning("El Prefab no tine Rigibody");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 directionForce = transform.right * force * 0.1f;

        Gizmos.DrawRay(shootPoint.position, directionForce);
        Gizmos.DrawSphere(shootPoint.position + directionForce, 0.2f);
    }


}
