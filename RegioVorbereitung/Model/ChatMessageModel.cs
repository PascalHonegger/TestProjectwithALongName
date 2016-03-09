using System;

namespace RegioVorbereitung.Model
{
	public class ChatMessageModel
	{
		public int Id { get; }

		public EmployeeModel Author { get; }

		public string Message { get; }

		public DateTime Created { get; }

		public ChatMessageModel(int id, EmployeeModel author, string message, DateTime created)
		{
			Id = id;
			Author = author;
			Message = message;
			Created = created;
		}

		public string FormattedString => $"[{Created}] {Author.Name}: {Message}";

		public override bool Equals(object obj)
		{
			var other = obj as ChatMessageModel;

			return other != null && Equals(other.Id, Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}
