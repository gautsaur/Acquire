using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Acquire {

    public class Cell : MonoBehaviour {

        #region fields
        public AudioSource someSound;
        public Material selectableMaterial;
        public Material baseMaterial;
        public int row;
        public int column;
        public Board board;
        public GameObject hotelTile = null;
        public GameObject hotelTilePrefab;
        public GameManager gameManager;
        public bool selectable;
        public GameObject top;
        public GameObject right;
        public GameObject bottom;
        public GameObject left;
        #endregion

        #region Methods
        // Start is called before the first frame update
        void Start() {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            someSound.GetComponent<AudioSource>();
        }

        /// <summary>
        /// Changes cell material to appropriate textures depending on what cells are selectable by the current player
        /// </summary>
        void Update() {
            GameObject hotelTile = Instantiate(hotelTilePrefab) as GameObject;
            hotelTile.GetComponent<HotelTile>().row = row;
            hotelTile.GetComponent<HotelTile>().column = column;
            hotelTile.transform.localPosition = new Vector3(-100, -100, -100);
            hotelTile.name = "hotelTile " + row + "," + column;
            HotelTile ht = hotelTile.GetComponent<HotelTile>();
            if (selectable == true && gameManager.playerTurn.hotelTiles.Contains(ht)) {
                GetComponent<Renderer>().material = selectableMaterial;
            } else {
                GetComponent<Renderer>().material = baseMaterial;
                GetComponent<Cell>().selectable = false;
            }
            Destroy(hotelTile);
        }

        /// <summary>
        /// Creates a hotelTile gameobject and places it in this cell, handling merging logic
        /// </summary>
        /// <returns>Bool indicating if tile was placed successfully</returns>
        public bool AddHotelTile() {

            List<Cell> occupiedNeighbors = GetOccupiedNeighbors();
            bool tileIsPlaced = true;
            bool neighborsHaveChain = false;

            foreach (Cell cell in occupiedNeighbors) {
                if (cell.hotelTile.GetComponent<HotelTile>().HotelChain != null) {
                    neighborsHaveChain = true;
                }
            }

            if (occupiedNeighbors.Count == 0 || gameManager.hotelChains.Count != 0 || neighborsHaveChain == true) {

                GameObject hotelTile = Instantiate(hotelTilePrefab) as GameObject;
                hotelTile.transform.parent = this.transform;
                hotelTile.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
                hotelTile.GetComponent<HotelTile>().row = row;
                hotelTile.GetComponent<HotelTile>().column = column;
                this.hotelTile = hotelTile;

                if (occupiedNeighbors.Count != 0) {

                    HotelChain largestChain = GetLargestChain();


                     // Handling merging of chains
                    foreach (Cell cell in occupiedNeighbors) {
                        if (cell.hotelTile.GetComponent<HotelTile>().HotelChain != null && occupiedNeighbors.Count > 1 && cell.hotelTile.GetComponent<HotelTile>().HotelChain.hotelTiles.Count < 11) {

                            foreach (GameObject tile in cell.hotelTile.GetComponent<HotelTile>().HotelChain.hotelTiles.ToList()) {

                                if (!(gameManager.hotelChains.Contains(tile.GetComponent<HotelTile>().HotelChain)) && !tile.GetComponent<HotelTile>().HotelChain.Equals(largestChain)) {
                                    gameManager.hotelChains.Add(tile.GetComponent<HotelTile>().HotelChain);
                                }

                                if (tile.GetComponent<HotelTile>().HotelChain != null) {
                                    tile.GetComponent<HotelTile>().HotelChain.hotelTiles.Remove(tile);
                                }

                                tile.GetComponent<HotelTile>().HotelChain = largestChain;
                                tile.GetComponent<HotelTile>().SetMaterialColor(largestChain.color);
                            }


                            hotelTile.GetComponent<HotelTile>().HotelChain = largestChain;
                            SetTileColor(largestChain.color);
                        } else if (cell.hotelTile.GetComponent<HotelTile>().HotelChain != null && cell.hotelTile.GetComponent<HotelTile>().HotelChain.hotelTiles.Count >= 11 && !cell.hotelTile.GetComponent<HotelTile>().HotelChain.Equals(largestChain)) {
                            GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = "Cannot merge chains that are safe!";
                            Debug.Log("Cannot merge chains that are safe!");
                            Destroy(hotelTile);
                            tileIsPlaced = false;
                            return tileIsPlaced;

                        } else if (cell.hotelTile.GetComponent<HotelTile>().HotelChain == null && largestChain != null) {
                            cell.hotelTile.GetComponent<HotelTile>().HotelChain = largestChain;
                            cell.SetTileColor(largestChain.color);
                        }
                    }

                    foreach (Cell cell in occupiedNeighbors) {

                        // Create new hotel chain
                        if (cell.hotelTile.GetComponent<HotelTile>().HotelChain == null && hotelTile.GetComponent<HotelTile>().HotelChain == null && gameManager.hotelChains.Count != 0) {

                            CreateChain(cell, out largestChain);

                            // Add hoteltile in this cell adjacent hoteltiles to existing chain
                        } else {

                            hotelTile.GetComponent<HotelTile>().HotelChain = largestChain;
                            SetTileColor(largestChain.color);
                            cell.hotelTile.GetComponent<HotelTile>().HotelChain = hotelTile.GetComponent<HotelTile>().HotelChain;
                            cell.SetTileColor(hotelTile.GetComponent<HotelTile>().HotelChain.color);
                        }
                    }
                }
            } else {
                tileIsPlaced = false;
            }
            return tileIsPlaced;
        }

        /// <summary>
        /// Creates a new hotel chain
        /// </summary>
        /// <param name="cell">Cell containing tile to create chain with</param>
        /// <param name="largestChain">Out parameter to be used for adding additional tiles to new chain</param>
        public void CreateChain(Cell cell, out HotelChain largestChain) {

            cell.hotelTile.GetComponent<HotelTile>().HotelChain = gameManager.hotelChains[0];
            hotelTile.GetComponent<HotelTile>().HotelChain = gameManager.hotelChains[0];
            SetTileColor(gameManager.hotelChains[0].color);
            cell.SetTileColor(gameManager.hotelChains[0].color);
            largestChain = gameManager.hotelChains[0];
            Debug.Log(gameManager.playerTurn + " Created " + gameManager.hotelChains[0].name + " hotel chain!");
            GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = gameManager.playerTurn + " Created " + gameManager.hotelChains[0].name + " hotel chain!";
            gameManager.playerTurn.ownedStocks.Add(largestChain.availableStocks[0]);
            largestChain.availableStocks.RemoveAt(0);
            gameManager.hotelChains.RemoveAt(0);
        }

        /// <summary>
        /// Places hotelTile when clicked if this cell is empty and is selectable for the current player
        /// </summary>
        public void OnMouseDown() {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = "";
                if (IsEmpty() && selectable == true) {

                    if (AddHotelTile()) {
                        someSound.PlayOneShot(someSound.clip);
                        if (gameManager.hotelChains.Count == 7) {
                            gameManager.playerTurn.DrawHotelTile();
                            gameManager.UpdateChainCounts();
                            gameManager.ChangePlayerTurn();
                            gameManager.DisplayChainInfo();
                            gameManager.MarkPlayableCells();
                            GameObject.Find("lblPlayerBalance").GetComponent<UnityEngine.UI.Text>().text = "$" + gameManager.playerTurn.balance;
                        } else {
                            gameManager.playerTurn.DrawHotelTile();
                            gameManager.UpdateChainCounts();
                            gameManager.DisplayChainInfo();
                            gameManager.buyStocksUI.SetActive(true);
                        }

                        Debug.Log(gameManager.playerTurn + " Added tile at cell (" + row + "," + column + ")");

                        GameObject.Find("lblPlayerIndicator").GetComponent<UnityEngine.UI.Text>().text = gameManager.playerTurn + ", it's your turn!";
                    } else if (gameManager.hotelChains.Count == 0) {
                        GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = "All hotel chains are created!";
                        Debug.Log("Cannot place a tile that will create a new hotel chain since all hotel chains have been created.");
                    }

                } else if (!IsEmpty()) {
                    GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = "The cell already has a tile in it!";
                    Debug.Log("The cell already has a tile in it!");
                } else if (selectable == false) {
                    GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = "You dont have a tile to place there!";
                    Debug.Log("You dont have a tile to place there!");
                }
            } 
        }

        /// <summary>
        /// Sets the color of
        /// </summary>
        /// <param name="color"></param>
        public void SetTileColor(Color color) {
            if (hotelTile != null) {
                hotelTile.GetComponent<HotelTile>().SetMaterialColor(color);
            } else {
                throw new Exception("This cell doesn't have a hotel tile.");
            }
        }

        /// <summary>
        /// Checks if this cell has adjacent vertical or horizontal cells that have hotelTiles, and returns them in a list
        /// </summary>
        /// <returns>List of adjacent cells with hotel tiles (Will be empty if this cell is isolated)</returns>
        public List<Cell> GetOccupiedNeighbors() {
            List<Cell> occupiedNeighbors = new List<Cell>();
            if (bottom != null && bottom.GetComponent<Cell>().IsEmpty() == false) {
                occupiedNeighbors.Add(bottom.GetComponent<Cell>());
            }
            if (top != null && top.GetComponent<Cell>().IsEmpty() == false) {
                occupiedNeighbors.Add(top.GetComponent<Cell>());
            }
            if (right != null && right.GetComponent<Cell>().IsEmpty() == false) {
                occupiedNeighbors.Add(right.GetComponent<Cell>());
            }
            if (left != null && left.GetComponent<Cell>().IsEmpty() == false) {
                occupiedNeighbors.Add(left.GetComponent<Cell>());
            }
            return occupiedNeighbors;
        }

        /// <summary>
        /// Gets the largest hotelChain of this cells surrounding neighbors
        /// </summary>
        /// <returns>Null if cell is isolated from tiles, or the largest hotel chain</returns>
        public HotelChain GetLargestChain() {
            HotelChain largestChain = null;

            foreach (Cell cell in GetOccupiedNeighbors()) {
                if (cell.hotelTile.GetComponent<HotelTile>().HotelChain != null) {
                    if (largestChain == null || largestChain.hotelTiles.Count < cell.hotelTile.GetComponent<HotelTile>().HotelChain.hotelTiles.Count) {
                        largestChain = cell.hotelTile.GetComponent<HotelTile>().HotelChain;
                    }
                }
            }
            return largestChain;
        }

        /// <summary>
        /// Checks if this cell has a hotelTile placed on it.
        /// </summary>
        /// <returns>Boolean indicating if this cell is empty</returns>
        public bool IsEmpty() {
            if (hotelTile == null) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
    }
}
