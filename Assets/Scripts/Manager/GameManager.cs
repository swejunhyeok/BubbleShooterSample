using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JH
{
    public class GameManager : SingletonController<GameManager>
    {

        #region InGame

        private Dictionary<float, List<int>> _oddDetect = new Dictionary<float, List<int>>();

        public Dictionary<float, List<int>> OddDetect
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

        private Dictionary<float, List<int>> _evenDetect = new Dictionary<float, List<int>>();

        public Dictionary<float, List<int>> EvenDetect
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
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    bool isStart = true;
                    float degree = -1;
                    List<int> indexList = new List<int>();
                    foreach (string splitString in line.Split(","))
                    {
                        if(isStart)
                        {
                            if (float.TryParse(splitString, out float parseDegree))
                            {
                                degree = parseDegree;
                            }
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
