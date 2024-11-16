using UnityEngine;

namespace TictactoeTictactoe.SlidingPuzzle
{
    [RequireComponent(typeof(Renderer))]
    public class ChangeColorDemo : MonoBehaviour
    {
        public void ChangeColorRed() {
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("color changed to red");
        }
        public void ChangeColorYellow() {
            GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log("color changed to yellow");
        }
        public void ChangeColorBlue() {
            GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("color changed to blue");
        }
        
    }
}