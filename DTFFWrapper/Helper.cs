/*
    This file is part of DTFFWrapper.

    DTFFWrapper - Daniele's Tools Wrapper for FFmpeg
    Copyright (C) 2022 daniznf

    DTFFWrapper is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    DTFFWrapper is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
    
    https://github.com/daniznf/DTFFWrapper
 */

using System.Diagnostics;
using System.Text.RegularExpressions;
using DTFFWrapper.Encoders.Audio;

namespace DTFFWrapper
{
    public static class Helper
    {
        /// <summary>
        /// Initializes all needed properties: paths, versions, pixel formats, sample formats, ...
        /// </summary>
        /// <returns></returns>
        public static async Task InitializeAll()
        {
            await FindFFmpegPath(true);
            await FindFFmpegVersion();
            await FindPixelFormats();
            await FindSampleFormats();
            
            await FindFFprobePath(true);
            await FindFFprobeVersion();

            await FindFFplayPath(true);
            await FindFFplayVersion();
        }
        
        public static readonly StringComparison InvIgnCase = StringComparison.InvariantCultureIgnoreCase;

        [Flags]
        public enum FilenameAppends
        {
            None = 0,
            Bitrate = 1,
            FrameRate = 2,
            FrameSize = 4,
            SamplingRate = 8
            // ...
        }

        [Flags]
        public enum FileRenameModes
        {
            None = 0,
            AppendNumber = 1,
            AppendTime = 2,
            AppendDate = 4
        }

        /// <summary>
        /// Specifies what to append at the end of filename, before extension.<br/>
        /// <example> E.g.: filename_25_1920x1080</example><br/>
        /// You can specify several values to be appended to every filename generated by GetDestinationPath.
        /// </summary>
        public static FilenameAppends FilenameAppend { get; set; } = FilenameAppends.Bitrate | 
            FilenameAppends.FrameRate | FilenameAppends.FrameSize | FilenameAppends.SamplingRate;

        /// <summary>
        /// Specifies how to rename a file if one with the same name already exists.<br/>
        /// You can specify several modes of renaming a file.
        /// </summary>
        public static FileRenameModes FileRenameMode { get; set; } = FileRenameModes.AppendTime;

        #region FFexes
        /*/// <summary>
        /// Specifies to search for ffmpeg exes in children of parent directory.
        /// </summary>
        public static bool SearchOneLevelAbove { get; set; } = true;*/

        /// <summary>
        /// Path retrieved by FindFFmpegPath which decides the best exe to use.
        /// </summary>
        public static string? FFmpegPath { get; private set; }

        /// <summary>
        /// Path retrieved by FindFFprobePath which decides the best exe to use.
        /// </summary>
        public static string? FFprobePath { get; private set; }

        /// <summary>
        /// Path retrieved by FindFFplayPath which decides the best exe to use.
        /// </summary>
        public static string? FFplayPath { get; private set; }

        /// <summary>
        /// <inheritdoc cref="FindExePath(string, bool)" />
        /// If not found, ffmpeg.exe will be started without specifying a path.
        /// </summary>
        /// <param name="SearchOneLevelAbove">If true, the search is extended to parent folder's children.</param>
        public static async Task FindFFmpegPath(bool SearchOneLevelAbove)
        {
            FFmpegPath = await Task.Run(() => FindExePath("ffmpeg.exe", SearchOneLevelAbove));
        }

        /// <summary>
        /// <inheritdoc cref="FindExePath(string, bool)" />
        /// If not found, ffprobe.exe will be started without specifying a path.
        /// </summary>
        /// <param name="SearchOneLevelAbove">If true, the search is extended to parent folder's children.</param>
        public static async Task FindFFprobePath(bool SearchOneLevelAbove)
        {
            FFprobePath = await Task.Run(() => FindExePath("ffprobe.exe", SearchOneLevelAbove));
        }

        /// <summary>
        /// <inheritdoc cref="FindExePath(string, bool)" />
        /// If not found, ffplay.exe will be started without specifying a path.
        /// </summary>
        /// <param name="SearchOneLevelAbove">If true, the search is extended to parent folder's children.</param>
        public static async Task FindFFplayPath(bool SearchOneLevelAbove)
        {
            FFplayPath = await Task.Run(() => FindExePath("ffplay.exe", SearchOneLevelAbove));
        }

        /// <summary>
        /// Searches for specified file name, recursively in current directory.
        /// </summary>
        /// <param name="fileName">Filename to find (with extension).</param>
        /// <param name="searchOneLevelAbove">Step up by one directory and recurse all children directories.</param>
        /// <returns>Complete path of fileName if found, or <paramref name="fileName"/> if none was found.</returns>
        private static string FindExePath(string fileName, bool searchOneLevelAbove)
        {
            string? filePath;
            string currentDir;

            currentDir = Directory.GetCurrentDirectory();
            filePath = FindExePath(fileName, currentDir);

            if (searchOneLevelAbove)
            {
                // If still not found, step one level up. 
                if (filePath == null)
                {
                    string dir;
                    string[] directories;
                    DirectoryInfo? parentDir;
                    parentDir = Directory.GetParent(currentDir);
                    if (parentDir != null)
                    {
                        directories = Directory.GetDirectories(parentDir.FullName);
                        for (int i = 0; i < directories.Length; i++)
                        {
                            dir = directories[i];
                            if (dir != currentDir)
                            {
                                filePath = FindExePath(fileName, dir);
                                if (filePath != null) { break; }
                            }
                        }
                    }
                }
            }

            // If still not found, try to launch it directly.
            filePath ??= fileName;

            return filePath;
        }

        /// <summary>
        /// Searches for specified file name recursively in specified directory.
        /// </summary>
        /// <param name="fileName">File to search.</param>
        /// <param name="directory">Directory to search into.</param>
        /// <returns>Complete path of file found, or null if none was found.</returns>
        private static string? FindExePath(string fileName, string directory)
        {
            string? toReturn = null;
            string[] files, directories;
            string curFile;

            fileName = Path.GetFileName(fileName);
            files = Directory.GetFiles(directory);

            // Search fileName in current directory.
            for (int i = 0; i < files.Length; i++)
            {
                curFile = Path.GetFileName(files[i]);
                if (curFile.Equals(fileName, InvIgnCase))
                {
                    toReturn = Path.Combine(directory, curFile);
                    break;
                }
            }

            // If not found, recurse into subdirectories.
            if (toReturn == null)
            {
                directories = Directory.GetDirectories(directory);
                for (int i = 0; i < directories.Length; i++)
                {
                    toReturn = FindExePath(fileName, directories[i]);
                    if (toReturn != null) { break; }
                }
            }

            return toReturn;
        }
        #endregion

        private static Process StartProcess(string processPath, string[] arguments)
        {
            Process process;
            process = new()
            {
                StartInfo = new()
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = processPath
                }
            };

            for (int i = 0; i < arguments.Length; i++)
            {
                process.StartInfo.ArgumentList.Add(arguments[i]);
            }

            process.Start();

            return process;
        }

        #region Versions
        /// <summary>
        /// FFmpeg version retrieved by FindFFmpegVersion.
        /// </summary>
        public static string? FFmpegVersion { get; private set; }

        /// <summary>
        /// FFprobe version retrieved by FindFFprobeVersion.
        /// </summary>
        public static string? FFprobeVersion { get; private set; }

        /// <summary>
        /// FFplay version retrieved by FindFFplayVersion.
        /// </summary>
        public static string? FFplayVersion { get; private set; }

        /// <summary>
        /// Calls ffmpeg process, parses its version and stores it in <paramref name="FFmpegVersion"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static async Task FindFFmpegVersion()
        { 
            if (FFmpegPath == null)
            {
                throw new ArgumentNullException(nameof(FFmpegPath), "Please call " + nameof(FindFFmpegPath));
            }

            Process process = StartProcess(FFmpegPath, new string[] { "-version" });
            FFmpegVersion = ParseVersion("ffmpeg", process.StandardOutput) ?? ParseVersion("ffmpeg", process.StandardError);
            await process.WaitForExitAsync();
        }

        /// <summary>
        /// Calls ffprobe process, parses its version and stores it in <paramref name="FFprobeVersion"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static async Task FindFFprobeVersion()
        {
            if (FFprobePath == null)
            {
                throw new ArgumentNullException(nameof(FFprobePath), "Please call " + nameof(FindFFprobePath));
            }

            Process process = StartProcess(FFprobePath, new string[] { "-version" });
            FFprobeVersion = ParseVersion("ffprobe", process.StandardOutput) ?? ParseVersion("ffprobe", process.StandardError);
            await process.WaitForExitAsync();
        }

        /// <summary>
        /// Calls ffplay process, parses its version and stores it in <paramref name="FFplayVersion"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static async Task FindFFplayVersion()
        {
            if (FFplayPath == null)
            {
                throw new ArgumentNullException(nameof(FFplayPath), "Please call " + nameof(FindFFplayPath));
            }

            Process process = StartProcess(FFplayPath, new string[] { "-version" });
            FFplayVersion = ParseVersion("ffplay", process.StandardOutput) ?? ParseVersion("ffplay", process.StandardError);
            await process.WaitForExitAsync();
        }

        /// <summary>
        /// Parses the input stream searching for a version matching Major.Minor[.*][.*] pattern.
        /// </summary>
        /// <param name="filter">Parses lines containing this string.</param>
        /// <param name="stream">Stream to read.</param>
        /// <returns>Version as string if found, or null if not found.</returns>
        private static string? ParseVersion(string filter, StreamReader stream)
        {
            string? line;
            Match match;
            string regStr;

            regStr = @"\d+\.\d+(\.\d)?(\.\d)?";
            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();

                if ((line != null) && line.Contains(filter, InvIgnCase) &&
                        line.Contains("version", InvIgnCase))
                {
                    match = Regex.Match(line, regStr);
                    if (match.Success)
                    {
                        return match.Value;
                    }
                }
            }

            return null;
        }
        #endregion

        #region PixelFormats
        /// <summary>
        /// List of <paramref name="PixelFormat"/> structs, retrieved by ParsePixelFormats method.
        /// </summary>
        public static readonly List<PixelFormat> PixelFormatList = new();

        /// <summary>
        /// Calls ffmpeg process, parses all pixel formats and stores them in <paramref name="PixelFormatList"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static async Task FindPixelFormats()
        {
            if (FFmpegPath == null)
            {
                throw new ArgumentNullException(nameof(FFmpegPath), "Please call " + nameof(FindFFmpegPath));
            }

            Process process = StartProcess(FFmpegPath, new string[] { "-v", "error", "-pix_fmts" });
            ParsePixelFormats(process.StandardOutput);
            ParsePixelFormats(process.StandardError);
            await process.WaitForExitAsync();
        }

        private static void ParsePixelFormats(StreamReader stream)
        {
            string? line;
            string flags, name, nbComponents, bitsPerPixel, bitDepths;
            string[] splitted;
            int[] depths;

            PixelFormat pFormat;

            Match match;
            // IO... argb                   4             32      8-8-8-8
            // ..H.. opencl                 0              0      0
            string regStr = @"([A-Z|\.]{5})\s+(\w+)\s+(\d)\s+(\d+)\s+(\d+\-?\d*\-?\d*\-?\d*)";

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                if (line == null) { continue; }

                match = Regex.Match(line, regStr);
                if (match.Success)
                {
                    flags = match.Groups[1].Value;
                    name = match.Groups[2].Value;
                    nbComponents = match.Groups[3].Value;
                    bitsPerPixel = match.Groups[4].Value;
                    bitDepths = match.Groups[5].Value;

                    PixelFormats pFormats = new PixelFormats();

                    if (flags.Contains('I', InvIgnCase)) { pFormats |= PixelFormats.I; }
                    if (flags.Contains('O', InvIgnCase)) { pFormats |= PixelFormats.O; }
                    if (flags.Contains('H', InvIgnCase)) { pFormats |= PixelFormats.H; }
                    if (flags.Contains('P', InvIgnCase)) { pFormats |= PixelFormats.P; }
                    if (flags.Contains('B', InvIgnCase)) { pFormats |= PixelFormats.B; }

                    splitted = bitDepths.Split('-');
                    depths = new int[splitted.Length];
                    for (int i = 0; i < splitted.Length; i++)
                    {
                        depths[i] = Int32.Parse(splitted[i]);
                    }

                    pFormat = new PixelFormat(
                        pFormats,
                        name,
                        Int32.Parse(nbComponents),
                        Int32.Parse(bitsPerPixel),
                        depths 
                    );

                    PixelFormatList.Add(pFormat);
                }
            }
        }
        #endregion

        #region SampleFormats
        /// <summary>
        /// List of <paramref name="SampleFormat"/> structs, retrieved by ParseSampleFormats method.
        /// </summary>
        public static readonly List<SampleFormat> SampleFormatList = new();

        /// <summary>
        /// Calls ffmpeg process, parses all pixel formats and stores them in <paramref name="SampleFormatList"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        public static async Task FindSampleFormats()
        {
            if (FFmpegPath == null)
            {
                throw new ArgumentNullException(nameof(FFmpegPath), "Please call " + nameof(FindFFmpegPath));
            }

            Process process = StartProcess(FFmpegPath, new string[] { "-v", "error", "-sample_fmts" });
            ParseSampleFormats(process.StandardOutput);
            ParseSampleFormats(process.StandardError);

            await process.WaitForExitAsync();
        }

        private static void ParseSampleFormats(StreamReader stream)
        {
            string? line;
            string name, depth;
            SampleFormat sFormat;

            Match match;
            // u8        8
            string regStr = @"(\w+\d*\w*)\s+(\d+)";

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                if (line == null) { continue; }

                match = Regex.Match(line, regStr);
                if (match.Success)
                {
                    name = match.Groups[1].Value;
                    depth = match.Groups[2].Value;

                    sFormat = new SampleFormat(name, Int32.Parse(depth));
                    SampleFormatList.Add(sFormat);
                }
            }
        }
        #endregion

        #region FilePath
        /// <summary>
        /// Automatically generates complete path of file destination, appending and renaming, if necessary, 
        /// as specified by <paramref name="FilenameAppend"/> and <paramref name="FileRenameMode"/> .
        /// Video source path can be the same as audio source path.
        /// </summary>
        public static string GetDestinationPath(string destinationPath, IEncoder enc,
            string? overrideDestinationDir = null,
            double? frameRate = null, Size? frameSize = null)
        {
            string toReturn;
            string? dir = Path.GetDirectoryName(destinationPath);

            // WAV encoder would return strings like "u16le" but we want "WAV".
            string encString = enc.Name ?? ((enc is WAV) ? "WAV" : enc.FFName);

            string fileName = Path.GetFileNameWithoutExtension(destinationPath);

            if (FilenameAppend != FilenameAppends.None)
            {
                if (FilenameAppend.HasFlag(FilenameAppends.Bitrate) && enc.Bitrate != null && enc.Bitrate.HasValue)
                {
                    fileName += $"_{enc.Bitrate}";
                }
                if (FilenameAppend.HasFlag(FilenameAppends.FrameRate) && frameRate != null && frameRate.HasValue)
                {
                    fileName += $"_{frameRate.Value}";
                }

                if (FilenameAppend.HasFlag(FilenameAppends.FrameSize) && frameSize != null)
                {
                    fileName += $"_{frameSize.Width}x{frameSize.Height}";
                }

                if (FilenameAppend.HasFlag(FilenameAppends.SamplingRate) && (enc is IAudioEncoder aEnc) &&
                aEnc.AudioSamplingRate != null && aEnc.AudioSamplingRate.HasValue)
                {
                    fileName += $"_{aEnc.AudioSamplingRate}";
                }
            }

            if (overrideDestinationDir != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                toReturn = Path.Combine(dir ?? String.Empty, encString, fileName);
                toReturn = Path.ChangeExtension(toReturn, enc.FileExtension);
            }

            // After generating the name, a file with the same name could already exist.
            if (File.Exists(toReturn))
            {
                string newExt = Path.GetExtension(toReturn);
                toReturn = toReturn.Replace(newExt, null);

                if (FileRenameMode.HasFlag(FileRenameModes.AppendDate))
                {
                    // filename_20220123
                    toReturn += $"_{DateTime.Now.ToString("yyyyMMdd")}";
                }

                if (FileRenameMode.HasFlag(FileRenameModes.AppendTime))
                {
                    // filename_0123 or filename_20220123_0123
                    toReturn += $"_{DateTime.Now.ToString("HHmm")}";
                }

                if (FileRenameMode.HasFlag(FileRenameModes.AppendNumber))
                {
                    // filename_01 or filename_20220123_01 or filename_0123_01 or filename_20220123_0123_01
                    for (int n = 0; n < 100; n++)
                    {
                        toReturn = IncrementNameNumber(toReturn);
                        // A filename with that _number could already exist, so retry for 100 times.
                        if (!File.Exists(toReturn + newExt)) { break; }
                    }
                }

                toReturn += newExt;
            }

            return toReturn;
        }
        #endregion

        /// <summary>
        /// Adds or increments a number at the end of given name.<br/>
        /// <example>E.g.: <c>Name</c> becames <c>Name_01</c></example><br/>
        /// <example>E.g.: <c>Other_name_02</c> becames <c>Other_name_03</c></example>
        /// </summary>
        /// <param name="name">Name to add a number to. Can be with or without final _number.</param>      
        public static string IncrementNameNumber(string name)
        {
            // name_example_01
            string matchValue;
            int num = 0;
            Match match = Regex.Match(name, @".*_(\d+)");
            if (match.Success)
            {
                matchValue = match.Groups[1].Value;
                if (Int32.TryParse(matchValue, out num))
                {
                    name = name.Replace("_" + matchValue, String.Empty);
                }
                num++;
            }
            name += $"_{num.ToString("00")}";
            return name;
        }
    }
}