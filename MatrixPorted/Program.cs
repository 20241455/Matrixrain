using System.Runtime.InteropServices;
using System.Timers;
using Timer = System.Timers.Timer;

namespace MatrixPorted {
	public enum TerminalCharFlag {
		Bright = 1 << 0,
		NoRespawn = 1 << 1,
		AlternateColor = 1 << 2
	}
	class Program {
		static Timer updateTimer = new(70);
		static (char, int, int)[,] terminalContent = new (char, int, int)[0, 0];
		static TerminalCharFlag[,] terminalMask = new TerminalCharFlag[0, 0];
		static TerminalCharFlag[,] targetMask = new TerminalCharFlag[0, 0];
		static string cmdline = "";
		static string prevcmdline = "";
		static bool command_active = false;
		static bool last_command_active = false;
		static Random random = new Random();
		static Effect? currentEffect;
		static int counter = 0;

		[DllImport("kernel32.dll")]
		static extern IntPtr GetStdHandle(int nStdHandle);
		[DllImport("kernel32.dll")]
		static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);
		[DllImport("kernel32.dll")]
		static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

		static string ColorizeText<T>(T text, int r, int g, bool alter) {
			if (alter) {
				return "\x1b[38;2;" + g + ";0;" + r + "m" + text;
			} else {
				return "\x1b[38;2;" + r + ";" + g + ";0m" + text;
			}
		}
		static int RandomDifference()
		{
			int num;
			do {
				num = random.Next() % 5 - 2;
			} while (num == 0);
			return num;
		}

		static void SetupUpdateTimer()
		{
			updateTimer.Elapsed += UpdateScreen;
			updateTimer.AutoReset = true;
			updateTimer.Enabled = true;
		}
		/*
		 * Resizes the terminal buffers, when the terminal window resizes.
		 * command_activate manipulates the default values for new values.
		 */
		static void ResizeTerminalContent()
		{
			(char, int, int)[,] oldTerminalContent = ((char, int, int)[,])terminalContent.Clone();
			TerminalCharFlag[,] oldMask = (TerminalCharFlag[,])terminalMask.Clone();
			terminalContent = new (char, int, int)[Console.WindowWidth, Console.WindowHeight];
			terminalMask = new TerminalCharFlag[Console.WindowWidth, Console.WindowHeight];

			SetupInitalMask();
			/*
			for (int y = 0; y < Math.Min(terminalContent.GetLength(1), oldTerminalContent.GetLength(1)); y++) {
				for (int x = 0; x < Math.Min(terminalContent.GetLength(0), oldTerminalContent.GetLength(0)); x++) {
					terminalContent[x, y] = oldTerminalContent[x, y];
					terminalMask[x, y] = oldMask[x, y];
				}
			}
			if (!command_active) {
				for (int x = 0; x < terminalContent.GetLength(0); x++) {
					for (int y = 0; y < terminalContent.GetLength(1); y++) {
						if (x >= oldTerminalContent.GetLength(0) || y >= oldTerminalContent.GetLength(1)) {
							terminalContent[x, y] = (
								(char)(random.Next() % ('z' - '!') + '!'),
								random.Next() % 88 + 40,
								RandomDifference()
							);
							terminalMask[x, y] = 0;
						}
					}
				}
			}*/
		}

		/// Update color of character (by adding third item to second item) and if the color is below 40 or above 127,
		/// then the differential item gets inverted and the character gets randomized, if mask is set to true.
		static void UpdateChar(ref (char, int, int) character, in TerminalCharFlag mask)
		{
			if (character.Item3 != 0 && (character.Item2 <= 40 || character.Item2 >= 127)) {
				if (mask != TerminalCharFlag.NoRespawn) {
					character.Item1 = (char)(random.Next() % ('z' - '!') + '!');
					character.Item3 *= -1;
				} else {
					character.Item1 = ' ';
					character.Item3 = 0;
				}
			}

			character.Item2 += character.Item3;
		}

		/// <summary>
		/// Updates every character in terminal buffer with the method UpdateChar.
		/// </summary>
		static void UpdateTerminalContent()
		{
			int xl = terminalContent.GetLength(0);
			int yl = terminalContent.GetLength(1);
			
			for (int x = 0; x < terminalContent.GetLength(0); x++) {
				Parallel.For(0, yl, y => UpdateChar(ref terminalContent[x, y], terminalMask[x, y]));
			}
		}
		/// <summary>
		/// 1. It resizes the terminal buffer if the window size has been changed.
		/// 2. Updates every character.
		/// 3. Writes the currently typed command (if any)
		/// </summary>
		/// <param name="data">This parameter is ignored.</param>
		/// <param name="_">This parameter is also ignored.</param>
		static void UpdateScreen(Object? data, System.Timers.ElapsedEventArgs? _)
		{
			if ((terminalContent.GetLength(0) != Console.WindowWidth) || (terminalContent.GetLength(1) != Console.WindowHeight)) {
				ResizeTerminalContent();
			}
			UpdateTerminalContent();
			int y = 0;
			if (cmdline.Length != 0) {
				Console.SetCursorPosition(0, 0);
				Console.Write(ColorizeText((cmdline + "█").PadRight(Console.WindowWidth), 127, 64, true));
				y++;
			}
			
			for (; y < terminalContent.GetLength(1); y++) {
				Console.SetCursorPosition(0, y);
				string line = "";
				for (int x = 0; x < terminalContent.GetLength(0); x++) {
					(char, int, int) ch = terminalContent[x, y];
					TerminalCharFlag mask = terminalMask[x, y];
					int brightness = (mask & TerminalCharFlag.Bright) != 0 ? 80 : 30;
					line += ColorizeText(ch.Item1, ch.Item2 + brightness, ch.Item2 / 2 + brightness, (mask & TerminalCharFlag.AlternateColor) != 0);
				}
				Console.Write(line);
			}
			Console.SetCursorPosition(0, 0);
		}

		static char[] PUSH_DOWN_CHARS = { 'g', 'p', 'q', 'y' };
		static void SetupInitalMask()
		{
			string MESSAGE = "Bitte geben Sie\nIhren Namen ein.";
			for (int x = 0; x < terminalContent.GetLength(0); x++) {
				for (int y = 0; y < terminalContent.GetLength(1); y++) {
					terminalMask[x, y] = 0;
					terminalContent[x, y] = (
						(char)(random.Next() % ('z' - '!') + '!'),
						random.Next() % 88 + 40,
						RandomDifference()
					);
				}
			}
			if (Console.WindowWidth < 20 * 8) {
				return;
			}
			int min_fixed_x = int.MaxValue;
			string[] splitted = MESSAGE.Split("\n");
			int newline_amount = splitted.Length;
			int fixedy = Console.WindowHeight / 2 - 8 - 4;
			
			foreach (string slice in MESSAGE.Split("\n")) {
				int fixedx = Console.WindowWidth / 2 - (slice.Length * 4);
				min_fixed_x = Math.Min(min_fixed_x, fixedx);

				for (int idx = 0; idx < slice.Length; idx++) {
					ulong bitmap = Font.FONT[(int)slice[idx] - ' '];

					for (int x = 0; x < 8; x++) {
						if (PUSH_DOWN_CHARS.Contains(slice[idx])) {
							terminalMask[x + idx * 8 + fixedx, fixedy + 0] = TerminalCharFlag.NoRespawn;
							terminalMask[x + idx * 8 + fixedx, fixedy + 1] = TerminalCharFlag.NoRespawn;
							terminalContent[x + idx * 8 + fixedx, fixedy + 0] = (' ', 0, 0);
							terminalContent[x + idx * 8 + fixedx, fixedy + 1] = (' ', 0, 0);
						}
						for (int y = 0; y < 8; y++) {
							TerminalCharFlag mask = ((bitmap >> (y * 8 + x)) & 0x1) == 0x1 ? (TerminalCharFlag.Bright | TerminalCharFlag.AlternateColor) : TerminalCharFlag.NoRespawn;
							terminalMask[x + idx * 8 + fixedx, y + fixedy + (PUSH_DOWN_CHARS.Contains(slice[idx]) ? 2 : 0)] = mask;
							if (mask == TerminalCharFlag.NoRespawn) {
								terminalContent[x + idx * 8 + fixedx, y + fixedy + (PUSH_DOWN_CHARS.Contains(slice[idx]) ? 2 : 0)] = (
									' ',
									random.Next() % 88 + 40,
									RandomDifference()
								);
							}
						}
					}
				}
				fixedy += 11;
			}
		}
		static void SetupMask(string message)
		{
			last_command_active = command_active;
			ClearMask(false, Console.WindowWidth, command_active ? Console.WindowHeight - Font.SKULL.GetLength(0) : Console.WindowHeight);
			command_active = true;
			targetMask = (TerminalCharFlag[,])terminalMask.Clone();
			int yskulloffset = Console.WindowHeight - Font.SKULL.GetLength(0);
			int xskulloffset = Console.WindowWidth - Font.SKULL.GetLength(1);
			for (int x = 0; x < Font.SKULL.GetLength(0); x++) {
				for (int y = 0; y < Font.SKULL.GetLength(1); y++) {
					targetMask[y, x + yskulloffset] = Font.SKULL[x, y] ? 0 : TerminalCharFlag.NoRespawn;
				}
			}
			for (int x = 0; x < Font.SKULL.GetLength(0); x++) {
				for (int y = 0; y < Font.SKULL.GetLength(1); y++) {
					targetMask[y + xskulloffset, x + yskulloffset] = Font.SKULL[x, y] ? 0 : TerminalCharFlag.NoRespawn;
				}
			}
			int min_fixed_x = int.MaxValue;
			string[] splitted = message.Split("\n");
			int newline_amount = splitted.Length;
			int fixedy = 0;
			foreach (string slice in splitted) {
				int fixedx = Console.WindowWidth / 2 - (slice.Length * 4);
				min_fixed_x = Math.Min(min_fixed_x, fixedx);

				for (int idx = 0; idx < slice.Length; idx++) {
					ulong bitmap = Font.FONT[(int)slice[idx] - ' '];

					for (int x = 0; x < 8; x++) {
						for (int y = 0; y < 8; y++) {
							int xcoord = x + idx * 8 + fixedx;
							int ycoord = y + fixedy + (PUSH_DOWN_CHARS.Contains(slice[idx]) ? 2 : 0);
							if (((bitmap >> (y * 8 + x)) & 0x1) == 0x1) {
								targetMask[xcoord, ycoord] = TerminalCharFlag.Bright;
							}
						}
					}
				}
				fixedy += 9;
			}
			counter++;
			switch (0) {
				case 0:
					currentEffect = new RainEffect(targetMask, terminalMask, terminalContent, last_command_active);
					break;
				case 1:
					currentEffect = new AppearEffect(message.Length, targetMask, terminalMask, terminalContent);
					break;
			}
		}
		static void ClearMask(bool state, int width = int.MaxValue, int height = int.MaxValue)
		{
			command_active = false;
			if (state) {
				SetupInitalMask();
				return;
			}
			
			if (width == int.MaxValue) {
				width = terminalContent.GetLength(0);
			}
			if (height == int.MaxValue) {
				height = terminalContent.GetLength(1);
			}
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					if (state && (terminalMask[x, y] != 0 || terminalContent[x, y].Item1 == ' ')) {
						terminalMask[x, y] = 0;
						terminalContent[x, y] = (
							(char)(random.Next() % ('z' - '!') + '!'),
							random.Next() % 88 + 40,
							RandomDifference()
						);
					} else if (!state && terminalMask[x, y] != TerminalCharFlag.NoRespawn) {
						terminalMask[x, y] = TerminalCharFlag.NoRespawn;
						terminalContent[x, y].Item3 = RandomDifference();
					}
				}
			}
		}
		static bool handlePressedKey(char key)
		{
			switch (key) {
				case '\x1b':
					if (counter == 0) {
						return false;
					}
					currentEffect = null;
					counter = 0;
					ClearMask(true);
					break;
				case '\r':
					if (cmdline != "") {
						switch (cmdline) {
							case "":
								return true;
							default:
								if (cmdline.Length * 8 > Console.WindowWidth) {
									string newcommand = "";
					
									int linelength = 0;
									foreach (string splitted in cmdline.Split(" ")) {
										if (splitted.Length * 8 > Console.WindowWidth) {
											cmdline = "Name ist zu lang.";
											goto case "";
										}
										if ((linelength + splitted.Length) * 8 > Console.WindowWidth) {
											linelength = 0;
											newcommand += '\n';
										}
										newcommand += splitted;
										newcommand += ' ';
										linelength += splitted.Length + 1;
									}
									cmdline = newcommand;
								}
								if (Font.SKULL.GetLength(0) + 12 + 8 * cmdline.AsSpan().Count('\n') > Console.WindowHeight) {
									cmdline = "Vergrößern Sie das Fenster.";
									goto case "";
								}

								SetupMask(cmdline);
								break;
						}
						prevcmdline = cmdline;
						if (cmdline != "Name ist zu lang.") {
							cmdline = "";
						}
						
					}
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
			Console.ForegroundColor = ConsoleColor.Green;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.CursorVisible = false;

			var handle = GetStdHandle(-11);
			int mode;
			GetConsoleMode(handle, out mode);
			SetConsoleMode(handle, mode | 0x4);
			ResizeTerminalContent();
			ClearMask(true);
			SetupUpdateTimer();

			while (handlePressedKey(Console.ReadKey().KeyChar)) {}

			SetConsoleMode(handle, mode);
			updateTimer.Enabled = false;

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.CursorVisible = true;
			Console.Clear();
		}
	}
}
