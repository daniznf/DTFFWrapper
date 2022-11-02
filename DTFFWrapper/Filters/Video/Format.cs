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

namespace DTFFWrapper.Filters.Video
{
    public class Format : IFilterSingle
    {
        public string FFName => "format";
        public string? Name { get; }

        public Pin InputPin { get; set; }
        public Pin OutputPin { get; set; }

        public List<PixelFormat> RequestedPixelFormats { get; }
        
        public Format(string? filterName = null)
        {
            RequestedPixelFormats = new();

            Name = filterName ?? "PixelFormat";
            InputPin = new($"{Name}_in");
            OutputPin = new($"{Name}_out");
        }

        public string FFString()
        {
            //format=pix_fmts=yuv420p|yuv444p|yuv410p
            string toReturn = string.Empty;

            if (RequestedPixelFormats.Count > 0)
            {
                toReturn = $"{FFName}=pix_fmts=";

                for (int i = 0; i < RequestedPixelFormats.Count; i++)
                {
                    toReturn += $"{RequestedPixelFormats[i].Name}|";
                }

                if (toReturn.EndsWith('|')) { toReturn = toReturn.Remove(toReturn.Length - 1); }
            }

            return $"{InputPin.FFString()} {toReturn} {OutputPin.FFString()}";
        }
    }
}
