using Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] Button restartButton;

    private void OnEnable()
    {
        restartButton.onClick.AddListener(Restart);
        GameEvents.OnLoadLevel += SetLevelText;
        GameEvents.OnGameFinish += SetEnd;
    }

    private void OnDisable()
    {
        restartButton.onClick.RemoveListener(Restart);
        GameEvents.OnLoadLevel -= SetLevelText;
        GameEvents.OnGameFinish -= SetEnd;
    }

    private void SetLevelText(int level)
    {
        if (level == 1)
        {
            StartCoroutine(StartInfo());
        }

        levelText.text = $"Level {level}";
        restartButton.gameObject.SetActive(false);
    }

    IEnumerator StartInfo()
    {
        infoText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        infoText.gameObject.SetActive(false);
    }

    private void SetEnd()
    {
        levelText.text = $"Completed!";
        restartButton.gameObject.SetActive(true);
    }

    void Restart()
    {
        GameManager.Instance.LoadGame(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
