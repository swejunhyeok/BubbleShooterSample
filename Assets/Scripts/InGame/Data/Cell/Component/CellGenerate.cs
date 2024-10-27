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

            private int _totalWeight = 0;

            public void LoadGenerateData(LitJson.JsonData generatesRoot)
            {
                _generateDatas = new GenerateData[generatesRoot.Count];

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

            public void GenerateObject()
            {
                if(Parent.Block.HasMiddleBlock)
                {
                    return;
                }
                StartCoroutine(GenerateCoroutine());
            }


            private struct MoveInfo
            {
                public Block Block;
                public Cell TargetCell;
            }
            private IEnumerator GenerateCoroutine()
            {
                int generateNum = ComputeGenerateNum();
                for(int i = 0; i < generateNum; ++i)
                {
                    BlockType type = GetBlockType();
                    Parent.Block.CreateBlock(type, true);

                    List<MoveInfo> moveInfos = new List<MoveInfo>();
                    Cell targetCell = Parent.GetArroundCell((int)Parent.Direction);
                    if(targetCell != null && targetCell.Type != CellType.FinishCell)
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

                    int totalMoveNum = moveInfos.Count;

                    for (int j = moveInfos.Count - 1; j >= 0; --j)
                    {
                        moveInfos[j].Block.RemovePivotCell();
                        moveInfos[j].TargetCell.Block.AddBlock(moveInfos[j].Block, false);
                        moveInfos[j].TargetCell.Block.MiddleBlock.Move.SetMoveEndAction(() => --totalMoveNum);
                        moveInfos[j].TargetCell.Block.MiddleBlock.Move.Move(BlockMove.MoveType.BlockFill);
                    }

                    while(totalMoveNum != 0)
                    {
                        yield return null;
                    }
                }
            }

            private int ComputeGenerateNum()
            {
                int generateNum = 0;
                Cell targetCell = Parent.GetArroundCell((int)Parent.Direction);
                while(targetCell != null && targetCell.Block.IsEmpty)
                {
                    ++generateNum;
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
