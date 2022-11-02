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

namespace DTFFWrapper.Filters.Audio
{
    public enum ChannelLayout
    {
        ChMono, ChStereo, Ch2_1, Ch3_0, Ch3_0Back, Ch4_0, ChQuad, ChQuadSide, Ch3_1, Ch5_0,
        Ch5_0Side, Ch4_1, Ch5_1, Ch5_1Side, Ch6_0, Ch6_0Front, ChHexagonal, Ch6_1,
        Ch6_1Back, Ch6_1Front, Ch7_0, Ch7_0Front, Ch7_1, Ch7_1Wide, Ch7_1WideSide,
        ChOctagonal, ChHexadecagonal, ChDownmix, Ch22_2
    }

    public static class ChannelLayoutHelper
    {
        public static readonly string ChMono = "mono";
        public static readonly string ChStereo = "stereo";
        public static readonly string Ch2_1 = "2.1";
        public static readonly string Ch3_0 = "3.0";
        public static readonly string Ch3_0Back = "3.0(back)";
        public static readonly string Ch4_0 = "4.0";
        public static readonly string ChQuad = "quad";
        public static readonly string ChQuadSide = "quad(side)";
        public static readonly string Ch3_1 = "3.1";
        public static readonly string Ch5_0 = "5.0";
        public static readonly string Ch5_0Side = "5.0(side)";
        public static readonly string Ch4_1 = "4.1";
        public static readonly string Ch5_1 = "5.1";
        public static readonly string Ch5_1Side = "5.1(side)";
        public static readonly string Ch6_0 = "6.0";
        public static readonly string Ch6_0Front = "6.0(front)";
        public static readonly string ChHexagonal = "hexagonal";
        public static readonly string Ch6_1 = "6.1";
        public static readonly string Ch6_1Back = "6.1";
        public static readonly string Ch6_1Front = "6.1(front)";
        public static readonly string Ch7_0 = "7.0";
        public static readonly string Ch7_0Front = "7.0(front)";
        public static readonly string Ch7_1 = "7.1";
        public static readonly string Ch7_1Wide = "7.1(wide)";
        public static readonly string Ch7_1WideSide = "7.1(wide-side)";
        public static readonly string ChOctagonal = "octagonal";
        public static readonly string ChHexadecagonal = "hexadecagonal";
        public static readonly string ChDownmix = "downmix";
        public static readonly string Ch22_2 = "22.2";

        public static string GetChannelLayoutString(ChannelLayout layout)
        {
            return layout switch
            {
                ChannelLayout.ChMono => ChMono,
                ChannelLayout.ChStereo => ChStereo,
                ChannelLayout.Ch2_1 => Ch2_1,
                ChannelLayout.Ch3_0 => Ch3_0,
                ChannelLayout.Ch3_0Back => Ch3_0Back,
                ChannelLayout.Ch4_0 => Ch4_0,
                ChannelLayout.ChQuad => ChQuad,
                ChannelLayout.ChQuadSide => ChQuadSide,
                ChannelLayout.Ch3_1 => Ch3_1,
                ChannelLayout.Ch5_0 => Ch5_0,
                ChannelLayout.Ch5_0Side => Ch5_0Side,
                ChannelLayout.Ch4_1 => Ch4_1,
                ChannelLayout.Ch5_1 => Ch5_1,
                ChannelLayout.Ch5_1Side => Ch5_1Side,
                ChannelLayout.Ch6_0 => Ch6_0,
                ChannelLayout.Ch6_0Front => Ch6_0Front,
                ChannelLayout.ChHexagonal => ChHexagonal,
                ChannelLayout.Ch6_1 => Ch6_1,
                ChannelLayout.Ch6_1Back => Ch6_1Back,
                ChannelLayout.Ch6_1Front => Ch6_1Front,
                ChannelLayout.Ch7_0 => Ch7_0,
                ChannelLayout.Ch7_0Front => Ch7_0Front,
                ChannelLayout.Ch7_1 => Ch7_1,
                ChannelLayout.Ch7_1Wide => Ch7_1Wide,
                ChannelLayout.Ch7_1WideSide => Ch7_1WideSide,
                ChannelLayout.ChOctagonal => ChOctagonal,
                ChannelLayout.ChHexadecagonal => ChHexadecagonal,
                ChannelLayout.ChDownmix => ChDownmix,
                ChannelLayout.Ch22_2 => Ch22_2,
                _ => String.Empty,
            };
        }

        public static readonly string[] LayoutMono = { "FC" };
        public static readonly string[] LayoutStereo = { "FL", "FR" };
        public static readonly string[] Layout2_1 = { "FL", "FR", "LFE" };
        public static readonly string[] Layout3_0 = { "FL", "FR", "FC" };
        public static readonly string[] Layout3_0Back = { "FL", "FR", "BC" };
        public static readonly string[] Layout4_0 = { "FL", "FR", "FC", "BC" };
        public static readonly string[] LayoutQuad = { "FL", "FR", "BL", "BR" };
        public static readonly string[] LayoutQuadSide = { "FL", "FR", "SL", "SR" };
        public static readonly string[] Layout3_1 = { "FL", "FR", "FC", "LFE" };
        public static readonly string[] Layout5_0 = { "FL", "FR", "FC", "BL", "BR" };
        public static readonly string[] Layout5_0Side = { "FL", "FR", "FC", "SL", "SR" };
        public static readonly string[] Layout4_1 = { "FL", "FR", "FC", "LFE", "BC" };
        public static readonly string[] Layout5_1 = { "FL", "FR", "FC", "LFE", "BL", "BR" };
        public static readonly string[] Layout5_1Side = { "FL", "FR", "FC", "LFE", "SL", "SR" };
        public static readonly string[] Layout6_0 = { "FL", "FR", "FC", "BC", "SL", "SR" };
        public static readonly string[] Layout6_0Front = { "FL", "FR", "FLC", "FRC", "SL", "SR" };
        public static readonly string[] LayoutHexagonal = { "FL", "FR", "FC", "BL", "BR", "BC" };
        public static readonly string[] Layout6_1 = { "FL", "FR", "FC", "LFE", "BC", "SL", "SR" };
        public static readonly string[] Layout6_1Back = { "FL", "FR", "FC", "LFE", "BL", "BR", "BC" };
        public static readonly string[] Layout6_1Front = { "FL", "FR", "LFE", "FLC", "FRC", "SL", "SR" };
        public static readonly string[] Layout7_0 = { "FL", "FR", "FC", "BL", "BR", "SL", "SR" };
        public static readonly string[] Layout7_0Front = { "FL", "FR", "FC", "FLC", "FRC", "SL", "SR" };
        public static readonly string[] Layout7_1 = { "FL", "FR", "FC", "LFE", "BL", "BR", "SL", "SR" };
        public static readonly string[] Layout7_1Wide = { "FL", "FR", "FC", "LFE", "BL", "BR", "FLC", "FRC" };
        public static readonly string[] Layout7_1WideSide = { "FL", "FR", "FC", "LFE", "FLC", "FRC", "SL", "SR" };
        public static readonly string[] LayoutOctagonal = { "FL", "FR", "FC", "BL", "BR", "BC", "SL", "SR" };
        public static readonly string[] LayoutHexadecagonal = { "FL", "FR", "FC", "BL", "BR", "BC", "SL", "SR", "TFL", "TFC", "TFR", "TBL", "TBC", "TBR", "WL", "WR" };
        public static readonly string[] LayoutDownmix = { "DL", "DR" };
        public static readonly string[] Layout22_2 = { "FL", "FR", "FC", "LFE", "BL", "BR", "FLC", "FRC", "BC", "SL", "SR", "TC", "TFL", "TFC", "TFR", "TBL", "TBC", "TBR", "LFE2", "TSL", "TSR", "BFC", "BFL", "BFR" };
    }
}
