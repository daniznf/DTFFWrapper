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
    public class NoneAudio : IAudioEncoder
    {
        public NoneAudio(string? encoderName = null)
        {
            Name = encoderName ?? "None";
            FileExtension = null;
            GenericFlags = GenericFlags.none;
        }

        public int? AudioBitrate 
        {
            get => null;
            set {; }
        }

        public int? AudioSamplingRate
        {
            get => null;
            set {; }
        }

        public int? AudioChannelsNumber
        {
            get => null;
            set {; }
        }

        public ChannelLayout? ChannelLayout
        {
            get => null;
            set {; }
        }

        public int? Bitrate
        {
            get => null;
            set {; }
        }

        public StrictLevel? StrictLevel
        {
            get => null;
            set {; }
        }

        public int? MaxRate
        {
            get => null;
            set {; }
        }

        public int? MinRate
        {
            get => null;
            set {; }
        }

        public int? BufSize
        {
            get => null;
            set {; }
        }

        public int? Trellis
        {
            get => null;
            set {; }
        }

        public GenericFlags GenericFlags { get; set; }

        public string FFName => String.Empty;
        public string? Name { get; }

        public string? FileExtension { get; set; }

        public string FFString()
        {
            return "-an";
        }
    }
}
