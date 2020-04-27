// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace azure_parameter_grouping.Models
{
    /// <summary> Parameter group. </summary>
    public partial class ParameterGroupingPostRequiredParameters
    {
        /// <summary> Initializes a new instance of ParameterGroupingPostRequiredParameters. </summary>
        /// <param name="path"> Path parameter. </param>
        /// <param name="body"> . </param>
        public ParameterGroupingPostRequiredParameters(string path, int body)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            Path = path;
            Body = body;
        }

        /// <summary> Initializes a new instance of ParameterGroupingPostRequiredParameters. </summary>
        /// <param name="customHeader"> . </param>
        /// <param name="query"> Query parameter with default. </param>
        /// <param name="path"> Path parameter. </param>
        /// <param name="body"> . </param>
        internal ParameterGroupingPostRequiredParameters(string customHeader, int? query, string path, int body)
        {
            CustomHeader = customHeader;
            Query = query;
            Path = path;
            Body = body;
        }

        public string CustomHeader { get; set; }
        /// <summary> Query parameter with default. </summary>
        public int? Query { get; set; }
        /// <summary> Path parameter. </summary>
        public string Path { get; set; }
        public int Body { get; set; }
    }
}
