using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Acquire {
    public class StockPurchaser : MonoBehaviour {

        GameObject buyStocksUI;
        GameManager gameManager;
        GameObject sellStocksUI;

        void Start() {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();      
        }

        void Update() {
            buyStocksUI = GameObject.Find("BuyStocksUI");
            sellStocksUI = GameObject.Find("SellStocksUI");
        }

        /// <summary>
        /// Method  for subtraction buttons in buyStocksUI
        /// </summary>
        public void SellStock() {
            HotelChain arcadia = null;
            HotelChain fanfare = null;
            HotelChain global = null;
            HotelChain luster = null;
            HotelChain regal = null;
            HotelChain nobel = null;
            HotelChain teton = null;

            // get references to all active hotelchains in the game
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    Cell cell = gameManager.board.cells[row, column].GetComponent<Cell>();
                    if (cell.hotelTile != null && cell.hotelTile.GetComponent<HotelTile>().HotelChain != null) {
                        HotelChain hc = cell.hotelTile.GetComponent<HotelTile>().HotelChain;

                        if (hc.Equals(new HotelChain("Arcadia", Color.blue))) {
                            arcadia = hc;
                        } else if (hc.Equals(new HotelChain("Fanfare", Color.green))) {
                            fanfare = hc;
                        } else if (hc.Equals(new HotelChain("Global", Color.gray))) {
                            global = hc;
                        } else if (hc.Equals(new HotelChain("Luster", Color.red))) {
                            luster = hc;
                        } else if (hc.Equals(new HotelChain("Regal", Color.magenta))) {
                            regal = hc;
                        } else if (hc.Equals(new HotelChain("Teton", Color.yellow))) {
                            teton = hc;
                        } else if (hc.Equals(new HotelChain("Nobel", Color.cyan))) {
                            nobel = hc;
                        }
                    }
                }
            }

            if (gameManager.boughtStocks.Count > 0) {
                if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractArcadiaStock" && arcadia != null && gameManager.boughtStocks.Contains(arcadia.availableStocks[0])) {
                    gameManager.playerTurn.Sell(arcadia, 1);
                    gameManager.boughtStocks.Remove(arcadia.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy Stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractFanFareStock" && fanfare != null && gameManager.boughtStocks.Contains(fanfare.availableStocks[0])) {
                    gameManager.playerTurn.Sell(fanfare, 1);
                    gameManager.boughtStocks.Remove(fanfare.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractGlobalStock" && global != null && gameManager.boughtStocks.Contains(global.availableStocks[0])) {
                    gameManager.playerTurn.Sell(global, 1);
                    gameManager.boughtStocks.Remove(global.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractLusterStock" && luster != null && gameManager.boughtStocks.Contains(luster.availableStocks[0])) {
                    gameManager.playerTurn.Sell(luster, 1);
                    gameManager.boughtStocks.Remove(luster.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractRegalStock" && regal != null && gameManager.boughtStocks.Contains(regal.availableStocks[0])) {
                    gameManager.playerTurn.Sell(regal, 1);
                    gameManager.boughtStocks.Remove(regal.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractNobelStock" && nobel != null && gameManager.boughtStocks.Contains(nobel.availableStocks[0])) {
                    gameManager.playerTurn.Sell(nobel, 1);
                    gameManager.boughtStocks.Remove(nobel.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnSubtractTetonStock" && teton != null && gameManager.boughtStocks.Contains(teton.availableStocks[0])) {
                    gameManager.playerTurn.Sell(teton, 1);
                    gameManager.boughtStocks.Remove(teton.availableStocks[0]);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                }
            }

            gameManager.DisplayChainInfo();
            GameObject.Find("lblPlayerBalance").GetComponent<Text>().text = "$" + gameManager.playerTurn.balance;
        }

        /// <summary>
        /// Method for adding buttons in buyStocksUi
        /// </summary>
        public void BuyStock() {
            HotelChain arcadia = null;
            HotelChain fanfare = null;
            HotelChain global = null;
            HotelChain luster = null;
            HotelChain regal = null;
            HotelChain nobel = null;
            HotelChain teton = null;

            // get references to all active hotelchains in the game
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    Cell cell = gameManager.board.cells[row, column].GetComponent<Cell>();
                    if (cell.hotelTile != null && cell.hotelTile.GetComponent<HotelTile>().HotelChain != null) {
                        HotelChain hc = cell.hotelTile.GetComponent<HotelTile>().HotelChain;

                        if (hc.Equals(new HotelChain("Arcadia", Color.blue))) {
                            arcadia = hc;
                        } else if (hc.Equals(new HotelChain("Fanfare", Color.green))) {
                            fanfare = hc;
                        } else if (hc.Equals(new HotelChain("Global", Color.gray))) {
                            global = hc;
                        } else if (hc.Equals(new HotelChain("Luster", Color.red))) {
                            luster = hc;
                        } else if (hc.Equals(new HotelChain("Regal", Color.magenta))) {
                            regal = hc;
                        } else if (hc.Equals(new HotelChain("Teton", Color.yellow))) {
                            teton = hc;
                        } else if (hc.Equals(new HotelChain("Nobel", Color.cyan))) {
                            nobel = hc;
                        }
                    }
                }
            }

            if (gameManager.boughtStocks.Count < 3) {
                if (EventSystem.current.currentSelectedGameObject.name == "btnAddArcadiaStock" && arcadia != null) {
                    gameManager.boughtStocks.Add(arcadia.availableStocks[0]);
                    gameManager.playerTurn.Buy(arcadia, 1);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy Stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnAddFanFareStock" && fanfare != null) {
                    gameManager.boughtStocks.Add(fanfare.availableStocks[0]);
                    gameManager.playerTurn.Buy(fanfare, 1);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnAddGlobalStock" && global != null) {
                    gameManager. boughtStocks.Add(global.availableStocks[0]);
                    gameManager. playerTurn.Buy(global, 1);
                    gameManager. buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnAddLusterStock" && luster != null) {
                    gameManager.boughtStocks.Add(luster.availableStocks[0]);
                    gameManager.playerTurn.Buy(luster, 1);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnAddRegalStock" && regal != null) {
                    gameManager.boughtStocks.Add(regal.availableStocks[0]);
                    gameManager.playerTurn.Buy(regal, 1);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnAddNobelStock" && nobel != null) {
                    gameManager.boughtStocks.Add(nobel.availableStocks[0]);
                    gameManager.playerTurn.Buy(nobel, 1);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else if (EventSystem.current.currentSelectedGameObject.name == "btnAddTetonStock" && teton != null) {
                    gameManager.boughtStocks.Add(teton.availableStocks[0]);
                    gameManager.playerTurn.Buy(teton, 1);
                    gameManager.buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Buy stocks";
                } else {
                    buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "That chain isn't currently on the board";
                }
            } else if (gameManager.boughtStocks.Count >= 3) {
                buyStocksUI.transform.Find("txtBuyStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "You can only buy 3 stocks per turn";
            }

            gameManager.DisplayChainInfo();
            GameObject.Find("lblPlayerBalance").GetComponent<Text>().text = "$" + gameManager.playerTurn.balance;
        }

        /// <summary>
        /// Method for closing buy chain UI and advancing game
        /// </summary>
        public void DoneBuying() {

            buyStocksUI.SetActive(false);
            gameManager.ChangePlayerTurn();
            gameManager.DisplayChainInfo();
            gameManager.MarkPlayableCells();
            GameObject.Find("lblPlayerIndicator").GetComponent<UnityEngine.UI.Text>().text = gameManager.playerTurn + ", it's your turn!";
            GameObject.Find("lblPlayerBalance").GetComponent<UnityEngine.UI.Text>().text = "$" + gameManager.playerTurn.balance;
            gameManager.boughtStocks.Clear();
        }

        /// <summary>
        /// Method for closing sell chain UI
        /// </summary>
        public void DoneSelling() {
            sellStocksUI.SetActive(false);

        }
    }
}

