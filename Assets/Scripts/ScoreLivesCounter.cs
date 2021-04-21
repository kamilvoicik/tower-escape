using UnityEngine;
using UnityEngine.UI;

public class ScoreLivesCounter : MonoBehaviour
{
    [SerializeField] Text livesText;
    [SerializeField] Text coinsText;

    public Config config;

    private int coins;
    public int currentLives;

    void Start()
    {
        currentLives = config.playerLives;
        livesText.text = currentLives.ToString();
        coinsText.text = coins.ToString();
    }

    public void AddToScore()
    {
        coins += config.pointsForCoin;
        coinsText.text = coins.ToString();
    }

    public void SubstractLives()
    {
        currentLives--;
        livesText.text = currentLives.ToString();
    }
}
