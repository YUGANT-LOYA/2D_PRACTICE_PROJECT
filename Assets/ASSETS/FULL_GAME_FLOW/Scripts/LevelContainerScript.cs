using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yugant_Library.Controller2D
{
    public class LevelContainerScript : BaseScreenHandler
    {
        public static LevelContainerScript instance;

        private void Awake()
        {
            CreateSingleton();
        }

        void CreateSingleton()
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

        protected override void Start()
        {

            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
