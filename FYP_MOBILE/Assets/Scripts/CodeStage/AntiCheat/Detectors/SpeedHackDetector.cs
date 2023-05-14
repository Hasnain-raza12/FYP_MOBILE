namespace CodeStage.AntiCheat.Detectors
{
	public class SpeedHackDetector : ActDetectorBase
	{
		public float interval;

		public byte maxFalsePositives;

		public int coolDown;

		private SpeedHackDetector()
		{
		}
	}
}
