using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CurriculumTreeDrawingManager : MonoBehaviour
{
    public Transform subjectsObject;
    public GameObject linePrefab;
    public Transform linesTransform;
    private List<Transform> subjectTransform = new List<Transform>();
    public int lineCnt = 10;
    public void Start()
    {
        for (int i = 0; i < subjectsObject.childCount; i++)
            subjectTransform.Add(subjectsObject.GetChild(i));
        drawTree();
    }

    private void drawTree()
    {
        List<List<int>> groupSubjects = new List<List<int>>();
        List<GroupBox> groupBoxes = new List<GroupBox>();
        for (int i = 0; i < SubjectTree.subjectsInfo.groupCount; i++)
        {
            groupSubjects.Add(new List<int>());
            groupBoxes.Add(new GroupBox());
        }
        for (int i = 0; i < subjectsObject.childCount; i++)
        {
            int groupId = SubjectTree.getSubject(i).subjectGroupId;
            if (SubjectTree.getSubject(i).subjectGroupId != -1)
            {
                groupSubjects[groupId].Add(i);
                groupBoxes[groupId].addNewPoint(subjectTransform[i].position);
            }
        }
        for (int i = 0; i < subjectsObject.childCount; i++)
        {
            foreach (int id in SubjectTree.getSubject(i).nextSubjects)
            {
                GameObject oneLine = Instantiate(linePrefab, subjectTransform[i].position, Quaternion.identity, linesTransform);
                LineRenderer lr = oneLine.GetComponent<LineRenderer>();
                Vector2 posA = subjectTransform[i].position;
                Vector2 posB = subjectTransform[id].position;
                Vector3 diff = posA - posB;
                lr.positionCount = lineCnt + 3;
                for (int line = 0; line < lineCnt; line++)
                {
                    float t;
                    if (line == 0)
                        t = 0;
                    else
                        t = (float)line / (lineCnt - 1);
                    Vector2 bezier = Bezier(posA, posA - new Vector2(0, diff.y / 3 - 1),
                        posB + new Vector2(0, diff.y / 3 - 0.5f), posB, t);
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

    private class GroupBox
    {
        public Vector2 P0 = Vector2.zero;
        public Vector2 P1 = Vector2.zero;
        bool isFirstTimeToEdit = true;
        public void addNewPoint(Vector2 newPoint)
        {
            if (isFirstTimeToEdit) { 
                P0 = newPoint;
                P1 = newPoint;
                isFirstTimeToEdit = false;
            }
            else
            {
                if (P0.x > newPoint.x)
                    P0.x = newPoint.x;
                if (P0.y < newPoint.y)
                    P0.y = newPoint.y;
                if (P1.x < newPoint.x)
                    P1.x = newPoint.x;
                if (P1.y > newPoint.y)
                    P1.y = newPoint.y;
            }
        }
        public void expandBox(float value)
        {
            P0.x -= value; P1.x += value;
            P0.y += value; P1.y -= value;
        }
        public void draw()
        {
            GameObject oneLine = Instantiate(linePrefab, subjectTransform[i].position, Quaternion.identity, linesTransform);

        }
    }
}
