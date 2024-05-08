using System.Collections.Generic;
using System.Linq;

namespace ProjectZ.Core.Characters
{
    public partial class EnemyControls : CharacterControls
    {
        private BehaviorTree.BehaviorTreeRunner _btRunner;

        private List<BehaviorTree.INode> _rootNodeList;

        /// <summary>
        /// 행동트리 초기화
        /// <para>~ 메서드의 재정의는 필수 (안그러면 기본 행동만 해요우) ~</para>
        /// </summary>
        protected virtual void InitRootNodeList()
        {
            // 적이라면 꼭 실행해야하는 행동 트리는 여기에 적으요
            // 새로운 행동트리는 appendbt나 insertbt를 사용하여요
            _rootNodeList = new()
            {

            };
        }

        private void InitBTRunner()
        {
            _btRunner = new BehaviorTree.BehaviorTreeRunner(new BehaviorTree.SelectorNode(_rootNodeList));
        }

        private void OperateBTRunner()
        {
            _btRunner.Operate();
        }

        /// <summary>
        /// 맨 뒤에 추가할 노드
        /// </summary>
        /// <param name="node"></param>
        protected void AppendBT(BehaviorTree.INode node)
        {
            _rootNodeList.Append(node);
        }

        /// <summary>
        /// 맨 앞에 추가할 노드
        /// </summary>
        /// <param name="nodeList"></param>
        protected void InsertBT(BehaviorTree.INode node)
        {
            _rootNodeList.Insert(0, node);
        }
    }
}
