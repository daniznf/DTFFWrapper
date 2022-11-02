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
using DTFFWrapper.Filters;
using Xunit.Abstractions;

namespace xTest
{
    public class Mapper_Test
    {
        private ITestOutputHelper output;

        public Mapper_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void NoneJoinedTest()
        {
            Mapper mapper = new();

            NoneVideo vEnc = new();
            NoneAudio aEnc = new();

            mapper.Add(new OutMap(new Pin("videoPin1"), vEnc, @"C:\my test\None\video", "-t 10 -other param"));
            mapper.Add(new OutMap(new Pin("videoPin2"), vEnc, @"C:\my test\None\video", "-t 10"));
            mapper.Add(new OutMap(new Pin("audioPin1"), aEnc, @"C:\my test\None\video"));
            mapper.Add(new OutMap(new Pin("audioPin2"), aEnc, @"C:\my test\None\video"));
            mapper.Add(new OutMap(new Pin("audioPin3"), aEnc, @"C:\my test\None\video"));
            mapper.Add(new OutMap(new Pin("audioPin4"), aEnc, @"C:\my test\None\video"));

            string actual = mapper.FFString();
            //output.WriteLine(actual);

            Assert.Contains(@"-map ""[videoPin1]"" -c:v -vn -t 10 -other param", actual);
            Assert.Contains(@"-map ""[videoPin2]"" -c:v -vn -t 10", actual);
            Assert.Contains(@"-map ""[audioPin1]"" -c:a -an", actual);
            Assert.Contains(@"-map ""[audioPin2]"" -c:a -an", actual);
            Assert.Contains(@"-map ""[audioPin3]"" -c:a -an", actual);
            Assert.Contains(@"-map ""[audioPin4]"" -c:a -an ""C:\my test\None\video""", actual);
        }

        [Fact]
        public void NoneSplitTest()
        {
            Mapper mapper = new();

            mapper.Add(new OutMap(new Pin("videoPin"), new NoneVideo(), @"C:\my test\None\video", "-t 10 -other param"));
            mapper.Add(new OutMap(new Pin("audioPin"), new NoneAudio(), @"C:\my test\None\audio"));

            string actual = mapper.FFString();
            //output.WriteLine(actual);

            Assert.Contains(@"-map ""[videoPin]"" -c:v -vn -t 10 -other param ""C:\my test\None\video""", actual);
            Assert.Contains(@"-map ""[audioPin]"" -c:a -an ""C:\my test\None\audio""", actual);
        }

        [Fact]
        public void HAPWAVJoinedTest()
        {
            Mapper mapper = new();

            HAP vEnc = new()
            {
                Chunks = 8,
                Format = HAP.HAPFormat.hap_q
            };
            
            mapper.Add(new OutMap(new Pin("vencoder_out_1"), vEnc, @"C:\my test\HAP\video", "-t 10 -other param"));

            WAV aEnc = new()
            {
                AudioChannelsNumber = 2,
                AudioSamplingRate = 44100
            };

            mapper.Add(new OutMap(new Pin("aencoder_out_1"), aEnc, @"C:\my test\WAV\audio"));
            mapper.Add(new OutMap(new Pin("aencoder_out_2"), aEnc, @"C:\my test\WAV\audio"));

            string actual = mapper.FFString();
            //output.WriteLine(actual);
            Assert.Contains(@"-map ""[vencoder_out_1]"" -c:v hap -format hap_q -chunks 8 -t 10 -other param ""C:\my test\HAP\video""", actual);
            Assert.Contains(@"-map ""[aencoder_out_1]"" -c:a u16le -ar 44100 -ac 2", actual);
            Assert.Contains(@"-map ""[aencoder_out_2]"" -c:a u16le -ar 44100 -ac 2 ""C:\my test\WAV\audio""", actual);
        }

        [Fact]
        public void HAPWAVSPlitTest()
        {
            Mapper mapper = new();

            HAP vEnc = new()
            {
                StrictLevel = StrictLevel.strict,
                Compressor = HAP.HAPCompressor.snappy,
                ColorSpace = ColorSpace.bt2020c,
                Chunks = 8,
                Format = HAP.HAPFormat.hap_q
            };

            vEnc.GenericFlags |= GenericFlags.gray;
            vEnc.GenericFlags |= GenericFlags.cgop;

            mapper.Add(new OutMap(new Pin("vencoder_out_1"), vEnc, @"C:\my test\HAP\video", "-t 10 -other param"));

            WAV aEnc = new()
            {
                AudioChannelsNumber = 2,
                AudioSamplingRate = 44100
            };

            mapper.Add(new OutMap(new Pin("aencoder_out_1"), aEnc, @"C:\my test\WAV\audio"));
            mapper.Add(new OutMap(new Pin("aencoder_out_2"), aEnc, @"C:\my test\WAV\audio"));

            string actual = mapper.FFString();
            //output.WriteLine(mapper.FFString());

            Assert.Contains(@"-map ""[vencoder_out_1]"" -c:v hap -strict strict -flags +gray+cgop -colorspace bt2020c " +
                @"-format hap_q -compressor snappy -chunks 8 -t 10 -other param ""C:\my test\HAP\video""", actual);
            Assert.Contains(@"-map ""[aencoder_out_1]"" -c:a u16le -ar 44100 -ac 2", actual);
            Assert.Contains(@"-map ""[aencoder_out_2]"" -c:a u16le -ar 44100 -ac 2 ""C:\my test\WAV\audio""", actual);
        }
    }
}
