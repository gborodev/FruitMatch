using Events;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    FadeEffect fade;

    [SerializeField] GameDatabase database;

    public int CurrentLevel { get; private set; }

    private void Start()
    {
        fade = GetComponentInChildren<FadeEffect>();

        GameEvents.OnGameOver += () => GameOver();
        GameEvents.OnNextGame += () => NextGame();
    }

    private void NextGame()
    {
        LoadGame(++CurrentLevel);
    }

    AsyncOperation load;
    public async void GameOver()
    {
        fade.StartFade();
        while (fade.IsFading)
        {
            await Task.Delay(100);
        }

        load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        while (!load.isDone)
        {
            await Task.Yield();
        }

        load = null;
        fade.EndFade();

        int levelIndex = CurrentLevel - 1;
        Level level = database.GetLevelsFromDatabase()[levelIndex];
        LevelManager.Instance.SetLevel(level);

        PlayerPrefs.SetInt("Level", CurrentLevel);
    }

    public async void LoadGame(int level)
    {
        Level[] levels = database.GetLevelsFromDatabase();
        int levelIndex = level - 1;

        if (levelIndex < 0 || levelIndex > levels.Length - 1) return;

        CurrentLevel = level;
        PlayerPrefs.SetInt("Level", CurrentLevel);

        fade.StartFade();
        while (fade.IsFading)
        {
            await Task.Delay(100);
        }

        load = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!load.isDone)
        {
            await Task.Yield();
        }

        load = null;
        fade.EndFade();

        Level loadLevel = levels[levelIndex];
        LevelManager.Instance.SetLevel(loadLevel);
    }
}
