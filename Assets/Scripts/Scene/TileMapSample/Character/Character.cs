using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RtsProject.Assets.Scripts.Scene.TileMapSample
{
    /// <summary>
    /// キャラクター
    /// </summary>
    public sealed class Character : MonoBehaviour
    {

        private void Awake ()
        {

        }

        /// <summary>
        /// 位置の設定
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition (Vector3 position)
        {
            transform.position = position;
        }
    }
}
