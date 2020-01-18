// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.ComponentModel;

namespace AppConfiguration.Models
{
    /// <summary> MISSING·SCHEMA-DESCRIPTION-CHOICE. </summary>
    public readonly partial struct Head7ItemsItem : IEquatable<Head7ItemsItem>
    {
        private readonly string? _value;

        /// <summary> Determines if two <see cref="Head7ItemsItem"/> values are the same. </summary>
        public Head7ItemsItem(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string KeyValue = "key";
        private const string LabelValue = "label";
        private const string ContentTypeValue = "content_type";
        private const string ValueValue = "value";
        private const string LastModifiedValue = "last_modified";
        private const string TagsValue = "tags";
        private const string LockedValue = "locked";
        private const string EtagValue = "etag";

        /// <summary> key. </summary>
        public static Head7ItemsItem Key { get; } = new Head7ItemsItem(KeyValue);
        /// <summary> label. </summary>
        public static Head7ItemsItem Label { get; } = new Head7ItemsItem(LabelValue);
        /// <summary> content_type. </summary>
        public static Head7ItemsItem ContentType { get; } = new Head7ItemsItem(ContentTypeValue);
        /// <summary> value. </summary>
        public static Head7ItemsItem Value { get; } = new Head7ItemsItem(ValueValue);
        /// <summary> last_modified. </summary>
        public static Head7ItemsItem LastModified { get; } = new Head7ItemsItem(LastModifiedValue);
        /// <summary> tags. </summary>
        public static Head7ItemsItem Tags { get; } = new Head7ItemsItem(TagsValue);
        /// <summary> locked. </summary>
        public static Head7ItemsItem Locked { get; } = new Head7ItemsItem(LockedValue);
        /// <summary> etag. </summary>
        public static Head7ItemsItem Etag { get; } = new Head7ItemsItem(EtagValue);
        /// <summary> Determines if two <see cref="Head7ItemsItem"/> values are the same. </summary>
        public static bool operator ==(Head7ItemsItem left, Head7ItemsItem right) => left.Equals(right);
        /// <summary> Determines if two <see cref="Head7ItemsItem"/> values are not the same. </summary>
        public static bool operator !=(Head7ItemsItem left, Head7ItemsItem right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="Head7ItemsItem"/>. </summary>
        public static implicit operator Head7ItemsItem(string value) => new Head7ItemsItem(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) => obj is Head7ItemsItem other && Equals(other);
        /// <inheritdoc />
        public bool Equals(Head7ItemsItem other) => string.Equals(_value, other._value, StringComparison.Ordinal);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string? ToString() => _value;
    }
}