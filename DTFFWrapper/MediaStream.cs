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

using DTFFWrapper.Encoders.Video;
using DTFFWrapper.Filters.Audio;

namespace DTFFWrapper
{
    public class MediaStream : Dictionary<string, string>
    {
        public static readonly string STREAM = "[STREAM]";
        public static readonly string INDEX = "index";
        public static readonly string CODEC_NAME = "codec_name";
        public static readonly string CODEC_LONG_NAME = "codec_long_name";
        public static readonly string PROFILE = "profile";
        public static readonly string CODEC_TYPE = "codec_type";
        public static readonly string CODEC_TAG_STRING = "codec_tag_string";
        public static readonly string CODEC_TAG = "codec_tag";
        public static readonly string WIDTH = "width";
        public static readonly string HEIGHT = "height";
        public static readonly string CODEC_WIDTH = "coded_width";
        public static readonly string CODEC_HEIGHT = "coded_height";
        public static readonly string CLOSED_CAPTIONS = "closed_captions";
        public static readonly string FILM_GRAIN = "film_grain";
        public static readonly string HAS_B_FRAMES = "has_b_frames";
        public static readonly string SAMPLE_ASPECT_RATIO = "sample_aspect_ratio";
        public static readonly string DISPLAY_ASPECT_RATIO = "display_aspect_ratio";
        public static readonly string PIX_FMT = "pix_fmt";
        public static readonly string LEVEL = "level";
        public static readonly string COLOR_RANGE = "color_range";
        public static readonly string COLOR_SPACE = "color_space";
        public static readonly string COLOR_TRANSFER = "color_transfer";
        public static readonly string COLOR_PRIMARIES = "color_primaries";
        public static readonly string CHROMA_LOCATION = "chroma_location";
        public static readonly string FIELD_ORDER = "field_order";
        public static readonly string REFS = "refs";
        public static readonly string IS_AVC = "is_avc";
        public static readonly string NAL_LENGTH_SIZE = "nal_length_size";
        public static readonly string ID = "id";
        public static readonly string R_FRAME_RATE = "r_frame_rate";
        public static readonly string AVG_FRAME_RATE = "avg_frame_rate";
        public static readonly string TIME_BASE = "time_base";
        public static readonly string START_PTS = "start_pts";
        public static readonly string START_TIME = "start_time";
        public static readonly string DURATION_TS = "duration_ts";
        public static readonly string DURATION = "duration";
        public static readonly string BIT_RATE = "bit_rate";
        public static readonly string MAX_BIT_RATE = "max_bit_rate";
        public static readonly string BITS_PER_RAW_SAMPLE = "bits_per_raw_sample";
        public static readonly string NB_FRAMES = "nb_frames";
        public static readonly string NB_READ_FRAMES = "nb_read_frames";
        public static readonly string NB_READ_PACKETS = "nb_read_packets";
        public static readonly string EXTRADATA_SIZE = "extradata_size";
        public static readonly string SAMPLE_FMT = "sample_fmt";
        public static readonly string SAMPLE_RATE = "sample_rate";
        public static readonly string CHANNELS = "channels";
        public static readonly string CHANNEL_LAYOUT = "channel_layout";
        public static readonly string BITS_PER_SAMPLE = "bits_per_sample";

        public static double? ParseFraction(string fractionString, int decimals = 3)
        {
            char divider;
            if (fractionString.Contains('/')) { divider = '/'; }
            else if (fractionString.Contains(':')) { divider = ':'; }
            else { return null; }

            if (Int32.TryParse(fractionString.Split(divider)[0], out int n) &&
                Int32.TryParse(fractionString.Split(divider)[1], out int m))
            {
                return Math.Round(1.0 * n / m, decimals);
            }
            else
            {
                return null;
            }
        }

        public int StreamIndex => this.ContainsKey(INDEX) ?
            Int32.TryParse(this[INDEX], out int index) ? index : -2 : -3;

        public string CodecName => this.ContainsKey(CODEC_NAME) ?
            this[CODEC_NAME] : String.Empty;

        public string CodecLongName => this.ContainsKey(CODEC_LONG_NAME) ?
            this[CODEC_LONG_NAME] : String.Empty;

        public string Profile => this.ContainsKey(PROFILE) ?
            this[PROFILE] : String.Empty;

        public string CodecType => this.ContainsKey(CODEC_TYPE) ?
            this[CODEC_TYPE] : String.Empty;

        public string CodecTagString => this.ContainsKey(CODEC_TAG_STRING) ?
            this[CODEC_TAG_STRING] : String.Empty;

        public string CodecTag => this.ContainsKey(CODEC_TAG) ?
            this[CODEC_TAG] : String.Empty;

        public int? Width => this.ContainsKey(WIDTH) ?
            Int32.TryParse(this[WIDTH], out int width) ? width : null : null;

        public int? Height => this.ContainsKey(HEIGHT) ?
            Int32.TryParse(this[HEIGHT], out int height) ? height : null : null;

        public int? CodecWidth => this.ContainsKey(CODEC_WIDTH) ?
            Int32.TryParse(this[CODEC_WIDTH], out int codecWidth) ?
                codecWidth : null : null;

        public int? CodecHeight => this.ContainsKey(CODEC_HEIGHT) ?
            Int32.TryParse(this[CODEC_HEIGHT], out int codecHeight) ?
                codecHeight : null : null;

        public int? ClosedCaptions => this.ContainsKey(CLOSED_CAPTIONS) ?
            Int32.TryParse(this[CLOSED_CAPTIONS], out int closedCaptions) ?
                closedCaptions : null : null;

        public int? FilmGrain => this.ContainsKey(FILM_GRAIN) ?
            Int32.TryParse(this[FILM_GRAIN], out int filmGrain) ?
                filmGrain : null : null;

        public bool? HasBFrames => this.ContainsKey(HAS_B_FRAMES) ?
            this[HAS_B_FRAMES] == "1" ? true :
            this[HAS_B_FRAMES] == "0" ? false :
            null : null;

        public double? SAR => this.ContainsKey(SAMPLE_ASPECT_RATIO) ?
            ParseFraction(this[SAMPLE_ASPECT_RATIO]) : null;

        public double? DAR => this.ContainsKey(DISPLAY_ASPECT_RATIO) ?
            ParseFraction(this[DISPLAY_ASPECT_RATIO]) : null;

        public PixelFormats? PixelFormat => this.ContainsKey(PIX_FMT) ?
            Enum.TryParse<PixelFormats>(this[PIX_FMT], out PixelFormats pixelFormat) ? pixelFormat : null : null;

        public int? Level => this.ContainsKey(LEVEL) ?
            Int32.TryParse(this[LEVEL], out int level) ? level : null : null;

        public ColorRange? ColorRange => this.ContainsKey(COLOR_RANGE) ?
            Enum.TryParse<ColorRange>(this[COLOR_RANGE], true, out ColorRange colorRange) ?
                colorRange : null : null;

        public ColorSpace? ColorSpace => this.ContainsKey(COLOR_SPACE) ?
            Enum.TryParse<ColorSpace>(this[COLOR_SPACE], true, out ColorSpace colorSpace) ?
                colorSpace : null : null;

        public ColorTransferCharacteristics? ColorTransfer => this.ContainsKey(COLOR_TRANSFER) ?
            Enum.TryParse(this[COLOR_TRANSFER].Replace('-', '_'), true, out ColorTransferCharacteristics colorTCR) ?
                colorTCR : null : null;

        public ColorPrimaries? ColorPrimaries => this.ContainsKey(COLOR_PRIMARIES) ?
            Enum.TryParse(this[COLOR_PRIMARIES].Replace('-', '_'), true, out ColorPrimaries colorPrimaries) ?
                colorPrimaries : null : null;

        public ChromaSampleLocation? ChromaLocation => this.ContainsKey(CHROMA_LOCATION) ?
            Enum.TryParse(this[CHROMA_LOCATION], true, out ChromaSampleLocation chromaSampleLocation) ?
                chromaSampleLocation : null : null;

        public FieldOrder? FieldOrder => this.ContainsKey(FIELD_ORDER) ?
            Enum.TryParse(this[FIELD_ORDER], true, out FieldOrder fieldOrder) ? fieldOrder : null : null;

        public int? Refs => this.ContainsKey(REFS) ?
            Int32.TryParse(this[REFS], out int refs) ? refs : null : null;

        public bool? IsAVC => this.ContainsKey(IS_AVC) ?
            Boolean.TryParse(this[IS_AVC], out bool isAVC) ? isAVC : null : null;

        public int? NALLengthSize => this.ContainsKey(NAL_LENGTH_SIZE) ?
            Int32.TryParse(this[NAL_LENGTH_SIZE], out int nalLenghtSize) ? nalLenghtSize : null : null;

        public string Id => this.ContainsKey(ID) ? this[ID] : String.Empty;

        public double? RFrameRate => this.ContainsKey(R_FRAME_RATE) ?
            ParseFraction(this[R_FRAME_RATE]) : null;

        public double? AverageFrameRate => this.ContainsKey(AVG_FRAME_RATE) ?
            ParseFraction(this[AVG_FRAME_RATE]) : null;

        public double? TimeBase => this.ContainsKey(TIME_BASE) ?
            ParseFraction(this[TIME_BASE]) : null;

        public int? StartPts => this.ContainsKey(START_PTS) ?
            Int32.TryParse(this[START_PTS], out int startPts) ? startPts : null : null;

        public double? StartTime => this.ContainsKey(START_TIME) ?
            Double.TryParse(this[START_TIME], out double startTime) ? startTime : null : null;

        public double? DurationTimeStamp => this.ContainsKey(DURATION_TS) ?
            Double.TryParse(this[DURATION_TS], out double durationTimeStamp) ?
                durationTimeStamp : null : null;

        public double? Duration => this.ContainsKey(DURATION) ?
            Double.TryParse(this[DURATION], out double duration) ? duration : null : null;

        public int? BitRate => this.ContainsKey(BIT_RATE) ?
            Int32.TryParse(this[BIT_RATE], out int bitRate) ? bitRate : null : null;

        public int? MaxBitRate => this.ContainsKey(MAX_BIT_RATE) ?
            Int32.TryParse(this[MAX_BIT_RATE], out int maxBitRate) ? maxBitRate : null : null;

        public int? BitsPerRawSample => this.ContainsKey(BITS_PER_RAW_SAMPLE) ?
            Int32.TryParse(this[BITS_PER_RAW_SAMPLE], out int bitPerRawSample) ?
                bitPerRawSample : null : null;

        public int? NBFrames => this.ContainsKey(NB_FRAMES) ?
            Int32.TryParse(this[NB_FRAMES], out int nbFrames) ? nbFrames : null : null;

        public int? NBReadFrames => this.ContainsKey(NB_READ_FRAMES) ?
            Int32.TryParse(this[NB_READ_FRAMES], out int nbReadFrames) ? nbReadFrames : null : null;

        public int? NBReadPackets => this.ContainsKey(NB_READ_PACKETS) ?
            Int32.TryParse(this[NB_READ_PACKETS], out int nbReadPackets) ? nbReadPackets : null : null;

        public int? ExtradataSize => this.ContainsKey(EXTRADATA_SIZE) ?
            Int32.TryParse(this[EXTRADATA_SIZE], out int extraDataSize) ? extraDataSize : null : null;

        public SampleFormat? SampleFormat => this.ContainsKey(SAMPLE_FMT) ?
            Enum.TryParse(this[SAMPLE_FMT], true, out SampleFormat sampleFormat) ? sampleFormat : null : null;

        public int? SampleRate => this.ContainsKey(SAMPLE_RATE) ?
            Int32.TryParse(this[SAMPLE_RATE], out int sampleRate) ? sampleRate : null : null;

        public int? Channels => this.ContainsKey(CHANNELS) ?
            Int32.TryParse(this[CHANNELS], out int channels) ? channels : null : null;

        public ChannelLayout? ChannelLayout => this.ContainsKey(CHANNEL_LAYOUT) ?
            Enum.TryParse(this[CHANNEL_LAYOUT], true, out ChannelLayout channelLayout) ? channelLayout : null : null;

        public int? BitsPerSample => this.ContainsKey(BITS_PER_SAMPLE) ?
            Int32.TryParse(this[BITS_PER_SAMPLE], out int bitsPerSample) ? bitsPerSample : null : null;

        public bool IsVideo => String.Equals(CodecType, "video", Helper.InvIgnCase);
        
        public bool IsAudio => String.Equals(CodecType, "audio", Helper.InvIgnCase);
    }
}