using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public async void ChangeScene(int lvl) => await SceneManager.LoadSceneAsync(lvl);
}
