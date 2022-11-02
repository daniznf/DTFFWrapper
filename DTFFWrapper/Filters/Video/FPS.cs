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
    public class FPS : IFilterSingle
    {
        public string FFName => "fps";
        public string? Name { get; }

        public Pin InputPin { get; set; }
        public Pin OutputPin { get; set; }

        public double Fps { get; set; }
        public static readonly double Source = 0;
        public static readonly double NTSC = 1.0 * 30000/1001;
        public static readonly double PAL = 25;
        public static readonly double Film = 24;
        public static readonly double NTSCFilm = 1.0 * 24000/1001;
        
        public FPS(double? fps, string? filterName = null)
        {
            Name = filterName ?? "FPS";
            InputPin = new($"{Name}_in");
            OutputPin = new($"{Name}_out");

            if (fps != null && fps.HasValue && fps > 0)
            {
                Fps = fps.Value;
            }
            else
            { 
                Fps = 0;  
            }
        }

        public string FFString()
        {
            // [in] fps=fps=25 [out]
            return $"{InputPin.FFString()} {FFName}=fps={Fps} {OutputPin.FFString()}";
        }
    }
}
