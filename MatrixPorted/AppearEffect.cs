using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixPorted
{
    class AppearEffect : Effect {
		(int, int) fixed_offset;
		int tick_counter = 1;
		int text_amount = 0;
		bool enable_tick_counter = false;
		public AppearEffect((int, int) fixedoffset, int textamount, bool[,] targetmask, bool[,] mask, (char, int, int)[,] content) : base(targetmask, mask, content)
		{
			this.fixed_offset = fixedoffset;
		}
		public override void UpdateTimer(Object? __, System.Timers.ElapsedEventArgs _)
		{
			Random random = new Random();
			if (!this.enable_tick_counter) {
				this.enable_tick_counter = true;
				foreach ((char, int, int) ele in this.terminalContent) {
					if (ele.Item1 != ' ') {
						this.enable_tick_counter = false;
					}
				}
			} else {
				this.tick_counter += 1;
			}

			if (this.tick_counter % 4 == 0 && this.enable_tick_counter) {
				if (this.tick_counter / 4 == text_amount) {
					this.FinishEffect();
				} else {
					int xoffset = this.tick_counter / 4 * 8 - 8 + fixed_offset.Item1;
					for (int x = xoffset; x < xoffset + 8; x++) {
						for (int y = 0; y < this.terminalMask.GetLength(1); y++) {
							if (this.terminalTargetMask[x, y]) {
								this.terminalMask[x, y] = true;
								this.terminalContent[x, y] = (
									(char)(random.Next() % ('z' - '!') + '!'),
									random.Next() % 128,
									((random.Next() % 2) * 2 - 1) * (random.Next() % 5 + 1)
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
