using RtsProject.Assets.Scripts.Common.Utility.Touch;
using UnityEngine;
using RtsProject.Assets.Scripts.Data;

namespace RtsProject.Assets.Scripts.Scene.TileMapSample
{
    /// <summary>
    /// タイルマップサンプルのシーン管理
    /// </summary>
    public sealed class SceneManager : MonoBehaviour
    {
        /// <summary>
        /// タイルUIパネル
        /// </summary>
        [SerializeField] private TileUIPanel tileUIPanel;

        /// <summary>
        /// キャラクター
        /// </summary>
        [SerializeField] private Character character;

        /// <summary>
        /// タッチ管理
        /// </summary>
        [SerializeField] private TouchManager touch;

        /// <summary>
        /// バトルUIパネル
        /// </summary>
        [SerializeField] private BattleUIPanel battleUIPanel;

        /// <summary>
        /// 起動時
        /// </summary>
        private void Awake ()
        {
            // TODO:マップ用データを取得する 要修正
            var mapData = new MapData(new Vector3Int(0, -2, 0));

            // 拡大ボタンを押したとき
            battleUIPanel.OnClickScaleUpButtonCallBack = () =>
            {
                tileUIPanel.ScaleUp();
            };

            // 縮小ボタンを押したとき
            battleUIPanel.OnClickScaleDownButtonCallBack = () =>
            {
                tileUIPanel.ScaleDown();
            };

            // キャラクター初期位置の設定
            var worldCharacterInitPosition = tileUIPanel.GetInitCharacterWorldPosition(mapData.CharacterInitCellPosition);

            // 位置の設定
            character.SetPosition(worldCharacterInitPosition);

            // タッチ押した時
            touch.OnTouchBegan = async () =>
            {
                // カメラのワールド座標に変換
                var touchCameraWorldPosition = touch.GetCameraTouchPosition(Camera.main);

                // ワールド座標をタイルマップのセル座標に変換
                var touchTileCellPosition = tileUIPanel.GetWorldToCellPosition(touchCameraWorldPosition);

                // 当たり判定をタッチした場合、ここで処理終了
                if (tileUIPanel.IsCollision(touchTileCellPosition)) return;

                // タッチした位置に選択用タイルを配置する
                tileUIPanel.SetSelectTile(touchTileCellPosition);

                // A-Starアルゴリズム用のクラス生成
                var astar = new Astar((Vector2Int)mapData.CharacterInitCellPosition, (Vector2Int)touchTileCellPosition);

                // 経路探索処理
                await astar.SearchPath((Vector2Int)touchTileCellPosition, tileUIPanel.IsCollision, tileUIPanel.SetRouteSearchTile);
            };
        }
    }
}