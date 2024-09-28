using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Transform buttonsContent;

    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        GameManager.Instance.LoadGame(1);
        buttonsContent.gameObject.SetActive(false);
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
