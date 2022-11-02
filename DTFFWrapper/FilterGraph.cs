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
    /// FilterGraph manages and connects all filters and filterchains.
    /// <paramref name="IFilters"/> can be added one by one or in already prepared <paramref name="FilterChain"/> 
    /// objects.<br/> 
    /// If added one by one, each <paramref name="IFilter"/> must be connected manually to another 
    /// <paramref name="IFilter"/> or <paramref name="FilterChain"/>.<br/>
    /// If added in <paramref name="FilterChain"/>, filters inside each chain will be automatically connected.<br/>
    /// Connections between <paramref name="IFilter"/> and <paramref name="FilterChain"/> objects or between 
    /// <paramref name="FilterChain"/> objects or between <paramref name="IFilter"/> objects must be done manually.<br/>
    /// A Wrapper instance will contain only one FilterGraph instance.
    /// </summary>
    public class FilterGraph : IFFBase
    {
        public FilterGraph()
        {
            ChainList = new();
            FilterList = new();
            AllPins = new();
        }

        /// <summary>
        /// Contains all <paramref name="Pin"/> objects of the FilterGraph, after calling ReadAllPins().
        /// </summary>
        private readonly List<Pin> AllPins;

        /// <summary>
        /// List of <paramref name="FilterChain"/> objects.
        /// </summary>
        public List<FilterChain> ChainList { get; }

        /// <summary>
        /// List of <paramref name="IFilter"/> objects that are not inside any <paramref name="FilterChain"/>.
        /// Use these filters to connect several <paramref name="FilterChain"/> objects or several other 
        /// filters (split filters, etc).
        /// Connections must be done via ConnectTo method.
        /// </summary>
        public List<IFilter> FilterList { get; set; }

        public string FFName => "filter_complex";

        public string? Name => "FilterGraph";

        /// <summary>
        /// Produces a complete string for FilterGraph.
        /// All <paramref name="IFilterSingle"/> and <paramref name="FilterChain"/> objects must be already 
        /// connected before calling this method.
        /// All <paramref name="FilterChains"/> objects will auto connect their internal filters.
        /// Pin uniqueness is checked at the beginning of this method.
        /// </summary>
        public string FFString()
        {
            CheckPinUnique();

            string toReturn = $"-{FFName} \"";
            
            for (int i = 0; i < ChainList.Count; i++)
            {
                toReturn += $"{ChainList[i].FFString()}; ";
            }
            
            for (int i = 0; i < FilterList.Count; i++)
            {
                toReturn += $"{FilterList[i].FFString()}; ";
            }

            if (toReturn.EndsWith("; ")) { toReturn = $"{toReturn.Remove(toReturn.Length - 2)}"; }

            toReturn += "\"";

            return toReturn;
        }

        /// <summary>
        /// Reads all output pins in the FilterGraph and puts them in the AllPins list.
        /// </summary>
        private void ReadAllPins()
        {
            FilterChain iChain;
            // For all chains in ChainList.
            for (int i = 0; i < ChainList.Count; i++)
            {
                iChain = ChainList[i];
                // Add input and output pins of this list to AllPins.
                AllPins.Add(iChain.InputPin);
                AllPins.Add(iChain.OutputPin);

                // For all filters of this chain.
                for (int j = 0; j < iChain.Count; j++)
                {
                    ReadAllPins(iChain[j]);
                }
            }

            // For all filters in FilterList
            for (int i = 0; i < FilterList.Count; i++)
            {
                ReadAllPins(FilterList[i]);
            }
        }

        private void ReadAllPins(IFilter filter)
        {
            if (filter is IFilterMulti fMulti)
            {
                for (int i = 0; i < fMulti.InputPins.Count; i++)
                {
                    AllPins.Add(fMulti.InputPins[i]);
                }
                
                for (int i = 0; i < fMulti.OutputPins.Count; i++)
                {        
                    AllPins.Add(fMulti.OutputPins[i]);
                }
            }

            if (filter is IFilterSingle fSingle)
            {
                AllPins.Add(fSingle.InputPin);
                AllPins.Add(fSingle.OutputPin);
            }
        }

        /// <summary>
        /// Checks that each <paramref name="Pin"/>'s name is unique among all output pins in the FilterGraph.
        /// Duplicates will be renamed appending "_number".
        /// </summary>
        private void CheckPinUnique()
        {
            ReadAllPins();

            Pin toRename;
            // for all pins
            for (int i = 0; i < AllPins.Count; i++)
            {
                // for all remaining pins
                for (int j = i + 1; j < AllPins.Count; j++)
                {
                    toRename = AllPins[j];
                    
                    // if the name is equal, but pin I is not connected to pin J
                    if (AllPins[i].Name.Equals(toRename.Name, StringComparison.InvariantCultureIgnoreCase) && 
                        (AllPins[j] != AllPins[i]))
                    {
                        toRename.Name = Helper.IncrementNameNumber(toRename.Name);
                    }
                }
            }
        }
    }
}
