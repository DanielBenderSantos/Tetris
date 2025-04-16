using UnityEngine;
using System.Collections.Generic;

public class spawnTetro : MonoBehaviour
{
    public int proxPeca;
    public Transform[] criaPecas;
    public List<GameObject> mostraPecas;

    // Referência para o prefab da Ghost Piece (GameObject)
    public GameObject ghostPrefab;

    void Start(){
        proxPeca = Random.Range(0, 7);
        ProximaPeca();
    }

    public void ProximaPeca(){
        // Instancia a peça normal
        GameObject novaPeca = Instantiate(criaPecas[proxPeca], transform.position, Quaternion.identity).gameObject;

 
        proxPeca = Random.Range(0, 7);

        // Desativa as outras peças no UI
        for (int i = 0; i < mostraPecas.Count; i++) {
            mostraPecas[i].SetActive(false);
        }  
        mostraPecas[proxPeca].SetActive(true);
    }
}
