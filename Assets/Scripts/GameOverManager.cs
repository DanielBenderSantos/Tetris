using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
     public TextMeshProUGUI textoScore;
    public void ReiniciarJogo()
    {
        SceneManager.LoadScene("Play"); 
    }
    
}
