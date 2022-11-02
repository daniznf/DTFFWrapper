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
using DTFFWrapper.Filters;
using DTFFWrapper.Filters.Video;
using Xunit.Abstractions;

namespace xTest.Filters.Video
{
    public class VideoFormat_Test
    {
        private ITestOutputHelper output;
        PixelFormat yuv420p, yuva444p16le, yuv410p, opencl;

        public VideoFormat_Test(ITestOutputHelper output)
        {
            this.output = output;

            // All PixelFormat will be retrieved automatically.
            yuv420p = new PixelFormat(PixelFormats.I | PixelFormats.O, "yuv420p", 3, 12, new int[] { 8, 8, 8 });
            yuva444p16le = new PixelFormat(PixelFormats.I | PixelFormats.O, "yuva444p16le", 4, 64, new int[] { 16, 16, 16, 16 });
            yuv410p = new PixelFormat(PixelFormats.I | PixelFormats.O, "yuv410p", 3, 9, new int[] { 8, 8, 8, 8 });
            opencl = new PixelFormat(PixelFormats.H, "opencl", 0, 0, new int[] { 0 });
        }

        [Fact]
        public void yuv420p_()
        {
            Format format = new()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            format.RequestedPixelFormats.Add(yuv420p);

            string expected = "[in] format=pix_fmts=yuv420p [out]";
            string actual = format.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void yuva444p16le_()
        {
            Format format = new()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            format.RequestedPixelFormats.Add(yuva444p16le);

            string expected = "[in] format=pix_fmts=yuva444p16le [out]";
            string actual = format.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void yuv420p_yuva444p16le_yuv410p()
        {
            Format format = new()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            format.RequestedPixelFormats.Add(yuv420p);
            format.RequestedPixelFormats.Add(yuva444p16le);
            format.RequestedPixelFormats.Add(yuv410p);

            string expected = "[in] format=pix_fmts=yuv420p|yuva444p16le|yuv410p [out]";
            string actual = format.FFString();
            Assert.Equal(expected, actual.Trim());
        }
    }
}