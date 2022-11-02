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
    public class NullVideo : IFilterSingle
    {
        public string FFName => "null";
        public string? Name { get; }

        public Pin InputPin { get; set; }
        public Pin OutputPin { get; set; }

        public NullVideo(string? filterName = null)
        {
            Name = filterName ?? "NullVideo";
            InputPin = new($"{Name}_in");
            OutputPin = new($"{Name}_out");
        }

        public string FFString()
        {
            return $"{InputPin.FFString()} {FFName} {OutputPin.FFString()}";
        }
    }
}
