using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurriculumSystem : MonoBehaviour
{
    public const int curriculumStats = 5;
    public class Curriculum
    {
        private long id;
        private string name;
        private bool isAvailable;
        private long tier;
        private List<int> stat;
        private Dictionary<int, string> statList = new Dictionary<int, string>()
        {
            {0, "theory"},
            {1, "mana"},
            {2, "craft"},
            {3, "element"},
            {4, "attack"},
        };
        public Curriculum() { }

    }
}
