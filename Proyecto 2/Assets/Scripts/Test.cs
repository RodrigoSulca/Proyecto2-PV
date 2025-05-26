using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform target;            // El jugador o el punto de enfoque
    public Vector3 offset = new Vector3(0f, 2f, -4f); // Offset de la cámara respecto al target
    public float sensitivity = 3f;      // Sensibilidad del mouse
    public Transform cameraTransform;
    public float speed;
    private Animator animator;

    private float yaw = 0f; // rotación horizontal

    void Start(){
        animator = GetComponent<Animator>();
    } 
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;

        // Acumulamos el ángulo horizontal (yaw)
        yaw += mouseX;

        // Rotamos este objeto (el CameraHolder)
        cameraTransform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Mantenemos la cámara en offset relativo al target
        cameraTransform.position = target.position + cameraTransform.rotation * offset;
        Movement();
    }

    private void Movement(){
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Direcciones relativas a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Eliminamos el componente vertical
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Dirección final
        Vector3 direction = forward * v + right * h;

        // Movimiento
        transform.position += direction * speed * Time.deltaTime;
        if (direction.magnitude > 0.1f){
            // Rotación suave hacia la dirección del movimiento
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
            animator.SetBool("Walking",true);
        }else{
            animator.SetBool("Walking",false);
        }
    }
}
