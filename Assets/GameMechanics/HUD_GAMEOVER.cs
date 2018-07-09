using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD_GAMEOVER : MonoBehaviour
{
    private Button botonReset;

    private bool reloading = false;

    private void Start()
    {
        reloading = false;

        botonReset = this.GetComponentInChildren<Button>();
    }

    public void ReloadScene()
    {
        if(reloading == false)
        {
            reloading = true;

            if (botonReset != null)
                botonReset.interactable = false;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}