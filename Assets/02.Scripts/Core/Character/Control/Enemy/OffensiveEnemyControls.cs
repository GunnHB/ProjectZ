using System;
using System.Collections.Generic;

using ProjectZ.Core.BehaviorTree;

namespace ProjectZ.Core.Characters
{
    public class OffensiveEnemyControls : EnemyControls
    {
        private float _combatIdleTime = 2f;

        protected override void InitRootNodeList()
        {
            AppendBT(AttackNode());
            AppendBT(ReadyToAttackNode());
            AppendBT(DetectTargetNode());

            base.InitRootNodeList();
        }

        private SequenceNode AttackNode()
        {
            return new SequenceNode(new List<INode>()
            {
                new ActionNode(CheckAttacking),     // 공격 중인지
                new ActionNode(CheckAttackRange),   // 공격 가능 범위인지
                new ActionNode(DoAttack),           // 공격
            });
        }

        #region AttackNode
        private INode.ENodeState CheckAttacking()
        {
            throw new NotImplementedException();
        }

        private INode.ENodeState CheckAttackRange()
        {
            throw new NotImplementedException();
        }

        private INode.ENodeState DoAttack()
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region ReadyToAttackNode
        private SequenceNode ReadyToAttackNode()
        {
            return new SequenceNode(new List<INode>()
            {
                new ActionNode(DoCombatIdle),   // 공격 대기
            });
        }

        private INode.ENodeState DoCombatIdle()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region DetectTargetNode
        private SequenceNode DetectTargetNode()
        {
            var nodeList = new List<INode>()
            {
                new ActionNode(DoDetectTarget),
                new ActionNode(DoChaseTarget),
            };

            return new SequenceNode(nodeList);
        }

        private INode.ENodeState DoDetectTarget()
        {
            throw new NotImplementedException();
        }

        private INode.ENodeState DoChaseTarget()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}