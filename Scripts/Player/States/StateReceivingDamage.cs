using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class PlayerCharacter
    {
        public class StateReceivingDamage : PlayerStateBase
        {
            private bool isCoolTime = false;
            private float coolTime = 1.0f;
            private CancellationTokenSource _ctsReceiveDamage = null;

            // このステートに来る前に確認する用
            public bool IsReceiveDamageCoolTime => isCoolTime;

            public override void OnEnter(PlayerCharacter owner, PlayerStateBase prevState)
            {
                _ctsReceiveDamage?.Cancel();
                _ctsReceiveDamage = new CancellationTokenSource();
                OnReceiveDamageCoolTimeCount(_ctsReceiveDamage.Token).Forget();
            }

            // すぐにIdleに戻す
            public override void OnUpdate(PlayerCharacter owner)
            {
                owner.ChangeState(_stateIdling);
            }

            private async UniTask OnReceiveDamageCoolTimeCount(CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(coolTime), cancellationToken: token);

                if (!token.IsCancellationRequested)
                {
                    isCoolTime = false;
                }
            }

            public void OnDestroy()
            {
                _ctsReceiveDamage?.Cancel();
                _ctsReceiveDamage?.Dispose();
                _ctsReceiveDamage = null;
            }
        }
    }
}
