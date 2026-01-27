using JetBrains.Lifetimes;

namespace ECS.FSM
{
	public interface IStateEnter : IStateHandler
	{
		void Enter(Lifetime lifetime);
	}
}