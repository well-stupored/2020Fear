using UnityEngine;

namespace Assets.Code
{
    [RequireComponent (typeof(Collider))]
    public class WallBehaviour : MonoBehaviour
    {
        public void Start()
        {
            AstarPath.active.UpdateGraphs(collider.bounds);
        }
    }
}
