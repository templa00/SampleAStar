using UnityEngine;

namespace RtsProject.Assets.Scripts.Scene.TileMapSample
{
    /// <summary>
    /// ノード管理
    /// </summary>
    public class Node
    {
        /// <summary>
        /// ステータス
        /// </summary>
        public enum Status
        {
            // 無し
            None,
            // 探索対象
            SearchTarget,
            // 探索保留
            SearchOnHold,
            // 探索済み
            Searched,
            // 当たり判定
            Collision
        }

        /// <summary>
        /// 位置
        /// </summary>
        public Vector2Int Position;

        /// <summary>
        /// ステータス
        /// </summary>
        public Status status;

        /// <summary>
        /// 実際の移動数
        /// ※開始位置からの実際に移動した回数
        /// </summary>
        public int realMovingCount;

        /// <summary>
        /// 推定の移動数
        /// ※開始位置から目標位置までの推定（壁を気にしない場合の）移動数
        /// </summary>
        public int estimatedMovingCount;

        /// <summary>
        /// 親ノード
        /// ※自身のノードに移動する前のノード
        /// </summary>
        public Node parentNode = null;

        /// <summary>
        /// ステータスの設定をする
        /// </summary>
        public void SetStatus (Status _status) => status = _status;

        /// <summary>
        /// 親ノードの設定をする
        /// </summary>
        public void SetParentNode (Node node) => parentNode = node;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Node (Vector2Int _startPos, Status _status, int _realMovingCount)
        {
            Position = _startPos;
            status = _status;
            realMovingCount = _realMovingCount;
            estimatedMovingCount = 0;
        }

        /// <summary>
        /// 推定の合計移動数の取得
        /// </summary>
        /// <returns>推定の合計移動数(実移動数 + 推定の移動数)</returns>
        public int GetEstimatedTotalMovingCount () => realMovingCount + estimatedMovingCount;

        /// <summary>
        /// 推定の移動数設定
        /// </summary>
        public void SetEstimatedMovingCount (Vector2Int _startPos, Vector2Int _targetPos)
        {
            var diffX = Mathf.Abs(_targetPos.x - _startPos.x);
            var diffY = Mathf.Abs(_targetPos.y - _startPos.y);

            // 大きいほうを設定する
            estimatedMovingCount = Mathf.Max(diffX, diffY);

            //Debug.Log("pos" + Position);
            //Debug.Log("estimatedMovingCount" + estimatedMovingCount);
        }

        /// <summary>
        /// ログ出力
        /// </summary>
        public void DebugLog ()
        {
            Debug.Log($"Node.DebugLog()====================================================================");
            Debug.Log($"Pos:{Position}");
            Debug.Log($"status:{status}");
            Debug.Log($"realMovingCount:{realMovingCount}");
            Debug.Log($"estimatedMovingCount:{estimatedMovingCount}");
            Debug.Log($"GetEstimatedTotalMovingCount():{GetEstimatedTotalMovingCount()}");
            Debug.Log($"====================================================================");
        }
    }
}
