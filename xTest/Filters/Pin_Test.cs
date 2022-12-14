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

using DTFFWrapper.Filters;
using Xunit.Abstractions;

namespace xTest.Filters
{
    public class Pin_Test
    {
        private ITestOutputHelper output;

        public Pin_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SimpleName()
        {
            Pin testPin = new("SimpleName");
            Assert.Equal("SimpleName", testPin.Name);
        }

        [Fact]
        public void StrangeCharactersName()
        {
            Pin testPin = new("S!h\"o?u$l%d_N&o/t_C(o)n=t?a^i'n[S]t@r#a;n g?e*C\\h|aracters-0:1:2:3");
            Assert.Equal("S-h-o-u-l-d_N-o-t_C-o-n-t-a-i-n-S-t-r-a-n-g-e-C-h-aracters-0:1:2:3", testPin.Name);
        }

        [Fact]
        public void ConnectPin()
        {
            Pin fromPin = new("from");
            Pin toPin = new("to");

            Assert.Equal("from", fromPin.Name);
            Assert.Equal("to", toPin.Name);

            fromPin.ConnectTo(ref toPin);
            Assert.Equal("from", fromPin.Name);
            Assert.Equal("from", toPin.Name);

            fromPin.Name = "changed";
            Assert.Equal("changed", fromPin.Name);
            Assert.Equal("changed", toPin.Name);

            toPin.Name = "again";
            Assert.Equal("again", fromPin.Name);
            Assert.Equal("again", toPin.Name);
        }
    }
}