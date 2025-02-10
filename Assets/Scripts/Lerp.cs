using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Lerp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Vector3 control;
    [SerializeField] private Vector3 control2;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    [SerializeField] private float lerpTime;
    [SerializeField] private float lerpAmount = 3;

    [Header("Manual")]
    [Range(0, 1)][SerializeField] private float manualLerpControl = 0;
    [SerializeField] private bool manualLerp = false;

    private float lerpedValue;

    private void Start()
    {
        start = transform.position;
        lerpTime = 0;
        lerpedValue = 0;
    }

    private void Update()
    {
        if (manualLerp)
        {
            // Manually control the lerp
            lerpedValue = manualLerpControl;
        }
        else
        {
            lerpTime += Time.deltaTime;
            lerpedValue = Mathf.Clamp01(lerpTime / lerpAmount);
        }

        // Choose which type of Bezier curve to use, for now defaulting to Quadratic
        Vector3 newPosition = QuadraticBezier(start, control, end, lerpedValue); // Quadratic by default
        transform.position = newPosition;
    }

    // Quadratic Bezier curve calculation
    private Vector3 QuadraticBezier(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        return Mathf.Pow(1 - t, 2) * start + (1 - t) * control * 2 + t * t * end;
    }

    // Cubic Bezier curve calculation
    private Vector3 CubicBezier(Vector3 start, Vector3 control1, Vector3 control2, Vector3 end, float t)
    {
        return Mathf.Pow(1 - t, 3) * start + (1 - t) * control1 * 3 + Mathf.Pow(1 - t, 2) * control2 * 3 +
               t * t * t * end;
    }
}
