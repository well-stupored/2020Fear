using UnityEngine;

namespace Assets.Code
{
    public class MenuCameraBehaviour : MonoBehaviour
    {
		public bool GetMazeCenter = true;
        public Vector3 Target = Vector3.zero;
        public float CameraSpeed = 2;

		public void Start()
		{
			if (GetMazeCenter)
			{
				GameObject temp = GameObject.Find("Maze") as GameObject;
				Target = temp.GetComponent<MazeGenerator>().GetMiddleOfMaze();
			}
		}

        public void Update()
        {
            transform.RotateAround(Target, Vector3.up, CameraSpeed * Time.deltaTime);
            transform.LookAt(Target);
        }
    }
}
