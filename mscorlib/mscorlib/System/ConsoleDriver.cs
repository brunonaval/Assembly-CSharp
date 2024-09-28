using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x0200022E RID: 558
	internal static class ConsoleDriver
	{
		// Token: 0x06001958 RID: 6488 RVA: 0x0005EAE8 File Offset: 0x0005CCE8
		static ConsoleDriver()
		{
			if (!ConsoleDriver.IsConsole)
			{
				ConsoleDriver.driver = ConsoleDriver.CreateNullConsoleDriver();
				return;
			}
			if (Environment.IsRunningOnWindows)
			{
				ConsoleDriver.driver = ConsoleDriver.CreateWindowsConsoleDriver();
				return;
			}
			string environmentVariable = Environment.GetEnvironmentVariable("TERM");
			if (environmentVariable == "dumb")
			{
				ConsoleDriver.is_console = false;
				ConsoleDriver.driver = ConsoleDriver.CreateNullConsoleDriver();
				return;
			}
			ConsoleDriver.driver = ConsoleDriver.CreateTermInfoDriver(environmentVariable);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0005EB4D File Offset: 0x0005CD4D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static IConsoleDriver CreateNullConsoleDriver()
		{
			return new NullConsoleDriver();
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0005EB54 File Offset: 0x0005CD54
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static IConsoleDriver CreateWindowsConsoleDriver()
		{
			return new WindowsConsoleDriver();
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0005EB5B File Offset: 0x0005CD5B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static IConsoleDriver CreateTermInfoDriver(string term)
		{
			return new TermInfoDriver(term);
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x0005EB63 File Offset: 0x0005CD63
		public static bool Initialized
		{
			get
			{
				return ConsoleDriver.driver.Initialized;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x0005EB6F File Offset: 0x0005CD6F
		// (set) Token: 0x0600195E RID: 6494 RVA: 0x0005EB7B File Offset: 0x0005CD7B
		public static ConsoleColor BackgroundColor
		{
			get
			{
				return ConsoleDriver.driver.BackgroundColor;
			}
			set
			{
				if (value < ConsoleColor.Black || value > ConsoleColor.White)
				{
					throw new ArgumentOutOfRangeException("value", "Not a ConsoleColor value.");
				}
				ConsoleDriver.driver.BackgroundColor = value;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600195F RID: 6495 RVA: 0x0005EBA1 File Offset: 0x0005CDA1
		// (set) Token: 0x06001960 RID: 6496 RVA: 0x0005EBAD File Offset: 0x0005CDAD
		public static int BufferHeight
		{
			get
			{
				return ConsoleDriver.driver.BufferHeight;
			}
			set
			{
				ConsoleDriver.driver.BufferHeight = value;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x0005EBBA File Offset: 0x0005CDBA
		// (set) Token: 0x06001962 RID: 6498 RVA: 0x0005EBC6 File Offset: 0x0005CDC6
		public static int BufferWidth
		{
			get
			{
				return ConsoleDriver.driver.BufferWidth;
			}
			set
			{
				ConsoleDriver.driver.BufferWidth = value;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001963 RID: 6499 RVA: 0x0005EBD3 File Offset: 0x0005CDD3
		public static bool CapsLock
		{
			get
			{
				return ConsoleDriver.driver.CapsLock;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001964 RID: 6500 RVA: 0x0005EBDF File Offset: 0x0005CDDF
		// (set) Token: 0x06001965 RID: 6501 RVA: 0x0005EBEB File Offset: 0x0005CDEB
		public static int CursorLeft
		{
			get
			{
				return ConsoleDriver.driver.CursorLeft;
			}
			set
			{
				ConsoleDriver.driver.CursorLeft = value;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06001966 RID: 6502 RVA: 0x0005EBF8 File Offset: 0x0005CDF8
		// (set) Token: 0x06001967 RID: 6503 RVA: 0x0005EC04 File Offset: 0x0005CE04
		public static int CursorSize
		{
			get
			{
				return ConsoleDriver.driver.CursorSize;
			}
			set
			{
				ConsoleDriver.driver.CursorSize = value;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001968 RID: 6504 RVA: 0x0005EC11 File Offset: 0x0005CE11
		// (set) Token: 0x06001969 RID: 6505 RVA: 0x0005EC1D File Offset: 0x0005CE1D
		public static int CursorTop
		{
			get
			{
				return ConsoleDriver.driver.CursorTop;
			}
			set
			{
				ConsoleDriver.driver.CursorTop = value;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600196A RID: 6506 RVA: 0x0005EC2A File Offset: 0x0005CE2A
		// (set) Token: 0x0600196B RID: 6507 RVA: 0x0005EC36 File Offset: 0x0005CE36
		public static bool CursorVisible
		{
			get
			{
				return ConsoleDriver.driver.CursorVisible;
			}
			set
			{
				ConsoleDriver.driver.CursorVisible = value;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600196C RID: 6508 RVA: 0x0005EC43 File Offset: 0x0005CE43
		public static bool KeyAvailable
		{
			get
			{
				return ConsoleDriver.driver.KeyAvailable;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x0005EC4F File Offset: 0x0005CE4F
		// (set) Token: 0x0600196E RID: 6510 RVA: 0x0005EC5B File Offset: 0x0005CE5B
		public static ConsoleColor ForegroundColor
		{
			get
			{
				return ConsoleDriver.driver.ForegroundColor;
			}
			set
			{
				if (value < ConsoleColor.Black || value > ConsoleColor.White)
				{
					throw new ArgumentOutOfRangeException("value", "Not a ConsoleColor value.");
				}
				ConsoleDriver.driver.ForegroundColor = value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x0005EC81 File Offset: 0x0005CE81
		public static int LargestWindowHeight
		{
			get
			{
				return ConsoleDriver.driver.LargestWindowHeight;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x0005EC8D File Offset: 0x0005CE8D
		public static int LargestWindowWidth
		{
			get
			{
				return ConsoleDriver.driver.LargestWindowWidth;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001971 RID: 6513 RVA: 0x0005EC99 File Offset: 0x0005CE99
		public static bool NumberLock
		{
			get
			{
				return ConsoleDriver.driver.NumberLock;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x0005ECA5 File Offset: 0x0005CEA5
		// (set) Token: 0x06001973 RID: 6515 RVA: 0x0005ECB1 File Offset: 0x0005CEB1
		public static string Title
		{
			get
			{
				return ConsoleDriver.driver.Title;
			}
			set
			{
				ConsoleDriver.driver.Title = value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x0005ECBE File Offset: 0x0005CEBE
		// (set) Token: 0x06001975 RID: 6517 RVA: 0x0005ECCA File Offset: 0x0005CECA
		public static bool TreatControlCAsInput
		{
			get
			{
				return ConsoleDriver.driver.TreatControlCAsInput;
			}
			set
			{
				ConsoleDriver.driver.TreatControlCAsInput = value;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x0005ECD7 File Offset: 0x0005CED7
		// (set) Token: 0x06001977 RID: 6519 RVA: 0x0005ECE3 File Offset: 0x0005CEE3
		public static int WindowHeight
		{
			get
			{
				return ConsoleDriver.driver.WindowHeight;
			}
			set
			{
				ConsoleDriver.driver.WindowHeight = value;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001978 RID: 6520 RVA: 0x0005ECF0 File Offset: 0x0005CEF0
		// (set) Token: 0x06001979 RID: 6521 RVA: 0x0005ECFC File Offset: 0x0005CEFC
		public static int WindowLeft
		{
			get
			{
				return ConsoleDriver.driver.WindowLeft;
			}
			set
			{
				ConsoleDriver.driver.WindowLeft = value;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600197A RID: 6522 RVA: 0x0005ED09 File Offset: 0x0005CF09
		// (set) Token: 0x0600197B RID: 6523 RVA: 0x0005ED15 File Offset: 0x0005CF15
		public static int WindowTop
		{
			get
			{
				return ConsoleDriver.driver.WindowTop;
			}
			set
			{
				ConsoleDriver.driver.WindowTop = value;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x0005ED22 File Offset: 0x0005CF22
		// (set) Token: 0x0600197D RID: 6525 RVA: 0x0005ED2E File Offset: 0x0005CF2E
		public static int WindowWidth
		{
			get
			{
				return ConsoleDriver.driver.WindowWidth;
			}
			set
			{
				ConsoleDriver.driver.WindowWidth = value;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x0005ED3B File Offset: 0x0005CF3B
		public static bool IsErrorRedirected
		{
			get
			{
				return !ConsoleDriver.Isatty(MonoIO.ConsoleError);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x0005ED4A File Offset: 0x0005CF4A
		public static bool IsOutputRedirected
		{
			get
			{
				return !ConsoleDriver.Isatty(MonoIO.ConsoleOutput);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0005ED59 File Offset: 0x0005CF59
		public static bool IsInputRedirected
		{
			get
			{
				return !ConsoleDriver.Isatty(MonoIO.ConsoleInput);
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0005ED68 File Offset: 0x0005CF68
		public static void Beep(int frequency, int duration)
		{
			ConsoleDriver.driver.Beep(frequency, duration);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0005ED76 File Offset: 0x0005CF76
		public static void Clear()
		{
			ConsoleDriver.driver.Clear();
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0005ED84 File Offset: 0x0005CF84
		public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
		{
			ConsoleDriver.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, ' ', ConsoleColor.Black, ConsoleColor.Black);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0005EDA4 File Offset: 0x0005CFA4
		public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
		{
			ConsoleDriver.driver.MoveBufferArea(sourceLeft, sourceTop, sourceWidth, sourceHeight, targetLeft, targetTop, sourceChar, sourceForeColor, sourceBackColor);
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0005EDC9 File Offset: 0x0005CFC9
		public static void Init()
		{
			ConsoleDriver.driver.Init();
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0005EDD8 File Offset: 0x0005CFD8
		public static int Read()
		{
			return (int)ConsoleDriver.ReadKey(false).KeyChar;
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0005EDF3 File Offset: 0x0005CFF3
		public static string ReadLine()
		{
			return ConsoleDriver.driver.ReadLine();
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0005EDFF File Offset: 0x0005CFFF
		public static ConsoleKeyInfo ReadKey(bool intercept)
		{
			return ConsoleDriver.driver.ReadKey(intercept);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0005EE0C File Offset: 0x0005D00C
		public static void ResetColor()
		{
			ConsoleDriver.driver.ResetColor();
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0005EE18 File Offset: 0x0005D018
		public static void SetBufferSize(int width, int height)
		{
			ConsoleDriver.driver.SetBufferSize(width, height);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0005EE26 File Offset: 0x0005D026
		public static void SetCursorPosition(int left, int top)
		{
			ConsoleDriver.driver.SetCursorPosition(left, top);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0005EE34 File Offset: 0x0005D034
		public static void SetWindowPosition(int left, int top)
		{
			ConsoleDriver.driver.SetWindowPosition(left, top);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0005EE42 File Offset: 0x0005D042
		public static void SetWindowSize(int width, int height)
		{
			ConsoleDriver.driver.SetWindowSize(width, height);
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x0600198E RID: 6542 RVA: 0x0005EE50 File Offset: 0x0005D050
		public static bool IsConsole
		{
			get
			{
				if (ConsoleDriver.called_isatty)
				{
					return ConsoleDriver.is_console;
				}
				ConsoleDriver.is_console = (ConsoleDriver.Isatty(MonoIO.ConsoleOutput) && ConsoleDriver.Isatty(MonoIO.ConsoleInput));
				ConsoleDriver.called_isatty = true;
				return ConsoleDriver.is_console;
			}
		}

		// Token: 0x0600198F RID: 6543
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool Isatty(IntPtr handle);

		// Token: 0x06001990 RID: 6544
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int InternalKeyAvailable(int ms_timeout);

		// Token: 0x06001991 RID: 6545
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool TtySetup(string keypadXmit, string teardown, out byte[] control_characters, out int* address);

		// Token: 0x06001992 RID: 6546
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SetEcho(bool wantEcho);

		// Token: 0x06001993 RID: 6547
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool SetBreak(bool wantBreak);

		// Token: 0x040016E8 RID: 5864
		internal static IConsoleDriver driver;

		// Token: 0x040016E9 RID: 5865
		private static bool is_console;

		// Token: 0x040016EA RID: 5866
		private static bool called_isatty;
	}
}
