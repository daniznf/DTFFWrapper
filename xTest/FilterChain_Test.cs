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
using DTFFWrapper.Filters.Video;
using Xunit.Abstractions;

namespace xTest
{
    public class FilterChain_Test
    {
        private ITestOutputHelper output;

        public FilterChain_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void WithoutChainInputOutputPins()
        {
            FilterChain FC;

            FC = new("TestChain");

            FC.Add(new NullVideo()
            {
                InputPin = new("Ain"),
                OutputPin = new("Aout")
            });
            FC.Add(new NullVideo()
            {
                InputPin = new("Bin"),
                OutputPin = new("Bout")
            });
            FC.Add(new NullVideo()
            {
                InputPin = new("Cin"),
                OutputPin = new("Cout")
            });
            FC.Add(new NullVideo()
            {
                InputPin = new("Din"),
                OutputPin = new("Dout")
            });

            string expected = "[TestChain_in] null [Aout]; [Aout] null [Bout]; [Bout] null [Cout]; [Cout] null [TestChain_out]";
            string actual = FC.FFString();
            //output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WithChainInputOutputPins()
        {
            FilterChain FC;

            FC = new("TestChain");
            FC.InputPin = new("ChainInputPin");
            FC.OutputPin = new("ChainOutputPin");

            FC.Add(new NullVideo()
            {
                InputPin = new("Ain"),
                OutputPin = new("Aout")
            });
            FC.Add(new NullVideo()
            {
                InputPin = new("Bin"),
                OutputPin = new("Bout")
            });
            FC.Add(new NullVideo()
            {
                InputPin = new("Cin"),
                OutputPin = new("Cout")
            });
            FC.Add(new NullVideo()
            {
                InputPin = new("Din"),
                OutputPin = new("Dout")
            });

            string expected = "[ChainInputPin] null [Aout]; [Aout] null [Bout]; [Bout] null [Cout]; [Cout] null [ChainOutputPin]";
            string actual = FC.FFString();
            //output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}