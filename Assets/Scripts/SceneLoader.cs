using UnityEngine;

public class SceneLoader : MonoBehaviour {
    public void ReloadCurrentScene() =>
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
}