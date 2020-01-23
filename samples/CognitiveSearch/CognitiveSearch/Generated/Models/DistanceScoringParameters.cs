// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace CognitiveSearch.Models
{
    /// <summary> Provides parameter values to a distance scoring function. </summary>
    public partial class DistanceScoringParameters
    {
        /// <summary> The name of the parameter passed in search queries to specify the reference location. </summary>
        public string ReferencePointParameter { get; set; }
        /// <summary> The distance in kilometers from the reference location where the boosting range ends. </summary>
        public double BoostingDistance { get; set; }
    }
}
