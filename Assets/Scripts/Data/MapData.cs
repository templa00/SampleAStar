using UnityEngine;

namespace RtsProject.Assets.Scripts.Data
{
    /// <summary>
    /// マップデータ
    /// </summary>
    public sealed class MapData
    {
        /// <summary>
        /// キャラクター初期セル位置
        /// </summary>
        public Vector3Int CharacterInitCellPosition
        {
            get; private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterInitCellPosition"></param>
        public MapData (Vector3Int characterInitCellPosition)
        {
            CharacterInitCellPosition = characterInitCellPosition;
        }
    }
}
