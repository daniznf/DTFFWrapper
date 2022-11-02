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
    public class HAP_Test
    {
        private ITestOutputHelper output;

        public HAP_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Hap_Format_Strict()
        {
            HAP encoder = new();
            encoder.Format = HAP.HAPFormat.hap_q;

            encoder.StrictLevel = StrictLevel.strict;

            string actual = encoder.FFString();

            Assert.Contains("hap", actual);
            Assert.Contains("-format hap_q", actual);
            Assert.Contains("-strict strict", actual);
        }

        [Fact]
        public void Hap_Format_Compressor_Chunks_Other()
        {
            HAP encoder = new();
            encoder.Format = HAP.HAPFormat.hap_q;
            encoder.Compressor = HAP.HAPCompressor.snappy;
            encoder.Chunks = 8;

            encoder.Bitrate = 10000;
            encoder.GOP = 10;
            encoder.BFrames = 20;
            encoder.StrictLevel = StrictLevel.strict;
            encoder.GenericFlags = GenericFlags.cgop;
            encoder.GenericFlags |= GenericFlags.gray;

            string actual = encoder.FFString();

            Assert.Contains("hap", actual);
            Assert.Contains("-format hap_q", actual);
            Assert.Contains("-compressor snappy", actual);
            Assert.Contains("-chunks 8", actual);
            Assert.Contains("-b 10000", actual);
            Assert.Contains("-g 10", actual);
            Assert.Contains("-bf 20", actual);
            Assert.Contains("-strict strict", actual);
            Assert.Contains("-flags +gray+cgop", actual);
        }
    }
}