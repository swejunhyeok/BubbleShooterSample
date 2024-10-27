using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JH
{
    public class GameManager : SingletonController<GameManager>
    {

        #region InGame

        private Dictionary<int, List<int>> _oddDetect = new Dictionary<int, List<int>>();

        public Dictionary<int, List<int>> OddDetect
        {
            get
            {
                if (_oddDetect.Count == 0)
                {
                    InitDetect(true);
                }
                return _oddDetect;
            }
        }

        private Dictionary<int, List<int>> _evenDetect = new Dictionary<int, List<int>>();

        public Dictionary<int, List<int>> EvenDetect
        {
            get
            {
                if (_evenDetect.Count == 0)
                {
                    InitDetect(false);
                }
                return _evenDetect;
            }
        }

        private void InitDetect(bool isOdd)
        {
            using (StreamReader sr = new StreamReader(Application.dataPath + "/Resources/InGame/" + (isOdd?"odd":"even") + ".txt"))
            {
                int linenum = 0;
                while(!sr.EndOfStream)
                {
                    ++linenum;
                    string line = sr.ReadLine();
                    bool isStart = true;
                    int degree = -1;
                    List<int> indexList = new List<int>();
                    foreach (string splitString in line.Split(","))
                    {
                        if(isStart)
                        {
                            if (int.TryParse(splitString, out int parseDegree))
                            {
                                degree = parseDegree;
                            }
                            isStart = false;
                        }
                        else
                        {
                            if(int.TryParse(splitString, out int index))
                            {
                                indexList.Add(index);
                            }
                        }
                    }
                    if(isOdd)
                    {
                        _oddDetect.Add(degree, indexList);
                    }
                    else
                    {
                        _evenDetect.Add(degree, indexList);
                    }
                }
            }
        }


        #endregion

    }
}
