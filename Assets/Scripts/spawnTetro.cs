using UnityEngine;
using System.Collections.Generic;

public class spawnTetro : MonoBehaviour
{
    public int proxPeca;
    public Transform [] criaPecas;
    public List<GameObject> mostraPecas;
    void Start(){
        proxPeca = Random.Range(0,7);
        ProximaPeca();
    }
    public void ProximaPeca(){
        Instantiate(criaPecas[proxPeca], transform.position, Quaternion.identity);

        proxPeca = Random.Range(0,7);

        for (int i = 0; i < mostraPecas.Count; i++) {
            mostraPecas[i].SetActive(false);
        }  
        mostraPecas[proxPeca].SetActive(true);
    }
}
