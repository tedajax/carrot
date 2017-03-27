using UnityEngine;

public class GameManagerLocator
{
    private static GameManager gameManager = null;

    public static GameManager GameManager
    {
        get
        {
            if (gameManager == null) {
                var go = GameObject.FindGameObjectWithTag("GameController");
                if (go != null) {
                    gameManager = go.GetComponent<GameManager>();
                }
            }
            return gameManager;
        }
    }
}