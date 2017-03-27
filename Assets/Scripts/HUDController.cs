using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private GameObject scoreTextPrefab;
    public Canvas canvas;

    void Awake()
    {
        scoreTextPrefab = GameManagerLocator.GameManager.GetPrefab("score_text");
        canvas = FindObjectOfType<Canvas>();
    }

    public void CreateScoreText(int score, Vector3 worldPosition)
    {
        var scoreObject = Instantiate(scoreTextPrefab);
        var scoreNumber = scoreObject.GetComponent<ScoreNumberController>();
        scoreNumber.transform.SetParent(canvas.transform, false);
        scoreNumber.Init(score, worldPosition);
    }
}