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
    public interface IFilterMulti : IFilter
    {
        public List<Pin> InputPins { get; set; }
        public List<Pin> OutputPins { get; set; }

        /// <summary>
        /// Connects the Pin with <paramref name="outputPinName"/> of this filter to the InputPin of 
        /// <paramref name="destinationFilter"/>.
        /// After connecting, every modification made to one pin will be reflected into the other.
        /// </summary>
        public void ConnectTo(string outputPinName, IFilterSingle destinationFilter)
        {
            bool found = false;
            for (int i = 0; i < OutputPins.Count; i++)
            {
                if (OutputPins[i].Name == outputPinName)
                {
                    destinationFilter.InputPin = OutputPins[i];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new Exception($"Filter {Name} does not contain {outputPinName} pin.");
            }
        }

        /// <summary>
        /// Connects the Pin with <paramref name="outputPinName"/> of this filter to the Pin with 
        /// <paramref name="destinationPinName"/> of <paramref name="destinationFilter"/>.
        /// </summary>
        public void ConnectTo(string outputPinName, IFilterMulti destinationFilter, string destinationPinName)
        {
            bool foundIn = false;
            bool foundOut = false;
            for (int i = 0; i < destinationFilter.InputPins.Count; i++)
            {
                if (destinationFilter.InputPins[i].Name.Equals(destinationPinName, StringComparison.InvariantCultureIgnoreCase))
                {
                    foundIn = true;
                    for (int j = 0; j < OutputPins.Count; j++)
                    {
                        if (OutputPins[j].Name.Equals(outputPinName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            destinationFilter.InputPins[i] = OutputPins[j];
                            foundOut = true;
                            break;
                        }
                    }
                    if (foundOut)
                    {
                        break;
                    }
                    else
                    {
                        throw new Exception($"Filter {Name} does not contain {outputPinName} pin.");
                    }
                }
            }

            if (!foundIn)
            {
                throw new Exception($"Filter {destinationFilter.Name} does not contain {destinationPinName} pin.");
            }
        }

        /// <summary>
        /// Connects the Pin with <paramref name="outputPinName"/> of this filter to the InputPin of 
        /// <paramref name="destinationFilterChain"/>.
        /// </summary>
        public void ConnectTo(string outputPinName, FilterChain destinationFilterChain)
        {
            bool found = false;
            for (int i = 0; i < OutputPins.Count; i++)
            {
                if (OutputPins[i].Name == outputPinName)
                {
                    destinationFilterChain.InputPin = OutputPins[i];
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                throw new Exception($"Filter {Name} does not contain {outputPinName} pin.");
            }
        }
    }
}
