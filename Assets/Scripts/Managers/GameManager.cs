using Events;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    FadeEffect fade;

    [SerializeField] GameDatabase database;

    public int CurrentLevel { get; private set; }
    public int CurrentSceneIndex { get; private set; }

    private void Start()
    {
        fade = GetComponentInChildren<FadeEffect>();

        GameEvents.OnGameOver += () => GameOver();
        GameEvents.OnNextGame += () => NextGame();
    }

    public void NextGame()
    {
        LoadGame(++CurrentLevel);
    }

    public void GameOver()
    {
        StartCoroutine(GameReload());
    }

    AsyncOperation load;
    private IEnumerator GameReload()
    {
        fade.StartFade();
        while (fade.IsFading)
        {
            yield return null;
        }

        load = SceneManager.LoadSceneAsync(1);

        while (!load.isDone)
        {
            yield return null;
        }

        load = null;
        fade.EndFade();

        int levelIndex = CurrentLevel - 1;
        Level level = database.GetLevelsFromDatabase()[levelIndex];
        LevelManager.Instance.SetLevel(level);

        PlayerPrefs.SetInt("Level", CurrentLevel);
    }

    public void LoadGame(int level)
    {
        StartCoroutine(Loading(level));
    }

    private IEnumerator Loading(int level)
    {
        Level[] levels = database.GetLevelsFromDatabase();
        int levelIndex = level - 1;

        if (levelIndex < 0 || levelIndex > levels.Length - 1)
        {
            GameEvents.OnGameFinish?.Invoke();

            yield break;
        }

        CurrentLevel = level;
        PlayerPrefs.SetInt("Level", CurrentLevel);

        fade.StartFade();
        while (fade.IsFading)
        {
            yield return null;
        }

        load = SceneManager.LoadSceneAsync(1);

        while (!load.isDone)
        {
            yield return null;
        }

        load = null;
        fade.EndFade();

        Level loadLevel = levels[levelIndex];
        LevelManager.Instance.SetLevel(loadLevel);
    }
}
