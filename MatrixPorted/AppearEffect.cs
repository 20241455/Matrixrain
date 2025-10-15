using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixPorted
{
    class AppearEffect : Effect {
		int tick_counter = 1;
		int text_amount = 0;
		bool enable_tick_counter = false;
		public AppearEffect(int textamount, bool[,] targetmask, bool[,] mask, (char, int, int)[,] content) : base(targetmask, mask, content)
		{
		}
		public override void UpdateTimer(Object? __, System.Timers.ElapsedEventArgs _)
		{
			Random random = new Random();
			if (!this.enable_tick_counter) {
				this.enable_tick_counter = true;
				for (int x = 0; x < this.terminalContent.GetLength(0); x++) {
					for (int y = 0; y < this.terminalContent.GetLength(1) - Font.SKULL.GetLength(0); y++) {
						if (this.terminalContent[x, y].Item1 != ' ') {
							this.enable_tick_counter = false;
							break;
						}
					}
				}
			} else {
				this.tick_counter += 1;
			}

			if (this.tick_counter % 4 == 0 && this.enable_tick_counter) {
				if (this.tick_counter / 4 == Console.WindowWidth) {
					this.FinishEffect();
				} else {
					int xoffset = this.tick_counter / 4 * 8 - 8;
					for (int x = xoffset; x < xoffset + 8; x++) {
						for (int y = 0; y < this.terminalMask.GetLength(1) - Font.SKULL.GetLength(0); y++) {
							if (this.terminalTargetMask[x, y]) {
								this.terminalMask[x, y] = true;
								this.terminalContent[x, y] = (
									(char)(random.Next() % ('z' - '!') + '!'),
									random.Next() % 88 + 40,
									random.Next() % 5 - 2
								);
							} else {
								this.terminalMask[x, y] = false;
								this.terminalContent[x, y] = (' ', 0, 0);
							}
						}
					}
				}
			}
		}
    }
}
