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

using DTFFWrapper.Encoders.Video;
using DTFFWrapper.Filters.Video;
using DTFFWrapper.Encoders.Audio;
using DTFFWrapper.Filters.Audio;
using System.Diagnostics;

namespace DTFFWrapper
{
    public enum EncodingPass { OnePass = 1, TwoPass = 2 }
    public class Wrapper
    {
        /// <summary>
        /// Creates a new instance of <paramref name="Wrapper"/> with NoneVideo and NoneAudio default encoders.
        /// </summary>
        public Wrapper()
        {
            VideoEncoder = new NoneVideo();
            AudioEncoder = new NoneAudio();

            SourceFiles = new();

            SkipSubtitles = true;
            SkipDataStreams = true;
        }

        /// <summary>
        /// Version of this assembly.
        /// </summary>
        public static Version? WrapperVersion => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Time where to start encoding.
        /// </summary>
        protected Time? Start;

        /// <summary>
        /// Time to convert.
        /// </summary>
        protected Time? Duration;

        #region Video Properties
        /// <summary>
        /// List of <paramref name="MediaFile"/> that must contain at least one video or audio stream.
        /// For each <paramref name="MediaFile"/> added, the selected stream will be converted.
        /// To convert multiple streams of a source file, add it multiple times and select each stream.
        /// </summary>
        protected List<MediaFile> SourceFiles { get; }

        /// <summary>
        /// Video encoder to use for conversion.
        /// </summary>
        protected IVideoEncoder VideoEncoder;

        /// <summary>
        /// Overrides any timestamp stored in source file.
        /// </summary>
        protected double? InputFramerate;

        /// <summary>
        /// Convert to this constant framerate.
        /// </summary>
        protected double? OutputFramerate;

        /// <summary>
        /// Overrides frame size of source video.
        /// </summary>
        protected Size? InputFrameSize;

        /// <summary>
        /// Desired frame size of output video.
        /// </summary>
        protected Size? OutputFrameSize;

        /// <summary>
        /// Ensure frame size is multiple of this number. Usually 8.
        /// </summary>
        protected int? DivisibleBy;

        /*
        private int? pass;
        /// <summary>
        /// One pass or two passes encoding.
        /// </summary>
        protected int? Pass
        {
            get => pass;
            set
            {
                if (value.HasValue)
                {
                    if (value == 1 || value == 2)
                    {
                        pass = value.Value;
                    }
                }
            }
        }
        */

        /// <summary>
        /// One pass or two passes encoding.
        /// </summary>
        protected EncodingPass Pass;

        /// <summary>
        /// If true, output a single file with audio and video.
        /// If false, output separate files for audio and video.
        /// </summary>
        protected bool JoinAudioVideo;
        #endregion

        #region Audio Properties
        /// <summary>
        /// Audio encoder to use for conversion.
        /// </summary>
        protected IAudioEncoder AudioEncoder;

        /// <summary>
        /// Overrides audio channel layout of source file.
        /// </summary>
        protected ChannelLayout? InChannels;

        /// <summary>
        /// Audio channel layout of output file.
        /// </summary>
        protected ChannelLayout? OutChannels;

        /// <summary>
        /// Output every channel as single audio file.
        /// </summary>
        protected bool SplitAudioChannels;
        #endregion

        protected bool SkipSubtitles;
        protected bool SkipDataStreams;

        /// <summary>
        /// Forms first part of the string to pass to ffmpeg process.
        /// </summary>
        private string InputArguments()
        {
            string toReturn;
            toReturn = "-stats";
            toReturn += " -v warning";

            // do not overwrite
            toReturn += " -n";

            if (SkipSubtitles) { toReturn += $" -sn ";  }

            if (SkipDataStreams) { toReturn += $" -dn "; }

            if (VideoEncoder is NoneVideo && AudioEncoder is NoneAudio)
            {
                throw new ArgumentException("Video or Audio encoder must not be equal to None.");
            }

            if (VideoEncoder == null && AudioEncoder == null)
            {
                throw new ArgumentException("Video or Audio encoder must not be equal to null.");
            }

            if (VideoEncoder is NoneVideo)
            {
                toReturn += $" {VideoEncoder.FFString()}";
            }

            if (AudioEncoder is NoneAudio)
            {
                toReturn += $" {AudioEncoder.FFString()}";
            }

            if (Start != null)
            {
                toReturn += $" -ss {Start.TotalSeconds}s";
            }

            // Duration is set in OutParams in the Mapper.
            
            if (InputFramerate != null && InputFramerate.HasValue)
            {
                toReturn += $" -framerate {InputFramerate.Value}";
            }

            // -i should always be at last position of input arguments.
            
            for (int i = 0; i < SourceFiles.Count; i++)
            {
                // There are no problems adding several input files but using only some of them.
                toReturn += $" -i \"{SourceFiles[i].SourcePath}\"";
            }
            return toReturn;
        }

        private Process? FFProcess;
        /// <summary>
        /// Starts the conversion process.
        /// </summary>
        /// <param name="outputDataReceived">Handler to invoke when output data is received from the process.</param>
        /// <param name="errorDataReceived">Handler to invoke when error data is received from the process.</param>
        /// <param name="processExited">Handler to invoke when process exits.</param>
        /// <returns>Process ID of the started process.</returns>
        protected int StartFFProcess(DataReceivedEventHandler outputDataReceived, DataReceivedEventHandler errorDataReceived, EventHandler processExited)
        {
            string ffArguments = FFString();

            FFProcess = new()
            {
                StartInfo = new()
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = Helper.FFmpegPath,
                    Arguments = ffArguments
                }
            };

            FFProcess.OutputDataReceived += outputDataReceived;
            FFProcess.ErrorDataReceived += errorDataReceived;
            FFProcess.Exited += processExited;

            FFProcess.Start();
            
            return FFProcess.Id;
        }

        /// <summary>
        /// Wait for the conversion process to exit
        /// </summary>
        protected async Task WaitProcess()
        {
            if (FFProcess != null)
            {
                await FFProcess.WaitForExitAsync();
            }
        }

        /// <summary>
        /// Stops the conversion process.
        /// </summary>
        /// <returns><c>true</c> if process has exited or was null.</returns>
        protected bool StopFFProcess()
        {
            try
            {
                if (FFProcess != null)
                {
                    FFProcess.Close();

                    if (!FFProcess.HasExited)
                    {
                        FFProcess.Kill();
                    }
                    
                    bool hasExited = FFProcess.HasExited;
                    FFProcess.Dispose();
                    return hasExited;
                }
                return true;
            }
            catch (Exception E)
            {

            }
            return false;
        }

        protected string FFString()
        {
            FilterGraph fg = new();
            
            string? destinationPath = null;
            string? destinationVideoPath;
            string? destinationAudioPath;
            string inputArgs, filterArgs, mapArgs, ffArguments;

            Mapper mapper = new();
            string outVideoParams = String.Empty;
            string outAudioParams = String.Empty;
            if (Duration != null)
            {
                if (Duration.Unit == TimeUnit.Frame)
                {
                    outVideoParams += $"-frames {Duration.Frames}";
                    outAudioParams += $"-frames {Duration.Frames}";
                }
                else
                {
                    outVideoParams += $"-t {Duration.TotalSeconds}s";
                    outAudioParams += $"-t {Duration.TotalSeconds}s";
                }
            }

            // for each SourceFile, create destinationPath, input pin, output pin
            for (int i = 0; i < SourceFiles.Count; i++)
            {
                MediaFile source = SourceFiles[i];
                if (source.SelectedStreamIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(source.SelectedStreamIndex), source.SelectedStreamIndex, 
                        ($"{nameof(source.SelectedStreamIndex)} is {source.SelectedStreamIndex}. Has this video been probed?"));
                }

                //MediaStream selectedStream = source.InternalStreams[source.SelectedStreamIndex];
                MediaStream selectedStream = source.SelectedStream;

                if (selectedStream.IsVideo && (VideoEncoder is not NoneVideo))
                {
                    #region Video Filters
                    FilterChain videoChain = new();

                    if (OutputFrameSize != null)
                    {
                        // Size has its own DivisibleBy, so we will not use the DivisibleBy of Scale filter, here
                        videoChain.Add(new Scale(OutputFrameSize.Width, OutputFrameSize.Height));
                    }

                    if (OutputFramerate != null && OutputFramerate.HasValue)
                    {
                        videoChain.Add(new FPS(OutputFramerate.Value));
                    }

                    if (videoChain.Count == 0)
                    {
                        videoChain.Add(new NullVideo());
                    }

                    // There will not be another input pin with this name
                    videoChain.InputPin = new(i + ":v");
                    // There may be other output pins with this name. They will be renamed by FilterGraph.
                    videoChain.OutputPin = new("vOut");
                    fg.ChainList.Add(videoChain);
                    #endregion

                    #region Map Video
                    if (JoinAudioVideo)
                    {
                        // First added source takes precedence in naming the final file.
                        destinationPath = Helper.GetDestinationPath(SourceFiles[0].SourcePath, VideoEncoder, null, OutputFramerate, OutputFrameSize);
                        CreateParentDirectoryIfNotExists(destinationPath);

                        mapper.Add(new OutMap(
                            videoChain.OutputPin,
                            VideoEncoder,
                            destinationPath,
                            outVideoParams
                        ));
                    }
                    else
                    {
                        destinationVideoPath = Helper.GetDestinationPath(source.SourcePath, VideoEncoder, null, OutputFramerate, OutputFrameSize);
                        CreateParentDirectoryIfNotExists(destinationVideoPath);

                        mapper.Add(new OutMap(
                            videoChain.OutputPin,
                            VideoEncoder,
                            destinationVideoPath,
                            outVideoParams
                        ));
                    }
                    #endregion
                }

                if (selectedStream.IsAudio && (AudioEncoder is not NoneAudio))
                {                    
                    FilterChain audioChain = new();

                    #region Audio filters
                    // Other audio filters...

                    if (audioChain.Count == 0)
                    {
                        audioChain.Add(new NullAudio());
                    }    
                
                    audioChain.InputPin = new(i + ":a");
                    audioChain.OutputPin = new("aOut");
                    fg.ChainList.Add(audioChain);
                    #endregion

                    #region Map Audio
                    if (JoinAudioVideo)
                    {
                        // First added source takes precedence in naming the final file.

                        // destinationPath is video's destination path, if any.
                        if (destinationPath == null)
                        {
                            destinationPath = Helper.GetDestinationPath(SourceFiles[0].SourcePath, AudioEncoder, null);
                            CreateParentDirectoryIfNotExists(destinationPath);
                        }

                        mapper.Add(new OutMap(
                            audioChain.OutputPin,
                            AudioEncoder,
                            destinationPath,
                            outAudioParams
                        ));
                    }
                    else
                    {
                        destinationAudioPath = Helper.GetDestinationPath(source.SourcePath, AudioEncoder, null);
                        CreateParentDirectoryIfNotExists(destinationAudioPath);

                        mapper.Add(new OutMap(
                            audioChain.OutputPin,
                            AudioEncoder,
                            destinationAudioPath,
                            outVideoParams
                        ));
                    }
                    #endregion
                }
            }

            inputArgs = InputArguments();
            filterArgs = fg.FFString();
            mapArgs = mapper.FFString();
            ffArguments = $"{inputArgs} {filterArgs} {mapArgs}";
            return ffArguments;   
        }

        private void CreateParentDirectoryIfNotExists(string filePath)
        {
            string? destinationDirectory;
            destinationDirectory = Directory.GetParent(filePath)?.FullName;
            if (destinationDirectory != null && !Directory.Exists(destinationDirectory))
            { 
                Directory.CreateDirectory(destinationDirectory);
            }
        }
    }
}
