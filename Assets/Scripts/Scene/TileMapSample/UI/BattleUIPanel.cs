using System;
using UnityEngine;

namespace RtsProject.Assets.Scripts.Scene.TileMapSample
{
    /// <summary>
    /// バトルUIパネル
    /// </summary>
    public sealed class BattleUIPanel : MonoBehaviour
    {
        /// <summary>
        /// 拡大ボタン押下時のコールバック
        /// </summary>
        public Action OnClickScaleUpButtonCallBack{ private get; set; }

        /// <summary>
        /// 縮小ボタン押下時のコールバック
        /// </summary>
        public Action OnClickScaleDownButtonCallBack{ private get; set; }

        /// <summary>
        /// 拡大ボタン押下時
        /// </summary>
        public void OnClickScaleUpButton ()
        {
            OnClickScaleUpButtonCallBack?.Invoke();
        }

        /// <summary>
        /// 縮小ボタン押下時
        /// </summary>
        public void OnClickScaleDownButton ()
        {
            OnClickScaleDownButtonCallBack?.Invoke();
        }
    }
}
