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
    public enum IndividualChannel
    {
        FL, FR, FC, LFE, BL, BR, FLC, FRC, BC, SL, SR, TC, TFL, TFC, TFR, TBL,
        TBC, TBR, DL, DR, WL, WR, SDL, SDR, LFE2, TSL, TSR, BFC, BFL, BFR
    }

    public static class IndividualChannelHelper
    {
        public static readonly string FL = "Front Left";
        public static readonly string FR = "Front Right";
        public static readonly string FC = "Front Center";
        public static readonly string LFE = "Low Frequency";
        public static readonly string BL = "Back Left";
        public static readonly string BR = "Back Right";
        public static readonly string FLC = "Front Left Of Center";
        public static readonly string FRC = "Front Right Of Center";
        public static readonly string BC = "Back Center";
        public static readonly string SL = "Side Left";
        public static readonly string SR = "Side Right";
        public static readonly string TC = "Top Center";
        public static readonly string TFL = "Top Front Left";
        public static readonly string TFC = "Top Front Center";
        public static readonly string TFR = "Top Front Right";
        public static readonly string TBL = "Top Back Left";
        public static readonly string TBC = "Top Back Center";
        public static readonly string TBR = "Top Back Right";
        public static readonly string DL = "Downmix Left";
        public static readonly string DR = "Downmix Right";
        public static readonly string WL = "Wide Left";
        public static readonly string WR = "Wide Right";
        public static readonly string SDL = "Surround Direct Left";
        public static readonly string SDR = "Surround Direct Right";
        public static readonly string LFE2 = "Low Frequency 2";
        public static readonly string TSL = "Top Side Left";
        public static readonly string TSR = "Top Side Right";
        public static readonly string BFC = "Bottom Front Center";
        public static readonly string BFL = "Bottom Front Left";
        public static readonly string BFR = "Bottom Front Right";

        public static readonly string FrontLeft = "FL";
        public static readonly string FrontRight = "FR";
        public static readonly string FrontCenter = "FC";
        public static readonly string LowFrequency = "LFE";
        public static readonly string BackLeft = "BL";
        public static readonly string BackRight = "BR";
        public static readonly string FrontLeftOfCenter = "FLC";
        public static readonly string FrontRightOfCenter = "FRC";
        public static readonly string BackCenter = "BC";
        public static readonly string SideLeft = "SL";
        public static readonly string SideRight = "SR";
        public static readonly string TopCenter = "TC";
        public static readonly string TopFrontLeft = "TFL";
        public static readonly string TopFrontCenter = "TFC";
        public static readonly string TopFrontRight = "TFR";
        public static readonly string TopBackLeft = "TBL";
        public static readonly string TopBackCenter = "TBC";
        public static readonly string TopBackRight = "TBR";
        public static readonly string DownmixLeft = "DL";
        public static readonly string DownmixRight = "DR";
        public static readonly string WideLeft = "WL";
        public static readonly string WideRight = "WR";
        public static readonly string SurroundDirectLeft = "SDL";
        public static readonly string SurroundDirectRight = "SDR";
        public static readonly string LowFrequency2 = "LFE2";
        public static readonly string TopSideLeft = "TSL";
        public static readonly string TopSideRight = "TSR";
        public static readonly string BottomFrontCenter = "BFC";
        public static readonly string BottomFrontLeft = "BFL";
        public static readonly string BottomFrontRight = "BFR";

        public static string GetIndividualChannelString(IndividualChannel channel)
        {
            return channel switch
            {
                IndividualChannel.FL => FL,
                IndividualChannel.FR => FR,
                IndividualChannel.FC => FC,
                IndividualChannel.LFE => LFE,
                IndividualChannel.BL => BL,
                IndividualChannel.BR => BR,
                IndividualChannel.FLC => FLC,
                IndividualChannel.FRC => FRC,
                IndividualChannel.BC => BC,
                IndividualChannel.SL => SL,
                IndividualChannel.SR => SR,
                IndividualChannel.TC => TC,
                IndividualChannel.TFL => TFL,
                IndividualChannel.TFC => TFC,
                IndividualChannel.TFR => TFR,
                IndividualChannel.TBL => TBL,
                IndividualChannel.TBC => TBC,
                IndividualChannel.TBR => TBR,
                IndividualChannel.DL => DL,
                IndividualChannel.DR => DR,
                IndividualChannel.WL => WL,
                IndividualChannel.WR => WR,
                IndividualChannel.SDL => SDL,
                IndividualChannel.SDR => SDR,
                IndividualChannel.LFE2 => LFE2,
                IndividualChannel.TSL => TSL,
                IndividualChannel.TSR => TSR,
                IndividualChannel.BFC => BFC,
                IndividualChannel.BFL => BFL,
                IndividualChannel.BFR => BFR,
                _ => ""
            };
        }
    }
}
