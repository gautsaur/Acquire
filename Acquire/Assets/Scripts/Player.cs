using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Acquire {
    public class Player: IEquatable<Player> {

        #region Fields
        public int balance = 6000;
        public string name;
        public List<HotelTile> hotelTiles = new List<HotelTile>();
        public GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        public List<Stock> ownedStocks = new List<Stock>();
        #endregion

        #region Constructors
        public Player(string name) {
            this.name = name;
            for (int i = 0; i < 6; i++) {
                DrawHotelTile();
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Adds a random hotel tile that hasn't already been drawn to the player's list of placable tiles
        /// </summary>
        public void DrawHotelTile() {
            hotelTiles.Add(gameManager.availableTiles[0].GetComponent<HotelTile>());
            gameManager.availableTiles.RemoveAt(0);
        }

        /// <summary>
        /// Sells stocks from a defunct chain
        /// </summary>
        /// <param name="chain">Owning chain of the stocks to be sold</param>
        /// <param name="quantity">The amount of stocks to be sold</param>
        public void Sell(HotelChain chain, int quantity) {
            var sellableStocks = ownedStocks.Where(stock => stock.OwningChain.Equals(chain)).ToList();

            while (quantity != 0 && sellableStocks.Count != 0) {

                balance += sellableStocks[0].Price;
                chain.availableStocks.Add(sellableStocks[0]);
                ownedStocks.Remove(sellableStocks[0]);
                sellableStocks.RemoveAt(0);
                quantity--;
            }
        }

        /// <summary>
        /// Trade stock: 2 of defunct chain for 1 of the new chain's stock
        /// </summary>
        public void Trade() {

        }

        /// <summary>
        /// Purchase stocks from a hotel chain using balance
        /// </summary>
        /// <param name="chain">The chain to buy stocks in</param>
        /// <param name="quantity">The amount of stocks to buy (MAX = 3)</param>
        public void Buy(HotelChain chain, int quantity) {
            if (chain.availableStocks[0].Price * quantity > balance) {
                throw new Exception("This player doesn't have enough money to buy that specified number of stocks.");
            } else if (quantity > 3) {
                throw new Exception("The max amount of stocks you can buy in a turn is three.");
            } else {
                balance = balance - (chain.availableStocks[0].Price * quantity);
                ownedStocks.Add(chain.availableStocks[0]);
                chain.availableStocks.RemoveAt(0);
            }
        }

        /// <summary>
        /// Implementation of Equals method from IEquatable; compares two players by their names
        /// </summary>
        /// <param name="other">Other player to be compared against</param>
        /// <returns>Boolean indicating if these players are the same</returns>
        public bool Equals(Player other) {
            if (other == null) {
                return false;
            }
            if (this.name == other.name) {
                return true;
            } else {
                return false;
            }
        }

        public override string ToString() {
            return name;
        }
        #endregion 
    }
}
