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

namespace DTFFWrapper.Encoders.Video
{
    public enum ColorRange { tv, mpeg, pc, jpeg }

    public enum ColorSpace
    {
        rgb, bt709, fcc, bt470bg, smpte170m, smpte240m, ycocg, bt2020nc,
        bt2020_ncl, bt2020c, bt2020_cl, smpte2085, ictcp
    }

    public enum ColorPrimaries
    {
        bt709, bt470m, bt470bg, smpte170m, smpte240m, film,
        bt2020, smpte428, smpte428_1, smpte431, smpte432, jedec_p22
    }

    public enum ColorTransferCharacteristics
    {
        bt709, gamma22, gamma28, smpte170m, smpte240m, linear, log, log100, log_sqrt, 
        log316, iec61966_2_4, bt1361, bt1361e, iec61966_2_1, bt2020_10, bt2020_10bit, 
        bt2020_12, bt2020_12bit, smpte2084, smpte428, smpte428_1, arib_std_b67
    }

    public enum ChromaSampleLocation { left, center, topleft, top, bottomleft, bottom }

    public enum FieldOrder { progressive, tt, bb, tb, bt }

    public interface IVideoEncoder : IEncoder
    {
        int? GOP { get; set; }
        string GopFFString() =>
            GOP != null && GOP.HasValue ? $" -g {GOP.Value}" : String.Empty;
        
        int? BFrames { get; set; }

        string BFramesFFString() =>
            BFrames != null && BFrames.HasValue ? $" -bf {BFrames.Value}" : String.Empty;

        double? AspectRatio { get; set; }
        string AspectRatioFFString() =>
            AspectRatio != null && AspectRatio.HasValue ? $" -aspect {Math.Round(AspectRatio.Value,6)}" : String.Empty;

        int? KeyIntMin { get; set; }
        string KeyIntMinFFString() =>
            KeyIntMin != null && KeyIntMin.HasValue ? $" -keyint_min {KeyIntMin.Value}" : String.Empty;

        ColorSpace? ColorSpace { get; set; }
        string ColorSpaceFFString() =>
            ColorSpace != null && ColorSpace.HasValue ? $" -colorspace {ColorSpace.Value}" : String.Empty;

        ColorRange? ColorRange { get; set; }
        string ColorRangeFFString() => 
            ColorRange != null && ColorRange.HasValue ? $" -color_range {ColorRange.Value}" : String.Empty;

        int? BitrateTolerance { get; set; }
        string BitrateToleranceFFString() =>
            BitrateTolerance != null && BitrateTolerance.HasValue ? $" -bt {BitrateTolerance.Value}" : String.Empty;

        int? Threads { get; set; }
        string ThreadsFFString() =>
            Threads != null && Threads.HasValue ? $" -threads {Threads.Value}" : String.Empty;

        FieldOrder? FieldOrder { get; set; }
        string FieldOrderFFString() =>
            FieldOrder != null && FieldOrder.HasValue ? $" -field_order {FieldOrder.Value}" : String.Empty;

        public string FFStringVideoEncoder()
        {
            string toRetun = FFStringEncoder();
            toRetun += GopFFString();
            toRetun += BFramesFFString();
            toRetun += AspectRatioFFString();
            toRetun += KeyIntMinFFString();
            toRetun += ColorSpaceFFString();
            toRetun += ColorRangeFFString();
            toRetun += BitrateToleranceFFString();
            toRetun += ThreadsFFString();
            toRetun += FieldOrderFFString();

            return toRetun;
        }
    }
}
