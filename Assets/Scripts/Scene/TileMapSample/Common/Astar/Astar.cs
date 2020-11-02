using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RtsProject.Assets.Scripts.Scene.TileMapSample
{
    using Status = Node.Status;

    /// <summary>
    /// A-Starアルゴリズム
    /// </summary>
    public sealed class Astar
    {
        /// <summary>
        /// 8方向
        /// </summary>
        public enum EightDirection
        {    
            // 上
            Top,
            // 左
            Left,
            // 右
            Right,
            // 下
            Bottom,
            // 上左
            TopLeft,
            // 上右
            TopRight,
            // 下左
            BottomLeft,
            // 下右
            BottomRight,
            // 最大値
            Max,
        }

        /// <summary>
        /// 開始位置
        /// </summary>
        private Vector2Int StartPos;

        /// <summary>
        /// 目標位置
        /// </summary>
        private Vector2Int TargetPos;

        /// <summary>
        /// ノードリスト
        /// </summary>
        private List<Node> nodes = new List<Node>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        public Astar (Vector2Int startPos, Vector2Int targetPos)
        {
            // 開始ノード初期化
            var startNode = new Node(startPos, Node.Status.None, 0);

            // 推定の移動数設定
            startNode.SetEstimatedMovingCount(startPos, targetPos);

            // 開始ノードを追加
            nodes.Add(startNode);
        }

        /// <summary>
        /// 経路探索処理
        /// </summary>
        /// <param name="targetPosition">目標の位置</param>
        /// <param name="IsCollision">当たり判定か？の関数</param>
        /// <param name="SetRouteSearchTile">経路探索用のタイル配置関数</param>
        public async Task SearchPath(Vector2Int targetPosition, Func<Vector3Int, bool> IsCollision, Action<Vector3Int> SetRouteSearchTile)
        {
            // 探索するノード
            var searchNode = nodes[0];

            while (true)
            {
                // 自身がnullの場合処理終了
                if (this == null) return;

                // ターゲットに対しての８方向の位置データ作成
                var eightDirectionPositions = new Vector2Int[(int)EightDirection.Max];

                // ターゲットノードの8方向走査
                eightDirectionPositions[(int)EightDirection.TopRight] = new Vector2Int(searchNode.Position.x + 1, searchNode.Position.y - 1);// 上右
                eightDirectionPositions[(int)EightDirection.TopLeft] = new Vector2Int(searchNode.Position.x - 1, searchNode.Position.y - 1);// 上左
                eightDirectionPositions[(int)EightDirection.BottomLeft] = new Vector2Int(searchNode.Position.x - 1, searchNode.Position.y + 1);// 下左
                eightDirectionPositions[(int)EightDirection.BottomRight] = new Vector2Int(searchNode.Position.x + 1, searchNode.Position.y + 1);// 下右
                eightDirectionPositions[(int)EightDirection.Top] = new Vector2Int(searchNode.Position.x, searchNode.Position.y - 1);// 上
                eightDirectionPositions[(int)EightDirection.Left] = new Vector2Int(searchNode.Position.x - 1, searchNode.Position.y);// 左
                eightDirectionPositions[(int)EightDirection.Right] = new Vector2Int(searchNode.Position.x + 1, searchNode.Position.y);// 右
                eightDirectionPositions[(int)EightDirection.Bottom] = new Vector2Int(searchNode.Position.x, searchNode.Position.y + 1);// 下

                // 8方向ループ
                foreach (var position in eightDirectionPositions)
                {

                    // 同じ位置の物が存在する場合
                    if (nodes.Any(_node => _node.Position.Equals(position)))
                    {
                        // 次のループへ
                        continue;
                    }

                    // 開始ノード初期化(位置, ステータスをNone, 次の移動カウント)
                    var node = new Node(position, Status.None, searchNode.realMovingCount + 1);

                    // 当たり判定の場合
                    if (IsCollision((Vector3Int)position))
                    {
                        // ステータスを当たり判定にする
                        node.SetStatus(Status.Collision);

                        // 次のループへ
                        continue;
                    }

                    // 推定の移動数設定
                    node.SetEstimatedMovingCount(position, targetPosition);

                    // ステータスを探索対象にする
                    node.SetStatus(Status.SearchTarget);

                    // 親ノードに設定する
                    node.SetParentNode(searchNode);

                    // 探索対象のノードを追加していく
                    nodes.Add(node);
                }

                // 基準となるノードを探索済みにする
                searchNode.SetStatus(Status.Searched);

                // 次の探索するノードを決定する Where:探索対象であり、OrderBy:小さい順に並び変えて最初の物を取得する
                var nextSearchNode = nodes.Where(node => node.status.Equals(Status.SearchTarget))
                                    .OrderBy(node => node.GetEstimatedTotalMovingCount())
                                    .ThenBy(node => node.status)
                                    .FirstOrDefault();

                // 「ステータスが探索対象 && 次の探索するノード以外」&& 「推定の移動合計数 次の推定の移動合計数より大きい場合」 のステータスをすべて「探索保留」にする。
                nodes.Where(node => node.status.Equals(Status.SearchTarget) 
                         && node != nextSearchNode 
                         && node.GetEstimatedTotalMovingCount() > nextSearchNode.GetEstimatedTotalMovingCount()
                         )
                         .ToList()
                         .ForEach(node => node.SetStatus(Status.SearchOnHold));


                // 次に探索するターゲットが存在しない場合
                if (nextSearchNode == null)
                {
                    // 保留中のノードから、推定移動合計数の取得
                    nextSearchNode = nodes.Where(node => node.status.Equals(Status.SearchOnHold))
                                    .OrderBy(node => node.GetEstimatedTotalMovingCount())
                                    .ThenBy(node => node.status)
                                    .FirstOrDefault();
                }

                // 探索するノードを設定する
                searchNode = nextSearchNode;

                // 次に検索するノードが目標位置の場合
                if (nextSearchNode.Position.Equals(targetPosition))
                {
                    // ここでループ終了
                    break;
                }

                await Task.Delay(10);
            }

            // 目標位置からの親ノード
            var parentNode = searchNode.parentNode;

            // ルート位置リスト
            var rootPositionList = new List<Vector2Int>();

            // 親ノードがnullになるまで繰り返す
            while (parentNode != null)
            {
                // ルートノードに追加していく
                rootPositionList.Add(parentNode.Position);

                // 親ノードの親ノードを渡す
                parentNode = parentNode.parentNode;
            }

            // ルート位置リストを逆順にする
            rootPositionList.Reverse();

            foreach (var rootPosition in rootPositionList)
            {
                // 経路用のタイル配置
                SetRouteSearchTile((Vector3Int)rootPosition);

                await Task.Delay(100);
            }

            // 経路探索用のタイル配置
            //SetRouteSearchTile((Vector3Int)nextSearchNode.Position);


            // TODO:ループ処理、ループ回数をどうするか
        }
    }
}