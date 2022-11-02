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

using DTFFWrapper.Filters.Audio;

namespace DTFFWrapper.Encoders.Audio
{
    public interface IAudioEncoder : IEncoder
    {
        int? AudioBitrate { get; set; }
        string AudioBitrateFFString() =>
            AudioBitrate != null && AudioBitrate.HasValue ? $" -ab {AudioBitrate.Value}" : String.Empty;

        int? AudioSamplingRate { get; set; }
        string AudioSamplingRateFFString() =>
            AudioSamplingRate != null && AudioSamplingRate.HasValue ? $" -ar {AudioSamplingRate.Value}" : String.Empty;

        int? AudioChannelsNumber { get; set; }
        string AudioChannelsNumberFFString() =>
            AudioChannelsNumber != null && AudioChannelsNumber.HasValue ? $" -ac {AudioChannelsNumber.Value}" : String.Empty;

        ChannelLayout? ChannelLayout { get; set; }
        string ChannelLayoutFFString() =>
            ChannelLayout != null && ChannelLayout.HasValue ?
            $" -channel_layout {ChannelLayoutHelper.GetChannelLayoutString(ChannelLayout.Value)}" : String.Empty;

        string FFStringAudioEncoder()
        {
            string toRetun = FFStringEncoder();
            toRetun += AudioBitrateFFString();
            toRetun += AudioSamplingRateFFString();
            toRetun += AudioChannelsNumberFFString();
            toRetun += ChannelLayoutFFString();

            return toRetun;
        }
    }
}
