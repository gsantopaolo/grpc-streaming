﻿// Copyright (c) 2015, Yves Goergen, http://unclassified.software/source/consolehelper
//
// Copying and distribution of this file, with or without modification, are permitted provided the
// copyright notice and this notice are preserved. This file is offered as-is, without any warranty.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

//public static int Main()
//{
//    // Reset unsupported character encoding for exotic languages to en-US
//    ConsoleHelper.FixEncoding();

//    // Show the message where the user can see it
//    if (ConsoleHelper.IsInteractiveAndVisible)
//    {
//        // Console is visible, also use colour for this text
//        ConsoleHelper.WriteLine("Hello console.", ConsoleColor.Red);
//    }
//    else
//    {
//        // Console is not visible, choose another output (non-interactive session,
//        // no console window allocated, or output redirected)
//        MessageBox.Show("Hello window.");
//    }

//    // Interaction only if it is possible
//    if (!ConsoleHelper.IsInputRedirected)
//    {
//        Console.Write("Please enter your name: ");
//        Console.ReadLine();
//    }

//    // Move cursor and clear line
//    Console.Write("Your name is:");
//    ConsoleHelper.MoveCursor(-3);
//    Console.Write("needs more checking...");
//    ConsoleHelper.ClearLine();   // Oh well, doesn’t matter anyway.

//    // Progress bar only if the output is interactive and can be overwritten. Otherwise,
//    // all intermediate frames end up in the file being redirected to, in some form.
//    if (!ConsoleHelper.IsOutputRedirected)
//    {
//        // Activate progress bar and update it regularly
//        ConsoleHelper.ProgressTitle = "Downloading";
//        ConsoleHelper.ProgressTotal = 10;
//        for (int i = 0; i <= 10; i++)
//        {
//            ConsoleHelper.ProgressValue = i;
//            Thread.Sleep(500);
//            // Warning and error state is displayed in colour (yellow/red instead of green)
//            if (i >= 5)
//            {
//                ConsoleHelper.ProgressHasWarning = true;
//            }
//            if (i >= 8)
//            {
//                ConsoleHelper.ProgressHasError = true;
//            }
//        }
//        // Remove progress bar again
//        ConsoleHelper.ProgressTotal = 0;
//    }

//    // Show long text with proper word wrapping
//    ConsoleHelper.WriteWrapped("This very long text must be wrapped at the right end of the console window. But that should not tear apart words just where the line is over but move the excess word to the next line entirely. This will regard the actual width of the window.");

//    ConsoleHelper.WriteWrapped("This also works for tabular output like a listing of command line parameters:");

//    // Line wrapping for tabular output
//    ConsoleHelper.WriteWrapped("  /a    Just a short note.", true);
//    ConsoleHelper.WriteWrapped("  /b    The text in the following lines is wrapped so that it continues under the last content column (that is this description). That is recognised by the last occurrence of two spaces.", true);
//    ConsoleHelper.WriteWrapped("  /cde  Nothing important, really.", true);

//    // Clear input buffer to not use any premature keystrokes
//    ConsoleHelper.ClearKeyBuffer();
//    // Confirmation message with timeout (15 seconds) and vanishing dots
//    ConsoleHelper.Wait("Seen it all?", 15, true);

//    // Simple wait for any input key (not Ctrl, Shift, NumLock, Mute, and so on)
//    ConsoleHelper.Wait();

//    // Prevent closing the console window after program end when running with the
//    // debugger in Visual Studio. Without the debugger, Visual Studio will wait
//    // already. Conforming things here.
//    ConsoleHelper.WaitIfDebug();

//    // Show a red alert message and terminate the process with an exit code (12)
//    return ConsoleHelper.ExitError("All is lost!", 12);
//}

namespace Client
{
    public enum LineType
    {
        singleLine,
        doubleLine
    }
    /// <summary>
    /// Provides methods for easy console output, input and formatting.
    /// Wrote some methods by myself and also integrated works of the guys at these links
    /// https://unclassified.software/en/source/consolehelper
    /// </summary>
    internal static class ConsoleHelper
    {
        #region Encoding

        /// <summary>
        /// Fixes the encoding of the console window for unsupported UI cultures. This method
        /// should be called once at application startup.
        /// </summary>
        public static void FixEncoding()
        {
            // Source: %windir%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentUICulture.GetConsoleFallbackUICulture();
            if (Console.OutputEncoding.CodePage != 65001 &&
                Console.OutputEncoding.CodePage != Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage &&
                Console.OutputEncoding.CodePage != Thread.CurrentThread.CurrentUICulture.TextInfo.ANSICodePage)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
        }

        #endregion Encoding

        #region Environment

        #region Native interop

        /// <summary>
        /// Defines values returned by the GetFileType function.
        /// </summary>
        private enum FileType : uint
        {
            /// <summary>The specified file is a character file, typically an LPT device or a console.</summary>
            FileTypeChar = 0x0002,
            /// <summary>The specified file is a disk file.</summary>
            FileTypeDisk = 0x0001,
            /// <summary>The specified file is a socket, a named pipe, or an anonymous pipe.</summary>
            FileTypePipe = 0x0003,
            /// <summary>Unused.</summary>
            FileTypeRemote = 0x8000,
            /// <summary>Either the type of the specified file is unknown, or the function failed.</summary>
            FileTypeUnknown = 0x0000,
        }

        /// <summary>
        /// Defines standard device handles for the GetStdHandle function.
        /// </summary>
        private enum StdHandle : int
        {
            /// <summary>The standard input device. Initially, this is the console input buffer, CONIN$.</summary>
            Input = -10,
            /// <summary>The standard output device. Initially, this is the active console screen buffer, CONOUT$.</summary>
            Output = -11,
            /// <summary>The standard error device. Initially, this is the active console screen buffer, CONOUT$.</summary>
            Error = -12,
        }

        /// <summary>
        /// Retrieves the file type of the specified file.
        /// </summary>
        /// <param name="hFile">A handle to the file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern FileType GetFileType(IntPtr hFile);

        /// <summary>
        /// Retrieves a handle to the specified standard device (standard input, standard output,
        /// or standard error).
        /// </summary>
        /// <param name="nStdHandle">The standard device.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(StdHandle nStdHandle);

        /// <summary>
        /// Retrieves the window handle used by the console associated with the calling process.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Determines the visibility state of the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be tested.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        #endregion Native interop

        /// <summary>
        /// Gets a value indicating whether the current application has an interactive console and
        /// is able to interact with the user through it.
        /// </summary>
        public static bool IsInteractiveAndVisible
        {
            get
            {
                IntPtr consoleWnd = GetConsoleWindow();
                return Environment.UserInteractive &&
                    consoleWnd != IntPtr.Zero &&
                    IsWindowVisible(consoleWnd) &&
                    !IsInputRedirected &&
                    !IsOutputRedirected &&
                    !IsErrorRedirected;
            }
        }

        private static bool? isInputRedirected;
        private static bool? isOutputRedirected;
        private static bool? isErrorRedirected;

        /// <summary>
        /// Gets a value that indicates whether input has been redirected from the standard input
        /// stream.
        /// </summary>
        /// <remarks>
        /// The value is cached after the first access.
        /// </remarks>
        public static bool IsInputRedirected
        {
            get
            {
                if (isInputRedirected == null)
                {
                    isInputRedirected = GetFileType(GetStdHandle(StdHandle.Input)) != FileType.FileTypeChar;
                }
                return isInputRedirected == true;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether output has been redirected from the standard output
        /// stream.
        /// </summary>
        /// <remarks>
        /// The value is cached after the first access.
        /// </remarks>
        public static bool IsOutputRedirected
        {
            get
            {
                if (isOutputRedirected == null)
                {
                    isOutputRedirected = GetFileType(GetStdHandle(StdHandle.Output)) != FileType.FileTypeChar;
                }
                return isOutputRedirected == true;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the error output stream has been redirected from the
        /// standard error stream.
        /// </summary>
        /// <remarks>
        /// The value is cached after the first access.
        /// </remarks>
        public static bool IsErrorRedirected
        {
            get
            {
                if (isErrorRedirected == null)
                {
                    isErrorRedirected = GetFileType(GetStdHandle(StdHandle.Error)) != FileType.FileTypeChar;
                }
                return isErrorRedirected == true;
            }
        }

        #endregion Environment

        #region Cursor

        /// <summary>
        /// Moves the cursor in the current line.
        /// </summary>
        /// <param name="count">The number of characters to move the cursor. Positive values move to the right, negative to the left.</param>
        public static void MoveCursor(int count)
        {
            if (!IsOutputRedirected)
            {
                int x = Console.CursorLeft + count;
                if (x < 0)
                {
                    x = 0;
                }
                if (x >= Console.BufferWidth)
                {
                    x = Console.BufferWidth - 1;
                }
                Console.CursorLeft = x;
            }
        }

        /// <summary>
        /// Clears the current line and moves the cursor to the first column.
        /// </summary>
        public static void ClearLine()
        {
            if (!IsOutputRedirected)
            {
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.BufferWidth - 1));
                Console.CursorLeft = 0;
            }
            else
            {
                Console.WriteLine();
            }
        }

        #endregion Cursor

        #region Color output

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void Write(string text, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textColor"></param>
        /// <param name="backColor"></param>
        public static void Write(string text, ConsoleColor textColor, ConsoleColor backColor)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.Write(text);
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void WriteLine(string text, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textColor"></param>
        /// <param name="backColor"></param>
        public static void WriteLine(string text, ConsoleColor textColor, ConsoleColor backColor)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.WriteLine(text);
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        /// <summary>
        /// Writes a text with custom format control characters. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="formatter">A function that can set the console color depending on the input
        ///   character. Return false to hide the character.</param>
        public static void WriteLineFormatted(string text, Func<char, bool> formatter)
        {
            WriteFormatted(text, formatter);
            Console.WriteLine();
        }

        /// <summary>
        /// Writes a text with custom format control characters. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="formatter">A function that can set the console color depending on the input
        ///   character. Return false to hide the character.</param>
        public static void WriteFormatted(string text, Func<char, bool> formatter)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            foreach (char ch in text)
            {
                if (formatter(ch))
                {
                    Console.Write(ch);
                }
            }
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        #endregion Color output

        #region Progress bar

        private static string progressTitle;
        private static int progressValue;
        private static int progressTotal;
        private static bool progressHasWarning;
        private static bool progressHasError;

        /// <summary>
        /// Gets or sets the progress title, and updates the displayed progress bar accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static string ProgressTitle
        {
            get { return progressTitle; }
            set
            {
                if (value != progressTitle)
                {
                    progressTitle = value;
                    WriteProgress();
                }
            }
        }

        /// <summary>
        /// Gets or sets the current value of the progress, and updates the displayed progress bar
        /// accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static int ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (value != progressValue)
                {
                    progressValue = value;
                    WriteProgress();
                }
            }
        }

        /// <summary>
        /// Gets or sets the total value of the progress, and updates the displayed progress bar
        /// accordingly. Setting a value of zero or less clears the progress bar and resets its
        /// state.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static int ProgressTotal
        {
            get { return progressTotal; }
            set
            {
                if (value != progressTotal)
                {
                    progressTotal = value;
                    WriteProgress();
                    if (progressTotal <= 0)
                    {
                        // Reset progress
                        progressValue = 0;
                        progressHasWarning = false;
                        progressHasError = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a warning occured during processing, and
        /// updates the displayed progress bar accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static bool ProgressHasWarning
        {
            get { return progressHasWarning; }
            set
            {
                if (value != progressHasWarning)
                {
                    progressHasWarning = value;
                    WriteProgress();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an error occured during processing, and updates
        /// the displayed progress bar accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static bool ProgressHasError
        {
            get { return progressHasError; }
            set
            {
                if (value != progressHasError)
                {
                    progressHasError = value;
                    WriteProgress();
                }
            }
        }

        /// <summary>
        /// Writes the progress info in the current line, replacing the current line.
        /// </summary>
        private static void WriteProgress()
        {
            // Replace the current line with the new progress
            ClearLine();
            if (progressTotal > 0)
            {
                // Range checking
                int value = progressValue;
                if (value < 0) value = 0;
                if (value > progressTotal) value = progressTotal;

                Console.Write(progressTitle + " " + value.ToString().PadLeft(progressTotal.ToString().Length) + "/" + progressTotal + " ");

                // Use almost the entire remaining visible space for the progress bar
                int graphLength = 80;
                if (!IsOutputRedirected)
                {
                    graphLength = Console.WindowWidth - Console.CursorLeft - 4;
                }
                int graphPart = progressTotal > 0 ? (int)Math.Round((double)value / progressTotal * graphLength) : 0;

                ConsoleColor graphColor;
                if (progressHasError)
                    graphColor = ConsoleColor.DarkRed;
                else if (progressHasWarning)
                    graphColor = ConsoleColor.DarkYellow;
                else
                    graphColor = ConsoleColor.DarkGreen;
                Write(new string('█', graphPart), graphColor);
                Write(new string('░', graphLength - graphPart), ConsoleColor.DarkGray);
            }
        }

        #endregion Progress bar

        #region Line wrapping

        /// <summary>
        /// Writes a string in multiple lines, limited to the console window width, wrapping at
        /// spaces whenever possible and keeping the first line indenting for wrapped lines.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        /// <param name="tableMode">Indents to the last occurence of two spaces; otherwise indents to leading spaces.</param>
        public static void WriteWrapped(string text, bool tableMode = false)
        {
            int width = !IsOutputRedirected ? Console.WindowWidth : 80;
            foreach (string line in text.Split('\n'))
            {
                Console.Write(FormatWrapped(line.TrimEnd(), width, tableMode));
            }
        }

        /// <summary>
        /// Writes a string with custom format control characters in multiple lines, limited to the
        /// console window width, wrapping at spaces whenever possible and keeping the first line
        /// indenting for wrapped lines. The previous color is restored.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        /// <param name="formatter">A function that can set the console color depending on the input
        ///   character. Return false to hide the character.</param>
        /// <param name="tableMode">Indents to the last occurence of two spaces; otherwise indents to leading spaces.</param>
        public static void WriteWrappedFormatted(string text, Func<char, bool> formatter, bool tableMode = false)
        {
            int width = !IsOutputRedirected ? Console.WindowWidth : 80;
            foreach (string line in text.Split('\n'))
            {
                WriteFormatted(FormatWrapped(line.TrimEnd(), width, tableMode), formatter);
            }
        }

        /// <summary>
        /// Formats a string to multiple lines, limited to the specified width, wrapping at spaces
        /// whenever possible and keeping the first line indenting for wrapped lines.
        /// </summary>
        /// <param name="input">The input string to format.</param>
        /// <param name="width">The available width for wrapping.</param>
        /// <param name="tableMode">Indents to the last occurence of two spaces; otherwise indents to leading spaces.</param>
        /// <returns>The formatted string with line breaks and indenting in every line.</returns>
        public static string FormatWrapped(string input, int width, bool tableMode)
        {
            if (input.TrimEnd() == "") return Environment.NewLine;

            // Detect by how many spaces the text is indented. This amount will be used for every
            // following wrapped line.
            int indent = 0;
            if (tableMode)
            {
                indent = input.LastIndexOf("  ");
                if (indent != -1)
                {
                    indent += 2;
                }
                else
                {
                    indent = 0;
                }
            }
            else
            {
                while (input[indent] == ' ') indent++;
            }
            string indentStr = "";
            if (indent > 0)
            {
                indentStr = new string(' ', indent);
            }

            string output = "";
            bool haveReducedWidth = false;
            do
            {
                int pos = width - 1;
                if (pos >= input.Length)
                {
                    pos = input.Length;
                }
                else
                {
                    while (pos > 0 && input[pos] != ' ') pos--;
                    // If the line cannot be wrapped at a space, write it to the full width
                    if (pos == 0) pos = width - 1;
                }
                if (output != "")
                {
                    // Prepend indenting spaces for the following lines
                    output += indentStr;
                }
                output += input.Substring(0, pos) + Environment.NewLine;
                if (pos + 1 < input.Length)
                {
                    input = input.Substring(pos + 1);
                    // Reduce the available width by the indenting for the following lines
                    if (!haveReducedWidth)
                    {
                        width -= indent;
                        haveReducedWidth = true;
                    }
                }
                else
                {
                    input = "";
                }
            }
            while (input.Length > 0);
            return output;
        }

        #endregion Line wrapping

        #region Interaction

        /// <summary>
        /// Clears the key input buffer. Any keys that have been pressed but not yet processed
        /// before will be dropped.
        /// </summary>
        public static void ClearKeyBuffer()
        {
            if (!IsInputRedirected)
            {
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified key is an input key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsInputKey(ConsoleKey key)
        {
            int[] ignore = new[]
            {
                16,    // Shift (left or right)
				17,    // Ctrl (left or right)
				18,    // Alt (left or right)
				19,    // Pause
				20,    // Caps lock
				42,    // Print
				44,    // Print screen
				91,    // Windows key (left)
				92,    // Windows key (right)
				93,    // Menu key
				144,   // Num lock
				145,   // Scroll lock
				166,   // Back
				167,   // Forward
				168,   // Refresh
				169,   // Stop
				170,   // Search
				171,   // Favorites
				172,   // Start/Home
				173,   // Mute
				174,   // Volume Down
				175,   // Volume Up
				176,   // Next Track
				177,   // Previous Track
				178,   // Stop Media
				179,   // Play
				180,   // Mail
				181,   // Select Media
				182,   // Application 1
				183    // Application 2
			};
            return !ignore.Contains((int)key);
        }

        /// <summary>
        /// Waits for the user to press any key if in interactive mode and input is not redirected.
        /// </summary>
        /// <param name="message">The message to display. If null, a standard message is displayed.</param>
        /// <param name="timeout">The time in seconds until the method returns even if no key was pressed. If -1, the timeout is infinite.</param>
        /// <param name="showDots">true to show a dot for every second of the timeout, removing one dot each second.</param>
        public static void Wait(string message = null, int timeout = -1, bool showDots = false)
        {
            if (Environment.UserInteractive && !IsInputRedirected)
            {
                if (message == null)
                {
                    message = "Press any key to continue...";
                }

                if (message != "")
                {
                    ClearLine();
                    Console.Write(message);
                }
                if (timeout < 0)
                {
                    ClearKeyBuffer();
                    // Wait for a real input key
                    while (!IsInputKey(Console.ReadKey(true).Key))
                    {
                    }
                }
                else
                {
                    int counter;
                    if (showDots)
                    {
                        counter = timeout;
                        while (counter > 0)
                        {
                            counter--;
                            Console.Write(".");
                        }
                        timeout *= 1000;   // Convert to milliseconds
                        counter = 0;
                        int step = 100;   // Sleeping duration
                        int nextSecond = 1000;
                        ClearKeyBuffer();
                        while (!(Console.KeyAvailable && IsInputKey(Console.ReadKey(true).Key)) && counter < timeout)
                        {
                            Thread.Sleep(step);
                            counter += step;
                            if (showDots && counter > nextSecond)
                            {
                                nextSecond += 1000;
                                MoveCursor(-1);
                                Console.Write(" ");
                                MoveCursor(-1);
                            }
                        }
                        ClearKeyBuffer();
                    }
                    if (message != "")
                    {
                        Console.WriteLine();
                    }
                }
            }
        }

        /// <summary>
        /// Waits for the user to press any key if in debugging mode.
        /// </summary>
        /// <remarks>
        /// Visual Studio will wait after the program terminates only if not debugging. When the
        /// program was started with debugging, the console window is closed immediately. This
        /// method can be called at the end of the program to always wait once and be able to
        /// evaluate the last console output.
        /// </remarks>
        public static void WaitIfDebug()
        {
            if (Debugger.IsAttached)
            {
                Wait("Press any key to quit...");
            }
        }

        /// <summary>
        /// Writes an error message in red color, waits for a key and returns the specified exit
        /// code for passing it directly to the return statement.
        /// </summary>
        /// <param name="message">The error message to write.</param>
        /// <param name="exitCode">The exit code to return.</param>
        /// <returns></returns>
        public static int ExitError(string message, int exitCode)
        {
            ClearLine();
            using (new ConsoleColorScope(ConsoleColor.Red))
            {
                Console.Error.WriteLine(message);
            }
            WaitIfDebug();
            return exitCode;
        }

        #endregion Interaction

        #region Drawings
 
        /// <summary>
        /// Draws a rectangle inside the console
        /// </summary>
        /// <param name="width">Width in chars of the rectangle</param>
        /// <param name="height">Width in chars of the rectangle</param>
        /// <param name="location">location.Y will be Console.CursorTop, location.Y will be Console.CursorLeft. </param>
        /// <param name="borderColor">Rectangle color</param>
        public static void DrawRectangle(int width, int height, Point location, ConsoleColor borderColor = ConsoleColor.Gray, LineType lineType = LineType.singleLine)
        {
            //https://en.wikipedia.org/wiki/Windows-1252
            string topLeftAngle = "┌";
            string topRightAngle = "┐";
            string bottomLeftAngle = "└";
            string bottomRightAngle = "┘";
            string horizontal = "─";
            string vertical = "│";

            if (lineType == LineType.doubleLine)
            {
                topLeftAngle = "╔";
                topRightAngle = "╗";
                bottomLeftAngle = "╚";
                bottomRightAngle = "═";
                horizontal = "═";
                vertical = "║";
            }

            string s = topLeftAngle;
            string space = "";
            string temp = "";
            for (int i = 0; i < width; i++)
            {
                space += " ";
                s += horizontal;
            }

            for (int j = 0; j < location.X; j++)
                temp += " ";

            s += topRightAngle + "\n";

            for (int i = 0; i < height; i++)
                s += temp + vertical + space + vertical + "\n";

            s += temp + bottomLeftAngle;
            for (int i = 0; i < width; i++)
                s += horizontal;

            s += bottomRightAngle + "\n";

            Console.ForegroundColor = borderColor;
            Console.CursorTop = location.Y;
            Console.CursorLeft = location.X;
            Console.Write(s);
            Console.ResetColor();
        }

        /// <summary>
        /// Draws a line inside the console
        /// </summary>
        /// <param name="left">starting point</param>
        /// <param name="top">row</param>
        /// <param name="width">line width</param>
        /// <param name="lineColor">line color</param>
        public static void DrawLine(int left, int top, int width, ConsoleColor lineColor = ConsoleColor.Gray, LineType lineType = LineType.singleLine)
        {
            if (left >= Console.BufferWidth)
                return;
            if (top >= Console.BufferWidth)
                return;
            if (width + left - 1 >= Console.BufferWidth)
                return;

            string lineSymbol = lineType == LineType.singleLine ? "─" : "═";

            string s = "";
            for (int i = 0; i < width; i++)
            {
                s += lineSymbol;
            }
            Console.ForegroundColor = lineColor;

            Console.SetCursorPosition(left, top);
            Console.Write(s);
            Console.ResetColor();
        }

        #endregion

        #region text
        public static void WriteCenteredText(string text, int top, ConsoleColor textColor = ConsoleColor.Gray)
        {
            if (top >= Console.BufferWidth)
                return;

            int left = ((Console.BufferWidth - text.Length) / 2);

            if (left >= Console.BufferWidth)
                return;
            if (left + text.Length >= Console.BufferWidth)
                return;

            Console.ForegroundColor = textColor;
            Console.SetCursorPosition(left, top);
            Console.Write(text);
            Console.ResetColor();
        }
        #endregion
    }

    #region ConsoleColorScope helper class

    /// <summary>
    /// Changes the console foreground color and changes it back again.
    /// </summary>
    public class ConsoleColorScope : IDisposable
    {
        private ConsoleColor previousColor;

        /// <summary>
        /// Changes the console foreground color.
        /// </summary>
        /// <param name="color">The new foreground color.</param>
        public ConsoleColorScope(ConsoleColor color)
        {
            this.previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Changes the foreground color to its previous value.
        /// </summary>
        public void Dispose()
        {
            Console.ForegroundColor = previousColor;
        }
    }

    #endregion ConsoleColorScope helper class
}