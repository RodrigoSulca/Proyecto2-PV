using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    public float fadeDuration = 2f;
    public float destroyDelay = 0.5f;

    private Renderer[] renderers;
    private float fadeTimer;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        fadeTimer = fadeDuration;
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        while (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            float alpha = fadeTimer / fadeDuration;

            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    if (mat.HasProperty("_Color"))
                    {
                        Color c = mat.color;
                        c.a = alpha;
                        mat.color = c;
                    }
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
