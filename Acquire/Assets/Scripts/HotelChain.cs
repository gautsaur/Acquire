using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Acquire {
    public class HotelChain : IEquatable<HotelChain> {

        #region Fields
        public string name;
        public Color color;
        public Player firstLargetHolder;
        public Player secondLargetHolder;
        public List<Stock> availableStocks;
        public List<GameObject> hotelTiles;
        #endregion

        #region Constructors
        public HotelChain(string name, Color color) {
            this.name = name;
            this.color = color;
            hotelTiles = new List<GameObject>();
            availableStocks = new List<Stock>();
            AddStocks(300);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the number of stocks available to purchase for this chain
        /// </summary>
        /// <returns>Number of available stocks</returns>
        public int NumberOfStocksAvailable() {
            return availableStocks.Count;
        }

        /// <summary>
        /// Generate the max number of stocks (25) for this chain and add them to availableStocks
        /// </summary>
        /// <param name="price">The price of each stock</param>
        public void AddStocks(int price) {
            for (int i = 0; i < 25; i++) {
                availableStocks.Add(new Stock(this, price));
            }
        }

        /// <summary>
        /// Set the price of each stock belonging to this hotel chain
        /// </summary>
        /// <param name="price">The new price of each stock</param>
        public void SetStockPrice(int price) {
            foreach (Stock stock in availableStocks) {
                stock.Price = price;
            }
        }
        
        /// <summary>
        /// Check if this hotel chain is safe
        /// </summary>
        /// <returns>Boolean indicating if this chain is safe</returns>
        public bool IsSafe() {
            if (hotelTiles.Count >= 11) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Implementation of Equals method from IEquatable; compares two hotel chains by comparing their names
        /// </summary>
        /// <param name="other">The other hotel chain to compare against</param>
        /// <returns>Boolean indicating if these hotel chains are the same</returns>
        public bool Equals(HotelChain other) {
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


