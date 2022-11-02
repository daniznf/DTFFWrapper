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

using DTFFWrapper.Filters.Video;
using Xunit.Abstractions;

namespace xTest.Filters.Video
{
    public class FPS_Test
    {
        private ITestOutputHelper output;

        public FPS_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Fps25()
        {
            FPS fpsFilter = new(29.97)
            {
                InputPin = new("in"),
                OutputPin = new("out")
            };
            string expected = "[in] fps=fps=29.97 [out]";
            string actual = fpsFilter.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void FpsPAL()
        {
            FPS fpsFilter = new(FPS.PAL)
            {
                InputPin = new("in"),
                OutputPin = new("out")
            };
            string expected = "[in] fps=fps=25 [out]";
            string actual = fpsFilter.FFString();
            Assert.Equal(expected, actual.Trim());
        }
    }
}