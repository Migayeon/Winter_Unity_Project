using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterTurn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 학생들 커리큘럼 진행
        foreach (StudentGroup[] period in PlayerInfo.studentGroups)
        {
            foreach (StudentGroup group in period)
            {
                group.CurriculumSequence();
            }
        }

        // 재화 변동

        // BeforeTurn 불러오기, 1턴 추가
        TurnManager.turn++;
        SceneManager.LoadScene("Curriculum");
    }
}
