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
    public class Wrapper_Test: Wrapper
    {
        private string sourceDir, videoSourceName, audioSourceName, videoSourcePath, audioSourcePath;
        private ITestOutputHelper output;

        public Wrapper_Test(ITestOutputHelper output)
        {
            this.output = output;
            sourceDir = "C:\\DTFF_Test\\Source videos";
            videoSourceName = "Video.vid";
            audioSourceName = "Audio.aud";
            videoSourcePath = Path.Combine(sourceDir, videoSourceName);
            audioSourcePath = Path.Combine(sourceDir, audioSourceName);
        }

        [Fact]
        public void SourceVideo()
        {            
            AutoGeneratedMediaFile videoFile = new(videoSourcePath);
            videoFile.GenerateInfo();
            SourceFiles.Add(videoFile);
            VideoEncoder = new HAP();

            string actual = FFString();
            //output.WriteLine(actual);
            Assert.Contains($"-i \"{videoSourcePath}\"", actual);
            Assert.Contains(@"[0:v]", actual);
        }

        [Fact]
        public void SourceAudio()
        {
            AutoGeneratedMediaFile audioFile = new(audioSourcePath);
            audioFile.GenerateInfo();
            audioFile.SelectedStreamIndex = 1;
            SourceFiles.Add(audioFile);
            AudioEncoder = new WAV();

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains($"-i \"{audioSourcePath}\"", actual);
            Assert.Contains(@"[0:a]", actual);
        }

        [Fact]
        public void SourceSame()
        {
            AutoGeneratedMediaFile SameTestMediaFileVideo = new(videoSourcePath);
            SameTestMediaFileVideo.GenerateInfo();

            AutoGeneratedMediaFile SameTestMediaFileAudio = new(videoSourcePath);
            SameTestMediaFileAudio.GenerateInfo();
            SameTestMediaFileAudio.SelectedStreamIndex = 1;

            SourceFiles.Add(SameTestMediaFileVideo);
            SourceFiles.Add(SameTestMediaFileAudio);

            VideoEncoder = new HAP();
            AudioEncoder = new WAV();

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains($"-i \"{videoSourcePath}\" -i \"{videoSourcePath}\"", actual);
            Assert.Contains(@"[0:v]", actual);
            Assert.Contains(@"[1:a]", actual);
        }

        [Fact]
        public void SourceDistinct()
        {
            AutoGeneratedMediaFile videoFile = new(videoSourcePath);
            videoFile.GenerateInfo();

            AutoGeneratedMediaFile audioFile = new(audioSourcePath);
            audioFile.GenerateInfo();
            audioFile.SelectedStreamIndex = 1;

            SourceFiles.Add(videoFile);
            SourceFiles.Add(audioFile);

            VideoEncoder = new HAP();
            AudioEncoder = new WAV();

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains($"-i \"{videoSourcePath}\"", actual);
            Assert.Contains($"-i \"{audioSourcePath}\"", actual);
            Assert.Contains(@"[0:v]", actual);
            Assert.Contains(@"[1:a]", actual);
        }

        [Fact]
        public void VideoInputFramerate()
        {
            AutoGeneratedMediaFile videoFile = new(videoSourcePath);
            videoFile.GenerateInfo();
            SourceFiles.Add(videoFile);
            VideoEncoder = new HAP();
            InputFramerate = 30;

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains($"-i \"{videoSourcePath}\"", actual);
            Assert.Contains($"-filter_complex ", actual);
            Assert.Contains(@"[0:v]", actual);
            Assert.Contains($"-framerate 30", actual);
        }

        [Fact]
        public void VideoOutputFramerateFrameSize()
        {
            AutoGeneratedMediaFile videoFile = new(videoSourcePath);
            videoFile.GenerateInfo();
            SourceFiles.Add(videoFile);
            VideoEncoder = new HAP();
            OutputFramerate = 30;
            OutputFrameSize = new(1026, 766, 8);

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains($"-i \"{videoSourcePath}\"", actual);
            Assert.Contains($"-filter_complex ", actual);
            Assert.Contains($"[0:v]", actual);
            Assert.Contains($"] fps=fps=30 [", actual);
            Assert.Contains($"] scale=w=1024:h=768 [", actual);
        }

        [Fact]
        public void HAP()
        {
            AutoGeneratedMediaFile videoFile = new(videoSourcePath);
            videoFile.GenerateInfo();

            SourceFiles.Add(videoFile);
            OutputFramerate = 30;
            Start = new(10.0, OutputFramerate.Value);
            Duration = new(100, OutputFramerate.Value);
            VideoEncoder = new HAP();

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains(@"-ss 10s", actual);
            Assert.Contains(@"-i ""C:\DTFF_Test\Source videos\Video.vid""", actual);
            Assert.Contains(@"-filter_complex", actual);
            Assert.Contains(@"[0:v]", actual);
            Assert.Contains(@"] fps=fps=30 [", actual);
            Assert.Contains(@"[vOut]", actual);
            Assert.Matches(@"-map ""\[vOut\]"" -c:v hap -frames 100 ""C:\\DTFF_Test\\Source videos\\HAP\\Video(_\d+)*.mov""", actual);
        }

        [Fact]
        public void WAV()
        {
            AutoGeneratedMediaFile audioFile = new(audioSourcePath);
            audioFile.GenerateInfo();
            audioFile.SelectedStreamIndex = 1;
            SourceFiles.Add(audioFile);

            Start = new(20.0, 25);
            Duration = new(10.0, 25);

            AudioEncoder = new WAV();

            string actual = FFString();
            //output.WriteLine(actual);

            Assert.Contains(@"-ss 20s", actual);
            Assert.Contains(@"-i ""C:\DTFF_Test\Source videos\Audio.aud""", actual);
            Assert.Contains(@"-filter_complex", actual);
            Assert.Contains(@"[0:a]", actual);
            Assert.Contains(@"] anull [", actual);
            Assert.Contains(@"[aOut]", actual);
            Assert.Matches(@"-map ""\[aOut\]"" -c:a u16le -t 10s ""C:\\DTFF_Test\\Source videos\\WAV\\Audio(_\d+)*.wav""", actual);
        }

        [Fact]
        public void SourceSameNoneVideo()
        {
            AutoGeneratedMediaFile SameTestMediaFileVideo = new(videoSourcePath);
            SameTestMediaFileVideo.GenerateInfo();

            AutoGeneratedMediaFile SameTestMediaFileAudio = new(videoSourcePath);
            SameTestMediaFileAudio.GenerateInfo();
            SameTestMediaFileAudio.SelectedStreamIndex = 1;

            SourceFiles.Add(SameTestMediaFileVideo);
            SourceFiles.Add(SameTestMediaFileAudio);

            VideoEncoder = new NoneVideo();
            AudioEncoder = new WAV();

            string actual = FFString();
            //output.WriteLine(actual);
            Assert.Contains($"-i \"{videoSourcePath}\" -i \"{videoSourcePath}\"", actual);
            Assert.DoesNotContain(@"[0:v]", actual);
            Assert.Contains(@"[1:a]", actual);
        }
    }
}