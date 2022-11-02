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

namespace DTFFWrapper.Filters.Audio
{
    public class AudioFormat : IFilterSingle
    {
        public string FFName => "aformat";
        public string? Name { get; }

        public Pin InputPin { get; set; }
        public Pin OutputPin { get; set; }

        public List<SampleFormat> RequestedSampleFormats { get; }
        public List<int> RequestedSampleRates { get; }
        public List<ChannelLayout> RequestedChannelLayouts { get; }

        public AudioFormat(string? filterName = null)
        {
            RequestedSampleFormats = new();
            RequestedSampleRates = new();
            RequestedChannelLayouts = new();

            Name = filterName ?? "AudioFormat";
            InputPin = new($"{Name}_in");
            OutputPin = new($"{Name}_out");
        }

        public string FFString()
        {
            // aformat=sample_fmts=u8|s16:channel_layouts=stereo
            string toReturn = String.Empty;
            
            if (RequestedSampleFormats.Count > 0)
            {                
                toReturn = $"{FFName}=sample_fmts=";

                for (int i = 0; i < RequestedSampleFormats.Count; i++)
                {
                    toReturn += $"{RequestedSampleFormats[i].Name}|";
                }

                if (toReturn.EndsWith('|')) { toReturn = toReturn.Remove(toReturn.Length - 1); }

                if (RequestedSampleRates.Count > 0)
                {
                    toReturn += ":sample_rates=";
                    for (int i = 0; i < RequestedSampleRates.Count; i++)
                    {
                        toReturn += $"{RequestedSampleRates[i]}|";
                    }
                }
                if (toReturn.EndsWith('|')) { toReturn = toReturn.Remove(toReturn.Length - 1); }

                if (RequestedChannelLayouts.Count > 0)
                {
                    toReturn += ":channel_layouts=";
                    for (int i = 0; i < RequestedChannelLayouts.Count; i++)
                    {
                        toReturn += $"{ChannelLayoutHelper.GetChannelLayoutString(RequestedChannelLayouts[i])}|";
                    }
                }
                if (toReturn.EndsWith('|')) { toReturn = toReturn.Remove(toReturn.Length - 1); }
            }
            return $"{InputPin.FFString()} {toReturn} {OutputPin.FFString()}";
        }
    }
}
