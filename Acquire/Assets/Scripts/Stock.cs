using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Acquire {
    public class Stock : IEquatable<Stock> {

        private int price;
        private HotelChain owningChain;

        public Stock(HotelChain owningChain, int price) {
            this.owningChain = owningChain;
            this.price = price;
        }

        public int Price {
            set {
                if (value >= 0) {
                    price = value;
                } else {
                    price = value;
                }
            }
            get {
                if (owningChain.hotelTiles.Count <= 2) {
                    return 300;
                } else if (owningChain.hotelTiles.Count == 3) {
                    return 400;
                } else if (owningChain.hotelTiles.Count == 4) {
                    return 500;
                } else if (owningChain.hotelTiles.Count == 5) {
                    return 600;
                } else if (owningChain.hotelTiles.Count >= 6 && owningChain.hotelTiles.Count <= 10) {
                    return 700;
                } else if (owningChain.hotelTiles.Count >= 11 && owningChain.hotelTiles.Count <= 20) {
                    return 800;
                } else if (owningChain.hotelTiles.Count >= 21 && owningChain.hotelTiles.Count <= 30) {
                    return 900;
                } else if (owningChain.hotelTiles.Count >= 31 && owningChain.hotelTiles.Count <= 40) {
                    return 1000;
                } else if (owningChain.hotelTiles.Count >= 41) { 
                    return 1100;
                } else {
                    return 300;
                }
            }
        }

        public HotelChain OwningChain {
            get {
                return owningChain;
            }
            set {
                owningChain = value;
            }
        }

        /// <summary>
        /// Implementation of Equals method from IEquatable; compares two hotel tiles by their row and column values
        /// </summary>
        /// <param name="other">The other stock to compare against</param>
        /// <returns>Boolean indicating if these stocks are the same</returns>
        public bool Equals(Stock other) {
            if (other == null) {
                return false;
            }
            if (this.OwningChain.Equals(other.OwningChain)) {
                return true;
            } else {
                return false;
            }
        }
    }
}

