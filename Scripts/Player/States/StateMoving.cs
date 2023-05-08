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

            public override void OnUpdate(PlayerCharacter owner)
            {
                Move(owner);
            }

            private void Move(PlayerCharacter owner)
            {
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

                    moveVec *= 0.5f;
                    Vector3 newPos = new Vector3(owner.rectTransform.position.x + moveVec.x, owner.rectTransform.position.y + moveVec.y, owner.rectTransform.position.z);
                    owner.rectTransform.position = newPos;
                }
            }

            private async UniTask OnStandStateChanging(PlayerCharacter owner, CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(ChangeStandingStateAwaitTime), cancellationToken: token);

                // 遷移までにキャンセルが発生(再移動)が行われていない
                if (!token.IsCancellationRequested)
                {
                    owner.ChangeState(_stateStanding);
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