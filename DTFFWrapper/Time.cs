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
    public enum TimeUnit { Second, Frame}
    
    public class Time
    {
        /// <summary>
        /// Gets time unit of this Time instance. 
        /// It is automatically set when creating <paramref name="Time"/> instance.
        /// </summary>
        public TimeUnit Unit { get; private set; }

        #region Seconds
        /// <summary>
        /// Creates a new instance of <paramref name="Time"/>, from number of <paramref name="seconds"/> and <paramref name="frameRate"/>.
        /// </summary>
        /// <param name="seconds">Total number of seconds</param>
        /// <param name="frameRate">A framerate like <c>24.997</c>, <c>60</c>, etc.</param>
        public Time(double seconds, double frameRate)
        {
            HMS = String.Empty;
            Unit = TimeUnit.Second;
            FrameRate = frameRate;
            TotalSeconds = seconds;
        }

        /// <summary>
        /// Sets H, M, S, ms accordingly to number of <paramref name="seconds"/>.
        /// </summary>
        /// <param name="seconds"></param>
        private void ParseSecond(double seconds)
        {
            // 39036.123s
            double sec = 1.0 * seconds;
            double min = 0;
            double hrs = 0;
            double millis;

            if (sec >= 60)
            {
                min = sec / 60;
                sec -= 60 * Math.Truncate(min);
                if (min > 60)
                {
                    hrs = min / 60;
                    min -= 60 * Math.Truncate(hrs);
                }
            }

            H = Convert.ToInt32(Math.Truncate(hrs));
            M = Convert.ToInt32(Math.Truncate(min));
            S = Convert.ToInt32(Math.Truncate(sec));

            millis = (sec - Math.Truncate(sec)) * 1000;
            // 10.51 - 10 = 0.50999999999999979
            ms = Math.Round(millis, 3);
        }

        private double H, M, S;
        /// <summary>
        /// ms will actually contain also microseconds.<br/>
        /// <example>E.g.: <c>123.456ms</c> equals <c>123456us</c></example>
        /// </summary>
        private double ms;

        private double totalSeconds;
        /// <summary>
        /// Total seconds of this <paramref name = "Time"/> instance.
        /// If changing this value is needed, create a new <paramref name="Time"/> instance.<br/>
        /// <example>E.g.: <c>3600.0</c></example>
        /// </summary>
        public double TotalSeconds
        {
            get => totalSeconds;
            private set
            {
                totalSeconds = value;
                ParseSecond(value);
                HMS = $"{H.ToString("00")}:{M.ToString("00")}:{S.ToString("00")}.{Math.Round(ms * 1000)}";
                // Setting private frames avoids loops.
                frames = ToFrames(value, FrameRate);
            }
        }

        /// <summary>
        /// Converts number of <paramref name="frames"/> in number of seconds at specified <paramref name="frameRate"/>.
        /// </summary>
        /// <param name="frames">Number of frames.</param>
        /// <param name="frameRate">A framerate like <c>24.997</c>, <c>60</c>, etc.</param>
        /// <returns>Total number of seconds.</returns>
        public static double ToSeconds(int frames, double frameRate) =>
            frameRate > 0 ? 1.0 * frames / frameRate : 0;
        #endregion

        #region Frames
        /// <summary>
        /// Creates a new instance of <paramref name="Time"/>, from number of <paramref name="frames"/> and <paramref name="frameRate"/>.
        /// </summary>
        /// <param name="frames">Total number of frames.</param>
        /// <param name="frameRate">A framerate like <c>24.997</c>, <c>60</c>, etc.</param>
        public Time(int frames, double frameRate)
        {
            HMS = String.Empty;
            Unit = TimeUnit.Frame;
            FrameRate = frameRate;
            Frames = frames;
        }

        /// <summary>
        /// Framerate for internal calculations.
        /// If changing this value is needed, create a new <paramref name="Time"/> instance.
        /// </summary>
        public double FrameRate { get; private set; }

        private int frames;
        /// <summary>
        /// Total number of frames of this <paramref name="Time"/> instance.<br/>
        /// <example>E.g.: <c>10000</c></example>
        /// </summary>
        public int Frames
        {
            get => frames;
            private set
            {
                frames = value;
                HMS = frames.ToString() + 'f';
                // Setting private totalSeconds avoids loops.
                totalSeconds = ToSeconds(frames, FrameRate);
                ParseSecond(totalSeconds);
            }
        }

        /// <summary>
        /// Converts <paramref name="seconds"/> to number of frames at specified <paramref name="frameRate"/>.
        /// </summary>
        /// <param name="seconds">Number of seconds.</param>
        /// <param name="frameRate">A framerate like <c>24.997</c>, <c>60</c>, etc.</param>
        /// <returns>Total number of frames.</returns>
        public static int ToFrames(double seconds, double frameRate) => 
            Convert.ToInt32(seconds * frameRate);
        #endregion

        #region HMS
        /// <summary>
        /// Creates a new instance of <paramref name="Time"/>, from <paramref name="timeString"/> and <paramref name="frameRate"/>.
        /// </summary>
        /// <param name="timeString">A time in HMS.<br/>
        /// <example>E.g.: <c>10:50:36.123456</c> or <c>50:36</c> or <c>36.1</c></example><br/>
        /// <example>E.g.: a number of seconds like <c>1000s</c></example><br/>
        /// <example>E.g.: a number of frames like <c>1000f</c></example></param><br/>
        /// <param name="frameRate">A framerate like <c>24.997</c>, <c>60</c>, etc.</param>
        public Time(string timeString, double frameRate)
        {
            HMS = String.Empty;
            try
            {
                FrameRate = frameRate;
                timeString = timeString.Replace(',', '.');
                if (timeString.EndsWith('s'))
                {
                    // 1536.123456s
                    Unit = TimeUnit.Second;
                    TotalSeconds = double.Parse(timeString.Substring(0, timeString.Length - 1), System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (timeString.EndsWith('f'))
                {
                    // 1500f
                    Unit = TimeUnit.Frame;
                    Frames = int.Parse(timeString.Substring(0, timeString.Length - 1));
                }
                else
                {
                    Unit = TimeUnit.Second;
                    string[] splitted;
                    // 10:50:36.123456
                    splitted = timeString.Split('.');

                    if (splitted.Length > 1)
                    {
                        string strms = splitted[1];
                        switch (strms.Length)
                        {
                            case 1:
                                strms += "00000";
                                break;
                            case 2:
                                strms += "0000";
                                break;
                            case 3:
                                strms += "000";
                                break;
                            case 4:
                                strms += "00";
                                break;
                            case 5:
                                strms += "0";
                                break;
                            case 6:
                                break;
                        }
                        ms = Convert.ToDouble(strms) / 1000.0;
                    }
                    else
                    {
                        // 10:50:36
                        ms = 0;
                    }

                    splitted = splitted[0].Split(':');
                    int hours, minutes, seconds;

                    hours = splitted.Length >= 3 ? Int32.Parse(splitted[splitted.Length - 3]) : 0;
                    minutes = splitted.Length >= 2 ? Int32.Parse(splitted[splitted.Length - 2]) : 0;
                    seconds = Int32.Parse(splitted[splitted.Length - 1]);

                    TotalSeconds = hours * 3600 + minutes * 60 + seconds * 1.0 + (ms / 1000.0);
                }
            }
            catch (Exception E)
            {
                Unit = TimeUnit.Second;
                TotalSeconds = 0;
            }
        }

        /// <summary>
        /// Gets this Time in HMS.<br/>
        /// <example>E.g.: If <paramref name="TimeUnit"/> is <c>Second</c>, it will return a string like <c>10:50:36.123456</c> or <c>00:00:01.0</c></example><br/>
        /// <example>E.g.: If <paramref name="TimeUnit"/> is <c>Frame</c>, it will return a string like <c>1234f</c></example>
        /// </summary>
        public string HMS { get; private set; }
        #endregion
    }
}