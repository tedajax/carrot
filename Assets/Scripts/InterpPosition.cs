using UnityEngine;

public class InterpPosition : MonoBehaviour
{
    public Vector3 direction;
    public float scalar;
    public AnimationCurve curve;
    public float duration;

    private Vector3 basePosition;
    private float startTime;

    void Awake()
    {
        basePosition = transform.localPosition;
        startTime = Time.time;
    }

    void Update()
    {
        float time = Mathf.Clamp01((Time.time - startTime) / duration);
        float curveValue = curve.Evaluate(time);
        transform.localPosition = basePosition + direction.normalized * curveValue * scalar;
    }
}