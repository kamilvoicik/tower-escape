using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private Config config;
    private ScoreLivesCounter counter;

    private void Awake()
    {
        config = GetComponent<Config>();
        counter = GetComponent<ScoreLivesCounter>();

        int numGameSession = FindObjectsOfType<GameSession>().Length;
        if(numGameSession > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if(counter.currentLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void LevelEnd()
    {
        SceneManager.LoadScene(Consts.WIN_SCREEN);
    }

    private void ResetGameSession()
    {
        SceneManager.LoadScene(Consts.LOSE_SCREEN);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        counter.SubstractLives();
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
