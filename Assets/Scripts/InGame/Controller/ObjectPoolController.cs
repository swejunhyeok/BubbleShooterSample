using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JH
{
    namespace BBS
    {
        public class ObjectPoolController : SingletonController<ObjectPoolController>
        {

            #region Instance

            void Awake()
            {
                if (_instance == null)
                {
                    _instance = this;
                }

                if (_instance != this)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DontDestroyOnLoad(gameObject);

                    InitObjectPool();
                }
            }

            #endregion

            #region Prefab

            [Header("- Prefab -")]
            [SerializeField]
            private GameObject _prefabLine;
            [SerializeField]
            private GameObject _prefabCell;
            [SerializeField]
            private GameObject _prefabBlock;

            #endregion

            #region Object pool

            private ObjectPool<Line> _objectPoolLine = new ObjectPool<Line>();
            public Line GetLine(Transform parent = null)
            {
                Line line = _objectPoolLine.GetObject();
                line.gameObject.SetActive(true);
                if(parent != null)
                {
                    line.transform.parent = parent;
                }
                return line;
            }
            public void Dispose(Line line)
            {
                _objectPoolLine.Dispose(line);
            }

            private ObjectPool<Cell> _objectPoolCell = new ObjectPool<Cell>();
            public Cell GetCell(Transform parent = null)
            {
                Cell cell = _objectPoolCell.GetObject();
                cell.gameObject.SetActive(true);
                if (parent != null)
                {
                    cell.transform.parent = parent;
                }
                return cell;
            }
            public void Dispose(Cell cell)
            {
                _objectPoolCell.Dispose(cell);
            }

            private ObjectPool<Block> _objectPoolBlock = new ObjectPool<Block>();
            public Block GetBlock(Transform parent = null)
            {
                Block block = _objectPoolBlock.GetObject();
                block.gameObject.SetActive(true);
                if(parent != null)
                {
                    block.transform.parent = parent;
                }
                return block;
            }
            public void Dispose(Block block)
            {
                _objectPoolBlock.Dispose(block);
            }

            #endregion

            #region General

            private void InitObjectPool()
            {
                _objectPoolCell.Init(_prefabCell, transform);
                _objectPoolBlock.Init(_prefabBlock, transform);
                _objectPoolLine.Init(_prefabLine, transform);
            }

            #endregion

        }
    }
}
