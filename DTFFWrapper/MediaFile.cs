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

namespace DTFFWrapper
{
    public class MediaFile
    {
        /// <summary>
        /// Instantiates a new MediaFile, but does not automatically probe file infos.
        /// Please call ProbeInfo to fetch file infos.
        /// </summary>
        /// <param name="sourcePath">Path of file source</param>
        /// <exception cref="FileNotFoundException"/>
        public MediaFile(string sourcePath)
        {
            selectedStreamIndex = -1;
            if (!File.Exists(sourcePath))
            {
                throw new FileNotFoundException(sourcePath);
            }
            SourcePath = sourcePath;
            InternalStreams = new();
        }

        /// <summary>
        /// Fetches informations from source file.
        /// If PathHelper.FFprobePath is missing or wrong, an exception will be thrown.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public async Task ProbeInfo()
        {
            if (Helper.FFprobePath != null)
            {
                InternalStreams.Clear();

                Process probeProcess;
                probeProcess = new()
                {
                    StartInfo = new(Helper.FFprobePath)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardError = true,
                        RedirectStandardOutput = true,
                        ArgumentList =
                        {
                            "-hide_banner",
                            "-v",
                            "error",
                            "-show_streams",
                            SourcePath
                        }
                    }
                };
                probeProcess.Start();
                ParseInfo(probeProcess.StandardError);
                ParseInfo(probeProcess.StandardOutput);
                await probeProcess.WaitForExitAsync();

                if (InternalStreams.Count > 0)
                {
                    SelectedStreamIndex = 0;
                }
                else
                {
                    throw new ArgumentException($"No streams were found in {SourcePath}", nameof(InternalStreams));
                }    
            }
        }

        protected void ParseInfo(StreamReader stream)
        {
            string? line;
            string[] splitted;
            MediaStream? intStream = null;

            while (!stream.EndOfStream)
            {
                line = stream.ReadLine();
                if (line != null)
                {
                    line = line.Trim();
                    if (line.Equals(MediaStream.STREAM, StringComparison.InvariantCultureIgnoreCase))
                    {
                        InternalStreams.Add(new());
                        intStream = InternalStreams.Last();
                    }
                    if (line.Contains('=') && intStream != null)
                    {
                        splitted = line.Split('=');
                        intStream.Add(splitted[0].Trim(), splitted[1].Trim());
                    }
                }
            }
        }

        public string SourcePath { get; }
        public List<MediaStream> InternalStreams { get; }

        public bool HasVideo => InternalStreams.Any(x =>
            string.Equals(x[MediaStream.CODEC_TYPE], "video", StringComparison.InvariantCultureIgnoreCase));
        public bool HasAudio => InternalStreams.Any(x =>
            string.Equals(x[MediaStream.CODEC_TYPE], "audio", StringComparison.InvariantCultureIgnoreCase));

        public MediaStream? FirstVideoStream => InternalStreams.FirstOrDefault(x =>
            string.Equals(x[MediaStream.CODEC_TYPE], "video", StringComparison.InvariantCultureIgnoreCase));
        public MediaStream? FirstAudioStream => InternalStreams.FirstOrDefault(x =>
            string.Equals(x[MediaStream.CODEC_TYPE], "audio", StringComparison.InvariantCultureIgnoreCase));

        private int selectedStreamIndex;
        /// <summary>
        /// Stream index that will be used for conversion, based on real stream index found in file.
        /// After probing file infos, the default value is 0, but can be set as needed.
        /// </summary>
        public int SelectedStreamIndex
        {
            get => selectedStreamIndex;
            set
            {
                if (InternalStreams.Any(s => s.StreamIndex == value))
                {
                    selectedStreamIndex = value;
                }
                else
                {
                    throw new ArgumentException($"No streams with id {value} were found in InternalStreams.", nameof(SelectedStreamIndex));
                }
            }
        }

        /// <summary>
        /// Selected stream with index equal to SelectedStreamIndex.
        /// </summary>
        public MediaStream SelectedStream => SelectedStreamIndex >= 0 ?
            InternalStreams[SelectedStreamIndex] : throw new ArgumentException(nameof(SelectedStreamIndex));
    }
}