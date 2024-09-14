using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoopStack
{
    public class GameManager : MonoBehaviour
    {
        private int level = 1;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        public static GameManager instance;
        [SerializeField] private GameObject selectedGo;
        [SerializeField] private GameObject selectedStand;
        [SerializeField] private GameObject GameWonPanel;
        [SerializeField] private GameObject GameLosePanel;
        [SerializeField] private Circle circle;
        public bool canMove;
        private int targetStandValue = 5;
        private int completedStandValue;

        private void Awake()
        {
            textMeshPro.text = $"Level {level}";
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
                {

                    if (hit.collider != null && hit.collider.CompareTag("Stand"))
                    {

                        if (selectedGo != null && selectedStand != hit.collider.gameObject)
                        {
                            Stand new_stand = hit.collider.GetComponent<Stand>();

                            if (new_stand.Circles.Count != 4 && new_stand.Circles.Count != 0)
                            {

                                if (circle.color == new_stand.Circles[^1].GetComponent<Circle>().color)
                                {
                                    selectedStand.GetComponent<Stand>().ChangeSocketProcess(selectedGo);

                                    circle.Move("PozisyonDegistir", hit.collider.gameObject, new_stand.GetSocket(), new_stand.firstMovementPoint.gameObject);

                                    new_stand.emptySockets++;

                                    new_stand.Circles.Add(selectedGo);

                                    new_stand.CheckCircles();

                                    selectedGo = null;
                                    selectedStand = null;
                                }
                                else
                                {
                                    circle.Move("SoketeGeriGit");

                                    selectedGo = null;
                                    selectedStand = null;
                                }
                            }
                            else if (new_stand.Circles.Count == 0)
                            {
                                selectedStand.GetComponent<Stand>().ChangeSocketProcess(selectedGo);

                                circle.Move("PozisyonDegistir", hit.collider.gameObject, new_stand.GetSocket(), new_stand.firstMovementPoint.gameObject);

                                new_stand.emptySockets++;

                                new_stand.Circles.Add(selectedGo);

                                new_stand.CheckCircles();

                                selectedGo = null;
                                selectedStand = null;
                            }
                            else
                            {
                                circle.Move("SoketeGeriGit");

                                selectedGo = null;
                                selectedStand = null;
                            }
                        }
                        else if (selectedStand == hit.collider.gameObject)
                        {
                            circle.Move("SoketeGeriGit");

                            selectedGo = null;
                            selectedStand = null;
                        }
                        else
                        {

                            Stand new_stand = hit.collider.GetComponent<Stand>();
                            if (new_stand.Circles.Count == 0)
                                return;
                            selectedGo = new_stand.GetLastCircle();
                            circle = selectedGo.GetComponent<Circle>();
                            canMove = true;

                            if (circle.canMove)
                            {
                                circle.Move("Secim", null, null, circle._stand.GetComponent<Stand>().firstMovementPoint.gameObject);

                                selectedStand = circle._stand;
                            }
                        }


                    }


                }

            }
        }

        public void StandCompleted()
        {
            completedStandValue++;
            if (completedStandValue == targetStandValue)
                GameWonPanel.SetActive(true);
        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void HomeScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
