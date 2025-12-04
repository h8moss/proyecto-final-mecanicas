using UnityEngine;
using UnityEngine.SceneManagement;

public class FailMenuController : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0); // Regresa a la escena 0
    }

    public void QuitGame()
    {
        Debug.Log("Juego Cerrado");
        Application.Quit(); // Sale del juego
    }
}
