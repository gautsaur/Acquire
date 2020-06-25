using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Acquire {
    public class Board : MonoBehaviour {

        #region Fields
        public GameObject cellPrefab;
        public GameObject[,] cells = new GameObject[9,12];
        public int hotelTileCount;
        public List<Player> players;
        #endregion

        #region Methods
        
        /// <summary>
        /// Instantiate the cells that will make up the game board when game is started
        /// </summary>
        void Awake() {
            // Create the GameBoard at runtime
            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    // Create new cells using cell prefab
                    GameObject cell = Instantiate(cellPrefab) as GameObject;
                    cell.transform.parent = this.transform;
                    cell.transform.localPosition = new Vector3(row, 0, column);
                    cell.name = "Cell " + row + "," + column;
                    // Set the fields for the cells
                    cell.GetComponent<Cell>().row = row;
                    cell.GetComponent<Cell>().column = column;
                    cell.GetComponent<Cell>().board = this;
                    cells[row, column] = cell;
                }
            }

            for (int row = 0; row < 9; row++) {
                for (int column = 0; column < 12; column++) {
                    SetAdjacentCells(cells[row, column]);
                }
            }
            GameObject.Find("GameManager").GetComponent<GameManager>().SetUpGame();
        }

        /// <summary>
        /// Sets the links to adjacent cells of the provided cell
        /// </summary>
        /// <param name="cell">The cell to have its adjacent cells set</param>
        public void SetAdjacentCells(GameObject cell) {
            int row = cell.GetComponent<Cell>().row;
            int column = cell.GetComponent<Cell>().column;
            if (row != 8) {
                cell.GetComponent<Cell>().bottom = cells[row + 1, column];
            }
            if (row != 0) {
                cell.GetComponent<Cell>().top = cells[row - 1, column];
            }
            if (column != 11) {
                cell.GetComponent<Cell>().right = cells[row, column + 1];
            }
            if (column != 0) {
                cell.GetComponent<Cell>().left = cells[row, column - 1];
            }
        }
        #endregion
    }
}

