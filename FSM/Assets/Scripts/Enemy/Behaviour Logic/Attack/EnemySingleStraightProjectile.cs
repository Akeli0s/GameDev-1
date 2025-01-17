using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Attac-Straigh-Single Prohectile",
    menuName = "Enemy Logic/Attack Logic/Straight Single Wander"
)]
public class EnemySingleStraightProjectile : EnemyAttackSOBase
{
    private float _timer;
    private float _exitTimer;

    [SerializeField]
    private float _timeBetweenShots = 2f;

    [SerializeField]
    private float _timeTillExit = 1f;

    [SerializeField]
    private float _distanceToCountExit = 3f;

    [SerializeField]
    public float _bulletSpeed = 10f;

    [SerializeField]
    public Rigidbody2D BulletPrefab;

    public override void DoAnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.DoAnimationTriggerEventLogic(triggerType);
    }

    public override void DoEnterLogic()
    {
        base.DoEnterLogic();
    }

    public override void DoExitLogic()
    {
        base.DoExitLogic();
    }

    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        enemy.MoveEnemy(Vector2.zero);

        if (_timer > _timeBetweenShots)
        {
            _timer = 0f;

            Vector2 dir = (playerTransform.position - enemy.transform.position).normalized;

            Rigidbody2D bullet = GameObject.Instantiate(
                BulletPrefab,
                enemy.transform.position,
                Quaternion.identity
            );
            bullet.velocity = dir * _bulletSpeed;
        }

        if (!enemy.IsWithStrinikingDistance)
        {
            _exitTimer += Time.deltaTime;
            if (_exitTimer > _timeTillExit)
            {
                enemy.StateMachine.ChangeState(enemy.ChaseState);
            }
        }
        else
        {
            _exitTimer = 0f;
        }

        _timer += Time.deltaTime;
    }

    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }

    public override void Initialize(GameObject gameObject, Enemy enemy)
    {
        base.Initialize(gameObject, enemy);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
