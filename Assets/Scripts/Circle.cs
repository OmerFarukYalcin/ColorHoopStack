using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoopStack
{
    public class Circle : MonoBehaviour
    {
        public GameObject _stand;
        public GameObject socket;
        public bool canMove;
        public string color;

        private GameObject movementPosition;
        private GameObject targetStand;

        bool selected, changePosition, backToTheSocket, insertSocket;

        public void Move(string process, GameObject stand = null, GameObject socket = null, GameObject targetObject = null)
        {

            switch (process)
            {
                case "Secim":
                    BGMUSIC.instance.PlaySoundEffect(SFX.socketSfx);
                    movementPosition = targetObject;
                    selected = true;
                    break;

                case "PozisyonDegistir":
                    BGMUSIC.instance.PlaySoundEffect(SFX.circleInsertSfx);
                    targetStand = stand;
                    this.socket = socket;
                    movementPosition = targetObject;
                    changePosition = true;
                    break;
                case "SoketeGeriGit":
                    backToTheSocket = true;
                    break;
            }

        }


        void Update()
        {
            if (selected)
            {
                transform.position = Vector3.Lerp(transform.position, movementPosition.transform.position, .2f);

                if (Vector3.Distance(transform.position, movementPosition.transform.position) < .10)
                {
                    selected = false;
                }
            }
            if (changePosition)
            {
                transform.position = Vector3.Lerp(transform.position, movementPosition.transform.position, .2f);

                if (Vector3.Distance(transform.position, movementPosition.transform.position) < .10)
                {
                    changePosition = false;
                    insertSocket = true;
                }
            }
            if (insertSocket)
            {
                transform.position = Vector3.Lerp(transform.position, socket.transform.position, .2f);

                if (Vector3.Distance(transform.position, socket.transform.position) < .10)
                {
                    transform.position = socket.transform.position;
                    insertSocket = false;
                    _stand = targetStand;

                    if (_stand.GetComponent<Stand>().Circles.Count > 1)
                    {

                        _stand.GetComponent<Stand>().Circles[^2].GetComponent<Circle>().canMove = false;
                    }

                    GameManager.instance.canMove = false;
                }
            }
            if (backToTheSocket)
            {
                transform.position = Vector3.Lerp(transform.position, socket.transform.position, .2f);

                if (Vector3.Distance(transform.position, socket.transform.position) < .10)
                {
                    transform.position = socket.transform.position;
                    backToTheSocket = false;
                    GameManager.instance.canMove = false;
                }
            }
        }
    }
}
