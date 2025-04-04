public class Define
{
	public enum Layer
	{
		Monster = 8,
		Player = 9,
		Ground = 10,
		Block = 11,
	}

	public enum CursorType
	{
		None,
		Attack,
		Hand,
	}

	public enum Scene
	{
		Unknown,
		Login,
		Lobby,
		Game,
	}

	public enum Sound
	{
		Bgm,
		Effect,
		MaxCount,
	}

	public enum UIEvent
	{
		Click,
		Drag,
	}

	public enum MouseEvent
	{
		Press,
		PointerDown,
        PointerUp,
		Click,
	}

	public enum CameraMode
	{
		QuaterView,
		BackView,
	}
}
   
