using UnityEngine;

[CreateAssetMenu(menuName = "Create EnemyStatus")]
public class EnemyStatusData : ScriptableObject
{
	// ----------------------------------------------
	// 設定項目
	[SerializeField]
	private Sprite _sprite = null;

	[SerializeField]
	private string _name = "";

	[SerializeField]
	private float _hp = 100.0f;

	[SerializeField]
	private float _attackPow = 2.0f;

	[SerializeField]
	private float _moveSpped = 5.0f;

	[SerializeField]
	public float _moveWaitTime = 0.5f;

	[SerializeField]
	public float _receiveDamageCoolTime = 0.5f;

	// ----------------------------------------------
	// アクセサ
	public Sprite Sprite => _sprite;

	public string Name => _name;

	public float Hp => _hp;

	public float AttackPow => _attackPow;

	public float MoveSpeed => _moveSpped;

	public float MoveWaitTime => _moveWaitTime;

	public float ReceiveDamageCoolTime => _receiveDamageCoolTime;
}
