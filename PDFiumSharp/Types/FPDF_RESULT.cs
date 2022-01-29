#region Copyright and License

/*
This file is part of PDFiumSharp, a wrapper around the PDFium library for the .NET framework.
Copyright (C) 2017 Tobias Meyer
License: Microsoft Reciprocal License (MS-RL)
*/

#endregion

namespace PDFiumSharp.Types
{
    public enum FPDF_RESULT
    {
        /// <summary>
        ///     No error.
        /// </summary>
        SUCCESS = 0,

        /// <summary>
        ///     Error.
        /// </summary>
        ERROR = -1
    }

    public static class PDF_RESULT_Extension
    {
        public static string GetDescription(this FPDF_RESULT error)
        {
            switch (error)
            {
                case FPDF_RESULT.SUCCESS: return "No error.";
                case FPDF_RESULT.ERROR: return "Error.";
                default: return $"{error} (No description available).";
            }
        }
    }
}