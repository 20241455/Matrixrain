using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixPorted
{
	public class RainEffect : Effect {
		int[] rainMask;
		bool targetMaskValue = false;
		bool reactivate_effect;
		public RainEffect(TerminalCharFlag[,] targetmask, TerminalCharFlag[,] mask, (char, int, int)[,] content, bool activeeffect) : base(targetmask, mask, content)
		{
			this.reactivate_effect = activeeffect;
			Random random = new Random();
			rainMask = new int[mask.GetLength(0)];
			for (int idx = 0; idx < rainMask.Length; idx++) {
				rainMask[idx] = random.Next() % 20 - 19;
			}
		}
		public override void UpdateTimer(Object __, System.Timers.ElapsedEventArgs _)
		{
			Random random = new Random();
			for (int x = 0; x < this.terminalMask.GetLength(0); x++) {
				for (int y = 0; y < this.terminalMask.GetLength(1) - Font.SKULL.GetLength(0); y++) {
					this.terminalMask[x, y] &= ~TerminalCharFlag.NoRespawn;
				}
			}
			bool updated = false;
			for (int idx = 0; idx < rainMask.Length; idx++) {
				if (rainMask[idx] < this.terminalMask.GetLength(1) && rainMask[idx] >= 0) {
					updated = true;
					if (this.reactivate_effect) {
						this.terminalMask[idx, rainMask[idx]] = 0;
						this.terminalContent[idx, rainMask[idx]].Item1 = (char)(random.Next() % ('z' - '!') + '!');
						this.terminalContent[idx, rainMask[idx]].Item2 = random.Next() % 88 + 40;
						this.terminalContent[idx, rainMask[idx]].Item3 = (random.Next() % 5) - 2;
					} else {
						if (((this.terminalTargetMask[idx, rainMask[idx]] & TerminalCharFlag.NoRespawn) != 0) != targetMaskValue) {
							this.terminalMask[idx, rainMask[idx]] |= TerminalCharFlag.NoRespawn;
							this.terminalContent[idx, rainMask[idx]].Item1 = ' ';
							this.terminalContent[idx, rainMask[idx]].Item2 = 0;
							this.terminalContent[idx, rainMask[idx]].Item3 = 0;
						} else {
							this.terminalMask[idx, rainMask[idx]] = this.terminalTargetMask[idx, rainMask[idx]];
							this.terminalContent[idx, rainMask[idx]].Item3 = (random.Next() % 5) - 2;
						}
					}
				}
				rainMask[idx] += 1;
			}
			if (!updated) {
				if (this.reactivate_effect) {
					rainMask = new int[terminalMask.GetLength(0)];
					for (int idx = 0; idx < rainMask.Length; idx++) {
						rainMask[idx] = random.Next() % 20 - 19;
					}
					this.reactivate_effect = false;
				} else {
					this.FinishEffect();
				}
			}
		}
	}
}
