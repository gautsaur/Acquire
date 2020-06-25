using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Acquire {
    public class GameManager : MonoBehaviour {
        private static GameManager instance;
        public Board board;
        public int numPlayers;
        public LinkedList<Player> players = new LinkedList<Player>();
        public List<HotelChain> hotelChains = new List<HotelChain>();
        public List<GameObject> availableTiles = new List<GameObject>();
        public GameObject hotelTilePrefab;
        public GameObject chainSelectionUI;
        public GameObject buyStocksUI;
        public GameObject sellStocksUI;
        public GameObject hotelInformationLabels;
        public Player playerTurn;
        public List<Stock> boughtStocks;
        public Cell selectedCell;
        public List<string> playerNames;

        /// <summary>
        /// Set up the GameManager at the start of the game
        /// </summary>
        void Awake() {
            if (instance == null) {
                instance = this;
            }


            DontDestroyOnLoad(transform.gameObject);
        }

        public void SetUpGame() {
            
            boughtStocks = new List<Stock>();
            chainSelectionUI = GameObject.Find("ChainSelectionUI");
            chainSelectionUI.SetActive(false);
            buyStocksUI = GameObject.Find("BuyStocksUI");
            buyStocksUI.SetActive(false);
            sellStocksUI = GameObject.Find("SellStocksUI");
            sellStocksUI.SetActive(false);
            hotelInformationLabels = GameObject.Find("HotelInformationLabels");
            GenerateHotelTiles();
            AddPlayers();
            int i = 0;
            foreach (Player player in players) {
                player.name = playerNames[i];
                i++;
            }
            hotelChains.Add(new HotelChain("Arcadia", Color.blue));
            hotelChains.Add(new HotelChain("Fanfare", Color.green));
            hotelChains.Add(new HotelChain("Global", Color.gray));
            hotelChains.Add(new HotelChain("Luster", Color.red));
            hotelChains.Add(new HotelChain("Nobel", Color.cyan));
            hotelChains.Add(new HotelChain("Regal", Color.magenta));
            hotelChains.Add(new HotelChain("Teton", Color.yellow));
            board = GameObject.Find("GameBoard").GetComponent<Board>();
            GameObject.Find("lblPlayerIndicator").GetComponent<UnityEngine.UI.Text>().text = playerTurn + ", place a tile!";
            MarkPlayableCells();
        }

        /// <summary>
        /// Checks every cell in the board and marks them as selectable if the current player has a tile to place
        /// </summary>
        public void MarkPlayableCells() {
            Player currentPlayer = playerTurn;
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    GameObject hotelTile = Instantiate(hotelTilePrefab) as GameObject;
                    hotelTile.GetComponent<HotelTile>().row = row;
                    hotelTile.GetComponent<HotelTile>().column = column;
                    hotelTile.transform.localPosition = new Vector3(-100, -100, -100);
                    hotelTile.name = "hotelTile " + row + "," + column;
                    HotelTile ht = hotelTile.GetComponent<HotelTile>();
                    if (currentPlayer.hotelTiles.Contains(ht)) {
                        board.cells[row, column].GetComponent<Cell>().selectable = true;
                    }
                    Destroy(hotelTile);
                }
            }


        }

        /// <summary>
        /// Set the current player turn to the next player in the linkedlist of players
        /// </summary>
        public void ChangePlayerTurn() {
            LinkedListNode<Player> node = players.Find(playerTurn);
            if (node.Next != null) {
                playerTurn = node.Next.Value;
            } else {
                playerTurn = players.First.Value;
            }
        }

        // Add players to the game
        public void AddPlayers() {
            for (int i = 1; i <= numPlayers; i++) {
                players.AddLast(new Player("Player " + i));
            }

            playerTurn = players.First.Value;
        }

        /// <summary>
        /// Instantiate all the hotel tiles to be used in the game, add them to a list, then shuffle the list
        /// Players then draw from this list to get tiles to play
        /// </summary>
        public void GenerateHotelTiles() {
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    GameObject hotelTile = Instantiate(hotelTilePrefab) as GameObject;
                    hotelTile.GetComponent<HotelTile>().row = row;
                    hotelTile.GetComponent<HotelTile>().column = column;
                    hotelTile.transform.parent = this.transform;
                    hotelTile.transform.localPosition = new Vector3(0, 0, 0);
                    hotelTile.name = "hotelTile " + row + "," + column;
                    availableTiles.Add(hotelTile);
                }
            }
            // Shuffle tiles
            availableTiles = availableTiles.OrderBy(x => Random.value).ToList();
        }

        /// <summary>
        /// Loops through cells in game board and adds tiles to corresponsing hotelchains' list of hotel tiles
        /// </summary>
        public void UpdateChainCounts() {
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    Cell cell = board.cells[row, column].GetComponent<Cell>();
                    if (cell.hotelTile != null && cell.hotelTile.GetComponent<HotelTile>().HotelChain != null && !cell.hotelTile.GetComponent<HotelTile>().HotelChain.hotelTiles.Contains(cell.hotelTile)) {
                        cell.hotelTile.GetComponent<HotelTile>().HotelChain.hotelTiles.Add(cell.hotelTile);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the size labels for each hotel chain
        /// </summary>
        public void DisplayChainInfo() {
            int arcadiaSize = 0;
            int arcadiaStocks = 25;
            int fanfareSize = 0;
            int fanfareStocks = 25;
            int globalSize = 0;
            int globalStocks = 25;
            int lusterSize = 0;
            int lusterStocks = 25;
            int regalSize = 0;
            int regalStocks = 25;
            int tetonSize = 0;
            int tetonStocks = 25;
            int nobelSize = 0;
            int nobelStocks = 25;

            int arcadiaStockPrice = 300;
            int fanfareStockPrice = 300;
            int globalStockPrice = 300;
            int lusterStockPrice = 300;
            int regalStockPrice = 300;
            int tetonStockPrice = 300;
            int nobelStockPrice = 300;

            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    Cell cell = board.cells[row, column].GetComponent<Cell>();
                    if (cell.hotelTile != null && cell.hotelTile.GetComponent<HotelTile>().HotelChain != null) {
                        HotelChain hc = cell.hotelTile.GetComponent<HotelTile>().HotelChain;

                        if (hc.Equals(new HotelChain("Arcadia", Color.blue))) {
                            arcadiaSize = hc.hotelTiles.Count;
                            arcadiaStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                arcadiaStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Arcadia").Price;
                        } else if (hc.Equals(new HotelChain("Fanfare", Color.green))) {
                            fanfareSize = hc.hotelTiles.Count;
                            fanfareStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                fanfareStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Fanfare").Price;
                        } else if (hc.Equals(new HotelChain("Global", Color.gray))) {
                            globalSize = hc.hotelTiles.Count;
                            globalStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                globalStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Global").Price;
                        } else if (hc.Equals(new HotelChain("Luster", Color.red))) {
                            lusterSize = hc.hotelTiles.Count;
                            lusterStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                lusterStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Luster").Price;
                        } else if (hc.Equals(new HotelChain("Regal", Color.magenta))) {
                            regalSize = hc.hotelTiles.Count;
                            regalStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                regalStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Regal").Price;
                        } else if (hc.Equals(new HotelChain("Teton", Color.yellow))) {
                            tetonSize = hc.hotelTiles.Count;
                            tetonStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                tetonStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Teton").Price;
                        } else if (hc.Equals(new HotelChain("Nobel", Color.cyan))) {
                            nobelSize = hc.hotelTiles.Count;
                            tetonStocks = hc.availableStocks.Count;
                            if (hc.NumberOfStocksAvailable() != 0)
                                nobelStockPrice = hc.availableStocks.Find(x => x.OwningChain.name == "Nobel").Price;
                        }
                    }
                }
            }

            foreach (Player player in players) {
                foreach (Stock stock in player.ownedStocks) {
                    if (stock.OwningChain.Equals(new HotelChain("Arcadia", Color.blue))) {
                        arcadiaStocks = stock.OwningChain.availableStocks.Count;
                    } else if (stock.OwningChain.Equals(new HotelChain("Fanfare", Color.green))) {
                        fanfareStocks = stock.OwningChain.availableStocks.Count;
                    } else if (stock.OwningChain.Equals(new HotelChain("Global", Color.gray))) {
                        globalStocks = stock.OwningChain.availableStocks.Count;
                    } else if (stock.OwningChain.Equals(new HotelChain("Luster", Color.red))) {
                        lusterStocks = stock.OwningChain.availableStocks.Count;
                    } else if (stock.OwningChain.Equals(new HotelChain("Regal", Color.magenta))) {
                        regalStocks = stock.OwningChain.availableStocks.Count;
                    } else if (stock.OwningChain.Equals(new HotelChain("Teton", Color.yellow))) {
                        tetonStocks = stock.OwningChain.availableStocks.Count;
                    } else if (stock.OwningChain.Equals(new HotelChain("Nobel", Color.cyan))) {
                        nobelStocks = stock.OwningChain.availableStocks.Count;
                    }
                }
            }

            int arcadiaOwnedStocks = 0;
            int fanfareOwnedStocks = 0;
            int globalOwnedStocks = 0;
            int lusterOwnedStocks = 0;
            int regalOwnedStocks = 0;
            int tetonOwnedStocks = 0;
            int nobelOwnedStocks = 0;

            foreach (Stock stock in playerTurn.ownedStocks) {
                if (stock.OwningChain.Equals(new HotelChain("Arcadia", Color.blue))) {
                    arcadiaOwnedStocks++;
                } else if (stock.OwningChain.Equals(new HotelChain("Fanfare", Color.green))) {
                    fanfareOwnedStocks++;
                } else if (stock.OwningChain.Equals(new HotelChain("Global", Color.gray))) {
                    globalOwnedStocks++;
                } else if (stock.OwningChain.Equals(new HotelChain("Luster", Color.red))) {
                    lusterOwnedStocks++;
                } else if (stock.OwningChain.Equals(new HotelChain("Regal", Color.magenta))) {
                    regalOwnedStocks++;
                } else if (stock.OwningChain.Equals(new HotelChain("Teton", Color.yellow))) {
                    tetonOwnedStocks++;
                } else if (stock.OwningChain.Equals(new HotelChain("Nobel", Color.cyan))) {
                    nobelOwnedStocks++;
                }
            }

            foreach (Player player in players) {
                foreach (Stock stock in player.ownedStocks) {
                    if (stock.OwningChain.Equals(new HotelChain("Arcadia", Color.blue))) {
                        arcadiaStockPrice = stock.Price;
                    } else if (stock.OwningChain.Equals(new HotelChain("Fanfare", Color.green))) {
                        fanfareStockPrice = stock.Price;
                    } else if (stock.OwningChain.Equals(new HotelChain("Global", Color.gray))) {
                        globalStockPrice = stock.Price;
                    } else if (stock.OwningChain.Equals(new HotelChain("Luster", Color.red))) {
                        lusterStockPrice = stock.Price;
                    } else if (stock.OwningChain.Equals(new HotelChain("Regal", Color.magenta))) {
                        regalStockPrice = stock.Price;
                    } else if (stock.OwningChain.Equals(new HotelChain("Teton", Color.yellow))) {
                        tetonStockPrice = stock.Price;
                    } else if (stock.OwningChain.Equals(new HotelChain("Nobel", Color.cyan))) {
                        nobelStockPrice = stock.Price;
                    }
                }
            }
            
            hotelInformationLabels.transform.Find("lblArcadiaSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = arcadiaSize.ToString();
            hotelInformationLabels.transform.Find("lblArcadiaStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = arcadiaStocks.ToString();
            hotelInformationLabels.transform.Find("lblFanFareSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = fanfareSize.ToString();
            hotelInformationLabels.transform.Find("lblFanFareStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = fanfareStocks.ToString();
            hotelInformationLabels.transform.Find("lblGlobalSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = globalSize.ToString();
            hotelInformationLabels.transform.Find("lblGlobalStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = globalStocks.ToString();
            hotelInformationLabels.transform.Find("lblLusterSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = lusterSize.ToString();
            hotelInformationLabels.transform.Find("lblLusterStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = lusterStocks.ToString();
            hotelInformationLabels.transform.Find("lblRegalSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = regalSize.ToString();
            hotelInformationLabels.transform.Find("lblRegalStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = regalStocks.ToString();
            hotelInformationLabels.transform.Find("lblTetonSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = tetonSize.ToString();
            hotelInformationLabels.transform.Find("lblTetonStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = tetonStocks.ToString();
            hotelInformationLabels.transform.Find("lblNobelSize").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = nobelSize.ToString();
            hotelInformationLabels.transform.Find("lblNobelStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = nobelStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = playerTurn.name + "\r\nOwned\r\nStocks:";

            hotelInformationLabels.transform.Find("lblOwnedArcadiaStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = arcadiaOwnedStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedFanFareStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = fanfareOwnedStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedGlobalStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = globalOwnedStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedLusterStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = lusterOwnedStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedRegalStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = regalOwnedStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedTetonStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = tetonOwnedStocks.ToString();
            hotelInformationLabels.transform.Find("lblOwnedNobelStocks").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = nobelOwnedStocks.ToString();

            hotelInformationLabels.transform.Find("lblArcadiaStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + arcadiaStockPrice;
            hotelInformationLabels.transform.Find("lblFanFareStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + fanfareStockPrice;
            hotelInformationLabels.transform.Find("lblGlobalStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + globalStockPrice;
            hotelInformationLabels.transform.Find("lblLusterStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + lusterStockPrice;
            hotelInformationLabels.transform.Find("lblRegalStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + regalStockPrice;
            hotelInformationLabels.transform.Find("lblNobelStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + nobelStockPrice;
            hotelInformationLabels.transform.Find("lblTetonStockPrice").gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = "$" + tetonStockPrice;
        }

        public int NumPlayers {
            get {
                return numPlayers;
            }
            set {
                if (value > 1) {
                    numPlayers = value;
                } else {
                    numPlayers = 2;
                }
            }
        }

        public static GameManager Instance {
            get {
                return instance;
            }
        }
    }
}

