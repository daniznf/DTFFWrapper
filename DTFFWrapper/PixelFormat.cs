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
    [Flags]
    public enum PixelFormats
    {
        None = 0,
        I = 1,
        O = 2,
        H = 4,
        P = 8,
        B = 16
    }

    public struct PixelFormat
    {
        public PixelFormat(PixelFormats flags, string name, int nbComponents, int bitsPerPixel, int[] bitDepth)
        {
            Flags = flags;
            Name = name;
            NBComponents = nbComponents;
            BitsPerPixel = bitsPerPixel;

            if (bitDepth.Length < 1 || bitDepth.Length > 4)
            {
                throw new ArgumentException(nameof(BitDepth));
            }
            BitDepth = bitDepth;
        }

        public PixelFormats Flags { get; }
        public string Name { get; }
        public int NBComponents { get; }
        public int BitsPerPixel { get; }
        public int[] BitDepth { get; }
    }
}