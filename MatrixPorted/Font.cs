using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixPorted {
	class Font {
		// Source: ascii.co.uk
		public static bool[,] SKULL = {
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, true, true, true, true, false, false, true, false, false, false, false, false, false, false, true, true, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, true, true, false, false, true, false, false, false, false, false, false, true, true, true, false, false, true, false, false, true, true, true, true, false, true, false, false, true, false, false, true, false, true, false, false, false, false, false, false, true, false, false, true, true, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, true, true, false, false, false, false, false, true, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, true, true, true, true, true, true, false, false, true, false, false, false, false, false, true, false, false, false, false, false, true, true, false, false, false, false, false, false },
			{ false, true, true, true, true, true, false, false, false, false, false, true, true, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, false, false, false, false, false, true, true, true, true, true, false },
			{ true, false, true, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, true, true, true, true, true, true, true, true, true, false, true, false, false, false, false, false, false, true, false, false, false, false, false, false, true, true, true, false, true, true, false, false, false, false, false, false, true, false, true },
			{ true, false, true, false, false, false, false, false, true, false, false, true, true, true, false, false, true, false, false, false, false, false, true, true, true, true, true, true, true, true, true, false, false, false, false, false, false, true, false, false, false, false, false, true, true, true, false, false, true, false, false, true, false, false, false, false, false, true, false, true },
			{ true, false, false, true, false, false, false, true, false, false, false, false, false, true, true, true, false, false, true, false, false, false, true, true, false, true, false, true, false, true, false, false, false, false, false, false, false, true, false, false, false, true, true, true, false, false, true, false, false, false, false, false, true, false, false, false, true, false, false, true },
			{ false, true, true, false, true, false, true, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, true, false, true, false, false, true, false },
			{ false, false, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, true, false, false, false, true, false, false },
			{ false, false, true, true, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, true, false, true, true, false, false, false },
			{ false, false, false, false, true, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, true, true, false, false, false },
			{ false, false, false, false, true, false, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, true, false, false, false, false },
			{ false, false, false, false, false, false, true, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, true, false, false, false, false },
			{ false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, false, false, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
			{ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, true, true, false, false, true, true, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false },
		};

		// Source: Homemade. Source code: RegularCoder239(Github). Small chars rewritten.
		public static ulong[] FONT = {
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
			0x7e427e403e000000, // a
			0x3e4242423e020200, // b
			0x7e0202027e000000, // c
			0x7f4141417f404000, // d
			0x7e027e427e000000, // e
			0x080808083e083000, // f
			0x3f407e4141615e00, // g
			0x414141413f010100, // h
			0x3c1818181c001800, // i
			0x3c66626060006000, // j
			0x42221a2642020200, // k
			0x1824040404040400, // l
			0x494949492f000000, // m
			0x414141413f000000, // n
			0x3c4242423c000000, // o
			0x01017f4141417f00, // p
			0x40407f4141417f00, // q
			0x040404241c000000, // r
			0x7e407e027e000000, // s
			0x300808083e080800, // t
			0x7c42424242000000, // u
			0x1824244242420000, // v
			0x3c5a5a5a5a000000, // w
			0x4224182442000000, // x
			0x404040407c424242, // y
			0x7e0c18307e000000, // z
			0x0, // {
			0x4040404040404040, // |
			0x0, // }
			0x0, // ~
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x7a4a623e6242623e, // ß
			0x0,
			0x0,
			0x0,
			0x0,
			0x7e427e403e002400, // ä
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x3c4242423c002400, // ö
			0x0,
			0x0,
			0x0,
			0x0,
			0x0,
			0x7c42424242002400, // ü
		};
	}
}
