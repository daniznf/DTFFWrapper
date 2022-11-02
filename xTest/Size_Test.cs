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
using Xunit.Abstractions;

namespace xTest
{
    public class Size_Test
    {
        private ITestOutputHelper output;

        public Size_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SizeDivisibleBy()
        {
            int w = 1026;
            int h = 766;
            Size s = new(w, h, 8);

            Assert.Equal(1024, s.Width);
            Assert.Equal(768, s.Height);
        }

        [Fact]
        public void SizeNoDivisibleBy()
        {
            int w = 1026;
            int h = 766;
            Size s = new(w, h);

            Assert.Equal(w, s.Width);
            Assert.Equal(h, s.Height);
        }
    }
}