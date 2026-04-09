using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float speed = 3f;

    private CharacterController controller;
    private float verticalVelocity;
    private float gravity = -9.81f;

    private void Awake()
    {
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 moveDir = Vector3.zero;

        if (distance <= detectionRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            moveDir = direction * speed;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }

        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDir.y = verticalVelocity;
        controller.Move(moveDir * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
