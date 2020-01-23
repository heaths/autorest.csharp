// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace CognitiveSearch.Models
{
    /// <summary> A skill that extracts text from image files. </summary>
    public partial class OcrSkill : Skill
    {
        /// <summary> Initializes a new instance of OcrSkill. </summary>
        public OcrSkill()
        {
            OdataType = "#Microsoft.Skills.Vision.OcrSkill";
        }
        /// <summary> A value indicating which algorithm to use. Default is printed. </summary>
        public TextExtractionAlgorithm? TextExtractionAlgorithm { get; set; }
        /// <summary> The language codes supported for input by OcrSkill. </summary>
        public OcrSkillLanguage? DefaultLanguageCode { get; set; }
        /// <summary> A value indicating to turn orientation detection on or not. Default is false. </summary>
        public bool? ShouldDetectOrientation { get; set; }
    }
}
