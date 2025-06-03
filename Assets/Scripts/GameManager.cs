using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _abductBonus = 3;
    [SerializeField] private int _hintBonus = 4;

    public TextMeshProUGUI abductText, hintText;

    public AlienAbductor alienAbd;
    public CarHintManager carHintMgr;


    // Start is called before the first frame update
    void Start()
    {
        abductText.text = _abductBonus.ToString();
        hintText.text = _hintBonus.ToString();
    }

    public void TriggerHint()
    {
        SubtractHintBonus(1);
    }

    public int SubtractHintBonus(int hintBonusValue)
    {
        if (_hintBonus < hintBonusValue)
        {
            Debug.Log("Not Enough Bonus");
        }
        else
        {
            carHintMgr.TriggerHintCheck();
            _hintBonus = _hintBonus - hintBonusValue;
            Debug.Log(" current abductBonus = " + _hintBonus);
            UpdateBonuses();

        }
        return _hintBonus;
    }

    public void TriggerAbduct()
    {
        SubtractAbductBonus(1);
    }
    public int SubtractAbductBonus(int abductValue)
    {
        if (_abductBonus < abductValue)
        {
            Debug.Log("Not Enough Bonus");
        }
        else
        {
            alienAbd.DoRandomAbduct();
            _abductBonus = _abductBonus - abductValue;
            Debug.Log(" current abductBonus = " + _abductBonus);
            UpdateBonuses();

        }
        return _abductBonus;

    }

    private void UpdateBonuses()
    {
        abductText.text = _abductBonus.ToString();
        hintText.text = _hintBonus.ToString();
    }
   
   public void NextLevel()
   {
       int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels available.");
             UnityEngine.SceneManagement.SceneManager.LoadScene(0);
           
       }
   }
}
