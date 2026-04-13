using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private int _sceneBuildIndex;

    [Button("Change Scene", ButtonSizes.Large)]
    public void ChangeScene()
    {
        SceneManager.LoadScene(_sceneBuildIndex);
    }
}