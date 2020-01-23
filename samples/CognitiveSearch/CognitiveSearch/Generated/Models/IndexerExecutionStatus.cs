// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace CognitiveSearch.Models
{
    /// <summary> Represents the status of an individual indexer execution. </summary>
    public enum IndexerExecutionStatus
    {
        /// <summary> transientFailure. </summary>
        TransientFailure,
        /// <summary> success. </summary>
        Success,
        /// <summary> inProgress. </summary>
        InProgress,
        /// <summary> reset. </summary>
        Reset
    }
}
