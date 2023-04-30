using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yugant_Library.Controller2D
{
    public class PopUpHandlerScript : MonoBehaviour
    {
        public static PopUpHandlerScript instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
