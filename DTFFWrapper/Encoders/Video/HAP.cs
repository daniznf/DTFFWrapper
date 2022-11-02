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

namespace DTFFWrapper.Encoders.Video
{
    public class HAP : IVideoEncoder
    {
        public HAP(string? encoderName = null)
        {
            Name = encoderName ?? "HAP";
            FileExtension = "mov";
            GenericFlags = GenericFlags.none;
        }

        public enum HAPFormat { hap, hap_alpha, hap_q }
        
        public enum HAPCompressor { none, snappy }

        public int? GOP { get; set; }

        public int? BFrames { get; set; }

        public double? AspectRatio { get; set; }

        public int? KeyIntMin { get; set; }

        public ColorSpace? ColorSpace { get; set; }

        public ColorRange? ColorRange { get; set; }

        public int? BitrateTolerance { get; set; }

        public int? Threads { get; set; }

        public FieldOrder? FieldOrder { get; set; }

        public int? Bitrate { get; set; }

        public StrictLevel? StrictLevel { get; set; }

        public int? MaxRate { get; set; }

        public int? MinRate { get; set; }

        public int? BufSize { get; set; }

        public int? Trellis { get; set; }

        public GenericFlags GenericFlags { get; set; }

        public string FFName => "hap";
        public string? Name { get; }

        private string? fileExtension;
        public string? FileExtension
        {
            get => fileExtension;
            set => fileExtension = value?.Replace(".", String.Empty);
        }

        public HAPFormat? Format { get; set; }
        private string FormatFFString() =>
            Format != null && Format.HasValue ? $" -format {Format.Value}" : String.Empty;

        public HAPCompressor? Compressor { get; set; }
        private string CompressorFFString(HAPCompressor? compressor) =>
            compressor != null && compressor.HasValue ? $" -compressor {compressor.Value}" : String.Empty;

        public int? Chunks { get; set; }
        private string ChunksFFString(int? chunks) =>
            chunks != null && chunks.HasValue ? $" -chunks {chunks.Value}" : String.Empty;

        public string FFString()
        {
            string toReturn = $"{FFName}";
            
            toReturn += (this as IVideoEncoder).FFStringVideoEncoder();
            toReturn += FormatFFString();
            toReturn += CompressorFFString(Compressor);
            toReturn += ChunksFFString(Chunks);

            return toReturn;
        }
    }
}
