using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeHealthItem : MonoBehaviour
{
    public static event Action<float, int> OnSizeHealthCollected;
    public float sizeMultiplier = 1.5f; // Character size increased by 1.5 times.
    public int healthIncrease = 10; // Increases blood by 10 units.
    [SerializeField] AudioSource pickSound;

    public void Collect()
    {
        Debug.Log("Size & Health Item Collected!");
        OnSizeHealthCollected?.Invoke(sizeMultiplier, healthIncrease);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (pickSound != null) pickSound.Play();
            Collect();
        }
    }
}
