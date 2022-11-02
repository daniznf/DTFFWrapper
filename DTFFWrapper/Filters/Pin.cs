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

namespace DTFFWrapper.Filters
{
    /// <summary>
    /// Represents an input or output pin of Filters and FilterChains in FilterGraph.
    /// </summary>
    public class Pin : IFFBase
    {
        public Pin(string pinName)
        {
            name = String.Empty;
            Name = pinName;    
        }

        private string name;
        public string Name 
        {
            get => name;
            set
            {
                name = IFFBase.CleanName(value);
            }
        }

        public string FFName => String.Empty;

        /// <summary>
        /// Connects this pin to <paramref name="toPin"/>.
        /// After connecting, every modification made to one pin will be reflected into the other.
        /// </summary>
        public void ConnectTo(ref Pin toPin)
        {
            toPin = this;
        }

        public string FFString()
        {
            return $"[{Name}]";
        }

        public Pin Clone()
        {
            return new Pin(Name);
        }
    }
}
