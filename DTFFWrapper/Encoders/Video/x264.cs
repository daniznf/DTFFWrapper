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
    public class x264 : IVideoEncoder
    {
        public x264(string? encoderName = null)
        {
            Name = encoderName ?? "x264";
            FileExtension = "mp4";
            x264Params = new();
            GenericFlags = GenericFlags.none;
        }

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

        /// <summary>
        /// List of x264 parameters written in pairs of key and value.
        /// </summary>
        public List<KeyValuePair<string,string>> x264Params { get; }
        private string paramsFFString()
        {
            string toReturn = String.Empty;

            if (x264Params != null && x264Params.Count > 0)
            {
                toReturn = " -x264opts ";
                for (int i = 0; i < x264Params.Count; i++)
                {
                    if (i != 0)
                    {
                        toReturn += ":";
                    }
                    toReturn += $"{x264Params[i].Key}={x264Params[i].Value}";
                }
            }

            return toReturn;
        }

        public string FFName => "libx264";
        public string? Name { get; }

        private string? fileExtension;
        public string? FileExtension
        {
            get => fileExtension;
            set => fileExtension = value;
        }

        public string FFString()
        {
            string toReturn = $"{FFName}";

            toReturn += (this as IVideoEncoder).FFStringVideoEncoder();
            toReturn += paramsFFString();

            return toReturn;
        }
    }
}
