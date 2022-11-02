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
using DTFFWrapper.Encoders.Video;
using Xunit.Abstractions;

namespace xTest.Encoders.Video
{
    public class x264_Test
    {
        private ITestOutputHelper output;

        public x264_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void x264_bitrate()
        {
            x264 encoder = new();
            
            string actual = encoder.FFString();

            Assert.Contains("x264", actual);
        }

        [Fact]
        public void x264_params_Other()
        {
            x264 encoder = new();
            
            encoder.Bitrate = 10000;
            encoder.GOP = 10;
            encoder.BFrames = 20;
            encoder.StrictLevel = StrictLevel.strict;
            encoder.GenericFlags |= GenericFlags.OpenGOP;
            encoder.GenericFlags |= GenericFlags.gray;
            encoder.x264Params.Add(new KeyValuePair<string, string>("keyint", "10"));
            encoder.x264Params.Add(new KeyValuePair<string, string>("ref", "3"));

            string actual = encoder.FFString();

            Assert.Contains("x264", actual);
            Assert.Contains("-b 10000", actual);
            Assert.Contains("-g 10", actual);
            Assert.Contains("-bf 20", actual);
            Assert.Contains("-strict strict", actual);
            Assert.Contains("-flags +gray-cgop", actual);
            Assert.Contains("-x264opts keyint=10:ref=3", actual);
        }
    }
}