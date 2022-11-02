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
    public class Scale_Test
    {
        private ITestOutputHelper output;

        public Scale_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ScaleNotDivisible()
        {
            int w = 1026;
            int h = 766;
            Scale scale = new(w, h)
            {
                InputPin = new("in"),
                OutputPin = new("out")
            };
            string expected = "[in] scale=w=1026:h=766 [out]";
            string actual = scale.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void ScaleDivisible()
        {
            int w = 1026;
            int h = 766;
            Scale scale = new(w, h, 8)
            {
                InputPin = new("in"),
                OutputPin = new("out")
            };
            string expected = "[in] scale=w=1026:h=766:force_divisible_by=8 [out]";
            string actual = scale.FFString();
            Assert.Equal(expected, actual.Trim());
        }
    }
}
