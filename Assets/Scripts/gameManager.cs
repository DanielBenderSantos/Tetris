using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager:MonoBehaviour
{
    public static int altura = 20;
    public static int largura = 10;

    public int score = 0;
    public int linhas = 0;
    public int nivel = 0;
    public TextMeshProUGUI textoScore;
    public TextMeshProUGUI textoLinhas;
    public TextMeshProUGUI textoNivel;


    public int pontoDificuldade;
    public float dificuldade = 1 ;

    public bool pause = false;
    public static Transform[,] grade = new Transform[largura,altura];
    public GameObject pauseMenuUI;

    public void Update()
    {
       if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenuUI.activeSelf)
            {
                pause = !pause;
                Resume();
            }
            else
            {
                pause = !pause;
                Pause();
            }
        }

        textoScore.text = score.ToString();
        textoLinhas.text = "Linhas: " + linhas;
        textoNivel.text = "Nível: " + nivel;
    }
    public bool dentroGrade(Vector2 posicao){
        return((int)posicao.x >= 0 &&(int)posicao.x < largura && (int)posicao.y >= 0);
    }

    public Vector2 arredonda(Vector2 nA){ // Numero Arredondado
        return new Vector2(Mathf.Round(nA.x),Mathf.Round(nA.y));
    }

    public void atualizaGrade(tetroMov pecaTretris){
        for (int y=0; y<altura; y++){
            for (int x=0; x<largura; x++){
                if(grade[x,y] != null){
                    if(grade[x,y].parent == pecaTretris.transform){
                        grade[x,y] = null;
                    }
                }
            }
        }
        foreach(Transform peca in pecaTretris.transform){
            Vector2 posicao = arredonda(peca.position);

            if(posicao.y < altura){
                grade[(int)posicao.x,(int)posicao.y] = peca;
            }
        }

        
    }

    public Transform pocisaoTransformGrade(Vector2 posicao){
        if(posicao.y > altura-1){
            return null;
        }
        else{
            return grade[(int)posicao.x,(int)posicao.y];

        }
    }

    public bool linhaCheia(int y){
        for(int x=0; x < largura; x++){
            if(grade[x,y] == null){
                return false;        
            }
        }  
        return true;
    }

    public void deletaQuadrado(int y){
        for(int x=0; x < largura; x++){
            Destroy(grade[x,y].gameObject);
            grade[x,y] = null;  
        }
    }

    public void moveLinhaBaixo (int y) {
        for(int x=0; x < largura; x++){
            if(grade[x,y] != null){
                grade[x,y-1] = grade[x,y];
                grade[x,y] = null;
                grade[x,y-1].position += new Vector3(0,-1,0); 
            }
        }
    }

    public void moveTodasLinhasBaixo(int y) {
        for(int i = y; i < altura; i++){
            moveLinhaBaixo(i);
        }
    }

    public void apagaLinha(){
        for(int y=0; y < altura; y++){
            if(linhaCheia(y)){
                deletaQuadrado(y);
                moveTodasLinhasBaixo(y+1);
                y--;
                score += 100;
                pontoDificuldade += 100;
                linhas++;
            }
        }
    }
    public bool acimaGrade(tetroMov pecaTetroMino){
        for(int x=0; x< largura; x++){
            foreach(Transform quadrado in pecaTetroMino.transform){
                Vector2 posicao = arredonda(quadrado.position);

                if(posicao.y > altura -1){
                    return  true;
                }
            }
        }
        return false;
    }

    public void gameOver(){
        SceneManager.LoadScene("gameOver");
    }
    public void ReiniciarJogo()
    {
        SceneManager.LoadScene("Play"); 
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
     public void Sair(){
        Application.Quit();
    }
}
