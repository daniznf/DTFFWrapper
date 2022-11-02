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
    public class Time_Test
    {
        private ITestOutputHelper output;

        public Time_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void SecondsDecimal()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // Seconds with decimal part
            sec = 7890.123456;
            frameRate = 25;
            frames = Convert.ToInt32(sec * frameRate);
            hms = "02:11:30.123456";
            t = new(sec, frameRate);

            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(sec, t.TotalSeconds);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(hms, t.HMS);
        }

        [Fact]
        public void Frames()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // Frames
            frames = 37020;
            sec = 1234;
            frameRate = 30;
            hms = frames.ToString() + 'f';
            t = new(frames, frameRate);

            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
            Assert.Equal(hms, t.HMS);
        }

        [Fact]
        public void HMSLongDecimalPoint()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS long, with decimal part, with point
            hms = "01:02:03.456789";
            sec = 3723.456789;
            frameRate = 60;
            frames = 223407;
            t = new(hms, frameRate);

            Assert.Equal(hms.Replace(',', '.'), t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSLongDecimalComma()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS long, with decimal part, with comma
            hms = "01:02:03.456789";
            sec = 3723.456789;
            frameRate = 60;
            frames = 223407;
            t = new(hms.Replace('.', ','), frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSLongNoDecimal()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS long, without decimal part
            hms = "01:02:03";
            sec = 3723;
            frameRate = 60;
            frames = 223380;
            t = new(hms, frameRate);

            Assert.Equal(hms + ".0", t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSShortDecimalPoint()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS short, with decimal part with point
            sec = 10.0;
            hms = "00:00:10.0";
            frameRate = 60;
            frames = 600;
            t = new(sec.ToString().Replace(',', '.'), frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }
        
        [Fact]
        public void HMSShortDecimalComma()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS short, with decimal part with comma
            sec = 10.0;
            hms = "00:00:10.0";
            frameRate = 60;
            frames = 600;
            t = new(sec.ToString().Replace('.', ','), frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }
        
        [Fact]
        public void HMSShortNoDecimal()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS short, without decimal part
            sec = 100;
            hms = "00:01:40.0";
            frameRate = 120;
            frames = 12000;
            t = new(sec.ToString(), frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSFrames()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS frames
            frames = 123456;
            hms = frames.ToString() + 'f';
            frameRate = 50;
            sec = 2469.12;
            t = new(hms, frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSSecondDecimalPoint()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS seconds, with decimal part with point
            sec = 1234.516273;
            hms = sec.ToString().Replace(',', '.') + 's';
            frameRate = 30;
            frames = 37035;
            t = new(hms, frameRate);
            hms = "00:20:34.516273";

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSSecondDecimalComma()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS seconds, with decimal part with comma
            sec = 1234.516273;
            hms = sec.ToString().Replace('.', ',') + 's';
            frameRate = 30;
            frames = 37035;
            t = new(hms, frameRate);
            hms = "00:20:34.516273";

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSSecondNoDecimal()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS seconds, without decimal part 
            sec = 1234;
            hms = sec.ToString() + 's';
            frameRate = 30;
            frames = 37020;
            t = new(hms, frameRate);
            hms = "00:20:34.0";

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSWrongMinutes()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS with wrong minutes
            hms = "11:40:10.0";
            sec = 42010;
            frameRate = 60;
            frames = 2520600;
            t = new("00:700:10.0", frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSWrongSeconds()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS with wrong seconds
            hms = "00:01:10.0";
            sec = 70.0;
            frameRate = 60;
            frames = 4200;
            t = new("00:00:70.0", frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }

        [Fact]
        public void HMSInvalidFormat()
        {
            Time t;
            double sec, frameRate;
            int frames;
            string hms;

            // HMS invalid format
            hms = "00:00:00.0";
            sec = 0.0;
            frameRate = 60;
            frames = 0;
            t = new("not valid", frameRate);

            Assert.Equal(hms, t.HMS);
            Assert.Equal(frameRate, t.FrameRate);
            Assert.Equal(frames, t.Frames);
            Assert.Equal(sec, t.TotalSeconds);
        }
    }
}