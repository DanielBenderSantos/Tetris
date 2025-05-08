using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ScoreData
{
    public string nome;
    public int pontos;
}
[System.Serializable]
public class Ranking
{
    public List<ScoreData> lista = new List<ScoreData>();
}
public class gameManager:MonoBehaviour
{
    public static int altura = 20;
    public static int largura = 10;
    public int score = 0;
    public int linhas = 0;
    public int nivel = 1;
    public float dificuldade =  1;
    public string nivelDificuldade =  "facil";

    public TextMeshProUGUI textoScore;
    public TextMeshProUGUI textoLinhas;
    public TextMeshProUGUI textoNivel;
    public TextMeshProUGUI textonivelDificuldade;
    public TextMeshProUGUI rankingTexto;


    public int pontoDificuldade;
    public bool pause = false;
    private bool nomeJaSalvo = false;

    public Button botaoSalvar;

    public static Transform[,] grade = new Transform[largura,altura];
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;

    public TMP_InputField  nomeInput;
    public TextMeshProUGUI pontosTexto;
    public int pontuacaoFinal;

    private string caminho => Application.persistentDataPath + "/ranking.json";
    private Ranking ranking = new Ranking();

 
    private void Start() {
        dificuldade =  PlayerPrefs.GetFloat("dificuldade");
        nivelDificuldade =  PlayerPrefs.GetString("nivelDificuldade");
    }
    public void Update()
    {
        textoScore.text = "Pontos: " + score.ToString();
        textoLinhas.text = "Linhas: " + linhas;
        textoNivel.text = "Nível: " + nivel;
        textonivelDificuldade.text = "Dificuldade: " +  nivelDificuldade;

       if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !TMP_InputFieldEstaSelecionado())
        {
            if (pauseMenuUI.activeSelf)
                Resume();
            else
                Pause();
        }
        string nome = nomeInput.text;

        if (!string.IsNullOrEmpty(nome))
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
                SalvarPlacar();
            } 
        }

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
                    if(grade[x,y].parent == pecaTretris.transform)
                        grade[x,y] = null;
                }
            }
        }
        foreach(Transform peca in pecaTretris.transform){
            Vector2 posicao = arredonda(peca.position);
            if(posicao.y < altura)
                grade[(int)posicao.x,(int)posicao.y] = peca;
        }
    }

    public Transform pocisaoTransformGrade(Vector2 posicao){
        if(posicao.y > altura-1)
            return null;
        else
            return grade[(int)posicao.x,(int)posicao.y];
    }

    public bool linhaCheia(int y){
        for(int x=0; x < largura; x++){
            if(grade[x,y] == null)
                return false;        
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
                if(nivelDificuldade == "facil"){
                    score += 100;
                }
                
                if(nivelDificuldade == "medio"){
                    score += 200;
                }
                if(nivelDificuldade == "dificil"){
                    score += 500;
                }
                pontoDificuldade += 100;
                linhas++;
            }
        }
    }
    public bool acimaGrade(tetroMov pecaTetroMino){
        for(int x=0; x< largura; x++){
            foreach(Transform quadrado in pecaTetroMino.transform){
                Vector2 posicao = arredonda(quadrado.position);
                if(posicao.y > altura -1)
                    return  true;
            }
        }
        return false;
    }

    public void gameOver(){
        gameOverMenuUI.SetActive(true);
        pause = true;
        pontuacaoFinal = int.Parse(score.ToString()); 
        pontosTexto.text = "Parabéns! Seu placar foi: " + pontuacaoFinal;

        
        
    }
     public void ReiniciarJogo(){
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene("Play");
        pause = false;
    }
  
    public void Pause()
    {
        pause = !pause;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }


    public void Resume()
    {
        pause = !pause;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    
     public void Menu(){
        pause = !pause;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
 
    
public void SalvarPlacar()
{
    if (nomeJaSalvo)
    {
        Debug.Log("Nome já foi salvo.");
        return;
    }

    string nome = nomeInput.text;

    if (!string.IsNullOrEmpty(nome))
    {
        // Carrega dados antigos, se existirem
        if (File.Exists(caminho))
        {
            string jsonExistente = File.ReadAllText(caminho);
            ranking = JsonUtility.FromJson<Ranking>(jsonExistente);
        }

        ranking.lista.Add(new ScoreData { nome = nome, pontos = pontuacaoFinal });
        ranking.lista = ranking.lista.OrderByDescending(s => s.pontos).Take(10).ToList();

        string json = JsonUtility.ToJson(ranking, true);
        File.WriteAllText(caminho, json);

        nomeJaSalvo = true;
        nomeInput.interactable = false;

        // ✅ Aqui é onde você desativa o botão
        botaoSalvar.interactable = false;

        Debug.Log("Placar salvo com sucesso!");
        CarregarRanking();
    }
}



    void CarregarRanking()
    {
        if (!File.Exists(caminho))
        {
            rankingTexto.text = "Nenhum placar salvo ainda.";
            return;
        }

        string json = File.ReadAllText(caminho);
        Ranking ranking = JsonUtility.FromJson<Ranking>(json);

        string resultado = " Ranking:\n";

        int posicao = 1;
        foreach (var score in ranking.lista)
        {
            resultado += $"{posicao}. {score.nome} - {score.pontos} pts\n";
            posicao++;
        }
        rankingTexto.text = resultado;
    }

    private bool TMP_InputFieldEstaSelecionado()
{
    return UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null &&
           UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null;
}


}