using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovesManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI movesText;
    public int totalMoves;
    private int movesLeft;

    public GameObject GameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        movesLeft = totalMoves;
        movesText.text = "Moves: " + movesLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoves();
    }

    public int SubtractMoves(int _moves)
    {
        movesLeft = movesLeft - _moves;
        return movesLeft;
    }

    private void UpdateMoves()
    {
        movesText.text = $"Moves: {movesLeft}";

        if (movesLeft == 0)
        {
            GameOverPanel.SetActive(true);
            CarController[] carControllers = GameObject.FindObjectsOfType<CarController>();
            foreach (var cc in carControllers)
            {
                cc.enabled = false;
            }
        }
    }


}
