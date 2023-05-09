using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class PlayerCharacter
    {
        public class StateMoving : PlayerStateBase
        {
            private const float ChangeStandingStateAwaitTime = 1.0f;
            private bool isChanging = false;
            private CancellationTokenSource _cts = null;

            private float _curvePow = 1.0f;
            private float _addForcePow = 10.0f;

            public float CurvePow => _curvePow;
            public float AddForcePow => _addForcePow;

            public override void OnUpdate(PlayerCharacter owner)
            {
                Move(owner);
            }

            private void Move(PlayerCharacter owner)
            {
                if (owner._playerInput.actions["Move"].IsPressed())
                {
                    Vector2 inputVec = owner._playerInput.actions["Move"].ReadValue<Vector2>();
                    // 左右のカーブ調整
                    if (inputVec.x != 0)
                    {
                        _curvePow = Mathf.Clamp(_curvePow + inputVec.x * Time.deltaTime, owner.baseCurvePow - 0.2f, owner.baseCurvePow + 0.2f); ;
                        Debug.Log(_curvePow);
                    }
                    // 速度調整
                    if (inputVec.y != 0)
                    {
                        _addForcePow = Mathf.Clamp(_addForcePow + inputVec.y * Time.deltaTime, owner.baseAddforcePow - 2.0f, owner.baseAddforcePow + 2.0f);
                        Debug.Log(_addForcePow);
                    }
                }

                /*
                Vector2 moveVec = owner.playerInput.actions["Move"].ReadValue<Vector2>();

                // 未入力中は一定時間後にStandingへ遷移するようにする
                if (moveVec == Vector2.zero && !isChanging)
                {
                    isChanging = true;
                    _cts?.Cancel();
                    _cts = new CancellationTokenSource();
                    OnStandStateChanging(owner, _cts.Token).Forget();
                }
                //  入力しているので移動処理
                else if (moveVec != Vector2.zero)
                {
                    isChanging = false;
                    _cts?.Cancel();

                    moveVec *= Time.deltaTime;
                    Vector3 newPos = new Vector3(owner.transform.position.x + moveVec.x, owner.transform.position.y, owner.transform.position.z + moveVec.y);
                    owner.transform.position = newPos;
                }
                */
            }

            private async UniTask OnStandStateChanging(PlayerCharacter owner, CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ChangeStandingStateAwaitTime), cancellationToken: token);

                // 遷移までにキャンセルが発生(再移動)が行われていない
                if (!token.IsCancellationRequested)
                {
                    owner.ChangeState(_stateIdling);
                    isChanging = false;
                }
            }

            public void OnDestroy()
            {
                _cts?.Cancel();
                _cts?.Dispose();
                _cts = null;
            }
        }
    }
}