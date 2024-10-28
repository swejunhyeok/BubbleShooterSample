using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class CellGenerate : CellComponent
        {
            [System.Serializable]
            private struct GenerateData
            {
                public BlockType Type;
                public int Weight;
                public int Num;
            }

            [SerializeField]
            private GenerateData[] _generateDatas = null;

            private int _generateReduceNum = -1;

            private int _generateNum = 0;

            private int _totalWeight = 0;

            public void LoadGenerateData(LitJson.JsonData generatesRoot, int generateReduceNum)
            {
                _generateDatas = new GenerateData[generatesRoot.Count];
                _generateReduceNum = generateReduceNum;

                for (int i = 0; i < generatesRoot.Count; ++i)
                {
                    LitJson.JsonData generateRoot = generatesRoot[i];
                    _generateDatas[i] = new GenerateData()
                    {
                        Type = (BlockType)InGameUtils.ParseInt(ref generateRoot, ConstantData.LEVEL_DATA_BLOCK_TYPE, 0),
                        Weight = InGameUtils.ParseInt(ref generateRoot, ConstantData.LEVEL_DATA_GENERATE_WEIGHT, 0),
                        Num = InGameUtils.ParseInt(ref generateRoot, ConstantData.LEVEL_DATA_GENERATE_NUM, -1)
                    };
                    _totalWeight += _generateDatas[i].Weight;
                }
            }

            private BlockType GetBlockType()
            {
                int randomValue = Random.Range(0, _totalWeight);

                int selectIndex = -1;
                int summaryValue = 0;

                for (int i = 0; i < _generateDatas.Length; ++i)
                {
                    if (_generateDatas[i].Num == 0)
                    {
                        continue;
                    }
                    summaryValue += _generateDatas[i].Weight;
                    if (randomValue < summaryValue)
                    {
                        selectIndex = i;
                        break;
                    }
                }

                if (selectIndex == -1)
                {
                    if (Debug.isDebugBuild)
                    {
                        Debug.LogError("Generate data is error.");
                    }
                    return BlockType.None;
                }

                if (_generateDatas[selectIndex].Num != -1)
                {
                    --_generateDatas[selectIndex].Num;
                    _totalWeight -= _generateDatas[selectIndex].Weight;
                }
                return _generateDatas[selectIndex].Type;
            }

            public void GenerateObject(bool isFirst)
            {
                ++GameController.Instance.RunningGenerateEffect;
                if (Parent.Block.HasMiddleBlock)
                {
                    return;
                }
                StartCoroutine(GenerateCoroutine(isFirst));
            }


            private struct MoveInfo
            {
                public Block Block;
                public Cell TargetCell;
            }
            private int _totalMoveNum = 0;
            private IEnumerator GenerateCoroutine(bool isFirst)
            {
                int generateNum = ComputeGenerateNum();
                for(int i = 0; i < generateNum; ++i)
                {
                    BlockType type = GetBlockType();
                    Parent.Block.CreateBlock(type, true);
                    yield return null;

                    List<MoveInfo> moveInfos = new List<MoveInfo>();
                    Cell targetCell = Parent.GetArroundCell((int)Parent.Direction);
                    if(targetCell != null)
                    {
                        moveInfos.Add(new MoveInfo()
                        {
                            Block = Parent.Block.MiddleBlock,
                            TargetCell = targetCell,
                        });
                    }
                    while (targetCell != null && targetCell.Block.HasMiddleBlock)
                    {
                        Block middleBlock = targetCell.Block.MiddleBlock;

                        if (targetCell.Type == CellType.FinishCell)
                        {
                            break;
                        }
                        targetCell = targetCell.GetArroundCell((int)targetCell.Direction);
                        if (targetCell != null)
                        {
                            moveInfos.Add(new MoveInfo()
                            {
                                Block = middleBlock,
                                TargetCell = targetCell,
                            });
                        }
                        else
                        {
                            break;
                        }
                    }

                    _totalMoveNum = moveInfos.Count;

                    for (int j = moveInfos.Count - 1; j >= 0; --j)
                    {
                        moveInfos[j].Block.RemovePivotCell();
                        moveInfos[j].TargetCell.Block.AddBlock(moveInfos[j].Block, false);
                        moveInfos[j].TargetCell.Block.MiddleBlock.Move.SetMoveEndAction(() => ReduceTotalMoveNum());
                        moveInfos[j].TargetCell.Block.MiddleBlock.Move.Move(BlockMove.MoveType.BlockFill);
                    }

                    while(_totalMoveNum != 0)
                    {
                        yield return null;
                    }
                }
                if(!isFirst)
                {
                    _generateNum += generateNum;
                    while(_generateNum > _generateReduceNum)
                    {
                        _generateNum -= _generateReduceNum;
                        ReduceFinishCell();
                    }
                }
                --GameController.Instance.RunningGenerateEffect;
            }

            private void ReduceFinishCell()
            {
                List<Cell> connectedCell = new List<Cell>();
                Cell targetCell = Parent.GetArroundCell((int)Parent.Direction);
                while (targetCell != null)
                {
                    connectedCell.Add(targetCell);
                    if (targetCell.Type == CellType.FinishCell)
                    {
                        break;
                    }
                    targetCell = targetCell.GetArroundCell((int)targetCell.Direction);
                }
                if(connectedCell.Count >= 2)
                {
                    connectedCell[connectedCell.Count - 1].ChangeType(CellType.NormalCell);
                    connectedCell[connectedCell.Count - 2].ChangeType(CellType.FinishCell);
                }
            }

            private void ReduceTotalMoveNum()
            {
                --_totalMoveNum;
            }

            private int ComputeGenerateNum()
            {
                int generateNum = 0;
                Cell targetCell = Parent.GetArroundCell((int)Parent.Direction);
                while (targetCell != null)
                {
                    if (!targetCell.Block.HasMiddleBlock)
                    {
                        ++generateNum;
                    }
                    if (targetCell.Type == CellType.FinishCell)
                    {
                        break;
                    }
                    targetCell = targetCell.GetArroundCell((int)targetCell.Direction);
                }
                return generateNum;
            }

            #region General

            public override void Init()
            {
                base.Init();
                StopAllCoroutines();
            }

            public override void Dispose()
            {
                base.Dispose();
                StopAllCoroutines();
            }

            #endregion
        }
    }
}
