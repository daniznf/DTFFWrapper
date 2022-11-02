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

using DTFFWrapper.Encoders.Audio;
using DTFFWrapper.Encoders.Video;
using DTFFWrapper.Filters;

namespace DTFFWrapper
{
    public struct OutMap
    {
        public OutMap(Pin outPin, IEncoder outEncoder, string outPath, string? additionalParams = null)
        { 
            OutPin = outPin;
            OutEncoder = outEncoder;
            OutPath = outPath;
            AdditionalParams = additionalParams;
        }
                
        public Pin OutPin { get; set; }
        public IEncoder OutEncoder { get; set; }
        public string OutPath { get; set; }
        public string? AdditionalParams { get; set; }
    }
    
    /// <summary>
    /// Maps output pins to output paths with encoder strings. 
    /// Every OutMap contains the associations between them.
    /// </summary>
    public class Mapper : List<OutMap>, IFFBase
    {
        public string FFName => "map";
        public string? Name => "Mapper";

        public string FFString()
        {
            string toReturn = String.Empty;

            OutMap iMap, jMap;
            List<int> excluded = new();
            for (int i = 0; i < this.Count; i++)
            {
                if (excluded.Contains(i)) { continue; }

                toReturn += i > 0 ? " " : String.Empty;
                iMap = this[i];
                toReturn += $"-{FFName}";
                toReturn += $" \"{iMap.OutPin.FFString()}\"";
                toReturn += iMap.OutEncoder is IVideoEncoder ? " -c:v" : " -c:a";
                toReturn += $" {iMap.OutEncoder.FFString()}";
                toReturn += iMap.AdditionalParams != null && iMap.AdditionalParams.Length > 0 ? 
                    $" {iMap.AdditionalParams}" : String.Empty;
                
                // Tie up all OutMaps with the same OutPath
                for (int j = i + 1; j < this.Count; j++)
                {
                    jMap = this[j];
                    if (iMap.OutPath == jMap.OutPath)
                    {
                        toReturn += j > 0 ? " " : String.Empty;
                        excluded.Add(j);
                        toReturn += $"-{FFName}";
                        toReturn += $" \"{jMap.OutPin.FFString()}\"";
                        toReturn += jMap.OutEncoder is IVideoEncoder ? " -c:v" : " -c:a";
                        toReturn += $" {jMap.OutEncoder.FFString()}";
                        toReturn += jMap.AdditionalParams != null && jMap.AdditionalParams.Length > 0 ? 
                            $" {jMap.AdditionalParams}" : String.Empty;
                    }
                }

                toReturn += $" \"{ iMap.OutPath }\"";
            }
            
            return toReturn;
        }
    }
}
