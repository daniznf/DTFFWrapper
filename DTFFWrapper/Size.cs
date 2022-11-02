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

namespace DTFFWrapper
{
    public class Size
    {
        public Size(int width, int height, int? divisibleBy = 1)
        {
            if (divisibleBy != null && divisibleBy.HasValue && divisibleBy.Value > 0)
            {
                DivisibleBy = divisibleBy.Value;
            }
            else
            {
                DivisibleBy = 1;
            }

            Width = RoundToMultiple(width);
            Height = RoundToMultiple(height);
        }

        public int Width { get; }
        public int Height { get; }

        public int RoundToMultiple(int number)
        {
            return Convert.ToInt32(Math.Round(1.0 * number / DivisibleBy) * DivisibleBy);
        }

        /// <summary>
        /// Ensures that <paramref name="Width"/> and <paramref name="Height"/> of this Size object are divisible by this number.<br/>
        /// <example>E.g.: <c>1025</c>, when divisible by is 8, will become <c>1024</c></example>
        /// Values of 0 or 1 will not modify Size's values.
        /// </summary>
        public int DivisibleBy { get; }
    }
}