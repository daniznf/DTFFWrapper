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
    public enum StrictLevel { very, strict, normal, unofficial, experimental }

    [Flags]
    public enum GenericFlags
    {
        none = 0,
        mv4 = 1,
        qpel = 2,
        loop = 4,
        qscale = 8,
        pass1 = 16,
        pass2 = 32,
        gray = 64,
        psnr = 128,
        truncated = 256,
        drop_changed = 512,
        ildct = 1024,
        low_delay = 2048,
        global_header = 4096,
        bitexact = 8192,
        aic = 16384,
        ilme = 32768,
        cgop = 65536,
        output_corrupt = 131072,
        OpenGOP = 67108864,
    }

    public interface IEncoder : IFFBase
    {
        /// <summary>
        /// Automatic extension for this encoder.
        /// </summary>
        string? FileExtension { get; protected set; }

        int? Bitrate { get; set; }
        string BitrateFFString() =>
            Bitrate != null && Bitrate.HasValue ? $" -b {Bitrate.Value}" : String.Empty;

        StrictLevel? StrictLevel { get; set; }
        string StandardStrictLevelFFString() =>
            StrictLevel != null && StrictLevel.HasValue ? $" -strict {StrictLevel.Value}" : String.Empty;

        int? MaxRate { get; set; }
        string MaxRateFFString() =>
            MaxRate != null && MaxRate.HasValue ? $" -maxrate {MaxRate.Value}" : String.Empty;

        int? MinRate { get; set; }
        string MinRateFFString() =>
            MinRate != null && MinRate.HasValue ? $" -minrate {MinRate.Value}" : String.Empty;

        int? BufSize { get; set; }
        string BufSizeFFString() =>
            BufSize != null && BufSize.HasValue ? $" -bufsize {BufSize.Value}" : String.Empty;

        int? Trellis { get; set; }
        string TrellisFFString() =>
            Trellis != null && Trellis.HasValue ? $" -trellis {Trellis.Value}" : String.Empty;

        GenericFlags GenericFlags { get; set; }
        string FlagFFString()
        {
            string toReturn = String.Empty;
            if (GenericFlags!= GenericFlags.none)
            {
                toReturn = " -flags ";

                GenericFlags[] allFlags = Enum.GetValues<GenericFlags>();
                for (int i = 1; i < allFlags.Length; i++)
                {
                    GenericFlags iFlag = allFlags[i];
                    if (GenericFlags.HasFlag(iFlag))
                    {
                        if (iFlag == GenericFlags.OpenGOP) { toReturn += $"-{GenericFlags.cgop}"; }
                        else { toReturn += $"+{iFlag}"; }
                    }
                }
            }
            return toReturn;
        }

        string FFStringEncoder()
        {
            string toRetun = String.Empty;
            toRetun += BitrateFFString();
            toRetun += StandardStrictLevelFFString();
            toRetun += MaxRateFFString();
            toRetun += MinRateFFString();
            toRetun += BufSizeFFString();
            toRetun += TrellisFFString();
            toRetun += FlagFFString();

            return toRetun;
        }
    }
}
