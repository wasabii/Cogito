﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Cogito
{

    /// <summary>
    /// Represents a mime type media range, possibly including wildcards.
    /// </summary>
    [Serializable]
    public class MediaRange :
        ISerializable
    {

        /// <summary>
        /// Parses a new instance of <see cref="MediaRange"/> from a 'type/subtype' string.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MediaRange Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return new MediaRange(value);
        }

        public static implicit operator MediaRange(string value)
        {
            return value != null ? MediaRange.Parse(value) : null;
        }

        public static implicit operator string(MediaRange mediaRange)
        {
            return mediaRange != null ? mediaRange.ToString() : null;
        }

        public static bool operator ==(MediaRange a1, MediaRange a2)
        {
            return object.Equals(a1, a2);
        }

        public static bool operator !=(MediaRange a1, MediaRange a2)
        {
            return !object.Equals(a1, a2);
        }


        readonly MediaType type;
        readonly MediaType subtype;
        readonly MediaRangeParameters parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRange"/> class.
        /// </summary>
        MediaRange(MediaType type, MediaType subtype, MediaRangeParameters parameters)
        {
            Contract.Requires<ArgumentNullException>(parameters != null);

            this.type = type;
            this.subtype = subtype;
            this.parameters = parameters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaRange"/> class.
        /// </summary>
        /// <param name="value"></param>
        MediaRange(string value)
        {
            Contract.Requires<ArgumentNullException>(value != null);
            Contract.Requires<ArgumentOutOfRangeException>(!string.IsNullOrWhiteSpace(value));

            if (value.Equals("*"))
                value = "*/*";

            var parts = value.Split('/', ';');
            if (parts.Length < 2)
                throw new ArgumentException("Content type not in correct 'type/subType' format.", value);

            this.type = parts[0];
            this.subtype = parts[1].TrimEnd();
            this.parameters = parts.Length > 2 ? MediaRangeParameters.Parse(value.Substring(value.IndexOf(';'))) : new MediaRangeParameters();
        }

        /// <summary>
        /// Deserializes an instance of the <see cref="MediaRange"/> class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected MediaRange(SerializationInfo info, StreamingContext context)
            : this(info.GetString("MediaRange"))
        {
            Contract.Requires<ArgumentNullException>(info != null);
        }

        /// <summary>
        /// Media range type.
        /// </summary>
        public MediaType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Media range subtype.
        /// </summary>
        public MediaType Subtype
        {
            get { return subtype; }
        }

        /// <summary>
        /// Media range parameters
        /// </summary>
        public MediaRangeParameters Parameters
        {
            get { return parameters; }
        }

        /// <summary>
        /// Gets a value indicating if the media range is the */* wildcard.
        /// </summary>
        public bool IsWildcard
        {
            get { return type.IsWildcard && subtype.IsWildcard; }
        }

        /// <summary>
        /// Whether or not a media range matches another, taking into account wildcards.
        /// </summary>
        /// <param name="other">Other media range.</param>
        /// <returns>True if matching, false if not.</returns>
        public bool Matches(MediaRange other)
        {
            Contract.Requires<ArgumentNullException>(other != null);

            return type.Matches(other.type) && subtype.Matches(other.subtype);
        }

        /// <summary>
        /// Whether or not a media range matches another, taking into account wildcards and parameters.
        /// </summary>
        /// <param name="other">Other media range.</param>
        /// <returns>True if matching, false if not.</returns>
        public bool MatchesWithParameters(MediaRange other)
        {
            Contract.Requires<ArgumentNullException>(other != null);

            return Matches(other) && parameters.Matches(other.parameters);
        }

        /// <summary>
        /// Whether or not a media range matches any other media ranges, taking into account wildcards.
        /// </summary>
        /// <param name="others"></param>
        /// <returns></returns>
        public bool Matches(IEnumerable<MediaRange> others)
        {
            Contract.Requires<ArgumentNullException>(others != null);

            foreach (var i in others)
                if (Matches(i))
                    return true;

            return false;
        }

        /// <summary>
        /// Whether or not a media range matches any other media ranges, taking into account wildcards and parameters.
        /// </summary>
        /// <param name="others"></param>
        /// <returns></returns>
        public bool MatchesWithParameters(IEnumerable<MediaRange> others)
        {
            Contract.Requires<ArgumentNullException>(others != null);

            foreach (var i in others)
                if (MatchesWithParameters(i))
                    return true;

            return false;
        }

        public override string ToString()
        {
            if (parameters.Any())
                return string.Format("{0}/{1};{2}", type, subtype, parameters);
            else
                return string.Format("{0}/{1}", type, subtype);
        }

        public override bool Equals(object obj)
        {
            return obj is MediaRange ? MatchesWithParameters((MediaRange)obj) : false;
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ subtype.GetHashCode() ^ parameters.GetHashCode();
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MediaRange", ToString());
        }

    }

}