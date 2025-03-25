using UnityEngine;

namespace TetrisMechanics.Scripts
{
    public class BlockContainerMono : MonoBehaviour
    {
        public GameObject blockGameObject;

        public void SetActive(bool active)
        {
            blockGameObject.SetActive(active);
        }
    }
}
