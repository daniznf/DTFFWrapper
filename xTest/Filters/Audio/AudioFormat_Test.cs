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
using DTFFWrapper.Filters.Audio;
using Xunit.Abstractions;

namespace xTest.Filters.Audio
{
    public class AudioFormat_Test
    {
        private ITestOutputHelper output;
        SampleFormat u8, s16, s64;

        public AudioFormat_Test(ITestOutputHelper output)
        {
            this.output = output;
            
            // All SampleFormat will be retrieved automatically.
            u8 = new SampleFormat("u8", 8);
            s16 = new SampleFormat("s16", 16);
            s64 = new SampleFormat("s64", 64);
        }

        [Fact]
        public void s16_()
        {
            AudioFormat aFormat = new AudioFormat()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            aFormat.RequestedSampleFormats.Add(s16);

            string expected = "[in] aformat=sample_fmts=s16 [out]";
            string actual = aFormat.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void u8_s16_s64()
        {
            AudioFormat aFormat = new AudioFormat()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            aFormat.RequestedSampleFormats.Add(u8);
            aFormat.RequestedSampleFormats.Add(s16);
            aFormat.RequestedSampleFormats.Add(s64);

            string expected = "[in] aformat=sample_fmts=u8|s16|s64 [out]";
            string actual = aFormat.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void s16_Mono()
        {
            AudioFormat aFormat = new AudioFormat()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            aFormat.RequestedSampleFormats.Add(s16);
            aFormat.RequestedChannelLayouts.Add(ChannelLayout.ChMono);

            string expected = "[in] aformat=sample_fmts=s16:channel_layouts=mono [out]";
            string actual = aFormat.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void u8_s16_Mono_Stereo()
        {
            AudioFormat aFormat = new AudioFormat()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            aFormat.RequestedSampleFormats.Add(u8);
            aFormat.RequestedSampleFormats.Add(s16);
            aFormat.RequestedChannelLayouts.Add(ChannelLayout.ChMono);
            aFormat.RequestedChannelLayouts.Add(ChannelLayout.ChStereo);

            string expected = "[in] aformat=sample_fmts=u8|s16:channel_layouts=mono|stereo [out]";
            string actual = aFormat.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void u8_Mono_44100()
        {
            AudioFormat aFormat = new AudioFormat()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            aFormat.RequestedSampleFormats.Add(u8);
            aFormat.RequestedChannelLayouts.Add(ChannelLayout.ChMono);
            aFormat.RequestedSampleRates.Add(44100);

            string expected = "[in] aformat=sample_fmts=u8:sample_rates=44100:channel_layouts=mono [out]";
            string actual = aFormat.FFString();
            Assert.Equal(expected, actual.Trim());
        }

        [Fact]
        public void u8_s16_Mono_Stereo_44100_48000()
        {
            AudioFormat aFormat = new AudioFormat()
            {
                InputPin = new Pin("in"),
                OutputPin = new Pin("out")
            };

            aFormat.RequestedSampleFormats.Add(u8);
            aFormat.RequestedSampleFormats.Add(s16);
            aFormat.RequestedChannelLayouts.Add(ChannelLayout.ChMono);
            aFormat.RequestedChannelLayouts.Add(ChannelLayout.ChStereo);
            aFormat.RequestedSampleRates.Add(44100);
            aFormat.RequestedSampleRates.Add(48000);

            string expected = "[in] aformat=sample_fmts=u8|s16:sample_rates=44100|48000:channel_layouts=mono|stereo [out]";
            string actual = aFormat.FFString();
            Assert.Equal(expected, actual.Trim());
        }
    }
}