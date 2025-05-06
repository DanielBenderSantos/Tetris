using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public float dificuldade = 1 ;
    public string nivelDificuldade = "facil";


    void Start()
    {
        dificuldade = 1;
        nivelDificuldade = "facil";

    }
    public void IniciarJogo()
    {
        PlayerPrefs.SetFloat("dificuldade", dificuldade);
        PlayerPrefs.SetString("nivelDificuldade", nivelDificuldade);
        SceneManager.LoadScene("Play");
    }
  
       public void facil()
    {
        dificuldade = 1;
        nivelDificuldade = "facil";
        
    }
        public void medio()
    {
        dificuldade = 5;
        nivelDificuldade = "medio";
    }
        public void dificil()
    {
        dificuldade = 10;
        nivelDificuldade = "dificil";
    }


      public void Sair(){
        Application.Quit();
    }

    
    
}
