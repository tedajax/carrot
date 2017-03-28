using UnityEngine;

public class InterpScale : MonoBehaviour
{
    public Vector3 axis;
    public float scalar;
    public AnimationCurve curve;
    public float duration;

    private Vector3 baseScale;
    private float startTime;

    void Awake()
    {
        baseScale = transform.localScale;
        startTime = Time.time;
    }

    void Update()
    {
        float time = Mathf.Clamp01((Time.time - startTime) / duration);
        float curveValue = curve.Evaluate(time);
        transform.localScale = baseScale + axis * curveValue * scalar;
    }
}