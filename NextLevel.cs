using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject endPoint, FadeScreen;
    public string lvl2, transition;


    // Update is called once per frame
    async void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        float EndPos = Vector3.Distance(endPoint.transform.position, transform.position);

        if (currentScene.name == "Level 1")
        { 
            if (EndPos <= 3f && FadeScreen != null)
            {
                FadeScreen.GetComponent<Animation>().Play("FadeOutAnim");
                await Task.Delay(2000);
                SceneManager.LoadScene(transition);
            }
        }
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene(lvl2);
    }

    public void Menu() //<< change from transition to menu whenu get menu
    {
        SceneManager.LoadScene(transition);
    }
}
