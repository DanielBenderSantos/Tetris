using UnityEngine;

public class GhostPiece : MonoBehaviour
{
    private Transform target;
    private gameManager gManager;

    void Start() {
        gManager = FindFirstObjectByType<gameManager>();
    }

    public void SetTarget(Transform t) {
        target = t;
    }

    void Update() {
        if (target == null) return;

        // Copia a forma da peça
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).localPosition = target.GetChild(i).localPosition;
        }

        // Posiciona a ghost na mesma posição inicial da peça
        transform.position = target.position;
        transform.rotation = target.rotation;

        // Move pra baixo até bater
        while (PosicaoValida()) {
            transform.position += new Vector3(0, -1, 0);
        }

        // Volta um passo pra cima (última posição válida)
        transform.position += new Vector3(0, 1, 0);
    }

    bool PosicaoValida() {
        foreach (Transform child in transform) {
            Vector2 pos = gManager.arredonda(child.position);
            if (!gManager.dentroGrade(pos)) return false;

            var t = gManager.pocisaoTransformGrade(pos);
            if (t != null && t.parent != target) return false;
        }
        return true;
    }
}
