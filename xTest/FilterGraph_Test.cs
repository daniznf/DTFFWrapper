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

using System.Text.RegularExpressions;
using DTFFWrapper;
using DTFFWrapper.Filters;
using DTFFWrapper.Filters.Audio;
using DTFFWrapper.Filters.Video;
using Xunit.Abstractions;

namespace xTest
{
    public class FilterGraph_Test
    {
        private ITestOutputHelper output;

        public FilterGraph_Test(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Names()
        {
            NullVideo filterBeforeVideoChain, filterAfterVideoChain;
            FilterChain videoChain, audioChain;
            
            FilterGraph FG = new();

            videoChain = new("My video chain");
            videoChain.InputPin = new("vChain_in");
            videoChain.OutputPin = new("vChain_out");
            videoChain.Add(new NullVideo("nv1"));
            videoChain.Add(new NullVideo("nv2"));
            videoChain.Add(new NullVideo("nv3"));
            videoChain.Add(new NullVideo("nv4"));

            // This filter will connect to videoChain.
            filterBeforeVideoChain = new("Filter before vchain")
            {
                InputPin = new("before_vc_in"),
                OutputPin = new("before_vc_out")
            };

            (filterBeforeVideoChain as IFilterSingle).ConnectTo(videoChain);

            // videoChain will connects to this filter.
            filterAfterVideoChain = new("Filter after vchain")
            {
                InputPin = new("after_vc_in"),
                OutputPin = new("after_vc_out")
            };

            (videoChain as IFilterSingle).ConnectTo(filterAfterVideoChain);

            audioChain = new("My audio chain");

            audioChain.Add(new NullAudio("na1"));
            audioChain.Add(new NullAudio("na2"));
            audioChain.Add(new NullAudio("na3"));
            audioChain.Add(new NullAudio("na4"));

            audioChain.InputPin = new("aChain_in");
            audioChain.OutputPin = new("aChain_out");

            FG.FilterList.Add(filterBeforeVideoChain);
            FG.ChainList.Add(videoChain);
            FG.FilterList.Add(filterAfterVideoChain);

            FG.ChainList.Add(audioChain);

            string actual = FG.FFString();
            // output.WriteLine(actual);
            VerifyPinUniqueness(actual);
        }

        [Fact]
        public void Rename()
        {
            FilterChain videoChain1, videoChain2, videoChain3;
            FilterChain audioChain1, audioChain2;
            NullVideo nvBefore, nvAfter;

            FilterGraph FG = new();

            videoChain1 = new();
            videoChain1.Add(new NullVideo());
            videoChain1.Add(new NullVideo());
            videoChain1.Add(new NullVideo());
            videoChain1.Add(new NullVideo());

            nvBefore = new();
            FG.FilterList.Add(nvBefore);
            (nvBefore as IFilterSingle).ConnectTo(videoChain1);
            nvAfter = new();
            FG.FilterList.Add(nvAfter);
            (videoChain1 as IFilterSingle).ConnectTo(nvAfter);

            videoChain2 = new();
            videoChain2.Add(new NullVideo());
            videoChain2.Add(new NullVideo());
            videoChain2.Add(new NullVideo());
            videoChain2.Add(new NullVideo());

            // Create another video chain and connect two filterChains
            videoChain3 = new();
            videoChain3.Add(new NullVideo());
            (videoChain2 as IFilterSingle).ConnectTo(videoChain3);

            audioChain1 = new();

            audioChain1.Add(new NullAudio());
            audioChain1.Add(new NullAudio());
            audioChain1.Add(new NullAudio());
            audioChain1.Add(new NullAudio());

            audioChain2 = new();

            audioChain2.Add(new NullAudio());
            audioChain2.Add(new NullAudio());
            audioChain2.Add(new NullAudio());
            audioChain2.Add(new NullAudio());

            FG.ChainList.Add(videoChain1);
            FG.ChainList.Add(videoChain2);
            FG.ChainList.Add(videoChain3);
            FG.ChainList.Add(audioChain1);
            FG.ChainList.Add(audioChain2);

            string actual = FG.FFString();
            //output.WriteLine(actual);

            VerifyPinUniqueness(actual);
        }


        [Fact]
        public void ChainToChain()
        {
            FilterChain videoChain1, videoChain2, videoChain3, videoChain4;
            FilterGraph FG = new();

            videoChain1 = new();
            videoChain1.Add(new NullVideo());

            videoChain2 = new();
            videoChain2.Add(new NullVideo());

            videoChain3 = new();
            videoChain3.Add(new NullVideo());

            (videoChain2 as IFilterSingle).ConnectTo(videoChain3);

            videoChain4 = new();
            videoChain4.Add(new NullVideo());

            FG.ChainList.Add(videoChain1);
            FG.ChainList.Add(videoChain2);
            FG.ChainList.Add(videoChain3);
            FG.ChainList.Add(videoChain4);

            string actual = FG.FFString();
            //output.WriteLine(actual);

            VerifyPinUniqueness(actual);
        }

        private void VerifyPinUniqueness(string filterString)
        {
            int n;
            Match iPin;

            MatchCollection inputPins = Regex.Matches(filterString, @"\[(\w+)\] \w+");
            for (int i = 0; i < inputPins.Count; i++)
            {
                iPin = inputPins[i];
                n = inputPins.Count(m => m.Groups[1].Value.Equals(iPin.Groups[1].Value, Helper.InvIgnCase));
                Assert.Equal(1, n);
            }

            MatchCollection outputPins = Regex.Matches(filterString, @"\w+ \[(\w+)\]");
            for (int i = 0; i < outputPins.Count; i++)
            {
                iPin = outputPins[i];
                n = outputPins.Count(m => m.Groups[1].Value.Equals(iPin.Groups[1].Value, Helper.InvIgnCase));
                Assert.Equal(1, n);
            }
        }
    }
}