using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Suv
{
    public partial class EnemyBase
    {
        public class StateReceivingDamage : EnemyStateBase
        {
            private CancellationTokenSource _ctsReceiveDamageCoolTime = null;

            public override void OnEnter(EnemyBase owner, EnemyStateBase prevState)
            {
                _ctsReceiveDamageCoolTime?.Cancel();
                _ctsReceiveDamageCoolTime = new CancellationTokenSource();
                OnReceveDamageCoolTimeAsync(owner, _ctsReceiveDamageCoolTime.Token).Forget();
            }

            // ダメージ判定後、一定時間停止してから動き出す
            private async UniTask OnReceveDamageCoolTimeAsync(EnemyBase owner, CancellationToken token)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(owner._receiveDamageCoolTime), cancellationToken: token);

                if (!token.IsCancellationRequested)
                {
                    owner.ChangeState(_stateMoving);
                }
            }

            public void OnDestroy()
            {
                _ctsReceiveDamageCoolTime?.Cancel();
                _ctsReceiveDamageCoolTime?.Dispose();
                _ctsReceiveDamageCoolTime = null;
            }
        }
    }
}
