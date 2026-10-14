using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour
{
    bool isGameOver;

    void OnEnable()  => GameEvents.OnGameOver += HandleGameOver;
    void OnDisable() => GameEvents.OnGameOver -= HandleGameOver;

    void HandleGameOver() => isGameOver = true;

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}