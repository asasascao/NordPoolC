using System.Threading.Tasks;

namespace CaoNC.System.Channels
{
	internal sealed class TaskCompletionSource : TaskCompletionSource<VoidResult>
	{
		public TaskCompletionSource(CaoNC.TaskCreationOpts creationOptions)
			: base(creationOptions)
		{
		}

		public bool TrySetResult()
		{
			return TrySetResult(default(VoidResult));
		}
	}
}
