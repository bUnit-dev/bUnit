namespace Bunit;

/// <summary>
/// Represents a failure when an asynchronous task didn't complete at a given point.
/// </summary>
[Serializable]
public class TaskNotCompletedException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TaskNotCompletedException"/> class.
	/// </summary>
	public TaskNotCompletedException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskNotCompletedException"/> class.
	/// </summary>
	protected TaskNotCompletedException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="TaskNotCompletedException"/> class.
	/// </summary>
	protected TaskNotCompletedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
