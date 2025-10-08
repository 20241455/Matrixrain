using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MatrixPorted {
	class Program {
		static ulong[] FONT = {
			0x0, // SPACE
			0x1818001818181818, // !
			0x0000000066666600, // "
			0x24247e24247e2424, // #
			0x183e587c3e1a7c18, // $
			0x2152240c18122542, // %
			0x0, // &
			0x0000000018181800, // '
			0x380c040404040c38, // (
			0x1c3020202020301c, // )
			0x003c7e7e7e7e3c00, // *
			0x0018187e7e181800, // +
			0x000c181800000000, // ,
			0x0000003c3c000000, // -
			0x0018180000000000, // .
			0x0002040810204000, // /
			0x003c464a52623c00, // 0
			0x007e101012141800, // 1
			0x007e041820423c00, // 2
			0x003c40403c403c00, // 3
			0x0010107c12120200, // 4
			0x003e40403e027e00, // 5
			0x003c42423e027c00, // 6
			0x0002040810207e00, // 7
			0x003c42423c423c00, // 8
			0x007e40407e427e00, // 9
			0x0000181800181800, // :
			0x000c181800181800, // ;
			0x00700c02020c7000, // <
			0x00007e00007e0000, // =
			0x000e304040300e00, // >
			0x040004043c20203c, // ?
			0x0, // @
			0x414141417f41413e, // A
			0x3f4141413f41413f, // B
			0x7f0101010101017f, // C
			0x1f2141414141211f, // D
			0x7f0101011f01017f, // E
			0x010101011f01017f, // F
			0x7f4141710101417f, // G
			0x414141417f414141, // H
			0x7f0808080808087f, // I
			0x3844424040404040, // J
			0x2111090503050911, // K
			0x7f01010101010101, // L
			0x4141414149556341, // M
			0x6151514949454543, // N
			0x7f4141414141417f, // O
			0x010101017f41417f, // P
			0x30107f414141417f, // Q
			0x412111097f41417f, // R
			0x3f4040403e01017e, // S
			0x080808080808087f, // T
			0x3e41414141414141, // U
			0x0808141422224141, // V
			0x14142a2a41414141, // W
			0x4122140808142241, // X
			0x0808080808142241, // Y
			0x7f0204080810207f, // Z
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x5e61417e403e0000, // a
			0x7f4141417f010100, // b
			0x7f010101017f0000, // c
			0x7f4141417f404000, // d
			0x3e013f41413e0000, // e
			0x080808083e083000, // f
			0x3f407e4141615e00, // g
			0x414141413f010100, // h
			0x3c18181c00180000, // i
			0xc121010100010000, // j
			0x42221a2642020200, // k
			0x1824040404040400, // l
			0x49494949493f0000, // m
			0x41414141413f0000, // n
			0x3e414141413e0000, // o
			0x01017f4141417f00, // p
			0x40407f4141417f00, // q
			0x02020202261a0000, // r
			0x7f40407f017f0000, // s
			0x300808083e080800, // t
			0x3e41414141410000, // u
			0x8141422224100000, // v
			0x142a494141410000, // w
			0x4136080836410000, // x
			0x2040814224100000, // y
			0x7e040810207e0000  // z
		};

		static Timer updateTimer = new(50);
		static (char, int, int)[,] terminalContent = new (char, int, int)[0, 0];
		static bool[,] terminalLifetimeMask = new bool[0, 0];
		static string cmdline = "";
		static string prevcmdline = "";
		static bool command_active = false;
		static Random random = new Random();
		static Effect? currentEffect;
		static int counter = 0;

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool GetConsoleMode(IntPtr handle, out int mode);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetStdHandle(int handle);

		static void SetupUpdateTimer()
		{
			updateTimer.Elapsed += UpdateScreen;
			updateTimer.AutoReset = true;
			updateTimer.Enabled = true;
		}
		static void ResizeTerminalContent()
		{
			(char, int, int)[,] oldTerminalContent = ((char, int, int)[,])terminalContent.Clone();
			bool[,] oldLifetimeMask = (bool[,])terminalLifetimeMask.Clone();
			terminalContent = new (char, int, int)[Console.WindowWidth, Console.WindowHeight];
			terminalLifetimeMask = new bool[Console.WindowWidth, Console.WindowHeight];

			for (int y = 0; y < Math.Min(terminalContent.GetLength(1), oldTerminalContent.GetLength(1)); y++) {
				for (int x = 0; x < Math.Min(terminalContent.GetLength(0), oldTerminalContent.GetLength(0)); x++) {
					terminalContent[x, y] = oldTerminalContent[x, y];
					terminalLifetimeMask[x, y] = oldLifetimeMask[x, y];
				}
			}
		}
		static void UpdateChar(ref (char, int, int) character, ref bool mask)
		{
			if (character.Item3 != 0 && (character.Item2 <= 40 || character.Item2 >= 127)) {
				if (mask) {
					character.Item1 = (char)(random.Next() % ('z' - '!') + '!');
					character.Item3 *= -1;
				} else {
					character.Item1 = ' ';
					character.Item3 = 0;
				}
			}

			character.Item2 += character.Item3;
		}
		static void UpdateTerminalContent()
		{
			int xl = terminalContent.GetLength(0);
			int yl = terminalContent.GetLength(1);
			for (int x = 0; x < terminalContent.GetLength(0); x++) {
				for (int y = 0; y < terminalContent.GetLength(1); y++) {
					UpdateChar(ref terminalContent[x, y], ref terminalLifetimeMask[x, y]);
				}
			}
		}
		static void UpdateScreen(Object? data, System.Timers.ElapsedEventArgs? _)
		{
			if ((terminalContent.GetLength(0) != Console.WindowWidth) || (terminalContent.GetLength(1) != Console.WindowHeight)) {
				ResizeTerminalContent();
			}
			UpdateTerminalContent();
			Console.CursorVisible = false;
			int y = 0;
			if (cmdline.Length != 0) {
				Console.SetCursorPosition(0, 0);
				Console.Write("\x1b[38;2;127;64;0m" + cmdline + "█");
				for (int idx = cmdline.Length; idx < Console.WindowWidth; idx++) {
					Console.Write(" ");
				}
				y++;
			}
			for (; y < terminalContent.GetLength(1); y++) {
				Console.SetCursorPosition(0, y);
				string line = "";
				for (int x = 0; x < terminalContent.GetLength(0); x++) {
					(char, int, int) ch = terminalContent[x, y];
					line += "\x1b[38;2;127;" + ch.Item2 + ";0m" + ch.Item1;
				}
				Console.Write(line);
			}
			Console.SetCursorPosition(0, 0);
		}
		static void SetupMask(string message)
		{
			bool last_command_active = command_active;
			ClearMask(false);
			command_active = true;
			int fixedx = Console.WindowWidth / 2 - (message.Length * 4);
			int fixedy = Console.WindowHeight / 2 - 4;

			bool[,] targetMask = (bool[,])terminalLifetimeMask.Clone();
			for (int idx = 0; idx < message.Length; idx++) {
				ulong bitmap = FONT[(int)message[idx] - ' '];
				for (int x = 0; x < 8; x++) {
					for (int y = 0; y < 8; y++) {
						targetMask[x + idx * 8 + fixedx, y + fixedy] = ((bitmap >> (y * 8 + x)) & 0x1) == 0x1;
					}
				}
			}
			switch (counter++ % 2) {
				case 0:
					currentEffect = new RainEffect(targetMask, terminalLifetimeMask, terminalContent, last_command_active);
					break;
				case 1:
					currentEffect = new AppearEffect((fixedx, fixedy), message.Length, targetMask, terminalLifetimeMask, terminalContent);
					break;
			}
		}
		static void ClearMask(bool state)
		{
			command_active = false;
			for (int x = 0; x < terminalLifetimeMask.GetLength(0); x++) {
				for (int y = 0; y < terminalLifetimeMask.GetLength(1); y++) {
					if (state && !terminalLifetimeMask[x, y]) {
						terminalLifetimeMask[x, y] = true;
						terminalContent[x, y] = (
							(char)(random.Next() % ('z' - '!') + '!'),
							random.Next() % 128,
							((random.Next() % 2) * 2 - 1) * (random.Next() % 5 + 1)
						);
					} else if (!state && terminalLifetimeMask[x, y]) {
						terminalLifetimeMask[x, y] = false;
						terminalContent[x, y].Item3 = ((random.Next() % 2) * 2 - 1) * (random.Next() % 5 + 1);
					}
				}
			}
		}
		static bool handlePressedKey(char key)
		{
			switch (key) {
				case '\r':
					switch (cmdline) {
						case "exit":
							return false;
						default:
							SetupMask(cmdline);
							break;
					}
					prevcmdline = cmdline;
					cmdline = "";
					break;
				case '\b':
					if (cmdline.Length > 0) {
						cmdline = cmdline.Remove(cmdline.Length - 1);
					}
					break;
				default:
					cmdline += key;
					break;
			}
			return true;
		}
		static void Main(string[] args)
		{
			var handle = GetStdHandle(-11);
			int mode;
			GetConsoleMode(handle, out mode);
			SetConsoleMode(handle, mode | 0x4);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.BackgroundColor = ConsoleColor.Black;
			ResizeTerminalContent();
			ClearMask(true);
			SetupUpdateTimer();

			while (handlePressedKey(Console.ReadKey().KeyChar)) { }

			Console.Clear();
			SetConsoleMode(handle, mode);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
