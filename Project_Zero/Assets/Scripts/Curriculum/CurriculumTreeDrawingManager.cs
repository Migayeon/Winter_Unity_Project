using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CurriculumTreeDrawingManager : MonoBehaviour
{
    public Transform subjectsObject;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private Transform linesTransform;
    private List<Transform> subjectTransform = new List<Transform>();
    public int lineCnt = 10;
    public void Start()
    {
        subjectTransform = subjectsObject.GetComponentsInChildren<Transform>().ToList<Transform>();
        drawTree();
    }

    private void drawTree()
    {
        for(int i = 0; i < subjectsObject.childCount; i++)
        {
            foreach (int id in SubjectTree.getSubject(i).nextSubjects)
            {
                GameObject oneLine = Instantiate(linePrefab, subjectTransform[i].position, Quaternion.identity, linesTransform);
                LineRenderer lr = oneLine.GetComponent<LineRenderer>();
                Vector3 diff = subjectTransform[i].position - subjectTransform[id].position;
                lr.positionCount = lineCnt + 3;
                for (int line = 0; line < lineCnt; line++)
                {
                    float t;
                    if (line == 0)
                        t = 0;
                    else
                        t = (float)line / (lineCnt - 1);
                    Vector2 bezier = Bezier(subjectTransform[i].position, subjectTransform[i].position - new Vector3(0, diff.y / 3 - 1, 0),
                        subjectTransform[id].position + new Vector3(0, diff.y / 3 - 1, 0), subjectTransform[id].position, t);
                    lr.SetPosition(line, bezier);
                }
                lr.SetPosition(lineCnt, subjectTransform[id].position + new Vector3(-0.3f, 0.3f, 0));
                lr.SetPosition(lineCnt + 1, subjectTransform[id].position);
                lr.SetPosition(lineCnt + 2, subjectTransform[id].position + new Vector3(0.3f, 0.3f, 0));
            }
        }
    }
    private Vector2 Bezier(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
    {
        Vector2 M0 = Vector2.Lerp(P0, P1, t);
        Vector2 M1 = Vector2.Lerp(P1, P2, t);
        Vector2 M2 = Vector2.Lerp(P2, P3, t);

        Vector2 B0 = Vector2.Lerp(M0, M1, t);
        Vector2 B1 = Vector2.Lerp(M1, M2, t);

        return Vector2.Lerp(B0, B1, t);
    }
}
