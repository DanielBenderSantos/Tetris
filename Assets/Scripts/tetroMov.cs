using UnityEngine;
public class tetroMov : MonoBehaviour
{
    public bool podeRodar;
    public bool roda360;
    public float queda;
    public float velocidade;
    public float timer;
    public GameObject ghostPiecePrefab; // Prefab da peça fantasma
    private GameObject ghostPiece;      // Instância da peça fantasma
    private gameManager gManager;
    private spawnTetro gSpawner;
    void Start()
    {
        timer = velocidade;
        gManager = FindFirstObjectByType<gameManager>();
        gSpawner = FindFirstObjectByType<spawnTetro>();
    }

    void Update()
    {
        if (gManager.pause) return;
        AtualizaPecaFantasma();
        AtualizaDificuldade();
        // Reseta o timer se uma das teclas for solta
        if (Input.GetKeyUp(KeyCode.RightAlt) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            timer = velocidade;

        if (Input.GetKey(KeyCode.RightArrow))
            MoverHorizontal(Vector3.right);

        if (Input.GetKey(KeyCode.LeftArrow))
            MoverHorizontal(Vector3.left);

        if (Input.GetKey(KeyCode.DownArrow))
            MoverParaBaixoManual();

        if (Time.time - queda >= (1f / gManager.dificuldade) && !Input.GetKey(KeyCode.DownArrow))
            MoverParaBaixoAutomatico();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            checaRoda();
    }

   void AtualizaPecaFantasma()
    {
        if (ghostPiece != null){
            Destroy(ghostPiece);
        }
        ghostPiece = Instantiate(gameObject, transform.position, transform.rotation);
        ghostPiece.name = "GhostPiece";
        ghostPiece.GetComponent<tetroMov>().enabled = false; // desativa script na peça fantasma

        // Aplica material/cor de transparência (se quiser depois)
        foreach (Transform block in ghostPiece.transform)
        {
            var renderer = block.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                Color cor = renderer.color;
                cor.a = 0.3f; // deixa a peça semi-transparente
                renderer.color = cor;
            }
        }
        ghostPiece.transform.SetParent(transform.parent); // mantém a hierarquia
        UpdateGhostPiece();
    }

  void UpdateGhostPiece()
    {
        if (ghostPiece == null) return;
        while (posicaoValidaGhost(ghostPiece.transform))
            ghostPiece.transform.position += Vector3.down;
            ghostPiece.transform.position += Vector3.up;
    }

    void MoverHorizontal(Vector3 direcao)
    {
        timer += Time.deltaTime;
        if (timer > velocidade)
        {
            transform.position += direcao;
            timer = 0;
            if (posicaoValida())
                gManager.atualizaGrade(this);
            else
                transform.position -= direcao;
        }
    }

    void MoverParaBaixoManual()
    {
        timer += Time.deltaTime;
        if (timer > velocidade)
        {
            transform.position += Vector3.down;
            timer = 0;
            if (posicaoValida())
                gManager.atualizaGrade(this);
            else
                FinalizaMovimento();
        }
    }

    void MoverParaBaixoAutomatico()
    {
        transform.position += Vector3.down;
        if (posicaoValida())
            gManager.atualizaGrade(this);
        else
            FinalizaMovimento();
        queda = Time.time;
    }

    void FinalizaMovimento()
    {
        transform.position += Vector3.up;
        gManager.apagaLinha();
        if (gManager.acimaGrade(this))
            gManager.gameOver();

        gManager.score += 10;
        gManager.pontoDificuldade += 10;
        enabled = false;
        gSpawner.ProximaPeca();
        if (ghostPiece != null)
            Destroy(ghostPiece);
    }

    void AtualizaDificuldade()
    {
        if (gManager.pontoDificuldade >= 1000)
        {
            gManager.pontoDificuldade -= 1000;
            gManager.dificuldade += 0.5f;
            gManager.nivel++;
        }
    }

    void checaRoda()
    {
        if (!podeRodar) return;
        if (!roda360)
        {
            float zRot = transform.rotation.eulerAngles.z;
            float ajuste = zRot < 90f ? 90 : -90;
            transform.Rotate(0, 0, ajuste);
            if (!posicaoValida())
                transform.Rotate(0, 0, -ajuste);
            else
                gManager.atualizaGrade(this);
        }
        else
        {
            transform.Rotate(0, 0, -90);
            if (!posicaoValida())
                transform.Rotate(0, 0, 90);
            else
                gManager.atualizaGrade(this);
        }
    }

    bool posicaoValida()
    {
        foreach (Transform child in transform)
        {
            Vector2 posBloco = gManager.arredonda(child.position);
            if (!gManager.dentroGrade(posBloco) ||
                (gManager.pocisaoTransformGrade(posBloco) != null &&
                gManager.pocisaoTransformGrade(posBloco).parent != transform))
                return false;
        }
        return true;
    }

    bool posicaoValidaGhost(Transform ghostTransform)
    {
        foreach (Transform child in ghostTransform)
        {
            Vector2 pos = gManager.arredonda(child.position);
            if (!gManager.dentroGrade(pos) || (gManager.pocisaoTransformGrade(pos) != null && 
            gManager.pocisaoTransformGrade(pos).parent != transform))
                return false;
        }
        return true;
    }
}