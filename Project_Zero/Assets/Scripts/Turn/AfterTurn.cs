using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterTurn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 재화 변동

        // BeforeTurn 불러오기, 1턴 추가
        TurnManager.turn++;
        SceneManager.LoadScene("Curriculum");
    }
}
