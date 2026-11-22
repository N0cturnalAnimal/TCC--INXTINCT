
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransparency : MonoBehaviour
{
    [Header("Transparency")]
    [Range(0f, 1f)]
    public float transparentAlpha = 0.4f;

    public float fadeSpeed = 5f;

    private SpriteRenderer[] sprites;
    private Color[] originalColors;

    private HashSet<Collider2D> playerColliders = new HashSet<Collider2D>();

    private float targetAlpha = 1f;

    void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();

        originalColors = new Color[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            originalColors[i] = sprites[i].color;
        }
    }

    void Update()
    {
        float currentAlpha = sprites.Length > 0
            ? sprites[0].color.a / originalColors[0].a
            : 1f;

        currentAlpha = Mathf.MoveTowards(
            currentAlpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );

        for (int i = 0; i < sprites.Length; i++)
        {
            Color color = originalColors[i];
            color.a *= currentAlpha;

            sprites[i].color = color;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            playerColliders.Add(other);
            targetAlpha = transparentAlpha;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            playerColliders.Remove(other);

            if (playerColliders.Count == 0)
            {
                targetAlpha = 1f;
            }
        }
    }
}
