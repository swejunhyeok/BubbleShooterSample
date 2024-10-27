using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JH
{
    namespace BBS
    {
        public class LineDetectConveterUI : EditorWindow
        {

            [MenuItem("JH/Convert/Line detect")]
            private static void ShowWindow()
            {
                AssetDatabase.Refresh();
                EditorWindow window = EditorWindow.GetWindow(typeof(LineDetectConveterUI), utility: false, title: "Line Detect Converter");
            }

            #region GUI

            private bool _isStartOdd = false;
            private int _maxHeightNum = 22;

            private float _degreeInterval = 0;

            private void OnGUI()
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Is Start Odd?");
                _isStartOdd = EditorGUILayout.Toggle(_isStartOdd);

                EditorGUILayout.Space();
                _maxHeightNum = EditorGUILayout.IntField("Max height num", _maxHeightNum);

                EditorGUILayout.Space();
                _degreeInterval = EditorGUILayout.FloatField("Degree interval", _degreeInterval);

                EditorGUILayout.Space();

                if (GUILayout.Button("Convert"))
                {
                    OnClickBtn();
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }

            private void OnClickBtn()
            {
                if (SceneManager.GetActiveScene().name != "PlayScene")
                {
                    Debug.LogWarning("Run at PlayScene.");
                    return;
                }

                Vector2 mapPosition = InGameUtils.GetMapPosition();
                float maxYPosition = mapPosition.y + (ConstantData.HEIGHT_INTERVAL * (_maxHeightNum - 1)) + ConstantData.CIRCLE_RADIUS;

                Vector2[,] circles = new Vector2[_maxHeightNum, ConstantData.MAX_WIDTH_NUM];
                for (int y = 0; y < _maxHeightNum; ++y)
                {
                    Vector2 linePosition = mapPosition + new Vector2((y + (_isStartOdd ? 1 : 0)) % 2 == 1 ? 0.5f : 0, ConstantData.HEIGHT_INTERVAL * y);
                    Vector2 leftCirlePosition = new Vector2(-ConstantData.CIRCLE_RADIUS * (ConstantData.MAX_WIDTH_NUM - 1), 0);
                    for (int x = 0; x < ConstantData.MAX_WIDTH_NUM; ++x)
                    {
                        circles[y, x] = linePosition + leftCirlePosition + new Vector2(ConstantData.CIRCLE_RADIUS * 2 * x, 0);
                    }
                }

                Vector2 sponerPosition = UIController.Instance.PosMainSponer;
                StreamWriter writer = File.CreateText(Application.dataPath + "/Resources/InGame/" + (_isStartOdd ? "odd" : "even") + ".txt");

                for (float degree = ConstantData.Degree_LIMIT; degree <= 180 - ConstantData.Degree_LIMIT; degree += _degreeInterval)
                {
                    degree = Mathf.Round(degree * 100f) * 0.01f;
                    List<int> detectIndex = new List<int>();
                    if (degree == 90.0f)
                    {
                        for (int y = 0; y < circles.GetLength(0); ++y)
                        {
                            for (int x = 0; x < circles.GetLength(1); ++x)
                            {
                                if (circles[y, x].x >= -0.5f + sponerPosition.x && circles[y, x].x <= -0.5f + sponerPosition.x)
                                {
                                    detectIndex.Add(y * ConstantData.MAX_WIDTH_NUM + x);
                                }
                            }
                        }
                    }
                    else
                    {
                        // _sponerPosition = (x1, y1);
                        // y - y1 = m(x - x1);
                        // m = tan(degree);
                        // y = tan(degree) * x - tan(degree) * x1 + y1
                        List<LineEquationInfo> lineEquationInfos = new List<LineEquationInfo>();
                        float delta = sponerPosition.y;
                        float targetDegree = degree;
                        Vector2 targetPosition = sponerPosition;
                        int cnt = 0;
                        while (delta < maxYPosition)
                        {
                            float gradient = InGameUtils.ComputeGradient(targetDegree);
                            LineEquationInfo lineEquationInfo = new LineEquationInfo()
                            {
                                Gradient = gradient,
                                Delta = InGameUtils.ComputeDelta(gradient, targetPosition),
                                IsPositiveDirection = targetDegree < 90
                            };
                            lineEquationInfos.Add(lineEquationInfo);

                            if (targetDegree > 90)
                            {
                                targetDegree = 180 - targetDegree;
                                delta = InGameUtils.ComputeLineEquation(lineEquationInfo, -5);
                                targetPosition = new Vector2(-5, delta);
                            }
                            else
                            {
                                targetDegree = targetDegree + 90;
                                delta = InGameUtils.ComputeLineEquation(lineEquationInfo, 5);
                                targetPosition = new Vector2(5, delta);
                            }
                            ++cnt;
                            if (cnt == 100)
                            {
                                Debug.Log("Cancle");
                            }
                        }

                        for (int i = 0; i < lineEquationInfos.Count; ++i)
                        {
                            // circle position = (x1, y1)
                            // distance = abs(Gradient * x1 - y1 + Delata) / sqrt(Gradient * Gradient + 1)
                            LineEquationInfo lineEquationInfo = lineEquationInfos[i];
                            for (int y = 0; y < circles.GetLength(0); ++y)
                            {
                                if (lineEquationInfo.IsPositiveDirection)
                                {
                                    for (int x = 0; x < circles.GetLength(1); ++x)
                                    {
                                        float distance = InGameUtils.ComputeDistance(lineEquationInfo, circles[y, x]);
                                        if (distance <= ConstantData.CIRCLE_RADIUS * 1.5f)
                                        {
                                            detectIndex.Add(y * ConstantData.MAX_WIDTH_NUM + x);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int x = circles.GetLength(1) - 1; x >= 0; --x)
                                    {
                                        float distance = InGameUtils.ComputeDistance(lineEquationInfo, circles[y, x]);
                                        if (distance <= ConstantData.CIRCLE_RADIUS * 1.5f)
                                        {
                                            detectIndex.Add(y * ConstantData.MAX_WIDTH_NUM + x);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    int iDegree = Mathf.RoundToInt(degree * 100f);
                    sb.Append(iDegree.ToString() + ",");
                    for (int i = 0; i < detectIndex.Count; ++i)
                    {
                        sb.Append(detectIndex[i].ToString() + ",");
                    }
                    writer.WriteLine(sb.ToString());
                }

                writer.Close();
            }

            #endregion

        }
    }
}
