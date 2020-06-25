using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Acquire {

    public class RestartButtonScript : MonoBehaviour {

        public void RestartGame() {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            Board board = GameObject.Find("GameBoard").GetComponent<Board>();
            GameObject cellPrefab = board.cellPrefab;
            GameObject hotelTilePrefab = gameManager.hotelTilePrefab;

            for (int j = 0; j < gameManager.numPlayers; j++) {
                gameManager.players.RemoveFirst();
            }

            while (gameManager.hotelChains.Count != 0) {
                gameManager.hotelChains.RemoveAt(0);
            }

            // Regenerate hotel tiles
            while (gameManager.availableTiles.Count != 0) {
                gameManager.availableTiles.RemoveAt(0);
            }

            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    GameObject hotelTile = GameObject.Find("hotelTile " + row + "," + column);
                    hotelTile.transform.parent = board.transform;
                    hotelTile.transform.localPosition = new Vector3(0, 0, 0);
                    hotelTile.name = "hotelTile " + row + "," + column;
                    hotelTile.GetComponent<HotelTile>().HotelChain = null;
                    gameManager.availableTiles.Add(hotelTile);
                }
            }

            // Shuffle tiles
            gameManager.availableTiles = gameManager.availableTiles.OrderBy(x => Random.value).ToList();

            // Create the new GameBoard
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    // Create new cells using cell prefab
                    Destroy(GameObject.Find("Cell " + row + "," + column));
                    GameObject cell = Instantiate(cellPrefab) as GameObject;
                    cell.transform.parent = board.transform;
                    cell.transform.localPosition = new Vector3(row, 0, column);
                    cell.name = "Cell " + row + "," + column;
                    // Set the fields for the cells
                    cell.GetComponent<Cell>().row = row;
                    cell.GetComponent<Cell>().column = column;
                    cell.GetComponent<Cell>().board = board;
                    board.cells[row, column] = cell;
                }
            }

            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    board.SetAdjacentCells(board.cells[row, column]);
                }
            }

            gameManager.AddPlayers();
            int i = 0;
            foreach (Player player in gameManager.players) {
                player.name = gameManager.playerNames[i];
                i++;
            }
            gameManager.hotelChains.Add(new HotelChain("Arcadia", Color.blue));
            gameManager.hotelChains.Add(new HotelChain("Fanfare", Color.green));
            gameManager.hotelChains.Add(new HotelChain("Global", Color.gray));
            gameManager.hotelChains.Add(new HotelChain("Luster", Color.red));
            gameManager.hotelChains.Add(new HotelChain("Nobel", Color.cyan));
            gameManager.hotelChains.Add(new HotelChain("Regal", Color.magenta));
            gameManager.hotelChains.Add(new HotelChain("Teton", Color.yellow));
            gameManager.playerTurn = gameManager.players.First.Value;
            gameManager.UpdateChainCounts();
            gameManager.DisplayChainInfo();
            gameManager.MarkPlayableCells();
            GameObject.Find("lblGameWarning").GetComponent<UnityEngine.UI.Text>().text = "";
            GameObject.Find("lblPlayerIndicator").GetComponent<UnityEngine.UI.Text>().text = gameManager.playerTurn + ", place a tile!";
            GameObject.Find("lblPlayerBalance").GetComponent<UnityEngine.UI.Text>().text = "$" + gameManager.playerTurn.balance;

        }

    }
}

