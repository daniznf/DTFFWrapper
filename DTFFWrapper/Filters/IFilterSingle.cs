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
    public interface IFilterSingle : IFilter
    {
        public Pin InputPin { get; set; }
        public Pin OutputPin { get; set; }

        /// <summary>
        /// Connects <paramref name="OutputPin"/> of this filter to the InputPin of <paramref name="destinationFilter"/>.
        /// After connecting, every modification made to one pin will be reflected into the other.
        /// </summary>
        public void ConnectTo(IFilterSingle destinationFilter)
        {
            destinationFilter.InputPin = OutputPin;
        }

        /// <summary>
        /// Connects <paramref name="OutputPin"/> of this filter to the input Pin with <paramref name="destinationPinName"/> 
        /// of <paramref name="destinationFilter"/>.
        /// </summary>
        public void ConnectTo(IFilterMulti destinationFilter, string destinationPinName)
        {
            bool found = false;
            for (int i = 0; i < destinationFilter.InputPins.Count; i++)
            {
                if (destinationFilter.InputPins[i].Name.Equals(destinationPinName, StringComparison.InvariantCultureIgnoreCase))
                {
                    destinationFilter.InputPins[i] = OutputPin;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new Exception($"Filter {destinationFilter.Name} does not contain {destinationPinName} pin.");
            }
        }

        /// <summary>
        /// Connects <paramref name="OutputPin"/> of this filter to the InputPin of <paramref name="destinationFilterChain"/>.
        /// </summary>
        public void ConnectTo(FilterChain destinationFilterChain)
        {
            destinationFilterChain.InputPin = OutputPin;
        }
    }
}