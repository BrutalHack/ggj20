using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.BrutalHack.GlobalGameJam20
{
    public class TitleManager : MonoBehaviour
    {
        public void StartGame()
        {
            Debug.Log("Start Game");
            SceneManager.LoadScene("Game");
        }

        public void ExitGame()
        {
            Debug.Log("Exit Game");
            Application.Quit();
        }
    }
}