using UnityEngine;
using Yarn.Unity;
public class TextController : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public string nodeToStart;

    void Update()
    {

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2))
        {

            if (hit.collider.CompareTag("Player"))
            {

                if (Input.GetKeyDown(KeyCode.E)){
                    dialogueRunner.StartDialogue(nodeToStart);
                }
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
    }

    public void ShowKey(GameObject key)
    {
        key.SetActive(true);
    }
    public void DesactiveCanvas(GameObject canvas)
    {
        canvas.SetActive(false);
    }
}



