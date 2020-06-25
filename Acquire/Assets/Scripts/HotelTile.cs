using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Acquire {
    public class HotelTile : MonoBehaviour, IEquatable<HotelTile> {
        
        #region Fields
        public int row;
        public int column;
        public Cell currentCell;
        public string color;
        public HotelChain hotelChain = null;
        #endregion

        #region Constructors
        public HotelTile(int row, int column) {
            this.row = row;
            this.column = column;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the color of this hotel tile
        /// </summary>
        /// <param name="color">New color of this tile</param>
        public void SetMaterialColor(Color color) {
            this.GetComponent<Renderer>().material.SetColor("_Color", color);
        }

        /// <summary>
        /// Implementation of Equals method from IEquatable; compares two hotel tiles by their row and column values
        /// </summary>
        /// <param name="other">The other hotel tile to compare against</param>
        /// <returns>Boolean indicating if these hotel tiles are the same</returns>
        public bool Equals(HotelTile other) {
            if (other == null) {
                return false;
            }
            if (this.row == other.row && this.column == other.column) {
                return true;
            } else {
                return false;
            }
        }

        public override string ToString() {
            return "HotelTile " + row + "," + column;
        }
        #endregion

        #region Properties
        public string Color {
            get {
                return color;
            }
            set {
                color = value;
            }
        }

        public HotelChain HotelChain {
            get {
                return hotelChain;
            }
            set {
                hotelChain = value;
            }
        }
        #endregion
    }
}

