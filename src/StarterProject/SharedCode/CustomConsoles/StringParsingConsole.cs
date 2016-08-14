﻿#if MONOGAME
using Microsoft.Xna.Framework;
#elif SFML
using SFML.Graphics;
using Point = SFML.System.Vector2i;
#endif

using SadConsole;
using SadConsole.Consoles;
using SadConsole.StringParser;

namespace StarterProject.CustomConsoles
{
    class StringParsingConsole: Console
    {
        public StringParsingConsole():base(80, 25)
        {
            IsVisible = false;
            textSurface.DefaultForeground = ColorAnsi.White;
            Clear();
            int c = 59;
            int r = 1;

            ColoredString.CustomProcessor = CustomParseCommand;


            Print(1, r, "[c:r f:ansibluebright][c:r b:ansiblue]String parsing supports...                                                    ");
            SetGlyph(0, r, 221, Color.Black, ColorAnsi.Blue);

            r = 2;
            Color a = Color.Green;
            Print(1, ++r, "[c:r f:DodgerBlue]1.[c:r f:White] Multi-colored [c:r f:yellow]str[c:r f:red]ings");
            r++;
            Print(1, ++r, "[c:r f:DodgerBlue]2.[c:r f:White] You color the backgrounds: [c:r f:black][c:r b:Purple] Purple [c:u] [c:r b:Cyan] Cyan ");
            r++;
            Print(1, ++r, "[c:r f:DodgerBlue]3.[c:r f:White] [c:g f:LimeGreen:Orange:9]Gradients are [c:g b:Black:Red:Yellow:Red:Black:15]easily supported");
            r++;
            Print(1, ++r, "[c:r f:DodgerBlue]4.[c:r f:White] You [c:b 3]can apply [c:b 5:0.17]blink effects.");
            r++;
            Print(1, ++r, "[c:r f:DodgerBlue]5.[c:r f:White] You set [c:r f:greenyellow:9]mirroring [c:m 1]on any [c:m 2]text [c:u][c:u]you want.");

            r += 2;
            Print(1, r, "[c:r f:ansibluebright][c:r b:ansiblue]Examples                                                                      ");
            SetGlyph(0, r, 221, Color.Black, ColorAnsi.Blue);

            r += 2;

            SadConsole.Shapes.Line line = new SadConsole.Shapes.Line();
            line.StartingLocation = new Point(c, r);
            line.EndingLocation = new Point(c, r + 5);
            line.UseEndingCell = false;
            line.UseStartingCell = false;
            line.CellAppearance.GlyphIndex = 179;
            line.Draw(this);

            Print(1, r, "Some `[c:r f:red]text`[c:u] to print                       ");
            Print(c + 2, r, $"Some [c:r f:{Color.Red.ToParser()}]text[c:u] to [c:r f:red]print");

            Print(1, ++r, "Some `[c:r f:100,100,33]text`[c:u] to print");
            Print(c + 2, r, "Some [c:r f:255,255,0]text[c:u] to print");

            Print(1, ++r, "Some `[c:r b:ansiblackbright]text to `[c:r f:white]print");
            Print(c + 2, r, "Some [c:r b:ansiblackbright]text to [c:r f:white]print");

            Print(1, ++r, "`[c:m 1]Some `[c:r f:purple]text to`[c:u] pr`[c:u]int");
            Print(c + 2, r, "[c:m 1]Some [c:r f:purple]text to[c:u] pr[c:u]int");

            Print(1, ++r, "[c:r f:default]`[c:g f:red:green:18]`[c:g b:green:red:18]Some text to print");
            Print(c + 2, r, "[c:g f:red:green:18][c:g b:green:red:18]Some text to print");
        }

        ParseCommandBase CustomParseCommand(string command, string parameters, ColoredGlyph[] glyphString,
                                                          ITextSurface surface, SurfaceEditor editor, ParseCommandStacks commandStacks)
        {
            switch (command)
            {
                case "t":
                    return new ParseCommandRetext(parameters);
                default:
                    return null; ;
            }
        }

        class ParseCommandRetext : ParseCommandBase
        {
            public int Counter;
            public char Glyph;

            public ParseCommandRetext(string parameters)
            {
                string[] parts = parameters.Split(new char[] { ':' }, 2);

                // Count and glyph type provided
                if (parts.Length == 2)
                    Counter = int.Parse(parts[1]);
                else
                    Counter = -1;

                // Get character
                Glyph = parts[0][0];

                // No exceptions, set the type
                CommandType = CommandTypes.Glyph;
            }

            public override void Build(ref ColoredGlyph glyphState, ColoredGlyph[] glyphString, int surfaceIndex, ITextSurface surface, SurfaceEditor editor, ref int stringIndex, string processedString, ParseCommandStacks commandStack)
            {
                glyphState.Glyph = Glyph;

                if (Counter != -1)
                {
                    Counter--;

                    if (Counter == 0)
                        commandStack.RemoveSafe(this);
                }
            }
        }
    }
}