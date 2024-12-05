using UnityEngine;
using System.Collections;
using com.cyborgAssets.inspectorButtonPro;


public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Длительность дрожания
    public float shakeMagnitude = 0.5f; // Сила дрожания
    public float dampingSpeed = 1.0f; // Скорость затухания дрожания

    private Vector3 initialPosition;

    void Start()
    {
        
    }
    [ProButton]
    public void TriggerShake()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            initialPosition = transform.localPosition;
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(initialPosition.x + x, initialPosition.y + y, initialPosition.z);

            elapsed += Time.deltaTime * dampingSpeed;

            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}