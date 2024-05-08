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
        }

        private SelectorNode AttackNode()
        {
            return new SelectorNode(new List<INode>());
        }
    }
}
