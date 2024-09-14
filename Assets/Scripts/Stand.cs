using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoopStack
{
    public class Stand : MonoBehaviour
    {
        public Transform firstMovementPoint { get; private set; }
        [SerializeField] private List<GameObject> sockets = new();
        public int emptySockets;
        int completedCircles;
        [SerializeField] private List<GameObject> circles = new();
        void Awake()
        {
            firstMovementPoint = transform.Find("firstMovementPos");
        }

        public GameObject GetLastCircle()
        {
            return circles[^1];
        }

        public GameObject GetSocket()
        {
            return sockets[emptySockets];
        }

        public void ChangeSocketProcess(GameObject removedObject)
        {
            circles.Remove(removedObject);

            emptySockets--;

            emptySockets = Mathf.Clamp(emptySockets, 0, 4);

            if (circles.Count != 0)
            {
                circles[^1].GetComponent<Circle>().canMove = true;
            }
        }

        public void CheckCircles()
        {
            if (circles.Count == 4)
            {
                string color = circles[0].GetComponent<Circle>().color;
                foreach (var item in circles)
                {
                    if (color == item.GetComponent<Circle>().color)
                        completedCircles++;
                }

                if (completedCircles == 4)
                {
                    GameManager.instance.StandCompleted();
                    CompletedStandProcess();
                }
                else
                {
                    completedCircles = 0;
                }
            }
        }

        public void CompletedStandProcess()
        {

            foreach (var item in circles)
            {
                item.GetComponent<Circle>().canMove = false;

                Color32 color32 = item.GetComponent<MeshRenderer>().material.GetColor("_Color");

                color32.a = 150;

                item.GetComponent<MeshRenderer>().material.SetColor("_Color", color32);
            }

            gameObject.tag = "Untagged";
        }

        public List<GameObject> Circles => circles;
    }
}
