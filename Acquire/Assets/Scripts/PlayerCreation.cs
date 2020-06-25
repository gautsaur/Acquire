using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Acquire
{

    public class PlayerCreation : MonoBehaviour
    {
        List<Player> playerList = new List<Player>();
        public InputField theName;
        public Text theDisplay;
        int playerCount = 0;
        public GameObject addButton;
        public GameObject maxMessage;
        public GameObject playerLists;
        public GameObject launchButton;
        GameManager gameManager;

      

        public void DisplayName()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            playerCount++;
            theDisplay.text += theName.text + "    - PLAYER " + playerCount + "\r\n";
            gameManager.numPlayers = playerCount;
            gameManager.playerNames.Add(theName.text);

            Debug.Log(playerCount);

            if (playerCount >= 2)
            {
                launchButton.SetActive(true);
            }
            if (playerCount >= 5)
            {
                addButton.SetActive(false);
                maxMessage.SetActive(true);
                playerLists.SetActive(false);
            }
          
        }

    }
}
