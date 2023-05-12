using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Suv
{
    public class DamageUIManager : MonoBehaviour
    {
        private DamageText _damageTextPrefab;

        private void Awake()
        {
            OnLoadDamageText().Forget();
        }

        private async UniTask OnLoadDamageText()
        {
            var request = Resources.LoadAsync<DamageText>(ConstStringManager.RESOURCES_DAMAGETEXT);
            await request;

            _damageTextPrefab = request.asset as DamageText;
        }

        public void PopDamageText(Vector3 enemyPos, float damage)
        {
            if (!_damageTextPrefab) return;
            enemyPos.y += 1.0f;

            DamageText damageText = Instantiate(_damageTextPrefab, enemyPos, Quaternion.identity, transform);
            damageText.StartPop(damage.ToString());
        }
    }
}