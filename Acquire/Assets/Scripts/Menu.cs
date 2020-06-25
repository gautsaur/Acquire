using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace Acquire {
    public class Menu : MonoBehaviour {
        public void PreMenuToMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }

        public void PreMenuToOptions() {
            SceneManager.LoadScene("Options");
        }

        public void OptionsToPreMenu() {
            SceneManager.LoadScene("PreMenu");
        }
        public void PlayerDetailToMainGame() {
            SceneManager.LoadScene("Main");
        }

        public void MainMenuToPlayerDetail() {
            SceneManager.LoadScene("PlayerDetails");
        }

        public void ExitApplication() {
            Application.Quit();
        }

        public void SetFullScreen(bool isFullScreen) {
            Screen.fullScreen = isFullScreen;
            if (Screen.fullScreen) {
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;

            } else {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
        }
    }
}