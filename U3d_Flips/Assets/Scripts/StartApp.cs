using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartApp : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void Start()
    {
        LoadEntryScene();
    }

    private async void LoadEntryScene()
    {
        await Task.Delay(50);
        Debug.Log($"[StartApp][time] Loading scene start.. {Time.realtimeSinceStartup}");
        SceneManager.LoadScene(sceneName);
    }
}