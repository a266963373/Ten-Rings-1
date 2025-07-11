using UnityEngine;
using UnityEngine.SceneManagement;

public class WithdrawPanel : MonoBehaviour
{
    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void Withdraw()
    {
        GameSystem.I.Run = new();
        SceneManager.LoadScene("MainScene");
    }

}
