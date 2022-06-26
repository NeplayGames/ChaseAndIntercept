using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
   public void StartGame(int i){
        Time.timeScale = 1f;
       SceneManager.LoadScene(i);
   }
    bool pauseGame = false;

    public void Pause()
    {
        pauseGame = !pauseGame;
        Time.timeScale = pauseGame ? 0 : 1;

    }
    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   public void QuitGame(){
        Application.Quit();
    }
}