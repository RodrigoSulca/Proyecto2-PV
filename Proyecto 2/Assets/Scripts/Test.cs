using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ);

        if (movement.magnitude > 0.1f)
        {
            transform.Translate(speed * Time.deltaTime * movement.normalized, Space.World);

            transform.LookAt(transform.position + movement);
        }

        animator.SetFloat("Speed", movement.magnitude);

        
    }
}
