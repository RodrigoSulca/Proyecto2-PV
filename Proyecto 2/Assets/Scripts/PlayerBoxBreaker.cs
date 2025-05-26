using UnityEngine;
using System.Collections;

public class PlayerBoxBreaker : MonoBehaviour
{
    public float interactionRange = 13f;
    public KeyCode interactKey = KeyCode.E;

    public Animator animator;
    public GameObject crowbar;
    public float animationDelay = 0.6f;

    private GameObject currentTarget;


    void Start()
    {
        animator = GameObject.Find("Player").GetComponent<Animator>();
        if (crowbar) crowbar.SetActive(false);

    }

    void Update()
    {
        DetectBox();

        if (currentTarget != null && (Input.GetKeyDown(interactKey) || Input.GetMouseButtonDown(0)))
        {
            StartCoroutine(BreakSequence(currentTarget));
        }
    }

    void DetectBox()
    {
        Camera cam = Camera.main; 
        bool useCameraRay = false;
        Ray ray = useCameraRay
            ? Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0))
            : new Ray(transform.position + Vector3.up * 1f, transform.forward);

        //  línea roja para ver hacia dónde apunta
        Debug.DrawRay(ray.origin, ray.direction * interactionRange, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            Outline outline = hit.collider.GetComponentInParent<Outline>();
            if (outline != null)
            {
                if (currentTarget != hit.collider.gameObject)
                {
                    DisableOutline();
                    currentTarget = hit.collider.gameObject;
                    EnableOutline(currentTarget);
                }
                return;
            }
        }

        DisableOutline();
    }


    void EnableOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    void DisableOutline()
    {
        if (currentTarget != null)
        {
            Outline outline = currentTarget.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
            currentTarget = null;
        }
    }

    IEnumerator BreakSequence(GameObject target)
    {
        // Activar la crowbar
        crowbar.SetActive(true);

        Debug.Log("ACTIVANDO TRIGGER...");
        animator.SetTrigger("openBox");

        // Esperar a que la animación llegue al momento del golpe
        yield return new WaitForSeconds(animationDelay); // ej. 0.6 segundos

        // Luego destruir la caja
        var crate = target.GetComponent<ArionDigital.CrashCrate>();
        if (crate != null)
        {
            crate.Test(); // Desactiva wholeCrate, activa fracturedCrate
        }

        yield return new WaitForSeconds(0.3f); // tiempo de remate final



        // Ocultar crowbar
        crowbar.SetActive(false);

        // Quitar contorno
        DisableOutline();
    }

}
