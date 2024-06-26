using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using System;

namespace Yugant_Library.Controller2D
{
    public class GameControllerScript : MonoBehaviour
    {
        public static GameControllerScript instance;
        [SerializeField] Stack<EventID> gameFlowStack = new Stack<EventID>();
        [SerializeField] EventID currentEventID;
        [SerializeField] Transform levelContainer;
        [SerializeField] GameObject currLevel;
        [SerializeField] int currLevelId;


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

            Init();
        }

        void Init()
        {
            Debug.Log("Init Called.");
            currentEventID = EventID.HOME;
            AddEventToGameStack(currentEventID);
            CheckGameFlowStack();
            int index = UI_HandlerScript.instance.GetGameObjOfEventID(EventID.LEVEL_PANEL);
            Debug.Log("BS");
            Debug.Log("BS : " + UI_HandlerScript.instance.screensArr[index].gameObject.name);
        }

        private void Start()
        {

        }

        public void AddEventToGameStack(EventID id)
        {
            Debug.Log("Event Pushed : " + id);
            gameFlowStack.Push(id);
            SetCurrentEventID(gameFlowStack.Peek());
        }

        public void RemoveEventFromGameStack()
        {
            Debug.Log("Event Pop : " + gameFlowStack.Peek());
            gameFlowStack.Pop();
            SetCurrentEventID(gameFlowStack.Peek());
        }

        public void CheckGameFlowStack()
        {
            foreach (EventID id in gameFlowStack)
            {
                Debug.Log("Event Id : " + id);
            }
        }

        void ClearLevelContainer()
        {
            Debug.Log("Clear Level Container Called");
            if (levelContainer.transform.childCount > 0)
            {
                foreach (Transform tran in levelContainer)
                {
                    Destroy(tran);
                }
            }
        }

        public void CreateLevel()
        {
            ClearLevelContainer();
            Debug.Log("Create Level Called");
        }

        public EventID GetCurrentEventID()
        {
            Debug.Log("Current Event ID : " + currentEventID);
            return currentEventID;
        }

        public void SetCurrentEventID(EventID id)
        {
            currentEventID = id;
            Debug.Log("Current Event ID Set to : " + currentEventID);

        }

        public void SwitchScreen(EventID id)
        {
            Debug.Log("Screen Switching To : " + id);
        }


        public EventID GetEventID(BaseScreenHandler baseScreenPopUpObj)
        {
            return baseScreenPopUpObj.screenID;
        }
    }
}
