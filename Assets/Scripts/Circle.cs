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

        private bool selected, changePosition, backToTheSocket, insertSocket;

        /// <summary>
        /// Initiates a movement process for the circle.
        /// </summary>
        public void Move(string process, GameObject stand = null, GameObject socket = null, GameObject targetObject = null)
        {
            switch (process)
            {
                case "Select":
                    HandleSelection(targetObject);
                    break;

                case "ChangePosition":
                    HandlePositionChange(stand, socket, targetObject);
                    break;

                case "GoBackToSocket":
                    HandleReturnToSocket();
                    break;
            }
        }

        private void Update()
        {
            if (selected)
                SmoothMoveTowards(movementPosition.transform.position, () => selected = false);

            if (changePosition)
                SmoothMoveTowards(movementPosition.transform.position, () =>
                {
                    changePosition = false;
                    insertSocket = true;
                });

            if (insertSocket)
                SmoothMoveTowards(socket.transform.position, CompleteInsertion);

            if (backToTheSocket)
                SmoothMoveTowards(socket.transform.position, CompleteReturnToSocket);
        }

        /// <summary>
        /// Smoothly moves the circle towards a target position and triggers an action upon completion.
        /// </summary>
        private void SmoothMoveTowards(Vector3 targetPosition, System.Action onComplete)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                onComplete?.Invoke();
        }

        /// <summary>
        /// Handles the selection process when a circle is selected.
        /// </summary>
        private void HandleSelection(GameObject targetObject)
        {
            BGMUSIC.Instance.PlaySoundEffect(SFX.SocketSfx);
            movementPosition = targetObject;
            selected = true;
        }

        /// <summary>
        /// Handles the process of changing the circle's position.
        /// </summary>
        private void HandlePositionChange(GameObject stand, GameObject socket, GameObject targetObject)
        {
            BGMUSIC.Instance.PlaySoundEffect(SFX.CircleInsertSfx);
            targetStand = stand;
            this.socket = socket;
            movementPosition = targetObject;
            changePosition = true;
        }

        /// <summary>
        /// Handles the process of returning the circle to its original socket.
        /// </summary>
        private void HandleReturnToSocket()
        {
            backToTheSocket = true;
        }

        /// <summary>
        /// Completes the insertion process, setting the circle to its new socket.
        /// </summary>
        private void CompleteInsertion()
        {
            transform.position = socket.transform.position;
            insertSocket = false;
            _stand = targetStand;

            // Update the ability to move the circle below this one
            if (_stand.GetComponent<Stand>().Circles.Count > 1)
            {
                var belowCircle = _stand.GetComponent<Stand>().Circles[^2].GetComponent<Circle>();
                belowCircle.canMove = false;
            }

            GameManager.instance.canMove = false;
        }

        /// <summary>
        /// Completes the return process, resetting the circle to its socket.
        /// </summary>
        private void CompleteReturnToSocket()
        {
            transform.position = socket.transform.position;
            backToTheSocket = false;
            GameManager.instance.canMove = false;
        }

        /// <summary>
        /// Checks if the circle is at the target position.
        /// </summary>
        /// <param name="targetPosition">The position to check against.</param>
        /// <returns>True if the circle is close enough to the target position.</returns>
        public bool IsAtPosition(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) < 0.1f;
        }

        /// <summary>
        /// Updates the movement permissions of the circles on the stand.
        /// </summary>
        public void UpdateMovementPermissions()
        {
            if (_stand.GetComponent<Stand>().Circles.Count > 1)
            {
                var belowCircle = _stand.GetComponent<Stand>().Circles[^2].GetComponent<Circle>();
                belowCircle.canMove = false;
            }
        }
    }
}
