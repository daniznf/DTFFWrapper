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
    public class WAV : IAudioEncoder
    {
        public WAV(string? encoderName = null)
        {
            Name = encoderName ?? "WAV";
            FileExtension = "wav";
            Format = WAVFormat.u16le;
            GenericFlags = GenericFlags.none;
        }

        public enum WAVFormat
        {
            f32be, f32le, f64be, f64le, s16be, s16le, s24be, s24le, s32be, 
            s32le, s8, u16be, u16le, u24be, u24le, u32be, u32le, u8
        }

        public int? AudioBitrate { get; set; }

        public int? AudioSamplingRate { get; set; }

        public int? AudioChannelsNumber { get; set; }

        public ChannelLayout? ChannelLayout { get; set; }

        public int? Bitrate { get; set; }

        public StrictLevel? StrictLevel { get; set; }

        public int? MaxRate { get; set; }

        public int? MinRate { get; set; }

        public int? BufSize { get; set; }

        public int? Trellis { get; set; }

        public GenericFlags GenericFlags { get; set; }

        public WAVFormat Format { get; set; }

        public string FFName => Format.ToString();
        public string? Name { get; }

        public string? FileExtension { get; set; }

        public string FFString()
        {
            string toReturn = $"{FFName}";

            toReturn += (this as IAudioEncoder).FFStringAudioEncoder();

            return toReturn;
        }
    }
}
