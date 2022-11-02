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

using System.Text.RegularExpressions;

namespace DTFFWrapper
{
    public interface IFFBase
    {
        /// <summary>
        /// Name of this object, internally set, as used by FFmpeg.
        /// </summary>
        public string FFName { get; }

        /// <summary>
        /// Friendly name of this object.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Returns the given name cleaned by unwanted characters.
        /// Characters minus (-), underscore (_) and colon (:) are permitted.
        /// </summary>
        public static string CleanName(string name) => Regex.Replace(name, @"[^a-zA-Z0-9_:-]", "-");

        /// <summary>
        /// String to be passed to FFmpeg exe to use this object.
        /// </summary>
        public string FFString();
    }
}
