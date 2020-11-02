using UnityEngine;
using UnityEngine.Tilemaps;

namespace RtsProject.Assets.Scripts.Scene.TileMapSample
{
    /// <summary>
    /// タイトルUIパネル
    /// </summary>
    public sealed class TileUIPanel : MonoBehaviour
    {
        /// <summary>
        /// グリッド
        /// </summary>
        [SerializeField] private Grid grid;

        /// <summary>
        /// フィールドタイルマップ
        /// </summary>
        [SerializeField] private Tilemap fieldTilemap;

        /// <summary>
        /// オブジェクトタイルマップ
        /// </summary>
        [SerializeField] private Tilemap objectTilemap;

        /// <summary>
        /// 高いオブジェクトタイルマップ(キャラクターより高い)
        /// </summary>
        [SerializeField] private Tilemap highObjectTilemap;

        /// <summary>
        /// 当たり判定用のタイルマップ
        /// </summary>
        [SerializeField] private Tilemap collisionTilemap;

        /// <summary>
        /// 位置用タイルマップ
        /// </summary>
        [SerializeField] private Tilemap positionTilemap;

        /// <summary>
        /// 選択用タイルマップ
        /// </summary>
        [SerializeField] private Tilemap selectTilemap;

        /// <summary>
        /// 経路探索用タイルマップ
        /// </summary>
        [SerializeField] private Tilemap routeSearchTilemap;

        /// <summary>
        /// 初期位置のタイル
        /// </summary>
        [SerializeField] private Tile initPositionTile;

        /// <summary>
        /// 選択用タイル
        /// </summary>
        [SerializeField] private Tile selectTile;

        /// <summary>
        /// 当たり判定用タイル
        /// </summary>
        [SerializeField] private Tile collisionTile;

        /// <summary>
        /// 経路探索用タイルマップ
        /// </summary>
        [SerializeField] private Tile routeSearchTile;

        /// <summary>
        /// 起動時
        /// </summary>
        private void Awake ()
        {
            Debug.Log("=============================");
            Debug.Log("collisionTilemapのサイズ");
            Debug.Log(collisionTilemap.size);

            Debug.Log("fieldTilemapのサイズ");
            Debug.Log(fieldTilemap.size);
            fieldTilemap.CompressBounds();

            Debug.Log(fieldTilemap.size);
            Debug.Log("エディターからTilemapのマップサイズの変更はできないようだ。。。");
            Debug.Log("Tilemapのマップサイズはあまり使わないほうがいい？");

            collisionTilemap.GetTile(new Vector3Int(0, 0, 0));
            Debug.Log("=============================");
        }

        /// <summary>
        /// タイルマップ上でのキャラクターの初期位置を設定する
        /// </summary>
        /// <param name="position">タイルマップ上でのキャラクターの初期位置</param>
        /// <returns>キャラクターのワールド座標の初期位置</returns>
        public Vector3 GetInitCharacterWorldPosition (Vector3Int position)
        {
            // 位置用タイルマップに初期位置の設定を行う
            positionTilemap.SetTile(position, initPositionTile);

            // キャラクターの初期位置をワールド座標に変換
            var worldInitPosition = positionTilemap.GetCellCenterWorld(position);

            return worldInitPosition;
        }

        /// <summary>
        /// 対象の位置に選択用タイルを配置する
        /// </summary>
        /// <param name="position">対象のタイル位置</param>
        public void SetSelectTile (Vector3Int position)
        {
            // タイルをすべてクリアする
            selectTilemap.ClearAllTiles();

            // セレクトタイルを配置
            selectTilemap.SetTile(position, selectTile);
        }

        /// <summary>
        /// ワールド位置をセルの位置に変換する
        /// ※selectTilemapで取得する
        /// ※ほかのマップでも同じ位置が返って来るはず
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3Int GetWorldToCellPosition (Vector2 position)
        {
            // ワールド座標からセル座標に変換
            var tilePosition = selectTilemap.WorldToCell(position);

            return tilePosition;
        }

        /// <summary>
        /// 当たり判定か？
        /// </summary>
        /// <param name="position">セルの位置</param>
        /// <returns></returns>
        public bool IsCollision (Vector3Int position) => (collisionTile == collisionTilemap.GetTile(position));

        /// <summary>
        /// 対象の位置に経路探索タイルを配置する
        /// </summary>
        /// <param name="position">対象のタイル位置</param>
        public void SetRouteSearchTile (Vector3Int position)
        {
            // 経路探索タイルを配置
            routeSearchTilemap.SetTile(position, routeSearchTile);
        }

        /// <summary>
        /// 拡大
        /// </summary>
        public void ScaleUp ()
        {
            if (transform.localScale.x < 1.0f && transform.localScale.y < 1.0f)
            {
                transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y + 0.1f);
            }
        }

        /// <summary>
        /// 縮小
        /// </summary>
        public void ScaleDown ()
        {
            if (transform.localScale.x > 0.1f && transform.localScale.y > 0.1f)
            {
                transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f);
            }
        }
    }
}
