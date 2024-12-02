using System.Collections.Generic;
using UnityEngine;

namespace HoopStack
{
    public class Stand : MonoBehaviour
    {
        // Public property to access the first movement point
        public Transform firstMovementPoint { get; private set; }

        // Serialized fields to configure sockets and circles in the Unity Editor
        [SerializeField] private List<GameObject> sockets = new();
        [SerializeField] private List<GameObject> circles = new();

        // Number of empty sockets and completed circle stacks
        public int emptySockets;
        private int completedCircles;

        private void Awake()
        {
            // Find the first movement point for circle placement
            firstMovementPoint = transform.Find("firstMovementPos");
        }

        /// <summary>
        /// Gets the last circle in the stack.
        /// </summary>
        /// <returns>The last circle GameObject.</returns>
        public GameObject GetLastCircle()
        {
            return circles.Count > 0 ? circles[^1] : null;
        }

        /// <summary>
        /// Gets the next available socket for placement.
        /// </summary>
        /// <returns>The next available socket GameObject.</returns>
        public GameObject GetSocket()
        {
            return sockets.Count > emptySockets ? sockets[emptySockets] : null;
        }

        /// <summary>
        /// Handles the process of removing a circle and updating socket data.
        /// </summary>
        public void ChangeSocketProcess(GameObject removedObject)
        {
            if (circles.Contains(removedObject))
            {
                circles.Remove(removedObject);
                emptySockets = Mathf.Clamp(emptySockets - 1, 0, sockets.Count - 1);

                // Allow the new top circle to move, if any circles remain
                if (circles.Count > 0)
                {
                    circles[^1].GetComponent<Circle>().canMove = true;
                }
            }
        }

        /// <summary>
        /// Checks if all circles in the stack have the same color.
        /// </summary>
        public void CheckCircles()
        {
            if (circles.Count == 4)
            {
                string color = circles[0].GetComponent<Circle>().color;
                completedCircles = CountMatchingCircles(color);

                if (completedCircles == 4)
                {
                    GameManager.instance.StandCompleted();
                    ProcessCompletedStand();
                }
                else
                {
                    completedCircles = 0;
                }
            }
        }

        /// <summary>
        /// Processes a completed stand by disabling movements and updating visuals.
        /// </summary>
        private void ProcessCompletedStand()
        {
            foreach (var circle in circles)
            {
                var circleComponent = circle.GetComponent<Circle>();
                circleComponent.canMove = false;

                UpdateCircleTransparency(circle, 150); // Make the circle semi-transparent
            }

            gameObject.tag = "Untagged";
        }

        /// <summary>
        /// Counts the number of circles in the stack that match a given color.
        /// </summary>
        /// <param name="color">The color to check against.</param>
        /// <returns>The number of circles that match the given color.</returns>
        private int CountMatchingCircles(string color)
        {
            int count = 0;
            foreach (var circle in circles)
            {
                if (circle.GetComponent<Circle>().color == color)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Updates the transparency of a circle.
        /// </summary>
        /// <param name="circle">The circle GameObject to update.</param>
        /// <param name="alpha">The alpha value to set (0-255).</param>
        private void UpdateCircleTransparency(GameObject circle, byte alpha)
        {
            var renderer = circle.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                Color color = renderer.material.GetColor("_Color");
                color.a = alpha / 255f; // Convert alpha to normalized value
                renderer.material.SetColor("_Color", color);
            }
        }

        /// <summary>
        /// Adds a circle to the stack and updates socket data.
        /// </summary>
        public void AddCircle(GameObject circle)
        {
            if (circle != null)
            {
                circles.Add(circle);
                emptySockets = Mathf.Clamp(emptySockets + 1, 0, sockets.Count);
                UpdateCircleMovementPermissions();
            }
        }

        /// <summary>
        /// Updates the movement permissions of the top circle.
        /// </summary>
        private void UpdateCircleMovementPermissions()
        {
            if (circles.Count > 0)
            {
                circles[^1].GetComponent<Circle>().canMove = true;
            }
        }

        /// <summary>
        /// Provides access to the current list of circles.
        /// </summary>
        public List<GameObject> Circles => circles;
    }
}
