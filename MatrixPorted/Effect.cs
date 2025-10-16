using System.Timers;
using Timer = System.Timers.Timer;

namespace MatrixPorted
{
	public abstract class Effect {
		public TerminalCharFlag[,] terminalTargetMask;
		public TerminalCharFlag[,] terminalMask;
		public (char, int, int)[,] terminalContent;
		public Timer updateTimer = new Timer(40);
		public abstract void UpdateTimer(Object? obj, System.Timers.ElapsedEventArgs _);
		public event FinishedDelegate Finished;
	
		public delegate void FinishedDelegate();
		public Effect(TerminalCharFlag[,] targetmask, TerminalCharFlag[,] mask, (char, int, int)[,] terminalContent)
		{
			this.terminalTargetMask = targetmask;
			this.terminalMask = mask;
			updateTimer.Elapsed += UpdateTimer;
			updateTimer.AutoReset = true;
			updateTimer.Enabled = true;
			this.terminalContent = terminalContent;
		}
		protected void FinishEffect()
		{
			updateTimer.AutoReset = false;
			updateTimer.Enabled = false;
			Finished?.Invoke();
		}
		protected void StartEffect()
		{
			updateTimer.AutoReset = false;
			updateTimer.Enabled = false;
		}
		protected void StopEffect() => FinishEffect();
	}
}
