using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using ProjectZ.Core.BehaviorTree;
using System;

namespace ProjectZ.Core.Characters
{
    public partial class EnemyControls : CharacterControls
    {
        private BehaviorTreeRunner _btRunner;

        private List<INode> _rootNodeList = new();

        /// <summary>
        /// 행동트리 초기화
        /// <para>~ 메서드의 재정의는 필수 (안그러면 기본 행동만 해요우) ~</para>
        /// </summary>
        protected virtual void InitRootNodeList()
        {
            // 꼭 실행해야하는 행동 트리는 여기에 적으요
            // 새로운 행동은 상속받은 클래스에서 추가해주기
            AppendBT(new ActionNode(IdleNode));
        }

        private INode.ENodeState IdleNode()
        {
            return INode.ENodeState.RunningState;
        }

        private void InitBTRunner()
        {
            _btRunner = new BehaviorTreeRunner(new SelectorNode(_rootNodeList));
        }

        private void OperateBTRunner()
        {
            _btRunner.Operate();
        }

        /// <summary>
        /// 맨 뒤에 추가할 노드
        /// </summary>
        /// <param name="node"></param>
        protected void AppendBT(INode node)
        {
            _rootNodeList.Append(node);
        }

        /// <summary>
        /// 맨 앞에 추가할 노드
        /// </summary>
        /// <param name="nodeList"></param>
        protected void InsertBT(INode node)
        {
            _rootNodeList.Insert(0, node);
        }
    }
}
