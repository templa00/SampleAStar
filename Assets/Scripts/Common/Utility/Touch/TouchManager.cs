using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RtsProject.Assets.Scripts.Common.Utility.Touch
{
    /// <summary>
    /// タッチ管理
    /// </summary>
    public sealed class TouchManager : MonoBehaviour
    {
        /// <summary>
        /// 押された時
        /// </summary>
        public Action OnTouchBegan{ private get; set; }

        /// <summary>
        /// 押して移動している
        /// </summary>
        public Action OnTouchMoved{ private get;  set; }

        /// <summary>
        /// 離されたとき時
        /// </summary>
        public Action OnTouchEnded{ private get;  set; }

        /// <summary>
        /// 止まっている時
        /// </summary>
        public Action OnTouchStationary{ private get;  set; }

        /// <summary>
        /// キャンセル時
        /// </summary>
        public Action OnTouchCanceled{ private get;  set; }

        /// <summary>
        /// 更新中の場合
        /// </summary>
        private void Update ()
        {
            // UIを選択した場合ここで処理終了
            if (IsSelectedUI())return;

#if UNITY_EDITOR // UNITYエディタの場合

                if (Input.GetMouseButtonDown(0))
            {
                OnTouchBegan?.Invoke();
            }
            else if(Input.GetMouseButton(0))
            {
                OnTouchMoved?.Invoke();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                OnTouchEnded?.Invoke();
            }
#else // 実機の場合
            
            if (Input.touchCount > 0)
            {
                // 0番目のタッチ取得
                var touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                    {
                        OnTouchBegan?.Invoke();
                    }
                    break;
                    case TouchPhase.Moved:
                    {
                        OnTouchMoved?.Invoke();
                    }
                    break;
                    case TouchPhase.Ended:
                    {
                        OnTouchEnded?.Invoke();
                    }
                    break;
                    case TouchPhase.Stationary:
                    {
                        OnTouchStationary?.Invoke();
                    }
                    break;
                    case TouchPhase.Canceled:
                    {
                        OnTouchCanceled?.Invoke();
                    }
                    break;
                }
            }
#endif
        }

        /// <summary>
        /// タッチ位置を返す
        /// </summary>
        public Vector2 GetTouchPosition ()
        {
#if UNITY_EDITOR // UNITYエディタの場合
            return Input.mousePosition;
#else
            if (Input.touchCount > 0)
            {
                // 0番目のタッチ取得
                return Input.GetTouch(0).position;
            }

            return Vector2.zero;
#endif
        }

        /// <summary>
        /// カメラ座標に変換したタッチ位置を返す
        /// </summary>
        public Vector2 GetCameraTouchPosition (Camera camera)
        {
#if UNITY_EDITOR // UNITYエディタの場合
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
#else
            if (Input.touchCount > 0)
            {
                // 0番目のタッチ取得
                return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            return Vector2.zero;
#endif
        }

        /// <summary>
        /// UIを選択したか？
        /// </summary>
        /// <returns></returns>
        public bool IsSelectedUI ()
        {
            //ボタンとかクリックされていたら無効にする
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                return true;
            }
            //ImageとかTextでも被っていれば無効にする（マウスクリック）
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            //ImageとかTextでも被っていれば無効にする（タップ）
            if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return true;
            }

            return false;
        }
    }
}
