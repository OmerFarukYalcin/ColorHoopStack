using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoopStack
{
    public class GameManager : MonoBehaviour
    {
        // Level information
        private int level = 1;

        // UI elements and panels
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private GameObject GameWonPanel;
        [SerializeField] private GameObject GameLosePanel;

        // Selected objects and references
        [SerializeField] private GameObject selectedGo;
        [SerializeField] private GameObject selectedStand;
        [SerializeField] private Circle circle;

        // Game state flags
        public static GameManager instance;
        public bool canMove;

        // Game progress tracking
        private int targetStandValue = 5;
        private int completedStandValue;

        private void Awake()
        {
            textMeshPro.text = $"Level {level}";
            if (instance == null)
                instance = this;
            else
                Destroy(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseInput();
            }
        }

        /// <summary>
        /// Handles mouse input for selecting and interacting with stands and circles.
        /// </summary>
        private void HandleMouseInput()
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                if (hit.collider != null && hit.collider.CompareTag("Stand"))
                {
                    HandleStandInteraction(hit.collider.gameObject);
                }
            }
        }

        /// <summary>
        /// Handles interaction logic with a stand.
        /// </summary>
        private void HandleStandInteraction(GameObject standObject)
        {
            Stand newStand = standObject.GetComponent<Stand>();

            if (selectedGo != null && selectedStand != standObject)
            {
                HandleCirclePlacement(newStand);
            }
            else if (selectedStand == standObject)
            {
                ResetSelectedCircle();
            }
            else
            {
                SelectCircle(newStand);
            }
        }

        /// <summary>
        /// Handles the placement of the selected circle onto a new stand.
        /// </summary>
        private void HandleCirclePlacement(Stand newStand)
        {
            if (newStand.Circles.Count == 0 ||
                (newStand.Circles.Count < 4 && circle.color == newStand.Circles[^1].GetComponent<Circle>().color))
            {
                MoveCircleToNewStand(newStand);
            }
            else
            {
                ResetSelectedCircle();
            }
        }

        /// <summary>
        /// Moves the selected circle to the new stand.
        /// </summary>
        private void MoveCircleToNewStand(Stand newStand)
        {
            selectedStand.GetComponent<Stand>().ChangeSocketProcess(selectedGo);

            circle.Move("ChangePosition", newStand.gameObject, newStand.GetSocket(), newStand.firstMovementPoint.gameObject);

            newStand.emptySockets++;
            newStand.Circles.Add(selectedGo);

            newStand.CheckCircles();

            ResetSelection();
        }

        /// <summary>
        /// Resets the selected circle and returns it to its original position.
        /// </summary>
        private void ResetSelectedCircle()
        {
            circle.Move("GoBackToSocket");
            ResetSelection();
        }

        /// <summary>
        /// Resets the selection variables to null.
        /// </summary>
        private void ResetSelection()
        {
            selectedGo = null;
            selectedStand = null;
        }

        /// <summary>
        /// Selects the top circle from the specified stand.
        /// </summary>
        private void SelectCircle(Stand stand)
        {
            if (stand.Circles.Count == 0)
                return;

            selectedGo = stand.GetLastCircle();
            circle = selectedGo.GetComponent<Circle>();

            if (circle.canMove)
            {
                canMove = true;
                circle.Move("Select", null, null, stand.firstMovementPoint.gameObject);
                selectedStand = stand.gameObject;
            }
        }

        /// <summary>
        /// Called when a stand is completed.
        /// </summary>
        public void StandCompleted()
        {
            completedStandValue++;
            if (completedStandValue == targetStandValue)
            {
                GameWonPanel.SetActive(true);
            }
        }

        /// <summary>
        /// Reloads the current scene.
        /// </summary>
        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Loads the next level.
        /// </summary>
        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        /// <summary>
        /// Loads the home scene.
        /// </summary>
        public void HomeScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
