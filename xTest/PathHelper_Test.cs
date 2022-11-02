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

using DTFFWrapper;
using DTFFWrapper.Encoders.Audio;
using DTFFWrapper.Encoders.Video;
using Xunit.Abstractions;

namespace xTest
{
    public class PathHelper_Test
    {
        string sourceDir, videoSourceName, audioSourceName, videoSourcePath, audioSourcePath;
        private ITestOutputHelper output;

        public PathHelper_Test(ITestOutputHelper output)
        {
            this.output = output;
            sourceDir = "C:\\DTFF_Test\\Source videos";
            videoSourceName = "Video.vid";
            audioSourceName = "Audio.aud";
            videoSourcePath = Path.Combine(sourceDir, videoSourceName);
            audioSourcePath = Path.Combine(sourceDir, audioSourceName);

            Helper.InitializeAll().Wait();
        }

        [Fact]
        public void FindFFPath()
        {
            Assert.Contains("ffmpeg.exe", Helper.FFmpegPath);
            Assert.Matches(@"\d+\.\d+", Helper.FFmpegVersion);
            Assert.Contains("ffprobe.exe", Helper.FFprobePath);
            Assert.Matches(@"\d+\.\d+", Helper.FFprobeVersion);
            Assert.Contains("ffplay.exe", Helper.FFplayPath);
            Assert.Matches(@"\d+\.\d+", Helper.FFplayVersion);
        }

        [Fact]
        public async void FindPixelFormats()
        {
            await Helper.FindPixelFormats();

            Assert.True(Helper.PixelFormatList.Count > 0);
            PixelFormat p;

            p = Helper.PixelFormatList.First(f => f.Name.Equals("yuv420p", Helper.InvIgnCase));
            Assert.True(p.Flags.HasFlag(PixelFormats.I));
            Assert.True(p.Flags.HasFlag(PixelFormats.O));
            Assert.False(p.Flags.HasFlag(PixelFormats.H));
            Assert.False(p.Flags.HasFlag(PixelFormats.P));
            Assert.False(p.Flags.HasFlag(PixelFormats.B));
            Assert.Equal(12, p.BitsPerPixel);
            Assert.Equal(3, p.BitDepth.Length);
            Assert.Equal(8, p.BitDepth[0]);
            Assert.Equal(8, p.BitDepth[1]);
            Assert.Equal(8, p.BitDepth[2]);

            p = Helper.PixelFormatList.First(f => f.Name.Equals("gray", Helper.InvIgnCase));
            Assert.True(p.Flags.HasFlag(PixelFormats.I));
            Assert.True(p.Flags.HasFlag(PixelFormats.O));
            Assert.False(p.Flags.HasFlag(PixelFormats.H));
            Assert.False(p.Flags.HasFlag(PixelFormats.P));
            Assert.False(p.Flags.HasFlag(PixelFormats.B));
            Assert.Equal(8, p.BitsPerPixel);
            Assert.Single(p.BitDepth);
            Assert.Equal(8, p.BitDepth[0]);

            Assert.Contains(Helper.PixelFormatList, p => p.Flags.HasFlag(PixelFormats.H));
            Assert.Contains(Helper.PixelFormatList, p => p.Flags.HasFlag(PixelFormats.B));
            Assert.Contains(Helper.PixelFormatList, p => p.Flags.HasFlag(PixelFormats.P));
        }

        [Fact]
        public async void FindSampleFormats()
        {
            await Helper.FindSampleFormats();

            Assert.True(Helper.SampleFormatList.Count > 0);
            SampleFormat s = Helper.SampleFormatList.First(f => f.Name.Equals("s16", Helper.InvIgnCase));
            Assert.Equal(16, s.Depth);
        }

        [Fact]
        public void SourcePath()
        {
            string actual = Helper.GetDestinationPath(videoSourcePath, new HAP());
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\HAP\\Video(_\d*).mov", actual);
        }

        [Fact]
        public void PathAppendTime()
        {
            Helper.FileRenameMode = Helper.FileRenameModes.AppendTime;
            string actual = Helper.GetDestinationPath(videoSourcePath, new HAP());
            
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\HAP\\Video_\d{4}.mov", actual);
        } 

        [Fact]
        public void PathAppendDate()
        {
            Helper.FileRenameMode = Helper.FileRenameModes.AppendDate;
            string actual = Helper.GetDestinationPath(videoSourcePath, new HAP());
            
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\HAP\\Video_\d{8}.mov", actual);
        }

        [Fact]
        public void PathAppendDateTime()
        {
            Helper.FileRenameMode = Helper.FileRenameModes.AppendDate | Helper.FileRenameModes.AppendTime;
            string actual = Helper.GetDestinationPath(videoSourcePath, new HAP());
            
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\HAP\\Video_\d{8}_\d{4}.mov", actual);
        }

        [Fact]
        public void PathAppendNumber()
        {
            Helper.FileRenameMode = Helper.FileRenameModes.AppendNumber;
            string actual = Helper.GetDestinationPath(videoSourcePath, new HAP());
            
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\HAP\\Video_\d{2}.mov", actual);
        }

        [Fact]
        public void PathAppendFPSSize()
        {
            string actual = Helper.GetDestinationPath(videoSourcePath, new HAP(), null, 25, new Size(1024,768));
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\HAP\\Video_\d{2}_\d{4}x\d{3}.mov", actual);
        }

        [Fact]
        public void PathAppendBitrateFPSSize()
        {
            string actual = Helper.GetDestinationPath(videoSourcePath, new x264() { Bitrate = 10000}, null, 30, new Size(1280, 720));
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\x264\\Video_\d{5}_\d{2}_\d{4}x\d{3}.mp4", actual);
        }

        [Fact]
        public void PathAppendSamplingRate()
        {
            string actual = Helper.GetDestinationPath(audioSourcePath, new WAV() { AudioSamplingRate = 44100 });
            //output.WriteLine(actual);
            Assert.Matches(@"C:\\DTFF_Test\\Source videos\\WAV\\Audio_\d{5}.wav", actual);
        }

        /*
        [Fact]
        public void SourcePathOverride()
        {
        }
        */
    }
}