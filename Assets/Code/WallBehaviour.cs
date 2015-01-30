using UnityEngine;

namespace Assets.Code
{
    public class WallBehaviour : MonoBehaviour
    {
        public void Start()
        {
            AstarPath.active.UpdateGraphs(collider.bounds);
            transform.position += new Vector3(0, transform.localScale.y / 2, 0);
        }
    }
}
