using UnityEngine;
using UnityEngine.UI;

public class ScoreNumberController : MonoBehaviour
{
    public float duration;

    public AnimationCurve heightCurve;
    public float heightScalar;

    public AnimationCurve alphaCurve;

    private RectTransform rectTransform;
    private Text text;
    private float timeAlive = -1;
    private Vector3 basePosition;
    private Color baseColor;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<Text>();
    }

    public void Init(int score, Vector3 worldPosition)
    {
        text.text = score.ToString();
        basePosition = Camera.main.WorldToScreenPoint(worldPosition);
        rectTransform.position = basePosition;
        baseColor = text.color;
        timeAlive = 0;
    }

    void Update()
    {
        if (timeAlive < 0) {
            return;
        }

        timeAlive += Time.deltaTime;

        float ratio = timeAlive / duration;
        float height = heightCurve.Evaluate(ratio) * heightScalar;
        float alpha = alphaCurve.Evaluate(ratio);

        rectTransform.position = basePosition + Vector3.up * height;
        text.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

        if (timeAlive >= duration) {
            Destroy(gameObject);
        }
    }
}