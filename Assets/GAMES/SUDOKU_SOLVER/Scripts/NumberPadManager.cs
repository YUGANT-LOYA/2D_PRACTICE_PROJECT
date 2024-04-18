using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace YugantLoyaLibrary.SudokuSolver
{
    public class NumberPadManager : MonoBehaviour
    {
        public static NumberPadManager instance;

        public KeyPadNum[] allNumberKeyArr;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            VisibilityStatusOfAllKeys(false);
        }

        public void UpdateNumberKeys(int[] interactiveKeysArr)
        {
            foreach (KeyPadNum keyPadNum in allNumberKeyArr)
            {
                //Debug.Log($"Value {keyPadNum.KeyNum}  : {interactiveKeysArr.Contains(keyPadNum.KeyNum)}");
                keyPadNum.SetButtonStatus(interactiveKeysArr.Contains(keyPadNum.KeyNum));
            }
        }

        public void ResetNumberKeys()
        {
            foreach (KeyPadNum keyPadNum in allNumberKeyArr)
            {
                keyPadNum.SetButtonStatus(true);
            }
        }

        public void VisibilityStatusOfAllKeys(bool isActive)
        {
            foreach (KeyPadNum keyPadNum in allNumberKeyArr)
            {
                keyPadNum.gameObject.SetActive(isActive);
            }
        }
    }
}