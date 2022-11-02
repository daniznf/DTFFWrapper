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
    /// <summary>
    /// Output dimension.
    /// If width == 0, input width is used.
    /// If height == 0, input height is used.
    /// If width OR height is -n (n != 0), scale filter will maintain aspect ratio of input image
    /// </summary>
    public class Scale : IFilterSingle
    {
        public string FFName => "scale";
        public string? Name { get; }

        public Pin InputPin { get; set; }
        public Pin OutputPin { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int? ForceDivisibleBy { get; set; }

        public Scale(int? width, int? height, int? forceDivisibleBy = null, string? filterName = null)
        {
            Name = filterName ?? "Scale";
            InputPin = new($"{Name}_in");
            OutputPin = new($"{Name}_out");

            Width = width ?? 0;
            Height = height ?? 0;
            ForceDivisibleBy = forceDivisibleBy;
        }

        public string FFString()
        {
            // [in] scale=w=200:h=100 [out]
            string toReturn = $"{FFName}=";

            toReturn += $"w={Width}:h={Height}";
            
            if (ForceDivisibleBy != null && ForceDivisibleBy.HasValue)
            {
                toReturn += ":";
                toReturn += $"force_divisible_by={ForceDivisibleBy.Value}";
            }

            return $"{InputPin.FFString()} {toReturn} {OutputPin.FFString()}";
        }
    }
}
