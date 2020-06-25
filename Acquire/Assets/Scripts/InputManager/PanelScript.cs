using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Acquire
{
    public class PanelScript : MonoBehaviour
    {
        public GameObject panel;
        // Start is called before the first frame update
        void Start()
        {
            //Test
            //panel.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
        }
        public void PanScript()
        {
          panel.gameObject.SetActive(true);
        }
    }
}
