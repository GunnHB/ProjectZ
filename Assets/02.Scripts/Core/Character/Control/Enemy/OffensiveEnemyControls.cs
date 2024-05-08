using System;
using System.Collections.Generic;

using ProjectZ.Core.BehaviorTree;

namespace ProjectZ.Core.Characters
{
    public class OffensiveEnemyControls : EnemyControls
    {
        protected override void InitRootNodeList()
        {
            base.InitRootNodeList();

            AppendBT(AttackNode());
            AppendBT(ReadyToAttackNode());
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
                new ActionNode(DoCombatIdle),
            });
        }

        private INode.ENodeState DoCombatIdle()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}