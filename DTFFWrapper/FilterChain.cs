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

using DTFFWrapper.Filters;

namespace DTFFWrapper
{
    /// <summary>
    /// Contains a chain of <paramref name="IFilterSingle"/>, and provides a method to automatically connect them.
    /// </summary>
    public class FilterChain : List<IFilterSingle>, IFFBase, IFilterSingle
    {
        public FilterChain(string? ChainName = null)
        {
            name = String.Empty;
            Name = ChainName ?? "chain";
            InputPin = new($"{Name}_in");
            OutputPin = new($"{Name}_out");
        }

        /// <summary>
        /// Input pin of this chain.
        /// This pin will be connected to first filter in the chain.
        /// </summary>
        public Pin InputPin { get; set; }

        /// <summary>
        /// Output pin of this chain.
        /// Last filter of the chain will be connected to this.
        /// </summary>
        public Pin OutputPin { get; set; }

        public string FFName => String.Empty;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = IFFBase.CleanName(value);
            }
        }

        /// <summary>
        /// Connects all <paramref name="IFilterSingle"/> objects of this filter chain.
        /// </summary>
        /// <returns></returns>
        private void ConnectInternalFilters()
        {
            if (Count == 0) { throw new Exception($"Cannot connect filters: chain {Name} is empty"); }

            this[0].InputPin = InputPin;
            
            for (int i = 1; i < Count; i++)
            {
                this[i - 1].ConnectTo(this[i]);
                // Equivalent of:
                // this[i].InputPin = this[i - 1].OutputPin;
            }

            this[Count - 1].OutputPin = OutputPin;
        }

        /// <summary>
        /// Returns a string containing all FFStrings of all filters of this chain.
        /// Filters are automatically connected by this method, but not uniquely renamed.
        /// </summary>
        /// <returns></returns>
        public string FFString()
        {
            ConnectInternalFilters();
            string toReturn = String.Empty;
                
            for (int i = 0; i < Count; i++)
            {
                if (i > 0)
                {
                    toReturn += " ";
                }
                toReturn += $"{this[i].FFString()}";
                if (i < Count - 1) 
                { 
                    toReturn += ";"; 
                }
            }
            
            return toReturn;
        }
    }
}
