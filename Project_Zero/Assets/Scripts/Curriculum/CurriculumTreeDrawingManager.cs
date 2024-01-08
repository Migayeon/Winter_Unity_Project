using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CurriculumTreeDrawingManager : MonoBehaviour
{
    public Transform subjectsObject;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private Transform linesTransform;
    public List<Transform> subjectTransform = new List<Transform>();
    public int lineCnt = 10;
    public List<GroupBox> groupBoxes = new List<GroupBox>();
    public List<List<int>> groupSubjects = new List<List<int>>();
    public void Start()
    {
        for (int i = 0; i < subjectsObject.childCount; i++)
            subjectTransform.Add(subjectsObject.GetChild(i));
        drawTree(new List<int>());
    }

    public void drawTree(List<int> ids)
    {
        Debug.ClearDeveloperConsole();
        foreach (Transform child in linesTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < groupBoxes.Count; i++)
        {
            if (groupBoxes[i].inputLine != null) Destroy(groupBoxes[i].inputLine);
            if (groupBoxes[i].outputLine != null) Destroy(groupBoxes[i].outputLine);
            if (groupBoxes[i].Box != null) Destroy(groupBoxes[i].Box);
        }
        groupSubjects = new List<List<int>>();
        groupBoxes = new List<GroupBox>();
        for (int i = 0; i < SubjectTree.subjectsInfo.groupCount + 1; i++)
        {
            groupSubjects.Add(new List<int>());
            groupBoxes.Add(new GroupBox());
        }
        for (int i = 0; i < subjectsObject.childCount; i++)
        {
            int groupId = SubjectTree.getSubject(i).subjectGroupId;
            if (SubjectTree.getSubject(i).subjectGroupId != 0)
            {
                groupSubjects[groupId].Add(i);
                groupBoxes[groupId].addNewPoint(subjectTransform[i].position);
            }
        }
        for (int startId = 0; startId < subjectsObject.childCount; startId++)
        {
            foreach (int endId in SubjectTree.getSubject(startId).nextSubjects)
            {
                drawCurveById(startId, endId, ids.Contains(startId) && ids.Contains(endId));
            }
        }
        for (int j = 1; j < SubjectTree.subjectsInfo.groupCount + 1; j++)
        {
            groupBoxes[j].expandBox(1);
            groupBoxes[j].draw(linePrefab, linesTransform);
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
    public GameObject drawCurveById(int startId, int endId, bool selected)
    {
        GameObject oneLine = Instantiate(linePrefab, Vector2.zero, Quaternion.identity, linesTransform);
        List<Vector2> pos = getPos(startId, endId, oneLine, selected);
        if (pos != null)
        {
            drawCurve(pos[0], pos[1], oneLine, selected);
            return oneLine;
        }
        return null;
    }

    public void drawCurve(Vector2 posA, Vector2 posB, GameObject oneLine, bool selected)
    {
        LineRenderer lr = oneLine.GetComponent<LineRenderer>();
        if (selected)
        {
            lr.startColor = new Color(0.007f, 0.258f, 0.035f);
            lr.endColor = Color.yellow;
        }
        Vector2 diff = posA - posB;
        lr.positionCount = lineCnt;
        for (int line = 0; line < lineCnt; line++)
        {
            float t;
            if (line == 0)
                t = 0;
            else
                t = (float)line / (lineCnt - 1);
            Vector2 bezier = Bezier(posA, posA - new Vector2(0, diff.y / 3 + 0.5f),
                posB + new Vector2(0, diff.y / 3 + 0.5f), posB, t);
            lr.SetPosition(line, bezier);
        }
        if (selected)
        {
            for (int i = 0; i < lr.positionCount; i++)
                lr.SetPosition(i, new Vector3(lr.GetPosition(i).x, lr.GetPosition(i).y, -1));
        }
    }
    public List<Vector2> getPos(int startId, int endId, GameObject oneLine, bool selected)
    {
        int startGroupId = SubjectTree.getSubject(startId).subjectGroupId;
        int endGroupId = SubjectTree.getSubject(endId).subjectGroupId;
        Vector2 posA = subjectTransform[startId].position;
        if (startGroupId != 0)
        {
            posA.x = ((groupBoxes[startGroupId].P0 + groupBoxes[startGroupId].P1) / 2).x;
            posA.y = groupBoxes[startGroupId].P1.y - 1;
            groupBoxes[startGroupId].haveLineOutput = true;
            groupBoxes[startGroupId].select = selected ? true : groupBoxes[startGroupId].select;
            groupBoxes[startGroupId].outputLine = oneLine;
            if (selected)
            {
                LineRenderer lr = oneLine.GetComponent<LineRenderer>();
                lr.startColor = new Color(0.007f, 0.258f, 0.035f);
                lr.endColor = Color.yellow;
            }
        }
        Vector2 posB = subjectTransform[endId].position;
        if (endGroupId != 0)
        {
            posB.x = ((groupBoxes[endGroupId].P0 + groupBoxes[endGroupId].P1) / 2).x;
            posB.y = groupBoxes[endGroupId].P0.y + 1;
            groupBoxes[endGroupId].haveLineInput = true;
            groupBoxes[startGroupId].select = selected ? true : groupBoxes[startGroupId].select;
            groupBoxes[endGroupId].inputLine = oneLine;
            if (selected)
            {
                LineRenderer lr = oneLine.GetComponent<LineRenderer>();
                lr.startColor = new Color(0.007f, 0.258f, 0.035f);
                lr.endColor = Color.yellow;
            }
        }
        List<Vector2> result = new List<Vector2>
        {
            posA,
            posB
        };
        return result;
    }

    public class GroupBox
    {
        public Vector2 P0 = Vector2.zero;
        public Vector2 P1 = Vector2.zero;
        private bool isFirstTimeToEdit = true;
        public bool haveLineInput = false;
        public bool haveLineOutput = false;
        public GameObject outputLine = null;
        public GameObject inputLine = null;
        public GameObject Box;
        public bool select = false;
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

        public void draw(GameObject linePrefab, Transform linesTransform)
        {
            GameObject oneLine = Instantiate(linePrefab, P0, Quaternion.identity, linesTransform);
            Box = oneLine;
            LineRenderer lr = oneLine.GetComponent<LineRenderer>();
            if (select)
            {
                oneLine.GetComponent<LineRenderer>().startColor = new Color(0.007f, 0.258f, 0.035f);
                oneLine.GetComponent<LineRenderer>().endColor = Color.yellow;
            }
            lr.positionCount = 4;
            lr.loop = true;
            lr.SetPosition(0, P0);
            lr.SetPosition(1, new Vector2(P1.x, P0.y));
            lr.SetPosition(2, P1);
            lr.SetPosition(3, new Vector2(P0.x, P1.y));
            P0.x += 1; P1.x -= 1;
            P0.y -= 1; P1.y += 1;
        }
    }
}
