using Events;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;

    private int currentLevel = 1;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            currentLevel = PlayerPrefs.GetInt("Level");
        }
        LevelManager.Instance.SetLevel(currentLevel);

        GameEvents.OnGameOver += GameOver;
        GameEvents.OnNextGame += NextLevel;
    }

    public void NextLevel()
    {
        LevelManager.Instance.SetLevel(++currentLevel);
        PlayerPrefs.SetInt("Level", currentLevel);
    }

    AsyncOperation load;
    public async void GameOver()
    {
        load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!load.isDone)
        {
            await Task.Yield();
        }

        load = null;

        LevelManager.Instance.SetLevel(currentLevel);
        PlayerPrefs.SetInt("Level", currentLevel);
    }
    public async void SetNewStart()
    {
        currentLevel = 1;
        PlayerPrefs.SetInt("Level", currentLevel);

        load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!load.isDone)
        {
            await Task.Yield();
        }

        load = null;
        LevelManager.Instance.SetLevel(currentLevel);
    }
}
