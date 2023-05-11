using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Suv
{
    public class EnemyGenerator : MonoBehaviour
    {
        [SerializeField] GameObject _enemysParent;
        public List<EnemyStatusData> _enemyStatusDatas = new List<EnemyStatusData>();

        private const float _generateTimeSpan = 30.0f;
        private bool _isGenerating = false;
        private CancellationTokenSource _ctsGenerater = null;

        private EnemyBase _enemyBase = null;

        private void Update()
        {
            if (_isGenerating) return;

            _isGenerating = true;
            _ctsGenerater?.Cancel();
            _ctsGenerater = new CancellationTokenSource();
            OnGenerateEnemyAsync(_ctsGenerater.Token).Forget();
        }

        private async UniTask OnGenerateEnemyAsync(CancellationToken token)
        {
            // プレハブ未読込なら読み込む
            if (!_enemyBase)
            {
                var request = Resources.LoadAsync<EnemyBase>(ConstStringManager.RESOURCES_ENEMY);
                await request;

                _enemyBase = request.asset as EnemyBase;
            }

            int enemyCount = 0; 
            while(enemyCount < 5)
            {
                await UniTask.DelayFrame(1, cancellationToken: token);
                if (token.IsCancellationRequested) break;

                enemyCount++;
                Vector3 generatePos = new Vector3(10, 0.5f, 2 * enemyCount);
                EnemyBase newEnemy = Instantiate(_enemyBase, generatePos, Quaternion.identity, _enemysParent.transform);
                newEnemy.gameObject.name = newEnemy.gameObject.name + enemyCount;
                newEnemy.Initialize(_enemyStatusDatas[0]);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_generateTimeSpan), cancellationToken: token);
            if (!token.IsCancellationRequested) _isGenerating = false;
        }

        private void OnDestroy()
        {
            _ctsGenerater?.Cancel();
            _ctsGenerater?.Dispose();
            _ctsGenerater = null;
        }
    }
}
