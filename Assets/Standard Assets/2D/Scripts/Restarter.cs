using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    public class Restarter : MonoBehaviour
    {
        public bool loadOneTime = true;

        private void Awake()
        {
            loadOneTime = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (loadOneTime == true && other.tag.CompareTo("Player") == 0)
            {
                loadOneTime = false;

                SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
            }
        }
    }
}
